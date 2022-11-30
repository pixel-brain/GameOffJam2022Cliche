using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    [Header("Variables")]
    public float stunTime;

    public float telegraphTime;
    public float[] moveDuration;
    public float[] moveReturnTime;

    public float returnToStartSpeed;
    public float timeBtwnShots;
    public float bulletSpeed;
    float shootTimer;

    public float laserSpinSpeed;

    public float roomMoveSpeed;

    public float targetMoveSpeed;
    public float targetLockTime;
    public float targetStrikeSpeed;
    float lockTimer;

    public float zigzagMoveSpeed;


    [Header("References")]
    public GameObject wallBlock;
    public GameObject bossIconsCanvas;
    public ParticleSystem dieParticlesPrefab;
    public Animator anim;
    public Animator uiIconsAnim;
    public GameObject stealIconPrefab;
    public Transform player;
    public GameObject bulletPrefab;
    public GameObject lasers;
    public Transform[] roomWaypoints;
    public Transform[] zigzagWaypoints;
    public Transform target;
    public SpriteRenderer iconRend;
    public GameObject levelCannons;
    public Image[] uiIcons;
    public Sprite[] iconSprites;
    List<int> availableMoves = new List<int> { 1, 2, 3, 4, 5 };
    int currentWaypoint;
    int stunCountdown;
    int iconToRemove;

    public int currentMove;
    Vector2 startPos;

    private FMOD.Studio.EventInstance bossShootSFX;

    // Start is called before the first frame update
    void Start()
    {
        
        startPos = transform.position;
        stunCountdown = 2;
        Setup();
        StartCoroutine(NextMove());
    }

    // Update is called once per frame
    void Update()
    {
        // Return to start pos
        if (currentMove == 0)
        {
            

            transform.position = Vector2.MoveTowards(transform.position, startPos, Time.deltaTime * returnToStartSpeed);
        }
        // Shoot bullets
        else if (currentMove == 1)
        {
            if (shootTimer < 0)
            {
                FMODUnity.RuntimeManager.PlayOneShotAttached("event:/SFX/Enemies/Gunshot", gameObject);
                shootTimer = timeBtwnShots;
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360))));
                bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.right * bulletSpeed;
            }
            shootTimer -= Time.deltaTime;
        }
        // Shoot lasers
        else if (currentMove == 2)
        {
            lasers.SetActive(true);
            lasers.transform.eulerAngles += new Vector3(0, 0, laserSpinSpeed * Time.deltaTime);
        }
        // Move around room
        else if (currentMove == 3)
        {
            float dist = Vector2.Distance(transform.position, roomWaypoints[currentWaypoint].position);
            if (dist < 0.1f)
            {
                currentWaypoint = (currentWaypoint + 1) % roomWaypoints.Length;
            }
            transform.position = Vector2.MoveTowards(transform.position, roomWaypoints[currentWaypoint].position, Time.deltaTime * roomMoveSpeed);
        }
        // Target strike
        else if (currentMove == 4)
        {
            target.gameObject.SetActive(true);
            if (lockTimer > 0)
            {
                target.position = Vector2.MoveTowards(target.position, player.position, Time.deltaTime * targetMoveSpeed);
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, target.position, Time.deltaTime * targetStrikeSpeed);
            }
            lockTimer -= Time.deltaTime;
        }
        // Zigzag
        else if (currentMove == 5)
        {
            float dist = Vector2.Distance(transform.position, zigzagWaypoints[currentWaypoint].position);
            if (dist < 0.1f)
            {
                currentWaypoint = (currentWaypoint + 1) % zigzagWaypoints.Length;
            }
            transform.position = Vector2.MoveTowards(transform.position, zigzagWaypoints[currentWaypoint].position, Time.deltaTime * zigzagMoveSpeed);
        }
    }

    void Setup()
    {
        lasers.transform.rotation = Quaternion.identity;
        lasers.SetActive(false);
        target.transform.position = startPos;
        target.gameObject.SetActive(false);
        lockTimer = targetLockTime;
        currentWaypoint = 0;
        anim.SetBool("Spin", false);
    }

    public void StealIcon()
    {
        availableMoves.Remove(iconToRemove);
        uiIcons[iconToRemove - 1].color = Color.red;
        anim.SetTrigger("Hurt");
        uiIconsAnim.SetTrigger("Shake");
        if (availableMoves.Count == 0)
        {
            Die();
        }
    }

    void Die()
    {
        StopAllCoroutines();
        currentMove = 0;
        anim.SetTrigger("Die");
        uiIconsAnim.SetTrigger("Remove");
        levelCannons.SetActive(false);
        Instantiate(dieParticlesPrefab, transform.position, Quaternion.identity);
        GetComponent<Collider2D>().enabled = false;
        Destroy(bossIconsCanvas, 3f);
        Destroy(wallBlock, 3f);
        Destroy(gameObject, 2.7f);
    }

    IEnumerator NextMove()
    {
        
        // Get Random move
        int nextMove = availableMoves[Random.Range(0, availableMoves.Count)];

        // Telegraph move
        iconRend.sprite = iconSprites[nextMove - 1];
        yield return new WaitForSeconds(telegraphTime);

        // Do move
        currentMove = nextMove;
        anim.SetBool("Spin", true);
        yield return new WaitForSeconds(moveDuration[currentMove - 1]);
        iconRend.sprite = null;
        currentMove = 0;
  

        // Setup for next move
        Setup();

        // Return to center
        yield return new WaitForSeconds(moveReturnTime[nextMove - 1]);

        // Check for stun
        stunCountdown--;
        if (stunCountdown <= 0)
        {
            iconToRemove = availableMoves[Random.Range(0, availableMoves.Count)];
            stunCountdown = 2;

            GameObject stealIcon = Instantiate(stealIconPrefab, transform.position, Quaternion.identity);
            stealIcon.GetComponent<SpriteRenderer>().sprite = iconSprites[iconToRemove - 1];
            Destroy(stealIcon, stunTime);
            anim.SetBool("Tired", true);
            yield return new WaitForSeconds(stunTime);
            anim.SetBool("Tired", false);
        }

        // Recurse
        StartCoroutine(NextMove());
    }

}
