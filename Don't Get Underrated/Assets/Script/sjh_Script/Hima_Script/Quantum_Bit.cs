using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Quantum_Bit : Enemy_Info
{
    SolGryn solGryn;

    ArrayList arrayList;

    int[,] Rand = new int[2, 8] { { -1, 0, 1, 0, 1, 1, -1, -1 }, { 0, 1, 0, -1, 1, -1, 1, -1 } };

    private new void Awake()
    {
        base.Awake();
        cameraShake.mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        backGroundColor = GameObject.Find("Flash").GetComponent<BackGroundColor>();
        arrayList = new ArrayList();

        if (GameObject.FindGameObjectWithTag("Boss").TryGetComponent(out SolGryn user))
            solGryn = user;
    }
    // Start is called before the first frame update
    void Start()
    {
        float Solve = Mathf.Sign(transform.position.x);
        Sequence mysequence = DOTween.Sequence();
        mysequence.Append(transform.DOMove(new Vector3(Solve * 7, 2, 0), 1f).SetEase(Ease.OutBounce));
        mysequence.Append(transform.DOMove(new Vector3(0, 2, 0), 1f).SetEase(Ease.InExpo));
        mysequence.Join(transform.DOScale(new Vector3(1.3f, 1.3f, 0), 1f).SetEase(Ease.InCirc));
        mysequence.OnComplete(() =>
        {
            if (Solve >= 0)
                StartCoroutine(Tr_Co());
            else
                spriteRenderer.color = new Color(1, 1, 1, 0);
        });
   
    }
    IEnumerator Tr_Co()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0);
        backGroundColor.StartCoroutine(backGroundColor.Flash(Color.black, 0.5f, 2));
        cameraShake.StartCoroutine(cameraShake.Shake_Act(0.3f, 0.3f, 0.3f, false));
        yield return null;

        for (int i = -9; i <= 9; i++)
        {
            for (int j = -9; j <= 9; j++)
            {
                //GameObject f = ObjectPooler.SpawnFromPool("Time_Stop_For_Damage", new Vector3(i, j, 0));
                GameObject e = Instantiate(Weapon[0], new Vector3(i, j, 0), Quaternion.identity);
                if (e.TryGetComponent(out Weapon_Devil user))
                {
                    user.W_MoveSpeed(2);
                    user.W_MoveTo(Vector3.zero);
                }
                arrayList.Add(e);
            }
        }
        cameraShake.Origin_Camera();
        yield return cameraShake.StartCoroutine(cameraShake.Shake_Act(0.1f, 0.1f, 2f, false));
        foreach (var e in arrayList)
        {
            int Ran1 = Random.Range(0, 8);
            GameObject u = (GameObject)e;
            if (u != null && u.TryGetComponent(out Weapon_Devil user))
                user.W_MoveTo(new Vector3(Rand[0, Ran1], Rand[1, Ran1], 0));
        }
        arrayList.Clear();
        solGryn.Is_Next_Pattern = true;
        yield return null;
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }

}
