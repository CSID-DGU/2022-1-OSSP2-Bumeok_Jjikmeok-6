using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor_Traffic : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    private Vector3 originPosition;
    private Quaternion originRotation;

    private float shake_intensity;
    private float coef_shake_intensity = 0.2f;

    private void Awake()
    {
        if (TryGetComponent(out SpriteRenderer SR))
        {
            spriteRenderer = SR;
            spriteRenderer.color = Color.white;
        }
    }
    public IEnumerator Change_Color(float time_persist)
    {
        if (spriteRenderer == null)
            yield break;

        Color Alpha_1 = Color.white;
        Color Alpha_0 = Color.clear;
        float inverse_time_persist = StaticFunc.Reverse_Time(time_persist);

        float percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime * inverse_time_persist;
            spriteRenderer.color = Color.Lerp(Alpha_0, Alpha_1, percent);
            yield return null;
        }
        percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime * inverse_time_persist;
            spriteRenderer.color = Color.Lerp(Alpha_1, Alpha_0, percent);
            yield return null;
        }
        spriteRenderer.color = Alpha_1;
        yield return null;
    }
    public IEnumerator Shake_Act(float time_persist, float scale_dif)
    {
        if (spriteRenderer == null)
            yield break;

        originPosition = transform.position;
        originRotation = transform.rotation;
        shake_intensity = coef_shake_intensity;
        float percent = 0;
        float inverse_time_persist = StaticFunc.Reverse_Time(time_persist);
        while (percent < 1)
        {
            percent += Time.deltaTime * inverse_time_persist;
            transform.position = originPosition + Random.insideUnitSphere * shake_intensity;
            transform.localScale = new Vector3(transform.localScale.x + Time.deltaTime * scale_dif, transform.localScale.y + Time.deltaTime * scale_dif, 0);
            transform.transform.rotation = new Quaternion(
                                originRotation.x + Random.Range(-shake_intensity, shake_intensity) * 0.2f,
                                originRotation.y + Random.Range(-shake_intensity, shake_intensity) * 0.2f,
                                originRotation.z + Random.Range(-shake_intensity, shake_intensity) * 0.2f,
                                originRotation.w + Random.Range(-shake_intensity, shake_intensity) * 0.2f);
            yield return null;
        }
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}

