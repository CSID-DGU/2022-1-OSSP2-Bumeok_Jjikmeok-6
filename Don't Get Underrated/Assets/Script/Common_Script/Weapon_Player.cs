using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Player : Weapon // �÷��̾ �߻��ϴ� ����
{
    [SerializeField]
    float Boss_Damage = 3f;
    protected virtual new void Awake()
    {
        base.Awake();
    }
    // Start is called before the first frame updat
    public override void Weak_Weapon()
    {
        base.Weak_Weapon();
    }
    protected virtual new void OnTriggerEnter2D(Collider2D collision) // �÷��̾��� ����� ���� (1)������ �����, ��ź�� ���̴�. ���� �����ؾ��ϴ� �κ��� ���κ� �����Ǿ���.
    {
        base.OnTriggerEnter2D(collision);
        if (collision.gameObject != null && collision.CompareTag("Enemy") && collision.TryGetComponent(out F1_Homming_Enemy E))
        {
            E.OnDie();
            Weak_Weapon();
        } // ���� (1)�� ���� 4���� ������ ���� �������� ���� �ش� ���� �ݵ�� �׵��� ����
        if (collision.CompareTag("Boss") && collision.gameObject.TryGetComponent(out Boss_Info B))
        {
            B.TakeDamage(Boss_Damage);
            if (B.Unbeatable)
                Destroy(gameObject);
            else
                Weak_Weapon();
        }
        // ������ ���� ������ ���� ���� �ı� �� ���ܳ��� ����Ʈ�� �������� ����.
    }
}