using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteColor : MonoBehaviour // 배경 색상보다 Layer가 더 높은 스프라이트 색상 (게임 종료 시에 사용)
{
    // Start is called before the first frame update

    private SpriteRenderer spriteRenderer;

    private IEnumerator param;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public Color Get_BGColor()
    {
        return spriteRenderer.color;
    }
    public void Set_BG_Clear()
    {
        spriteRenderer.color = Color.clear;
    }
    public IEnumerator Change_Color_And_Wait(Color change_color, float ratio)
    {
        float percent = 0;
        Color origin_color = Get_BGColor();
        float inverse_time_persist = StaticFunc.Reverse_Time(ratio);
        while (percent < 1)
        {
            percent += Time.deltaTime * inverse_time_persist;
            spriteRenderer.color = Color.Lerp(origin_color, change_color, percent);
            yield return null;
        }
    }
    public IEnumerator Change_Color_Real_Time(Color change_color, float ratio)
    {
        float percent = 0;
        Color origin_color = Get_BGColor();
        float inverse_time_persist = StaticFunc.Reverse_Time(ratio);
        while (percent < 1)
        {
            percent += Time.unscaledDeltaTime * inverse_time_persist;
            spriteRenderer.color = Color.Lerp(origin_color, change_color, percent);
            yield return null;
        }
    }
    public void Change_Color(Color color, float time_persist)
    {
        if (param != null)
            StopCoroutine(param);
        param = Change_Color_And_Wait(color, time_persist);
        StartCoroutine(param);
    }
}
