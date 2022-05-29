using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Move_Gaze_Info : MonoBehaviour // �÷��̾� �޸� ���� ��������
{
    Slider slider;
    // Start is called before the first frame update
    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.value = 0;
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
    // Update is called once per frame
}