using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Player : Weapon
{
    protected virtual new void Awake()
    {
        base.Awake();
    }
    // Start is called before the first frame updat
    public override void Weak_Weapon()
    {
        if (Explosion != null)
            Instantiate(Explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    public void Create_Explode()
    {
        Instantiate(Explosion, Vector3.zero, Quaternion.identity);
    }
    private void OnTriggerEnter2D(Collider2D collision) // 트리거(콜라이더)
    {
        if (collision.gameObject != null && collision.CompareTag("Student") && collision.gameObject.TryGetComponent(out Student user1))
        {
            if (user1.get_Color() == new Color(0, 0, 1, 1))
                Destroy(gameObject);
        }
        if (collision.gameObject != null && collision.CompareTag("Enemy") && collision.TryGetComponent(out F1_Homming_Enemy user))
        {
            user.OnDie();
            Destroy(gameObject);
        }
        if (collision.CompareTag("Boss") && collision.gameObject.TryGetComponent(out Asura user3))
        {
            if (!user3.Unbeatable)
                user3.TakeDamage(3f);
            Destroy(gameObject);
        }
    }
}
