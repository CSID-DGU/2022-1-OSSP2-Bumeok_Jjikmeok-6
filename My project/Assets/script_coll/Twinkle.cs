using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Twinkle : MonoBehaviour
{
    float fadeTime = 0.1f;
    SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine("TwinkleLoop");
    }
    IEnumerator TwinkleLoop()
    {
        while (true)
        {
            yield return StartCoroutine(FadeEffect(1, 0));
            yield return StartCoroutine(FadeEffect(0, 1));
        }
    }
    IEnumerator FadeEffect(int start, int end)
    {
        float currentTime = 0.0f;
        float percent = 0.0f;
        while(percent < 1)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / fadeTime;
            Color color = spriteRenderer.color;
            color.a = Mathf.Lerp(start, end, percent);
            spriteRenderer.color = color;
            yield return null;
        }
        yield break;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
