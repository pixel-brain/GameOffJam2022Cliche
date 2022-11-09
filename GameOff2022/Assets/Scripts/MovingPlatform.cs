using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform[] waypoints;
    public float moveSpeed;
    Rigidbody2D rigi;
    Rigidbody2D playerRigi;
    Vector2 prevPos;

    int current;

    // Start is called before the first frame update
    void Start()
    {
        rigi = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float dist = Vector2.Distance(waypoints[current].position, transform.position);
        if (dist < 0.05f)
        {
            current++;
            if (current > waypoints.Length - 1)
            {
                current = 0;
            }
        }
        rigi.MovePosition(Vector2.MoveTowards(transform.position, waypoints[current].position, moveSpeed * Time.fixedDeltaTime));

        if (playerRigi != null && playerRigi.position.y > rigi.position.y)
        {
            Vector2 platformChange = rigi.position - prevPos;
            playerRigi.position += platformChange;
        }
        prevPos = rigi.position;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            playerRigi = collision.gameObject.GetComponent<Rigidbody2D>();
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            playerRigi = null;
        }
    }

}
