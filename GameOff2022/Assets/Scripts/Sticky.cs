using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sticky : MonoBehaviour
{
    public MovingPlatform platformScript;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        platformScript.CollisionDetected(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        platformScript.CollisionExited(collision);
    }

}
