using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Spin : MonoBehaviour
{
    public float[] rotations;
    public float rotateSpeed;
    public Ease easingFunction;
    int currentIndex;


    // Start is called before the first frame update
    void Start()
    {
        Turn();
    }

    void Update()
    {
        Debug.Log(transform.localEulerAngles.z);
    }

    void Turn()
    {
        float dist = Mathf.Abs(rotations[currentIndex] - transform.localEulerAngles.z);
        transform.DORotate(new Vector3(0, 0, rotations[currentIndex]), dist * 0.005f * rotateSpeed).SetEase(easingFunction)
            .OnComplete(() =>
            {
                Turn();
            });

        currentIndex = (currentIndex + 1) % rotations.Length;
    }

}
