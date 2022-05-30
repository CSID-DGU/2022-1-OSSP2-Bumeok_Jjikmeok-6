using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ImageColor : MonoBehaviour
{
    Image image;
    // Start is called before the first frame update
    
    private void Awake()
    {
        if (TryGetComponent(out Image I))
            image = I;
    }
    public void Init()
    {
        if (image != null)
            image.color = new Color(1, 1, 1, 0);
    }
    public Color Get_BGColor()
    {
        if (image != null)
            return image.color;
        else
            return Color.white;
    }
    public void Set_BGColor(Color new_c)
    {
        if (image != null)
            image.color = new_c;
    }

    public IEnumerator Change_BG(Color Change, float time_persist)
    {
        if (image == null)
            yield break;
        Color const_Color = Get_BGColor();
        float percent = 0;
        float inverse_time_persist = StaticFunc.Reverse_Time(time_persist);
        while (percent < 1)
        {
            percent += Time.deltaTime * inverse_time_persist;
            image.color = Color.Lerp(const_Color, Change, percent);
            yield return null;
        }
    }

    public IEnumerator Flash(Color flashColor, float Wait_Second, float time_persist)
    {
        if (image == null)
            yield break;
        float inverse_time_persist = StaticFunc.Reverse_Time(time_persist);

        image.color = flashColor;
        yield return YieldInstructionCache.WaitForSeconds(Wait_Second);

        float percent = 0;
        while(percent < 1)
        {
            percent += Time.deltaTime * inverse_time_persist;
            image.color = Color.Lerp(image.color, Color.clear, percent);
            yield return null;
        }
    }
    public IEnumerator Change_Origin_BG(Color Change, float time_persist)
    {
        if (image == null)
            yield break;
        float inverse_time_persist = StaticFunc.Reverse_Time(time_persist);
        Color const_Color = Get_BGColor();
        float percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime * inverse_time_persist;
            image.color = Color.Lerp(const_Color, Change, percent);
            yield return null;
        }
        percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime * inverse_time_persist;
            image.color = Color.Lerp(Change, const_Color, percent);
            yield return null;
        }
    }
    public void Stop_Coroutine()
    {
        Init();
        StopAllCoroutines();
    }
}
