using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    public float moveSpeed;
    public LayerMask groundLayers;
    Rigidbody2D rigi;
    BoxCollider2D col;

    // Start is called before the first frame update
    void Start()
    {
        rigi = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

        bool onGround = Physics2D.Raycast((Vector2)transform.position + new Vector2(transform.right.x * col.size.x / 2f, -col.size.y / 2f + 0.1f), -transform.up, 1.5f, groundLayers);
        bool lowWall = Physics2D.Raycast((Vector2)transform.position + new Vector2(transform.right.x * col.size.x / 2f, 0), transform.right, 1f, groundLayers);
        bool touchingWall = Physics2D.Raycast((Vector2)transform.position + new Vector2(transform.right.x * col.size.x / 2f, 0), transform.right, 0.06f, groundLayers);
        bool highWall = Physics2D.Raycast((Vector2)transform.position + new Vector2(transform.right.x * col.size.x / 2f, col.size.y / 2f + 0.3f), transform.right, 1f, groundLayers);
        // Turn       
        if (!onGround || touchingWall)
        {
            transform.localEulerAngles += new Vector3(0, 180, 0);
        }
        // Jump
        else if (lowWall && !highWall)
        {
            rigi.velocity = new Vector2(rigi.velocity.x, 6f);
        }
        rigi.velocity = new Vector2(transform.right.x * moveSpeed, rigi.velocity.y);
    }
}
