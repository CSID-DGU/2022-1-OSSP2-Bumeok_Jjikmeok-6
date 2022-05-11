using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Info : Life
{
    // Start is called before the first frame update

    protected virtual new void Awake()
    {
        base.Awake();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Playerrr"))
        {
            if (!collision.GetComponent<Player_Info>().Unbeatable)
                Weak_Weapon();
            collision.GetComponent<Player_Info>().TakeDamage(1);
        }
        if (collision.CompareTag("Player"))
        {
            if (!collision.GetComponent<Player_Info>().Unbeatable)
                Weak_Weapon();
            collision.GetComponent<Player_Info>().TakeDamage(1);
        }
    }
    public void Weak_Weapon()
    {
        if (When_Dead_Effect != null)
        {
            Instantiate(When_Dead_Effect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
    public override void OnDie()
    {
        Destroy(gameObject);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
