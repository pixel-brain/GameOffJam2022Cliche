using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OnRailsMove : MonoBehaviour
{
    public float moveTime;
    public Ease easingFunction;
    public LineRenderer lineRend;
    int currentIndex;

    // Start is called before the first frame update
    void Start()
    {
        Move();
    }

    void Move()
    {
        Vector2 nextPos = transform.parent.position + lineRend.GetPosition(currentIndex);
        float distance = Vector2.Distance(transform.position, nextPos);
        transform.DOMove(nextPos, moveTime * distance).SetEase(easingFunction)
            .OnComplete(() =>
            {
                Move();
            });
        currentIndex = (currentIndex + 1) % lineRend.positionCount;
    }
}
