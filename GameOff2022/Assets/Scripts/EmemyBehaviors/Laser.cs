using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public LineRenderer lineRend;
    public LayerMask hitLayers;
    public Transform firePoint;
    public Transform hitPoint;


    // Update is called once per frame
    void Update()
    {

        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, transform.right, Mathf.Infinity, hitLayers);
        if (hit.collider != null)
        {
            lineRend.SetPosition(0, firePoint.position);
            lineRend.SetPosition(1, hit.point);
            hitPoint.position = hit.point;
            hitPoint.transform.up = transform.up;
            if (hit.collider.CompareTag("Player"))
            {
                hit.collider.GetComponent<PlayerHealth>().Die();
            }
        }
    }
}
