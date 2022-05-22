using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Player : MonoBehaviour
{
    [SerializeField]
    GameObject Explosion;
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
        if (collision.CompareTag("Boss") && collision.gameObject.TryGetComponent(out DoPhan user3))
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
   
    public void OnBoom() // 최종 스테이지 (1) - 폭탄의 경우
    {
        GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] devil_weapon = GameObject.FindGameObjectsWithTag("Devil_Weapon");
        GameObject boss = GameObject.FindGameObjectWithTag("Boss");
        GameObject[] meteor = GameObject.FindGameObjectsWithTag("Meteor");
        GameObject[] meteor_line = GameObject.FindGameObjectsWithTag("Meteor_Line");
        GameObject[] meteor_traffic = GameObject.FindGameObjectsWithTag("Meteor_Traffic");
        foreach (var e in meteor)
        {
            Destroy(e);
        }
        foreach (var e in meteor_line)
        {
            Destroy(e);
        }
        foreach (var e in meteor_traffic)
        {
            Destroy(e);
        }
        foreach (var e in enemy)
        {
            e.GetComponent<F1_Homming_Enemy>().OnDie();
        }
        foreach (var e in devil_weapon)
        {
            e.GetComponent<Weapon_Devil>().Weak_Weapon();
        }
        if (boss != null)
        {
            boss.GetComponent<DoPhan>().TakeDamage(30);
        }
        Destroy(gameObject);
    }
}
