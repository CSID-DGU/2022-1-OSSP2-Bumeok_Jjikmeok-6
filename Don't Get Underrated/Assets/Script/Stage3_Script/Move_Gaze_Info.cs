using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Move_Gaze_Info : Slider_Viewer // 플레이어 달릴 때의 게이지바
{
    private new void Awake()
    {
        base.Awake();
    }
    public void HP_Down()
    {
        if (Check_Valid_Slider())
            slider.value += Time.deltaTime * 0.5f; // 2초 간 달릴 수 있도록 함
    }
    public void HP_Stop()
    {
        if (Check_Valid_Slider())
            slider.value = 0; // 달리는걸 멈추거나 지쳤을 때 게이지 초기화
    }
    public float Get_HP()
    {
        if (Check_Valid_Slider())
            return slider.value;
        else
            return 0;
    }
}