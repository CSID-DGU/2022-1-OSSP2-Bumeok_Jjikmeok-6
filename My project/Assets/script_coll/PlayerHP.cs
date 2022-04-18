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

    SpriteRenderer spriteRenderer; // �ν����Ϳ��� �ƹ��͵� �������� ������, �� ��������Ʈ�������� �ڵ����� �� ������Ʈ�� �ִ� ��������Ʈ���ͷ��� �����ȴ�.
                                   // �׷��� ��ũ��Ʈ�� ���� �߿��� ����

    PlayerController playerController;

    public float MaxHp => maxHP; // �ܺ� Ŭ�������� ���� ���� �� �ֵ��� public ���� �ϳ� �� ���� (SET�� �ȵ�. GET�� ����)
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
