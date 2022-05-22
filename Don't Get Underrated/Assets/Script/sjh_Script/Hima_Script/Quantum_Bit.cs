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
        Sequence mysequence = DOTween.Sequence();
        mysequence.Append(transform.DOMove(new Vector3(Mathf.Sign(transform.position.x) * 7, 2, 0), 1f).SetEase(Ease.OutBounce));
        mysequence.Append(transform.DOMove(new Vector3(0, 2, 0), 1f).SetEase(Ease.InExpo));
        mysequence.Join(transform.DOScale(new Vector3(1.3f, 1.3f, 0), 1f).SetEase(Ease.Linear));
        mysequence.OnComplete(() =>
        {
            StartCoroutine(Tr_Co());
        });
   
    }
    IEnumerator Tr_Co()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0);
        StartCoroutine(backGroundColor.Flash(Color.black, 0.5f, 2));
        StartCoroutine(cameraShake.Shake_Act(0.3f, 0.3f, 0.3f, false));

        Weapon[0].GetComponent<Movement2D>().MoveTo(Vector3.zero);
        Weapon[0].GetComponent<Movement2D>().MoveSpeed = 2;

        for (int i = -9; i <= 9; i++)
        {
            for (int j = -9; j <= 9; j++)
            {
                GameObject e = Instantiate(Weapon[0], new Vector3(i, j, 0), Quaternion.identity);
                arrayList.Add(e);
            }
        }
        yield return YieldInstructionCache.WaitForSeconds(2f);
        foreach (var e in arrayList)
        {
            int Ran1 = Random.Range(0, 8);
            GameObject u = (GameObject)e;
            if (u != null)
                u.GetComponent<Movement2D>().MoveTo(new Vector3(Rand[0, Ran1], Rand[1, Ran1], 0));
        }
        arrayList.Clear();
        solGryn.Is_Next_Pattern = true;
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
