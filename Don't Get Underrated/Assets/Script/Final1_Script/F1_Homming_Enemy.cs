using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F1_Homming_Enemy : Enemy_Info
{ 
    [SerializeField]
    float speed = 5;

    [SerializeField]
    public float rotateSpeed = 200f;

    Player_Final1 player_final1;

    Rigidbody2D rb;

    private new void Awake()
    {
        base.Awake();
        if (GameObject.FindGameObjectWithTag("Player") && GameObject.FindGameObjectWithTag("Player").TryGetComponent(out Player_Final1 PF1))
            player_final1 = PF1;
        if (TryGetComponent(out Rigidbody2D RB2D))
            rb = RB2D;
    }
    void Start()
    {
        Run_Life_Act(Auto_Dead());
        Run_Life_Act(Homming_Player());
    }
    IEnumerator Auto_Dead()
    {
        yield return YieldInstructionCache.WaitForSeconds(3.5f);
        OnDie();
        yield return null;
    }
    public override void OnDie()
    {
        base.OnDie();
    }
    IEnumerator Homming_Player()
    {
        if (player_final1 != null && rb != null)
        {
            while (true)
            {
                Vector2 direction = (Vector2)(GameObject.FindGameObjectWithTag("Player").transform.position) - rb.position;

                direction.Normalize();

                float rotateAmount = Vector3.Cross(direction, transform.up).z;

                rb.angularVelocity = -rotateAmount * rotateSpeed;

                rb.velocity = transform.up * speed;

                yield return null;
            }
        }
        else
            yield return null;
    }
}
