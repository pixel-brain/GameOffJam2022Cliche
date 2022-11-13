using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public float moveSpeed;
    public float engageDist;
    public float turnBackDist;
    public LineRenderer lineRend;
    Transform player;
    Vector2 startPos;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        startPos = transform.position;
        lineRend.SetPosition(0, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            float distFromPlayer = Vector2.Distance(player.position, transform.position);
            float distFromStart = Vector2.Distance(startPos, transform.position);
            Quaternion rot = Quaternion.identity;
            if (distFromPlayer < engageDist && distFromStart < turnBackDist)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

                float angle = Mathf.Atan2(player.position.y, player.position.x) * Mathf.Rad2Deg;
                rot = Quaternion.Euler(new Vector3(0, 0, angle));
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, startPos, moveSpeed * Time.deltaTime);
            }

            Quaternion lerpedRot = Quaternion.RotateTowards(transform.rotation, rot, Time.deltaTime * 115f);
            transform.rotation = lerpedRot;
            lineRend.SetPosition(1, transform.position);
        }

    }
}
