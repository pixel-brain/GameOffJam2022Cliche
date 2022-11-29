using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityBomb : MonoBehaviour
{
    public float activeDist;
    public float chargeTime;
    public float unchargeFactor;
    float chargeTimer;
    Transform player;
    Animator anim;
    public GameObject chargeIndicator;
    public ParticleSystem explodeParticlesPrefab;
    public LayerMask groundLayers;
    public LineRenderer lineRend;
    private FMOD.Studio.EventInstance chargeBombSFX;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        anim = GetComponent<Animator>();
        chargeTimer = chargeTime;
        lineRend.SetPosition(0, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            float dist = Vector2.Distance(player.position, transform.position);
            bool playerInView = !Physics2D.Raycast(transform.position, player.position - transform.position, dist, groundLayers);
            if (dist < activeDist && playerInView)
            {
                if (!IsPlaying(chargeBombSFX))
                {
                    chargeBombSFX = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Enemies/ProximityMine");
                    chargeBombSFX.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject.transform));
                    chargeBombSFX.start();
                    chargeBombSFX.release();
                }
                lineRend.SetPosition(1, player.position);
                anim.SetBool("Charging", true);
                // Explode
                if (chargeTimer < 0)
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Enemies/Bomb");
                    Instantiate(explodeParticlesPrefab, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                    if (IsPlaying(chargeBombSFX))
                    {
                        chargeBombSFX.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                        chargeBombSFX.release();
                    }
                }
                chargeTimer -= Time.deltaTime;
            }
            // Not in view/range
            else
            {
                if (IsPlaying(chargeBombSFX))
                {
                    chargeBombSFX.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    chargeBombSFX.release();
                }
                lineRend.SetPosition(1, transform.position);
                anim.SetBool("Charging", false);
                if (chargeTimer < chargeTime)
                    chargeTimer += Time.deltaTime * unchargeFactor;
            }
            //loadIndicator.localScale = Vector3.one * Mathf.Clamp(2.5f * (0.4f - (shotTimer / timeBtwnBursts)), 0, 1) * indicatorScale;
            chargeIndicator.transform.localScale = Vector2.one * Mathf.Clamp(2.5f * (0.4f - (chargeTimer / chargeTime)), 0, 1) * 0.86f;
        }   
    }
    bool IsPlaying(FMOD.Studio.EventInstance instance)
    {
        FMOD.Studio.PLAYBACK_STATE state;
        instance.getPlaybackState(out state);
        return state != FMOD.Studio.PLAYBACK_STATE.STOPPED;
    }
    private void OnDestroy()
    {
        if (IsPlaying(chargeBombSFX))
        {
            chargeBombSFX.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            chargeBombSFX.release();
        }

    }
}
