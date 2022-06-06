using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Devil : Weapon // ��(Enemy, Boss_Info)�� �߻��ϴ� ����
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
    protected virtual void OnCollisionEnter2D(Collision2D collision) // �÷��̾�� �ε����� �� �÷��̾�� �������� 1 �� ��, ������ �ı��ϵ���(Weak_Weapon).
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
    protected virtual new void OnTriggerEnter2D(Collider2D collision) // ���� ����
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