using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slider_Viewer : MonoBehaviour // 슬라이더에 대한 상위 클래스 (현재 6개의 클래스가 상속 중)
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
    protected bool Check_Valid_Slider() // 슬라이더의 정보를 찾을 수 없을 경우를 방지하는 코드
    {
        if (slider != null)
            return true;
        return false;
    }
}
