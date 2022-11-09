using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWait : MonoBehaviour
{
    public float waitTime;
    public float showSliderTime;
    float waitTimer;
    bool waited;
    SliderMiddle slider;
    Animator sliderAnim;

    public void Awake()
    {
        GameObject sliderObject = GameObject.Find("WaitSlider");
        slider = sliderObject.GetComponent<SliderMiddle>();
        sliderAnim = sliderObject.GetComponent<Animator>();
        ResetTime();
    }

    public void ResetTime()
    {
        sliderAnim.SetBool("Active", false);
        waitTimer = waitTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (waitTimer < 0)
        {
            if (waited == false)
            {
                waited = true;
                GetComponent<PlayerHealth>().BecomeInvinsible();
            }
        }

        if (waitTimer < showSliderTime)
        {
            sliderAnim.SetBool("Active", true);
        }

        slider.SetValue(waitTimer / waitTime);
        waitTimer -= Time.deltaTime;
        Debug.Log(waitTimer);
    }

}
