using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteColor : MonoBehaviour
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
    public IEnumerator Change_Color(Color change_color, float ratio)
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
    public void Change_C(Color color, float time_persist)
    {
        if (param != null)
            StopCoroutine(param);
        param = Change_Color(color, time_persist);
        StartCoroutine(param);
    }
}
