using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Player : MonoBehaviour
{
    [SerializeField]
    GameObject Explosion;
    Movement2D movement2D;

    protected virtual void Awake()
    {
        if (TryGetComponent(out Movement2D user))
            movement2D = user;
    }
    public void W_MoveTo(Vector3 Origin)
    {
        movement2D.MoveTo(Origin);
    }
    public void W_MoveSpeed(float Origin)
    {
        movement2D.MoveSpeed = Origin;
    }
    // Start is called before the first frame update
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
    public void Weak_Weapon()
    {
        if (Explosion != null)
        {
            Instantiate(Explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
   
    
}
