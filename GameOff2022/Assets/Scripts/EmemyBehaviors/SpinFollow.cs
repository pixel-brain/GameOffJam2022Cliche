using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinFollow : MonoBehaviour
{
    public float spinSpeed;
    public float rubberBandFactor;
    Transform player;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {

            Vector3 target = player.position;
            target.x = target.x - transform.position.x;
            target.y = target.y - transform.position.y;

            float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
            Quaternion rot = Quaternion.Euler(new Vector3(0, 0, angle));
            float rubberBand = Quaternion.Angle(transform.rotation, rot) * rubberBandFactor;
            Quaternion lerpedRot = Quaternion.RotateTowards(transform.rotation, rot, Time.deltaTime * (spinSpeed + rubberBand));
            transform.rotation = lerpedRot;

        }
    }
}
