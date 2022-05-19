using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Devil : MonoBehaviour
{
    [SerializeField]
    GameObject Explosion;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision) // 트리거(콜라이더)
    {
        if (collision.gameObject != null && collision.gameObject.CompareTag("Player") && collision.gameObject.TryGetComponent(out Player_Info user2))
        {
            if (!user2.Unbeatable)
                Weak_Weapon();
            user2.TakeDamage(1);
        }
    }
    public void Weak_Weapon()
    {
        if (Explosion != null)
        {
            Instantiate(Explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision) // 콜리전
    {
        if (collision.gameObject != null && collision.gameObject.CompareTag("Player") && collision.gameObject.TryGetComponent(out HimaController user2))
        {
            user2.TakeDamage(1);
        }
    }
}
