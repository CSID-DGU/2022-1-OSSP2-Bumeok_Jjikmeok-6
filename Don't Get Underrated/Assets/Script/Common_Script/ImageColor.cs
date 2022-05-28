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
        image = GetComponent<Image>();
    }
    public void Init()
    {
        image.color = new Color(1, 1, 1, 0);
    }
    public Color Get_BGColor()
    {
        return image.color;
    }
    public void Set_BGColor(Color new_c)
    {
        image.color = new_c;
    }

    public IEnumerator Change_BG(Color Change, float time_persist)
    {
        Color const_Color = Get_BGColor();
        float percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime / time_persist;
            image.color = Color.Lerp(const_Color, Change, percent);
            yield return null;
        }
    }

    public IEnumerator Flash(Color flashColor, float Wait_Second, float time_persist)
    {
        if (time_persist <= 0)
            time_persist = 1;

        image.color = flashColor;
        yield return YieldInstructionCache.WaitForSeconds(Wait_Second);

        float percent = 0;
        while(percent < 1)
        {
            percent += Time.deltaTime / time_persist;
            image.color = Color.Lerp(image.color, Color.clear, percent);
            yield return null;
        }
    }
    public IEnumerator Change_Origin_BG(Color Change, float time_persist)
    {
        if (time_persist <= 0)
            time_persist = 1;
        Color const_Color = Get_BGColor();
        float percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime / time_persist;
            image.color = Color.Lerp(const_Color, Change, percent);
            yield return null;
        }
        percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime / time_persist;
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
