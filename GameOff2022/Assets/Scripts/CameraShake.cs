using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public Animator flashAnim;
    CinemachineVirtualCamera vCam;
    CinemachineBasicMultiChannelPerlin noiseMod;
    float fullIntensity;
    float fullTime;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        vCam = GetComponent<CinemachineVirtualCamera>();
        noiseMod = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    void Update()
    {
        if (timer > 0)
        {
            noiseMod.m_AmplitudeGain = fullIntensity * (timer / fullTime);
            timer -= Time.unscaledDeltaTime * 60;
        }
        else
        {
            noiseMod.m_AmplitudeGain = 0;
        }
    }

    public void Shake(float intensity, float time, bool flash)
    {
        if (flash == true)
        {
            flashAnim.SetTrigger("Flash");
        }
        fullTime = time;
        timer = fullTime;
        fullIntensity = intensity;
    }

}
