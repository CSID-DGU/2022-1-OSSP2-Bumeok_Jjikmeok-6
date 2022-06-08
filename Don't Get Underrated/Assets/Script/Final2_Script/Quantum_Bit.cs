using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Quantum_Bit : Enemy_Info
{
    private SolGryn solGryn;

    private List<GameObject> Part;

    public int[,] Rand = new int[2, 8] { { -1, 0, 1, 0, 1, 1, -1, -1 }, { 0, 1, 0, -1, 1, -1, 1, -1 } };

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
        if (GameObject.Find("Flash") && GameObject.Find("Flash").TryGetComponent(out ImageColor IC))
            imageColor = IC;
        Part = new List<GameObject>();
        if (GameObject.FindGameObjectWithTag("Boss").TryGetComponent(out SolGryn SG))
            solGryn = SG;
        if (GameObject.Find("Enemy_Effect_Sound") && GameObject.Find("Enemy_Effect_Sound").TryGetComponent(out AudioSource AS1))
            EffectSource = AS1;
    }

    private void Start()
    {
        float Solve = Mathf.Sign(transform.position.x);

        DOTween.Sequence()
        .Append(transform.DOMove(new Vector3(Solve * 7, 2, 0), 1f).SetEase(Ease.OutBounce))
        .InsertCallback(0, () => Effect_Sound_OneShot(1))
        .Append(transform.DOMove(new Vector3(0, 2, 0), 1f).SetEase(Ease.InExpo))
        .Join(transform.DOScale(new Vector3(1.3f, 1.3f, 0), 1f).SetEase(Ease.InCirc))
         .InsertCallback(1.6f, () => Effect_Sound_OneShot(2))
        .OnComplete(() =>
        {
            My_Color = Color.clear;
            if (Solve >= 0)
                Run_Life_Act(Tr_Co());
        });
    }
    private IEnumerator Tr_Co()
    {
        Effect_Sound_OneShot(0);
        
        Flash(Color.black, 0.5f, 1f);

        for (int i = -8; i <= 8; i++)
        {
            for (int j = -8; j <= 8; j++)
            {
                if ((i >= -2 && i <= 2) && (j <= 0 && j >= -2))
                    continue;

                GameObject e = Instantiate(Weapon[0], new Vector3(1.3f * i, 1.3f * j, 0), Quaternion.identity);
                
                if (e.TryGetComponent(out Weapon_Devil WD))
                {
                    WD.W_MoveSpeed(2);
                    WD.W_MoveTo(Vector3.zero);
                    Part.Add(e);
                }
            }
        }

        yield return Camera_Shake_And_Wait(0.02f, 2f, true, false);
        foreach (var e in Part)
        {
            if (e != null)
            {
                int Ran1 = Random.Range(0, 8);
                if (e.TryGetComponent(out Weapon_Devil WD))
                    WD.W_MoveTo(new Vector3(Rand[0, Ran1], Rand[1, Ran1], 0));
            }
        }

        Part.Clear();
        solGryn.Is_Next_Pattern = true;

        OnDie();
        yield return null;
    }
}
