using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    float maxHP = 4;
    float currentHP;
    Enemy enemy;
    SpriteRenderer spriteRenderer;

    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;
    private void Awake()
    {
        currentHP = maxHP;
        enemy = GetComponent<Enemy>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;

        StartCoroutine("HitColorAnimation");
        if (currentHP <= 0)
        {
            enemy.OnDie();
        }
    }
    IEnumerator HitColorAnimation()
    {
        spriteRenderer.color = Color.green;
        yield return new WaitForSeconds(0.05f);
        spriteRenderer.color = Color.white;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
