using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    private FMOD.Studio.EventInstance chargeSFX;
    private FMOD.Studio.EventInstance shootSFX;

    [Header("Cannon Properties")]
    public float timeBtwnBursts;
    public float timeBtwnShots;
    public float timeBeforeFirst;
    public int burstCount;
    public float indicatorScale;
    [Header("Bullet Properties")]
    public float bulletSpeed;
    public float bulletGravity;
    [Header("References")]
    public ParticleSystem shootParticles;
    public GameObject bulletPrefab;
    public Transform loadIndicator;

    int currentShot;
    float shotTimer;

    // Start is called before the first frame update
    void Start()
    {
        currentShot = burstCount;
        shotTimer = timeBeforeFirst;
    }

    // Update is called once per frame
    void Update()
    {
        if (shotTimer < 0)
        {
            shotTimer = timeBtwnShots;
            GameObject bullet = Instantiate(bulletPrefab, shootParticles.transform.position, transform.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.right * bulletSpeed;
            bullet.GetComponent<Rigidbody2D>().gravityScale = bulletGravity;
            shootParticles.Play();
            currentShot--;

            shootSFX = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Enemies/Gunshot");
            shootSFX.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject.transform));
            shootSFX.start();
            shootSFX.release();
        }
        if (currentShot <= 0)
        {
            if (!IsPlaying(chargeSFX)) {
            chargeSFX = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Enemies/CannonCharge");
            chargeSFX.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject.transform));
            chargeSFX.start();
            chargeSFX.release();
        }
            currentShot = burstCount;
            shotTimer = timeBtwnBursts;
        }

        loadIndicator.localScale = Vector3.one * Mathf.Clamp(2.5f * (0.4f - (shotTimer / timeBtwnBursts)), 0, 1) * indicatorScale;

        shotTimer -= Time.deltaTime;
    }
    bool IsPlaying(FMOD.Studio.EventInstance instance)
    {
        FMOD.Studio.PLAYBACK_STATE state;
        instance.getPlaybackState(out state);
        return state != FMOD.Studio.PLAYBACK_STATE.STOPPED;
    }
}
