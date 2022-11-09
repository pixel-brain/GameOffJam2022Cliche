using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderMiddle : MonoBehaviour
{
    public RectTransform fill;

    public void SetValue(float value)
    {
        fill.localScale = new Vector3(Mathf.Clamp(value, 0, 1), 1);
    }

}
