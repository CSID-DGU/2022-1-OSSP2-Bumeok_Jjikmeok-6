using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slider_Viewer : MonoBehaviour
{
    protected Slider slider;
    protected virtual void Awake()
    {
        slider = GetComponent<Slider>();
        slider.value = 0;
    }
}
