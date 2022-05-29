using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Devil : Weapon
{
    protected virtual new void Awake()
    {
        base.Awake();
    }
    // Start is called before the first frame update
    public override void Weak_Weapon()
    {
        Debug.Log(Explosion);
        if (Explosion != null)
        {
            Instantiate(Explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision) // 콜리전
    {
        if (collision.gameObject != null && collision.gameObject.CompareTag("Player") && collision.gameObject.TryGetComponent(out Player_Info PI))
        {
            if (!PI.Unbeatable)
                Weak_Weapon();
            PI.TakeDamage(1);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision) // 트리거(콜라이더)
    {
        if (collision.gameObject != null && collision.gameObject.CompareTag("Player") && collision.gameObject.TryGetComponent(out Player_Info PI))
        {
            if (!PI.Unbeatable)
                Weak_Weapon();
            PI.TakeDamage(1);
        }
        if (collision.gameObject != null && collision.CompareTag("Student") && collision.gameObject.TryGetComponent(out Student S))
        {
            if (S.get_Color() == new Color(0, 0, 1, 1))
                Destroy(gameObject);
        }
    }
}