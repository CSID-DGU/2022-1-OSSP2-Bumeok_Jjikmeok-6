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
        // �� transform.position�� Boom ������Ʈ�� ��ġ��.
        Vector3 endPosition = Vector3.zero;
        float current = 0;
        float percent = 0;
        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / boomDelay;

            // boomDelay�� ������ �ð����� startPOsition���� endPosition���� �̵�
            // curve�� ������ �׷���ó�� ó���� ������ �̵��ϰ�, �������� �ٴٸ����� õõ�� �̵�
            transform.position = Vector3.Lerp(startPosition, endPosition, curve.Evaluate(percent));
            // curve.Evaluate = ��ġ �׷��� ���� �� y = f(x)�ε�, ���⼭�� x�� percent�� �����
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
