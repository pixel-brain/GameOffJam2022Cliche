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

    void Turn()
    {
        transform.DORotate(new Vector3(0, 0, rotations[currentIndex]), rotateSpeed).SetEase(easingFunction)
            .OnComplete(() =>
            {
                Turn();
            });

        currentIndex = (currentIndex + 1) % rotations.Length;
    }

}
