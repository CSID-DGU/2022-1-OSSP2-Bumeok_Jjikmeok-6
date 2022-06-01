using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumePercentage : MonoBehaviour
{
    Text volumeTxt;


    private void Awake()
    {
        if (TryGetComponent(out Text T))
            volumeTxt = T;
    }

    public void showVolumePersentage(float value)
    {
        volumeTxt.text = Mathf.RoundToInt(value * 100) + "%";
    }
}
