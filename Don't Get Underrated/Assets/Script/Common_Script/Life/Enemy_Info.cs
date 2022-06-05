using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Info : Life
{
    protected virtual new void Awake()
    {
        base.Awake();
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Player") && collision.TryGetComponent(out Player_Info HC))
        {
            if (!HC.Unbeatable)
            {
                HC.TakeDamage(1);
                Weak_Enemy();
            }
        }
    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject != null && collision.gameObject.CompareTag("Player") && collision.gameObject.TryGetComponent(out Player_Info HC))
        {
            if (!HC.Unbeatable)
            {
                HC.TakeDamage(1);
                Weak_Enemy();
            }
        }
    }
    protected void Weak_Enemy()
    {
        OnDie();
    }
    public override void TakeDamage(float damage)
    {
        OnDie();
    }
    public override void OnDie()
    {
        base.OnDie();
    }
    protected IEnumerator Shake_Act(float shake_intensity, float scale_ratio, float time_persist, bool is_Continue)
    {
        Vector3 originPosition;
        Quaternion originRotation;
        Vector3 originScale;
        originPosition = transform.position;
        originRotation =  transform.rotation;
        originScale = transform.localScale;
        float inverse_time_persist = StaticFunc.Reverse_Time(time_persist);
        while (true)
        {
            float percent = 0;
            while (percent < 1)
            {
                percent += Time.deltaTime * inverse_time_persist;
                transform.position = originPosition + Random.insideUnitSphere * shake_intensity;
                transform.localScale = new Vector3(transform.localScale.x + Time.deltaTime * scale_ratio, transform.localScale.y + Time.deltaTime * scale_ratio, 0);
                transform.transform.rotation = new Quaternion(
                                    originRotation.x + Random.Range(-shake_intensity, shake_intensity) * 0.2f,
                                    originRotation.y + Random.Range(-shake_intensity, shake_intensity) * 0.2f,
                                    originRotation.z + Random.Range(-shake_intensity, shake_intensity) * 0.2f,
                                    originRotation.w + Random.Range(-shake_intensity, shake_intensity) * 0.2f);
                yield return null;
            }
            if (!is_Continue)
            {
                transform.SetPositionAndRotation(originPosition, originRotation);
                transform.localScale = originScale;
                yield return null;
                yield break;
            }
        }
    }
}