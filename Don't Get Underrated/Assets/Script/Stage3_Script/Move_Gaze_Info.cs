using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Move_Gaze_Info : Slider_Viewer // �÷��̾� �޸� ���� ��������
{
    private new void Awake()
    {
        base.Awake();
    }
    public void HP_Down()
    {
        slider.value += Time.deltaTime * 0.5f;
    }
    public void HP_Stop()
    {
        slider.value = 0;
    }
    public float Get_HP()
    {
        return slider.value;
    }
}