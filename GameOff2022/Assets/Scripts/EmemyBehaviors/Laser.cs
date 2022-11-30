using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public LineRenderer lineRend;
    public LayerMask hitLayers;
    public Transform firePoint;
    public Transform hitPoint;
    public FMOD.Studio.EventInstance laserSFX;
    private FMOD.Studio.EventInstance laserSFXhit;

    private void Start()
    {

            laserSFX = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Enemies/Laser");
            laserSFX.start();
            laserSFXhit = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Enemies/Laser2");
            laserSFXhit.start();

    }


    // Update is called once per frame
    void Update()
    {
        //laserSFX.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject.transform));
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(laserSFX, gameObject.transform);
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, transform.right, Mathf.Infinity, hitLayers);
        if (hit.collider != null)
        {
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(laserSFXhit, hitPoint.transform);
            lineRend.SetPosition(0, firePoint.position);
            lineRend.SetPosition(1, hit.point);
            hitPoint.position = hit.point;
   
            hitPoint.transform.up = transform.up;
            if (hit.collider.CompareTag("Player"))
            {
                hit.collider.GetComponent<PlayerHealth>().Die();
            }
        }
    }
    private void OnDisable()
    {
        laserSFX.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        laserSFX.release();
        laserSFXhit.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        laserSFXhit.release();
    }

}
