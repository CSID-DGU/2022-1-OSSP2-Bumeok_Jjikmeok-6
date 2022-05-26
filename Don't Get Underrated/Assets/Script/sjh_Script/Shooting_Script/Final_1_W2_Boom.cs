using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Final_1_W2_Boom : Weapon_Player
{
    [SerializeField]
    AnimationCurve curve;

    Animator animator;

    private new void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        StartCoroutine(MoveToCenter());
    }
    IEnumerator MoveToCenter()
    {
        Vector3 startPosition = new Vector3(0, 7.7f, 0);
        Vector3 endPosition = Vector3.zero;
        float percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime * 2;
            transform.position = Vector3.Lerp(startPosition, endPosition, curve.Evaluate(percent));
            yield return null;
        }
        StartCoroutine(OnBoom());
    }
    IEnumerator OnBoom() // 최종 스테이지 (1) - 폭탄의 경우
    {
        GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] devil_weapon = GameObject.FindGameObjectsWithTag("Weapon_Devil");
        GameObject boss = GameObject.FindGameObjectWithTag("Boss");
        GameObject[] meteor = GameObject.FindGameObjectsWithTag("Meteor");

        foreach (var e in meteor)
            Destroy(e);

        foreach (var e in enemy)
        {
            if (e.TryGetComponent(out F1_Homming_Enemy f1))
                f1.OnDie();
        }
        foreach (var e in devil_weapon)
        {
            if (e.TryGetComponent(out Weapon_Devil f1))
                f1.Weak_Weapon();
        }
        if (boss != null)
        {
            if (boss.TryGetComponent(out Asura f1))
                f1.TakeDamage(30.0f);
        }
        Create_Explode();
        yield return StartCoroutine(Start_Camera_Shake_For_Wait(0.07f, 2f, true, false));

        Destroy(gameObject);
    }
}