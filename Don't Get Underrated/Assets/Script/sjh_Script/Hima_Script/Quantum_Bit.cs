using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Quantum_Bit : Enemy_Info
{
    private SolGryn solGryn;

    private List<GameObject> Part;

    private int[,] Rand = new int[2, 8] { { -1, 0, 1, 0, 1, 1, -1, -1 }, { 0, 1, 0, -1, 1, -1, 1, -1 } };

    private new void Awake()
    {
        base.Awake();
        backGroundColor = GameObject.Find("Flash").GetComponent<ImageColor>();
        Part = new List<GameObject>();
        if (GameObject.FindGameObjectWithTag("Boss").TryGetComponent(out SolGryn SG))
            solGryn = SG;
    }
    // Start is called before the first frame update
    private void Start()
    {
        float Solve = Mathf.Sign(transform.position.x);

        DOTween.Sequence()
        .Append(transform.DOMove(new Vector3(Solve * 7, 2, 0), 1f).SetEase(Ease.OutBounce))
        .Append(transform.DOMove(new Vector3(0, 2, 0), 1f).SetEase(Ease.InExpo))
        .Join(transform.DOScale(new Vector3(1.3f, 1.3f, 0), 1f).SetEase(Ease.InCirc))
        .OnComplete(() =>
        {
            if (Solve >= 0)
                Run_Life_Act(Tr_Co());
            else
                spriteRenderer.color = new Color(1, 1, 1, 0);
        });
    }
    private IEnumerator Tr_Co()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0);
        Flash(Color.black, 0.5f, 1f);
        for (int i = -9; i <= 9; i++)
        {
            for (int j = -9; j <= 9; j++)
            {
                GameObject e = Instantiate(Weapon[0], new Vector3(i, j, 0), Quaternion.identity);
                
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
            Debug.Log(e);
            if (e != null)
            {
                int Ran1 = Random.Range(0, 8);
                if (e.TryGetComponent(out Weapon_Devil WD))
                    WD.W_MoveTo(new Vector3(Rand[0, Ran1], Rand[1, Ran1], 0));
            }
           
        }
        Part.Clear();
        solGryn.Is_Next_Pattern = true;
        yield return null;
    }
}
