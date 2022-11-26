using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToParent : MonoBehaviour
{
    public float waitTime;
    public float moveSpeed;
    public float yOffset;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = waitTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < 0)
        {
            Vector2 targetPos = Vector2.up * yOffset;
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, targetPos, Time.deltaTime * moveSpeed);
            if ((Vector2)transform.localPosition == targetPos)
            {
                this.enabled = false;
            }
        }

        timer -= Time.deltaTime;
    }
}
