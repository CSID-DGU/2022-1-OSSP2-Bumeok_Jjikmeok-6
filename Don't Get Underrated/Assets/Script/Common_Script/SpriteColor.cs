using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteColor : MonoBehaviour
{
    // Start is called before the first frame update

    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public Color Get_BGColor()
    {
        return spriteRenderer.color;
    }
    public IEnumerator Change_Color(Color origin_color, Color change_color, float ratio)
    {
        float percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime / ratio;
            spriteRenderer.color = Color.Lerp(origin_color, change_color, percent);
            yield return null;
        }
    }
}
