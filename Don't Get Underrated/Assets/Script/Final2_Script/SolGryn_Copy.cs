using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolGryn_Copy : Enemy_Info
{
    private new void Awake()
    {
        base.Awake();
    }
    private new void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Player") && collision.TryGetComponent(out Player_Info HC))
        {
            if (!HC.Unbeatable)
                HC.TakeDamage(1);
        }
    }
    private new void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject != null && collision.gameObject.CompareTag("Player") && collision.gameObject.TryGetComponent(out Player_Info HC))
        {
            if (!HC.Unbeatable)
                HC.TakeDamage(1);
        }
    }
    public void Move_Straight(Vector3 late_vector)
    {
        Run_Life_Act(Move_Straight(transform.position, late_vector, 0.15f, OriginCurve));        
    }

    public void Move_Slerp_Distance(Vector3 Target, string dir)
    {
       Run_Life_Act(Move_Curve(transform.position, Target,
              Get_Center_Vector_For_Curve_Move(transform.position, Target, Vector3.Distance(transform.position, Target) * 0.85f, dir), 1, declineCurve));
    }
    public void Shake_Act()
    {
        Run_Life_Act(Shake_Act(0.2f, 0.2f, 0.5f, false));
    }

    public void Launch_SoyBean()
    {
        Instantiate(Weapon[0], transform.position, Quaternion.identity);
    }
    // Update is called once per frame
}
