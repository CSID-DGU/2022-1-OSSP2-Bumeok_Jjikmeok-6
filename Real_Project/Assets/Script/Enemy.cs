using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : HP_Info
{
    // Start is called before the first frame update

    [SerializeField]
    float attack_rate = 1;

    [SerializeField]
    int ScorePerEnemy = 50;

    [SerializeField]
    GameObject Die_Explosion;

    SpriteRenderer spriteRenderer;

    PlayerControl playerControl;

    new private void Awake()
    {
        base.Awake();
        CurrentHP = MaxHP;
        playerControl = GameObject.FindGameObjectWithTag("Playerrr").GetComponent<PlayerControl>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Playerrr"))
        {
            collision.GetComponent<PlayerControl>().TakeDamage(attack_rate);
            Destroy(gameObject);
        }
    }
    public void TakeDamage(float damage)
    {
        CurrentHP -= damage;
        StartCoroutine("Hit");
        if (CurrentHP <= 0)
        {
            OnDie();
        }
    }
    IEnumerator Hit()
    {
        spriteRenderer.color = Color.blue;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }
    void Start()
    {
        
    }

    public void OnDie()
    {
        playerControl.Score += ScorePerEnemy;
        Instantiate(Die_Explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
