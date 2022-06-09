using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Monster : Enemy_Info
{
    private Animator animator;

    private Vector3 Monster_Pos;

    private int CHK_Flag;

    private Player_Final2 himaController;

    private Vector3 hima_Pos;

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
        if (GameObject.FindGameObjectWithTag("Player") && GameObject.FindGameObjectWithTag("Player").TryGetComponent(out Player_Final2 HC))
            himaController = HC;
        if (GameObject.Find("Enemy_Effect_Sound") && GameObject.Find("Enemy_Effect_Sound").TryGetComponent(out AudioSource AS1))
            EffectSource = AS1;
    }
    public void Start_Attack(Vector3 Monster_Pos, int Flag)
    {
        this.Monster_Pos = Monster_Pos;

        CHK_Flag = Flag;
        Effect_Sound_OneShot(0);
        DOTween.Sequence()
          .Append(transform.DOMove(Monster_Pos, 0.5f).SetEase(Ease.OutBounce))
          .OnComplete(() =>
          {
              hima_Pos = himaController.transform.position;
              animator.SetTrigger("hehe");
          });
    }
    public void OnLazor() // 애니메이션 진행 도중 해당 함수 호출 (즉, 참조가 0개여도 지우면 안됨)
    {
        Run_Life_Act(Monster_Only_Lazor(Monster_Pos, 0.5f));
    }

    private IEnumerator Monster_Only_Lazor(Vector3 Origin, float time_persist)
    {
        Effect_Sound_OneShot(1);
        float percent = 0;
        float inverse_time_persist = StaticFunc.Reverse_Time(time_persist);

        Vector3 Target;
        if (CHK_Flag == 0)
            Target = Vector3.zero;
        else
            Target = hima_Pos;

        while (percent < 1)
        {
            percent += Time.deltaTime * inverse_time_persist;
            Launch_Weapon(ref Weapon[0], Target - Origin, Quaternion.identity, 110, Origin);
            yield return null;
        }
        animator.SetTrigger("die");
        yield return null;
    }


    private new void OnDestroy()
    {
        DOTween.KillAll();
        base.OnDestroy();
    }

    public override void OnDie()
    {
        base.OnDie();
    }
}
