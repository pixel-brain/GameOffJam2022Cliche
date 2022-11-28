using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealIcon : MonoBehaviour
{
    public float moveSpeed;
    Transform movePoint;
    bool done;

    private void Start()
    {
        GameObject points = GameObject.Find("StealIconPoints");
        int pointIndex = Random.Range(0, points.transform.childCount);
        movePoint = points.transform.GetChild(pointIndex).transform;
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, movePoint.position, Time.deltaTime * moveSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player") && done == false)
        {
            GameObject boss = GameObject.Find("Boss");
            if (boss != null)
            {
                done = true;
                boss.GetComponent<Boss>().StealIcon();
                GetComponent<Animator>().SetTrigger("Steal");
                GetComponent<SpriteRenderer>().color = Color.red;
                Destroy(gameObject, 0.8f);
            }
        }
    }
}
