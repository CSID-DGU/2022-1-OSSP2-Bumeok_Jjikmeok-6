using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Devil : Weapon // 적(Enemy, Boss_Info)이 발사하는 무기
{
    protected virtual new void Awake()
    {
        base.Awake();
    }
    // Start is called before the first frame update
    public override void Weak_Weapon()
    {
        base.Weak_Weapon();
    }
    protected virtual void OnCollisionEnter2D(Collision2D collision) // 플레이어와 부딧혔을 때 플레이어에게 데미지를 1 준 후, 본인을 파괴하도록(Weak_Weapon).
    {
        if (collision.gameObject != null && collision.gameObject.CompareTag("Player") && collision.gameObject.TryGetComponent(out Player_Info PI))
        {
            if (!PI.Unbeatable)
            {
                PI.TakeDamage(1);
                Weak_Weapon();
            }
        }
    }
    protected virtual new void OnTriggerEnter2D(Collider2D collision) // 이하 동문
    {
        base.OnTriggerEnter2D(collision);

        if (collision != null && collision.CompareTag("Player") && collision.TryGetComponent(out Player_Info PI))
        {
            if (!PI.Unbeatable)
            {
                PI.TakeDamage(1);
                Weak_Weapon();
            }
        }
    }
}