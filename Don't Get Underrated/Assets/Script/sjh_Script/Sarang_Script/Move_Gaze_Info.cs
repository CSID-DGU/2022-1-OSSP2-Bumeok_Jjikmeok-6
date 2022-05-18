using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Move_Gaze_Info : MonoBehaviour // 플레이어 달릴 때의 게이지바
{
    Slider slider;
    private IEnumerator hp_down;
    // Start is called before the first frame update
    private void Awake()
    {
        slider = GetComponent<Slider>();
        hp_down = I_HP_Down();
        slider.value = 0;
    }
    public void HP_Down()
    {
        StartCoroutine(hp_down);
    }
    public void HP_Stop()
    {
        StopCoroutine(hp_down);
        slider.value = 0;
    }
    IEnumerator I_HP_Down()
    {
        while(true)
        {
            slider.value += Time.deltaTime / 3;
            yield return null;
        }
    }

    // Update is called once per frame
}