using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W3_Boom : MonoBehaviour
{
    [SerializeField]
    AnimationCurve curve;

    float boomDelay = 0.5f;
    Animator animator;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        StartCoroutine("MoveToCenter");
    }
    IEnumerator MoveToCenter()
    {
        Vector3 startPosition = transform.position;
        // 이 transform.position은 Boom 오브젝트의 위치다.
        Vector3 endPosition = Vector3.zero;
        float current = 0;
        float percent = 0;
        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / boomDelay;

            // boomDelay에 설정된 시간동안 startPOsition부터 endPosition까지 이동
            // curve에 설정된 그래프처럼 처음엔 빠르게 이동하고, 목적지에 다다를수록 천천히 이동
            transform.position = Vector3.Lerp(startPosition, endPosition, curve.Evaluate(percent));
            // curve.Evaluate = 마치 그래프 같은 거 y = f(x)인데, 여기서는 x에 percent를 곁들인
            yield return null;
        }
        animator.SetTrigger("onBoom");
    }
    public void OnBoom()
    {
        GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] devil_weapon = GameObject.FindGameObjectsWithTag("Devil_Weapon");
        GameObject boss = GameObject.FindGameObjectWithTag("Boss");
        foreach(var e in enemy)
        {
            e.GetComponent<Enemy>().OnDie();
        }
        foreach (var e in devil_weapon)
        {
           e.GetComponent<Devil_Weapon>().OffLife();
        }
        if (boss != null)
        {
            boss.GetComponent<DoPhan>().TakeDamage(30);
        }
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
