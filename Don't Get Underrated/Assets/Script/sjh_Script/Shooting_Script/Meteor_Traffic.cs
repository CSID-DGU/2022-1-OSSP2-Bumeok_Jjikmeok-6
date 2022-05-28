using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor_Traffic : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    private Vector3 originPosition;
    private Quaternion originRotation;

    private float shake_intensity;
    private float coef_shake_intensity = .2f;
    private float percent;
    private Color temp;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }
    public IEnumerator Change_Color(int Count, float time_persist)
    {
        Color Change_A_1 = Color.white;
        Color Change_A_0 = new Color(1, 1, 1, 0);
        for (int j = 0; j < Count; j++)
        {
            float percent = 0;
            while (percent < 1)
            {
                percent += Time.deltaTime / time_persist;
                spriteRenderer.color = Color.Lerp(Change_A_0, Change_A_1, percent);
                yield return null;
            }
            percent = 0;
            while (percent < 1)
            {
                percent += Time.deltaTime / time_persist;
                spriteRenderer.color = Color.Lerp(Change_A_1, Change_A_0, percent);
                yield return null;
            }
        }
        spriteRenderer.color = Change_A_1;
        yield return null;
    }
    public IEnumerator Change_Color()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0);
        for (int i = 0; i < 3; i++)
        {
            percent = 0;
            temp = spriteRenderer.color;
            while (percent < 1)
            {
                percent += Time.deltaTime * 8;
                spriteRenderer.color = Color.Lerp(temp, new Color(1, 1, 1, 1), percent);
                yield return null;
            }

            temp = spriteRenderer.color;
            percent = 0;
            while (percent < 1)
            {
                percent += Time.deltaTime * 8;
                spriteRenderer.color = Color.Lerp(temp, new Color(1, 1, 1, 0), percent);
                yield return null;
            }
        }
        percent = 0;
        temp = spriteRenderer.color;
        while (percent < 1)
        {
            percent += Time.deltaTime * 8;
            spriteRenderer.color = Color.Lerp(temp, new Color(1, 1, 1, 1), percent);
            yield return null;
        }
        yield break;
    }
    public IEnumerator Shake_Act(float time_persist, float scale_dif)
    {
        originPosition = transform.position;
        originRotation = transform.rotation;
        shake_intensity = coef_shake_intensity;
        float percent = 0;
        while(percent < 1)
        {
            percent += Time.deltaTime / time_persist;
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

