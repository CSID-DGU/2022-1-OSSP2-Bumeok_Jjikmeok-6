using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor_Traffic : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    private Vector3 originPosition;
    private Quaternion originRotation;

    //private float shake_decay = 0.002f;
    private float shake_intensity;
    private float coef_shake_intensity = .2f;
    private float percent;
    private Color temp;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(1, 1, 1, 0);
    }

    void Start()
    {

    }
    public IEnumerator Change_Color()
    {
        for (int i = 0; i < 4; i++)
        {
            percent = 0;
            temp = spriteRenderer.color;
            while (percent < 1)
            {
                percent += Time.deltaTime * 4;
                spriteRenderer.color = Color.Lerp(temp, new Color(1, 1, 1, 1), percent);
                yield return YieldInstructionCache.WaitForEndOfFrame;
            }

            temp = spriteRenderer.color;
            percent = 0;
            while (percent < 1)
            {
                percent += Time.deltaTime * 4;
                spriteRenderer.color = Color.Lerp(temp, new Color(1, 1, 1, 0), percent);
                yield return YieldInstructionCache.WaitForEndOfFrame;
            }
        }
        percent = 0;
        temp = spriteRenderer.color;
        while (percent < 1)
        {
            percent += Time.deltaTime * 4;
            spriteRenderer.color = Color.Lerp(temp, new Color(1, 1, 1, 1), percent);
            yield return YieldInstructionCache.WaitForEndOfFrame;
        }
        yield break;

    }
    public IEnumerator Shake_Act()
    {
        originPosition = transform.position;
        originRotation = transform.rotation;
        shake_intensity = coef_shake_intensity;
        float percent = 0;
        while(percent < 1)
        {
            percent += Time.deltaTime;
            transform.position = originPosition + Random.insideUnitSphere * shake_intensity;
            transform.localScale = new Vector3(transform.localScale.x + Time.deltaTime / 6, transform.localScale.y + Time.deltaTime / 6, 0);
            transform.transform.rotation = new Quaternion(
                                originRotation.x + Random.Range(-shake_intensity, shake_intensity) * 0.2f,
                                originRotation.y + Random.Range(-shake_intensity, shake_intensity) * 0.2f,
                                originRotation.z + Random.Range(-shake_intensity, shake_intensity) * 0.2f,
                                originRotation.w + Random.Range(-shake_intensity, shake_intensity) * 0.2f);
            yield return YieldInstructionCache.WaitForEndOfFrame;
        }
    }
}
