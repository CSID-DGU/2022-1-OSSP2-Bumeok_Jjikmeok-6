using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F1_Homming_Enemy : Enemy_Info
{

    [SerializeField]
    int ScorePerEnemy = 50;

    PlayerCtrl_Tengai player_tengai;

    Rigidbody2D rb;

    [SerializeField]
    float speed = 5;

    [SerializeField]
    public float rotateSpeed = 200f;

    private new void Awake()
    {
        base.Awake();
        player_tengai = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl_Tengai>();
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        StartCoroutine(Auto_Dead());
        StartCoroutine(Homming_Player());
    }
    IEnumerator Auto_Dead()
    {
        yield return YieldInstructionCache.WaitForSeconds(3.5f);
        OnDie();
        yield return null;
    }
    public override void OnDie()
    {
        player_tengai.Final_Score += ScorePerEnemy;
        Instantiate(When_Dead_Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    IEnumerator Homming_Player()
    {
        while (true)
        {
            Vector2 direction = (Vector2)(GameObject.FindGameObjectWithTag("Player").transform.position) - rb.position;
            direction.Normalize();

            float rotateAmount = Vector3.Cross(direction, transform.up).z;

            rb.angularVelocity = -rotateAmount * rotateSpeed;

            rb.velocity = transform.up * speed;

            yield return YieldInstructionCache.WaitForEndOfFrame;
        }
    }

    // Update is called once per frame

}
