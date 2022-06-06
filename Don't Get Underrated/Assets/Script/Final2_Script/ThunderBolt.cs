using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderBolt : Weapon_Devil
{
    // <a href='https://kr.freepik.com/vectors/light'>Light 벡터는 upklyak - kr.freepik.com가 제작함</a>

    private new void Awake()
    {
        base.Awake();
        if (GameObject.Find("Flash") && GameObject.Find("Flash").TryGetComponent(out ImageColor IC))
            backGroundColor = IC;
        if (GameObject.Find("Weapon_Effect_Sound") && GameObject.Find("Weapon_Effect_Sound").TryGetComponent(out AudioSource AS1))
            EffectSource = AS1;
    }
    private new void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Player") && collision.TryGetComponent(out Player_Info HC))
        {
            if (!HC.Unbeatable)
                HC.TakeDamage(1);
        }
    }
    private new void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject != null && collision.gameObject.CompareTag("Player") && collision.gameObject.TryGetComponent(out Player_Info HC))
        {
            if (!HC.Unbeatable)
                HC.TakeDamage(1);
        }
    }
    void Start()
    {
        Effect_Sound_OneShot(0);
        Camera_Shake(0.006f, 0.3f, false, false);
        Flash(Color.white, 0.1f, 0.5f);
    }
}
