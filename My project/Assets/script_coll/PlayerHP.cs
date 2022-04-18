using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHP : MonoBehaviour
{
    [SerializeField]
    float maxHP = 10;

    [SerializeField]
    float currentHP;

    SpriteRenderer spriteRenderer; // 인스펙터에서 아무것도 설정하지 않으면, 이 스프라이트렌더러는 자동으로 그 오브젝트에 있는 스프라이트렌터러로 설정된다.
                                   // 그래서 스크립트가 존나 중요한 거임

    PlayerController playerController;

    public float MaxHp => maxHP; // 외부 클래스에서 값을 읽을 수 있도록 public 변수 하나 더 설정 (SET은 안됨. GET만 가능)
    public float CurrentHp
    {
        set
        {
            if (currentHP >= 9.7)
                currentHP--;
            currentHP = value;
        }
        get => currentHP;
    }

    public void Awake()
    {
        currentHP = maxHP;
        spriteRenderer = GetComponent<SpriteRenderer>();

    }
    // Start is called before the first frame update
    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        StartCoroutine("HitColorAnimation");
        if (currentHP <= 0)
        {
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            playerController.OnDie();
        }
    }
    IEnumerator HitColorAnimation()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
        yield break;
    }
    void Start()
    {
        
    }
   

    // Update is called once per frame
    void Update()
    {
        
    }
}
