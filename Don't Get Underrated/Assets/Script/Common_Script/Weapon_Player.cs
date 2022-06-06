using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Player : Weapon // 플레이어가 발사하는 무기
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
    protected virtual new void OnTriggerEnter2D(Collider2D collision) // 플레이어의 무기는 최종 (1)에서의 물방울, 폭탄이 다이다. 따라서 설정해야하는 부분이 상당부분 생략되었다.
    {
        base.OnTriggerEnter2D(collision);
        if (collision.gameObject != null && collision.CompareTag("Enemy") && collision.TryGetComponent(out F1_Homming_Enemy E))
        {
            E.OnDie();
            Weak_Weapon();
        } // 최종 (1)의 패턴 4에서 나오는 적을 공격했을 때는 해당 적이 반드시 죽도록 설정
        if (collision.CompareTag("Boss") && collision.gameObject.TryGetComponent(out Boss_Info B))
        {
            B.TakeDamage(Boss_Damage);
            if (B.Unbeatable)
                Destroy(gameObject);
            else
                Weak_Weapon();
        }
        // 보스가 무적 상태일 때는 무기 파괴 시 생겨나는 이펙트를 생성하지 않음.
    }
}