using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Info : Life
{
    // Start is called before the first frame update

    protected virtual new void Awake()
    {
        base.Awake();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!collision.GetComponent<Player_Info>().Unbeatable)
                Weak_Weapon();
            collision.GetComponent<Player_Info>().TakeDamage(1);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject != null && collision.gameObject.CompareTag("Player") && collision.gameObject.TryGetComponent(out HimaController user))
        {
            Weak_Weapon();
            user.TakeDamage(1);
        }
    }
    void Weak_Weapon()
    {
        if (When_Dead_Effect != null)
        {
            Instantiate(When_Dead_Effect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
    public override void TakeDamage(float damage)
    {
        OnDie();
    }
    public override void OnDie()
    {
        base.OnDie();
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
    protected IEnumerator Shake_Act(float shake_intensity, float scale_ratio, float time_persist, bool is_Continue)
    {
        Vector3 originPosition;
        Quaternion originRotation;
        Vector3 originScale;
        originPosition = transform.position;
        originRotation =  transform.rotation;
        originScale = transform.localScale;
        while (true)
        {
            float percent = 0;
            while (percent < 1)
            {
                percent += Time.deltaTime / time_persist;
                transform.position = originPosition + Random.insideUnitSphere * shake_intensity;
                transform.localScale = new Vector3(transform.localScale.x + Time.deltaTime * scale_ratio, transform.localScale.y + Time.deltaTime * scale_ratio, 0);
                transform.transform.rotation = new Quaternion(
                                    originRotation.x + Random.Range(-shake_intensity, shake_intensity) * 0.2f,
                                    originRotation.y + Random.Range(-shake_intensity, shake_intensity) * 0.2f,
                                    originRotation.z + Random.Range(-shake_intensity, shake_intensity) * 0.2f,
                                    originRotation.w + Random.Range(-shake_intensity, shake_intensity) * 0.2f);
                yield return YieldInstructionCache.WaitForEndOfFrame;
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