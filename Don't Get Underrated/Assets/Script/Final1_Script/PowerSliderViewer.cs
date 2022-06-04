using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSliderViewer : Slider_Viewer
{
    // Start is called before the first frame update

    IEnumerator Act;
    private new void Awake()
    {
        base.Awake();
    }
    public void Start_To_Decrease(float time_persist)
    {
        Stop_To_Decrease();
        Act = Decrease(time_persist);
        StartCoroutine(Act);
    }
    public void Stop_To_Decrease()
    {
        if (Act != null)
            StopCoroutine(Act);
    }
    IEnumerator Decrease(float time_persist)
    {
        if (!Check_Valid_Slider())
            yield break;
        slider.value = 1;
        float inverse_time_persist = StaticFunc.Reverse_Time(time_persist);
        float percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime * inverse_time_persist;
            slider.value = Mathf.Lerp(1, 0, percent);
            yield return null;
        }
        if (GameObject.FindGameObjectWithTag("Player") && GameObject.FindGameObjectWithTag("Player").TryGetComponent(out Player_Final1 PC_T))
        {
            PC_T.is_Power_Up = false;
            PC_T.Power_Slider.SetActive(false);
        }
           
        yield return null;
    }
}
