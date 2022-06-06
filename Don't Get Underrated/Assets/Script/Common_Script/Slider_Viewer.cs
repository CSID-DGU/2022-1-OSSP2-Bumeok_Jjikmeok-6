using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slider_Viewer : MonoBehaviour // �����̴��� ���� ���� Ŭ���� (���� 6���� Ŭ������ ��� ��)
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
    protected bool Check_Valid_Slider() // �����̴��� ������ ã�� �� ���� ��츦 �����ϴ� �ڵ�
    {
        if (slider != null)
            return true;
        return false;
    }
}
