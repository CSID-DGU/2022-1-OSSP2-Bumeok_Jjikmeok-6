using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Monster : Enemy_Info
{
    Animator animator;

    Vector3 Monster_Pos, Target_Pos;
    private new void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }
    public void Start_F(Vector3 Monster_Pos, Vector3 Target_Pos)
    {
        this.Monster_Pos = Monster_Pos;
        this.Target_Pos = Target_Pos;
        transform.DOMove(Monster_Pos, 0.5f).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            animator.SetTrigger("hehe");
        });
    }
    public void OnLazor()
    {
        Run_Life_Act(Monster_Only_Lazor(Monster_Pos, Target_Pos, 1));
    }

    IEnumerator Monster_Only_Lazor(Vector3 Origin, Vector3 Target, float time_persist)
    {
        float percent = 0;
        float inverse_time_persist = StaticFunc.Reverse_Time(time_persist);
        while (percent < 1)
        {
            percent += Time.deltaTime * inverse_time_persist;
            Launch_Weapon(ref Weapon[0], Target - Origin, Quaternion.identity, 14, Origin);
            yield return null;
        }
        animator.SetTrigger("die");
        yield return null;
    }

    public override void OnDie()
    {
        GameObject.FindGameObjectWithTag("Boss").GetComponent<SolGryn>().Is_Next_Pattern = true;
        Destroy(gameObject);
    }
}
