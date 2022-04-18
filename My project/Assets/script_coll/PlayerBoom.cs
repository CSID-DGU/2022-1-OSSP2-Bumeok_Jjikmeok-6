using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoom : MonoBehaviour
{
    [SerializeField]
    AnimationCurve curve;

    float boomDelay = 0.5f;
    Animator animator;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        StartCoroutine("MoveToCenter");
    }
    IEnumerator MoveToCenter()
    {
        Vector3 startPosition = transform.position;
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
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        // ���� ���� ������ "Enemy" �±׸� ���� ��� ������Ʈ ������ ������

        GameObject[] meteorites = GameObject.FindGameObjectsWithTag("Meteorite");
        //// ���� ���� ���� "Meteorite" �±׸� ���� ��� ������Ʈ ������ �����´�.
        
        foreach(var e in enemys)
        {
            e.GetComponent<Enemy>().OnDie();
        } // ��� �� �ı�

        foreach(var e in meteorites)
        {
            e.GetComponent<Meteorite>().OnDie();
        } // ��� � �ı�

        GameObject[] projectils = GameObject.FindGameObjectsWithTag("BossProjectile");
        foreach(var e in projectils)
        {
            e.GetComponent<EnemyProjectile>().OnDie();
        }

        //// ��ź ������Ʈ ���ε� ����
        Debug.Log("��ź ����!");
        Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
