using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileParallax : MonoBehaviour
{
    Transform cameraTransform;
    public float shiftPosition;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
        transform.position = new Vector3(cameraTransform.position.x, cameraTransform.position.y);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float xOffset = cameraTransform.position.x - transform.position.x;
        float yOffset = cameraTransform.position.y - transform.position.y;
        if (Mathf.Abs(xOffset) > shiftPosition)
        {
            transform.position += new Vector3(shiftPosition * Mathf.Sign(xOffset), 0);
        }
        if (Mathf.Abs(yOffset) > shiftPosition)
        {
            transform.position += new Vector3(0, shiftPosition * Mathf.Sign(yOffset));
        }

    }
}
