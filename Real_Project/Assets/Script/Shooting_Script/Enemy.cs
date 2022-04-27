using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : HP_Info
{

    [SerializeField]
    int ScorePerEnemy = 50;

    [SerializeField]
    GameObject Die_Explosion;

    SpriteRenderer spriteRenderer;

    PlayerControl playerControl;

    [SerializeField]
    GameObject[] Enemy_Weapon;

    new private void Awake()
    {
        base.Awake();
        CurrentHP = MaxHP;
        playerControl = GameObject.FindGameObjectWithTag("Playerrr").GetComponent<PlayerControl>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        StartCoroutine("Wow");
    }
    IEnumerator Wow()
    {
        yield return new WaitForSeconds(1f);
        while(true)
        {
            Instantiate(Enemy_Weapon[0], transform.position, Quaternion.identity);


            GameObject C1 = Instantiate(Enemy_Weapon[0], transform.position, Quaternion.identity);
            C1.GetComponent<Movement2D>().MoveTo(new Vector3(-1, 1.1428f, 0));


            GameObject C2 = Instantiate(Enemy_Weapon[0], transform.position, Quaternion.identity);
            C2.GetComponent<Movement2D>().MoveTo(new Vector3(-1, -1.1428f, 0));

            yield return new WaitForSeconds(0.6f);
        }
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Playerrr"))
        {
            if (!collision.GetComponent<PlayerControl>().Unbeatable_Player)
            {
                collision.GetComponent<PlayerControl>().Unbeatable_Player = true;
                collision.GetComponent<PlayerControl>().TakeDamage();
                Destroy(gameObject);
            }
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
