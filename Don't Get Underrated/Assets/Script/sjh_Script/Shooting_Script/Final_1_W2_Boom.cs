using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Final_1_W2_Boom : MonoBehaviour
{
    [SerializeField]
    AnimationCurve curve;

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        StartCoroutine(MoveToCenter());
    }
    IEnumerator MoveToCenter()
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = Vector3.zero;
        float percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime * 2;
            transform.position = Vector3.Lerp(startPosition, endPosition, curve.Evaluate(percent));
            yield return null;
        }
        animator.SetTrigger("onBoom");
    }
    public void OnBoom()
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
           e.GetComponent<Devil_Weapon>().Weak_Weapon();
        }
        if (boss != null)
        {
            boss.GetComponent<DoPhan>().TakeDamage(30);
        }
        Destroy(gameObject);
    }

 
}
