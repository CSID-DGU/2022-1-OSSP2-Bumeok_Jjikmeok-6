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
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        // 현재 게임 내에서 "Enemy" 태그를 가진 모든 오브젝트 정보를 가져옴

        GameObject[] meteorites = GameObject.FindGameObjectsWithTag("Meteorite");
        //// 현재 게임 내에 "Meteorite" 태그를 가진 모든 오브젝트 정보를 가져온다.
        
        foreach(var e in enemys)
        {
            e.GetComponent<Enemy>().OnDie();
        } // 모든 적 파괴

        foreach(var e in meteorites)
        {
            e.GetComponent<Meteorite>().OnDie();
        } // 모든 운석 파괴

        GameObject[] projectils = GameObject.FindGameObjectsWithTag("BossProjectile");
        foreach(var e in projectils)
        {
            e.GetComponent<EnemyProjectile>().OnDie();
        }

        //// 폭탄 오브젝트 본인도 삭제
        Debug.Log("폭탄 삭제!");
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
