using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Devil : MonoBehaviour
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
        if (collision.gameObject != null && collision.gameObject.CompareTag("Player") && collision.gameObject.TryGetComponent(out Player_Info user2))
        {
            if (!user2.Unbeatable)
                Weak_Weapon();
            user2.TakeDamage(1);
        }
    }
    public void Weak_Weapon()
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
        if (collision.gameObject != null && collision.gameObject.CompareTag("Player") && collision.gameObject.TryGetComponent(out Player_Info user2))
        {
            if (!user2.Unbeatable)
                Weak_Weapon();
            user2.TakeDamage(1);
        }
    }
}
