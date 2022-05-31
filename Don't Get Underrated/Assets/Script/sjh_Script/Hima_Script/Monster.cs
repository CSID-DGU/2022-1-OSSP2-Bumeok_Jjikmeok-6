using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Monster : Enemy_Info
{
    private Animator animator;

    private Vector3 Monster_Pos;

    private int CHK_Flag;

    private HimaController himaController;

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
    private new void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        CHK_Flag = 0;
        if (GameObject.FindGameObjectWithTag("Player") && GameObject.FindGameObjectWithTag("Player").TryGetComponent(out HimaController HC))
            himaController = HC;
    }
    public void Start_F(Vector3 Monster_Pos, int Flag)
    {
        this.Monster_Pos = Monster_Pos;
        CHK_Flag = Flag;
        transform.DOMove(Monster_Pos, 0.5f).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            animator.SetTrigger("hehe");
        });
    }
    public void OnLazor() // 애니메이션 진행 도중 해당 함수 호출 (즉, 참조가 0개여도 지우면 안됨)
    {
        Run_Life_Act(Monster_Only_Lazor(Monster_Pos, 1));
    }

    private IEnumerator Monster_Only_Lazor(Vector3 Origin, float time_persist)
    {
        float percent = 0;
        float inverse_time_persist = StaticFunc.Reverse_Time(time_persist);
        Vector3 Target;
        if (CHK_Flag == 0)
            Target = Vector3.zero;
        else
            Target = himaController.transform.position;
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
