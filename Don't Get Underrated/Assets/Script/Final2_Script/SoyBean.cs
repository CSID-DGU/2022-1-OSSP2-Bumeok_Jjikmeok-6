using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoyBean : Weapon_Devil
{
    [SerializeField]
    [Range(500f, 2000f)] float speed = 1000f;

    Rigidbody2D rb;

    [SerializeField]
    GameObject Disappear_Effect;

    float randomX, randomY;

    private int Count;
    // Start is called before the first frame update

    private new void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        Count = 0;
    }
    private void Start()
    {
        if (!GameObject.FindGameObjectWithTag("Player"))
        {
            randomX = Random.Range(-1f, 1f);
            randomY = Random.Range(-1f, 1f);
        }
        else if (GameObject.FindGameObjectWithTag("Player").TryGetComponent(out Player_Final2 HC))
        {
            randomX = HC.transform.position.x - transform.position.x;
            randomY = HC.transform.position.y - transform.position.y;
        }
        Vector2 dir = new Vector2(randomX, randomY).normalized;
        rb.AddForce(dir * speed);
    }
    private new void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject != null && collision.gameObject.CompareTag("Player") && collision.gameObject.TryGetComponent(out Player_Info PI))
            PI.TakeDamage(1);
        if (collision.gameObject != null && collision.gameObject.CompareTag("Ground"))
        {
            Start_Camera_Shake(0.01f, 0.1f, false, false);
            Count++;
            if (Count >= 4)
                OnDie();
        }
    }

    void OnDie()
    {
        Instantiate(Disappear_Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
