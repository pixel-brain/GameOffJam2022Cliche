using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovingPlatform : MonoBehaviour
{
    public LineRenderer lineRend;
    public float moveTime;
    public int startIndex;
    public Ease easingFunction;
    public bool sticky;
    int currentIndex;
    Rigidbody2D playerRigi;
    Vector2 prevPos;
    int touchedColliders;

    // Start is called before the first frame update
    void Start()
    {
        currentIndex = startIndex;
        Follow();
    }

    void Follow()
    {
        Vector2 nextPos = transform.parent.position + lineRend.GetPosition(currentIndex);
        float distance = Vector2.Distance(transform.position, nextPos);
        transform.DOMove(nextPos, moveTime * distance).SetEase(easingFunction)
            .OnUpdate(() =>
            {
                if (sticky && playerRigi != null)
                {
                    playerRigi.position += (Vector2)transform.position - prevPos;
                }
                prevPos = transform.position;
            })
            .OnComplete(() =>
            {
                Follow();
            });
        currentIndex++;
        if (currentIndex == lineRend.positionCount)
        {
            currentIndex = 0;
        }
    }

    public void CollisionDetected(Collision2D other)
    {
        if (other.transform.CompareTag("Player") && other.transform.position.y > other.contacts[0].point.y)
        {
            touchedColliders++;
            playerRigi = other.transform.GetComponent<Rigidbody2D>();
        }
    }

    public void CollisionExited(Collision2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            touchedColliders--;
            if (touchedColliders <= 0)
            {
                touchedColliders = 0;
                playerRigi = null;
            }
        }
    }
}