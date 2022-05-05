using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    int ScorePerEnemy = 50;

    [SerializeField]
    GameObject Die_Explosion;

    PlayerControl playerControl;

    Rigidbody2D rb;

    [SerializeField]
    float speed = 5;

    [SerializeField]
    public float rotateSpeed = 200f;

    private void Awake()
    {
        playerControl = GameObject.FindGameObjectWithTag("Playerrr").GetComponent<PlayerControl>();
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        //StartCoroutine(Auto_Dead());
        StartCoroutine(Homming_Player());
    }
    IEnumerator Auto_Dead()
    {
        yield return YieldInstructionCache.WaitForSeconds(5f);
        OnDie();
        yield return YieldInstructionCache.WaitForEndOfFrame;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Playerrr"))
        {
            if (!collision.GetComponent<PlayerControl>().Unbeatable_Player)
            {
                collision.GetComponent<PlayerControl>().Unbeatable_Player = true;
                collision.GetComponent<PlayerControl>().TakeDamage();
                Instantiate(Die_Explosion, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
    public void OnDie()
    {
        playerControl.Score += ScorePerEnemy;
        Instantiate(Die_Explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    IEnumerator Homming_Player()
    {
        while (true)
        {
            Vector2 direction = (Vector2)(GameObject.FindGameObjectWithTag("Playerrr").transform.position) - rb.position;
            direction.Normalize();

            float rotateAmount = Vector3.Cross(direction, transform.up).z;

            rb.angularVelocity = -rotateAmount * rotateSpeed;

            rb.velocity = transform.up * speed;

            yield return YieldInstructionCache.WaitForEndOfFrame;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
