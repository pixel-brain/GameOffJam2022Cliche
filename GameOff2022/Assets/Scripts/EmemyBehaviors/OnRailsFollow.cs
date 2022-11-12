using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnRailsFollow : MonoBehaviour
{
    public LineRenderer lineRend;
    public float moveSpeed;
    public float rubberBandFactor;
    public bool followX;
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
            if (followX)
            {
                float rubberSpeed = Mathf.Abs(player.position.x - transform.position.x) * rubberBandFactor;
                float targetX = Mathf.Clamp(player.position.x, transform.parent.position.x + lineRend.GetPosition(0).x, transform.parent.position.x + lineRend.GetPosition(1).x);
                float xPos = Mathf.MoveTowards(transform.position.x, targetX, Time.deltaTime * (moveSpeed + rubberSpeed));
                transform.position = new Vector3(xPos, transform.position.y, 0);
            }
            else
            {
                float rubberSpeed = Mathf.Abs(player.position.y - transform.position.y) * rubberBandFactor;
                float targetY = Mathf.Clamp(player.position.y, transform.parent.position.y + lineRend.GetPosition(0).y, transform.parent.position.y + lineRend.GetPosition(1).y);
                float yPos = Mathf.MoveTowards(transform.position.y, targetY, Time.deltaTime * (moveSpeed + rubberSpeed));
                transform.position = new Vector3(transform.position.x, yPos, 0);
            }
        }
    }
}
