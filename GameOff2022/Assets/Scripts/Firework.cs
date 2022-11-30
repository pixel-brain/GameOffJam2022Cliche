using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firework : MonoBehaviour
{
    public float minUpDist;
    public float maxUpDist;
    public float maxHeight;
    public float minUpSpeed;
    public float maxUpSpeed;
    public ParticleSystem upParticles;
    public GameObject blastParticlesPrefab;
    Rigidbody2D rigi;
    float upSpeed;
    float upDist;
    float triggeredYPos;

    private FMOD.Studio.EventInstance fireworkSFX;

    private void Start()
    {
        rigi = GetComponent<Rigidbody2D>();
        upSpeed = Random.Range(minUpSpeed, maxUpSpeed);
        upDist = Random.Range(minUpDist, maxUpDist);
    }

    private void FixedUpdate()
    {
        if (triggeredYPos != 0)
        {
            rigi.velocity = Vector2.up * upSpeed;
            // Blast
            if (rigi.position.y > upDist + triggeredYPos || rigi.position.y > maxHeight)
            {
                fireworkSFX.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                fireworkSFX.release();
                FMODUnity.RuntimeManager.PlayOneShotAttached("event:/SFX/LevelSFX/FireworksBoom", gameObject);
                Instantiate(blastParticlesPrefab, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            fireworkSFX = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/LevelSFX/Fireworks");
            fireworkSFX.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject.transform));
            fireworkSFX.start();
            fireworkSFX.release();

            triggeredYPos = rigi.position.y;
            rigi.rotation = 0;
            upParticles.Play();
        }
    }
}
