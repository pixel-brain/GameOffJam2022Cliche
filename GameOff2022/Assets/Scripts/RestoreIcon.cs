using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestoreIcon : MonoBehaviour
{
    public int iconNum;

    public float moveSpeed;
    public float dirCorrectSpeed;
    public Vector2 moveDir;

    public float stayTime;
    public float stayYOffset;
    public float disappearTime;
    Rigidbody2D rigi;
    Transform player;
    float timer;
    int stage;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        timer = stayTime;
        rigi = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Wait
        if (stage == 0)
        {
            if (timer < 0)
            {
                stage++;
            }
        }
        // Move towards player
        else if (stage == 1)
        {
            Vector2 playerOffsetPos = player.position + Vector3.up * stayYOffset;
            Vector2 target = playerOffsetPos - (Vector2)transform.position;
            moveDir = Vector2.MoveTowards(moveDir, target, dirCorrectSpeed * Time.deltaTime);
            rigi.velocity = moveDir.normalized * moveSpeed;

            float dist = Vector2.Distance(transform.position, playerOffsetPos);
            if (dist < 0.4f)
            {
                stage++;
                timer = disappearTime;
                // Set color to white
                GetComponent<SpriteRenderer>().color = Color.white;
                // Activate control for player
                player.GetComponent<PlayerMovement>().ActivateControl(iconNum);
            }
        }
        // Hang above player
        else if (stage == 2)
        {
            transform.position = player.position + Vector3.up * stayYOffset;
            if (timer < 0)
            {
                Destroy(gameObject);
            }
        }

        // Use timer to alternate between states
        if (stage != 1)
        {
            timer -= Time.deltaTime;
        }
    }
}
