using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slider_Viewer : MonoBehaviour
{
    protected Slider slider;
    protected virtual void Awake()
    {
        if (TryGetComponent(out Slider S))
        {
            slider = S;
            slider.value = 0;
        }
    }
    protected bool Check_Valid_Slider()
    {
        if (slider != null)
            return true;
        return false;
    }
}
