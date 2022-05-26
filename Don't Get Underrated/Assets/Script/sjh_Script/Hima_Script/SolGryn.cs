using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolGryn : Boss_Info
{

    // <a href='https://kr.freepik.com/vectors/light'>Light 벡터는 upklyak - kr.freepik.com가 제작함</a>

    [SerializeField]
    GameObject SolGryn_HP;

    [SerializeField]
    GameObject PalJeongDo_Thunder;

    [SerializeField]
    GameObject Straw;

    HimaController himaController;

    BackGroundColor PalJeongDo;

    TrailRenderer trailRenderer;

    GameObject Straw_Copy;

    SpriteColor spriteColor;

    List<GameObject> SolG_Copy;

    IEnumerator change_color;

    int[,] bangmeon = new int[2, 4] { { 1, -1, -1, 1 }, { 1, 1, -1, -1 } };

    private bool is_Next_Pattern = false;

    public bool Is_Next_Pattern
    {
        get { return is_Next_Pattern; }
        set { is_Next_Pattern = value; }
    }

    float[,] Pattern04_Move =
    {
        {4.81f, -0.38f },
        { -6.38f, -1.95f },
        { 7.36f, -2.8f },
        { 7.09f, 3.3f },
        { -5.02f, 2.76f }
    };
    private new void Awake()
    {
        base.Awake();
        CurrentHP = MaxHP;
        
        backGroundColor = GameObject.Find("Flash").GetComponent<BackGroundColor>();
        PalJeongDo = GameObject.Find("PalJeongDo").GetComponent<BackGroundColor>();
        himaController = GameObject.FindGameObjectWithTag("Player").GetComponent<HimaController>();
        trailRenderer = GetComponent<TrailRenderer>();
        SolGryn_HP.SetActive(false);
        trailRenderer.enabled = false;

        if (GameObject.Find("Jebal").TryGetComponent(out SpriteColor user))
            spriteColor = user;
        SolG_Copy = new List<GameObject>();
        for (int i = 0; i < 5; i++)
            Pattern_Total.Add(phase);
    }
    public void WelCome()
    {
        StartCoroutine(TeoKisis());
    }
    
    void Launch_SoyBean()
    {
        Instantiate(Weapon[7], transform.position, Quaternion.identity);
    }
    IEnumerator Blink_Bullet()
    {
        transform.localScale = new Vector3(0.7f, 0.7f, 0);  // 임시
        for (int i = 0; i < 5; i++)
        {
            Boss_W1(72 + (20 * i), 25, 360, 4, true);
            yield return YieldInstructionCache.WaitForSeconds(0.8f);
        }
        yield return null;
    }
    IEnumerator TeoKisis()
    {
        yield return StartCoroutine(Position_Lerp(new Vector3(0, 7, 0), new Vector3(0, 0, 0), 7f, declineCurve));

        Start_Camera_Shake(0.01f, 1, false, true);
        yield return StartCoroutine(Change_Color_Return_To_Origin(Color.white, new Color(1, 69 / 255, 69 / 255, 1), 1, false));

        for (int i = 0; i < 4; i++)
            SolG_Copy.Add(Instantiate(Weapon[5], transform.position, Quaternion.identity));

        for (int i = 0; i < 4; i++)
            SolG_Copy[i].GetComponent<SolGryn_Copy>().Move_Lerp_Distance(new Vector3(bangmeon[0, i] * 7, bangmeon[1, i] * 2.5f, 0));

        backGroundColor.StartCoroutine(backGroundColor.Change_Color_Return_To_Origin(backGroundColor.Get_BGColor(), new Color(1, 1, 1, 1), 2, false));
        yield return PalJeongDo.StartCoroutine(PalJeongDo.Change_Color(PalJeongDo.Get_BGColor(), new Color(1, 1, 1, 1), 2));

        yield return PalJeongDo.StartCoroutine(PalJeongDo.Change_Color(PalJeongDo.Get_BGColor(), new Color(1, 1, 1, 0.5f), 2)); // 효과음 넣어야한다 (낮아지는 소리)
        GameObject.Find("Flash").transform.SetAsLastSibling();

        for (int i = -1; i < 2; i++)
            Instantiate(PalJeongDo_Thunder, transform.position + (5 * new Vector3(i, 0, 0)), Quaternion.identity);

        backGroundColor.StartCoroutine(backGroundColor.Change_Color_Return_To_Origin(backGroundColor.Get_BGColor(), new Color(1, 1, 1, 1), 0.5f, false));
        yield return PalJeongDo.StartCoroutine(PalJeongDo.Change_Color(PalJeongDo.Get_BGColor(), new Color(1, 0, 0, 1), 0.5f));

        yield return YieldInstructionCache.WaitForSeconds(1f);

        foreach (var u in SolG_Copy)
            Destroy(u);

        yield return StartCoroutine(Size_Change(transform.localScale, transform.localScale / 2, 0.5f, OriginCurve));

        spriteColor.StartCoroutine(spriteColor.Change_Color(spriteColor.Get_BGColor(), new Color(1, 1, 1, 1), 1));
        yield return StartCoroutine(Size_Change(transform.localScale, transform.localScale * 20, 1f, inclineCurve));

        backGroundColor.Set_BGColor(new Color(1, 1, 1, 0));
        PalJeongDo.Set_BGColor(new Color(1, 1, 1, 0));
        yield return YieldInstructionCache.WaitForSeconds(1.5f); // 이동 후 카메라 정지 + 1.5초 정지

        spriteColor.StartCoroutine(spriteColor.Change_Color(spriteColor.Get_BGColor(), new Color(0, 0, 0, 1), 1));
        yield return YieldInstructionCache.WaitForSeconds(1.5f);  // 검정색 플래시 후 1.5초 정지

        spriteColor.StartCoroutine(spriteColor.Change_Color(spriteColor.Get_BGColor(), new Color(1, 1, 1, 0), 1));
        yield return StartCoroutine(Boss_Pattern());
    }
    void Continue_Camera_Shake()
    {
        Start_Camera_Shake(0.0025f, 1, false, true);
    }
    IEnumerator Boss_Pattern()
    {
        himaController.IsMove = true;
        himaController.Unbeatable = false;
        transform.position = new Vector3(7, 4, 0);
        transform.localScale = new Vector3(0.7f, 0.7f, 0);

        SolGryn_HP.SetActive(true);
        SolGryn_HP.GetComponent<BossHPSliderViewer>().F_HPFull(gameObject.GetComponent<SolGryn>());
        StartCoroutine(HP_Decrease());

        Continue_Camera_Shake();

        yield return StartCoroutine(Position_Slerp(transform.position, new Vector3(-4, 4, 0),
            Get_Center_Vector(transform.position, new Vector3(-4, 4, 0), Vector3.Distance(transform.position, new Vector3(-4, 4, 0)) * 0.85f, "anti_clock"), 4, OriginCurve, false));
        //yield return StartCoroutine(Pattern01());
       // yield return StartCoroutine(Pattern02());
        //yield return StartCoroutine(Pattern03());
        //yield return StartCoroutine(Pattern04());
        yield return StartCoroutine(Pattern05());
        yield return StartCoroutine(Pattern06());
        //while(true)
        //{
        //    yield return StartCoroutine(Change_Color_Lerp(SpriteRenderer_Color, new Color(1, 1, 1, 0), 0.33f, 0, DisAppear_Effect_1));
        //    int Pattern_Num = Random.Range(0, 5);
        //    switch (Pattern_Num)
        //    {
        //        case 0:
        //            Pattern_Total[Pattern_Num] = Pattern01();
        //            break;
        //        case 1:
        //            Pattern_Total[Pattern_Num] = Pattern02();
        //            break;
        //        case 2:
        //            Pattern_Total[Pattern_Num] = Pattern03();
        //            break;
        //        case 3:
        //            Pattern_Total[Pattern_Num] = Pattern04();
        //            break;
        //        case 4:
        //            Pattern_Total[Pattern_Num] = Pattern05();
        //            break;
        //    }
        //    SpriteRenderer_Color = Color.white;
        //    transform.position = new Vector3(7, 4, 0);
        //    yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(7, 0, 0), 1f, inclineCurve));
        //    yield return StartCoroutine((IEnumerator)Pattern_Total[Pattern_Num]);
        //}

        ////yield return StartCoroutine(First_Move());
    }
    IEnumerator Pattern01() // 완료
    {
        float[,] u1 = new float[3, 2] { { -4f, -0.4f }, { 4f, -0.4f }, { 4f, -6f } };
        float[,] u2 = new float[3, 2] { { 0, 2 }, { 0, -3.35f }, { -4, -3.35f } };

        trailRenderer.enabled = true;
        if (TryGetComponent(out TrailCollisions user1))
            user1.Draw_Collision_Line();

        transform.position = new Vector3(-4, 4, 0);
        yield return StartCoroutine(Change_Color_Lerp(SpriteRenderer_Color, new Color(1, 1, 1, 1), 0.33f, 0, DisAppear_Effect_1));

        for (int i = 0; i < 3; i++)
            yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(u1[i, 0], u1[i, 1], 0), 0.15f, declineCurve));

        transform.position = new Vector3(4, 2, 0);
        yield return StartCoroutine(Change_Color_Lerp(new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), 0.5f, 0, DisAppear_Effect_1));

        for (int i = 0; i < 3; i++)
            yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(u2[i, 0], u2[i, 1], 0), 0.15f, declineCurve));

        yield return StartCoroutine(Change_Color_Lerp(SpriteRenderer_Color, new Color(1, 1, 1, 0), 0.4f, 0, DisAppear_Effect_1));
        yield return StartCoroutine(Nachi_Color_Change(Color.red, Color.blue, 0.8f, true));

        GameObject nachi_x_g_1 = Instantiate(Weapon[6], new Vector3(6.3f, 1.87f, 0), Quaternion.identity);
        GameObject nachi_x_g_2 = Instantiate(Weapon[6], new Vector3(-5.61f, 1.64f, 0), Quaternion.identity);

        StartCoroutine(Blink_Bullet());
        nachi_x_g_1.GetComponent<Nachi_X>().StartCoroutine(nachi_x_g_1.GetComponent<Nachi_X>().Move(-1));
        yield return nachi_x_g_1.GetComponent<Nachi_X>().StartCoroutine(nachi_x_g_2.GetComponent<Nachi_X>().Move(1));
        yield return StartCoroutine(Nachi_Color_Change(Color.red, new Color(1, 1, 1, 0), 1f, false));

        trailRenderer.enabled = false;

        if (GameObject.Find("TrailCollider"))
            Destroy(GameObject.Find("TrailCollider"));
    }
    IEnumerator Nachi_Color_Change(Color Origin_C, Color Change_C, float time_persist, bool Is_Continue)
    {
        float percent = 0;
        for (int i = 0; i < 2; i++)
        {
            percent = 0;
            while (percent < 1)
            {
                percent += Time.deltaTime / time_persist;
                trailRenderer.endColor = Color.Lerp(Origin_C, Change_C, percent);
                trailRenderer.startColor = Color.Lerp(Origin_C, Change_C, percent);
                yield return null;
            }
            if (!Is_Continue)
                yield break;
            percent = 0;
            while (percent < 1)
            {
                percent += Time.deltaTime / time_persist;
                trailRenderer.endColor = Color.Lerp(Change_C, Origin_C, percent);
                trailRenderer.startColor = Color.Lerp(Change_C, Origin_C, percent);
                yield return null;
            }
        }
    }
    IEnumerator Pattern02()
    {
        transform.position = new Vector3(-7, 2.5f, 0);
        yield return StartCoroutine(Change_Color_Lerp(SpriteRenderer_Color, new Color(1, 1, 1, 1), 0.33f, 0, DisAppear_Effect_1));

        yield return StartCoroutine(Straw_Launch(new Vector3(-7, -2.5f, 0), Quaternion.Euler(0, 0, -90), new Vector3(-4.5f, -2.5f, 0), Quaternion.Euler(0, 0, -90),
          new Vector3(-3.1f, -2.5f, 0), Quaternion.identity, Vector3.right, 0.3f, 10, 0.2f));

        yield return StartCoroutine(Straw_Launch(new Vector3(7, -2.5f, 0), Quaternion.Euler(0, 0, 90), new Vector3(4.6f, -2.5f, 0), Quaternion.Euler(0, 0, 90),
          new Vector3(3.2f, -2.5f, 0), Quaternion.identity, Vector3.left, 0.3f, 10, 0.2f));

        yield return StartCoroutine(Straw_Launch(new Vector3(-3.11f, 4.3f, 0), Quaternion.Euler(0, 0, 180), new Vector3(-3.11f, 1.92f, 0), Quaternion.identity,
          new Vector3(-3.11f, 0.61f, 0), Quaternion.Euler(0, 0, 90), Vector3.down, 0, 7, 0.1f));

        yield return StartCoroutine(Straw_Launch(new Vector3(0, 4.3f, 0), Quaternion.Euler(0, 0, 180), new Vector3(0, 1.92f, 0), Quaternion.identity,
          new Vector3(0, 0.61f, 0), Quaternion.Euler(0, 0, 90), Vector3.down, 0, 7, 0.1f));

        yield return StartCoroutine(Straw_Launch(new Vector3(3.11f, 4.3f, 0), Quaternion.Euler(0, 0, 180), new Vector3(3.11f, 1.92f, 0), Quaternion.identity,
          new Vector3(3.11f, 0.61f, 0), Quaternion.Euler(0, 0, 90), Vector3.down, 0, 7, 0.1f));

        yield return StartCoroutine(Straw_Launch(new Vector3(-1.5f, 4.3f, 0), Quaternion.Euler(0, 0, 180), new Vector3(-1.5f, 1.92f, 0), Quaternion.identity,
          new Vector3(-1.5f, 0.61f, 0), Quaternion.Euler(0, 0, 90), Vector3.down, 0, 7, 0.1f));

        yield return StartCoroutine(Straw_Launch(new Vector3(1.5f, 4.3f, 0), Quaternion.Euler(0, 0, 180), new Vector3(1.5f, 1.92f, 0), Quaternion.identity,
          new Vector3(1.5f, 0.61f, 0), Quaternion.Euler(0, 0, 90), Vector3.down, 0, 7, 0.1f));

        transform.SetPositionAndRotation(new Vector3(0, 1.35f, 0), Quaternion.identity);
        yield return StartCoroutine(Change_Color_Lerp(SpriteRenderer_Color, new Color(1, 1, 1, 1), 1f, 0, DisAppear_Effect_1));
        yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(transform.position.x, transform.position.y - 1.35f, 0), 0.5f, inclineCurve));

        SolG_Copy = new List<GameObject>();
        for (int i = 0; i < 2; i++)
            SolG_Copy.Add(Instantiate(Weapon[5], transform.position, Quaternion.identity));

        SolGryn_Copy solGryn_Copy_1 = null, solGryn_Copy_2 = null;

        if (SolG_Copy[0].TryGetComponent(out SolGryn_Copy user1))
            solGryn_Copy_1 = user1;
        if (SolG_Copy[1].TryGetComponent(out SolGryn_Copy user2))
            solGryn_Copy_2 = user2;

        if (solGryn_Copy_1 != null && solGryn_Copy_2 != null)
        {
            solGryn_Copy_1.Move_Slerp_Distance(new Vector3(6.5f, 2.68f, 0), "anti_clock");
            solGryn_Copy_2.Move_Slerp_Distance(new Vector3(-6.5f, 2.68f, 0), "clock");

            yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(transform.position.x, 2.68f, 0), 0.75f, declineCurve));

            for (int i = 0; i < 4; i++)
            {
                solGryn_Copy_1.Shake_Act(); solGryn_Copy_2.Shake_Act();
                yield return StartCoroutine(Shake_Act(0.2f, 0.2f, 0.5f, false));

                solGryn_Copy_1.Launch_SoyBean(); solGryn_Copy_2.Launch_SoyBean();
                Launch_SoyBean();
                yield return YieldInstructionCache.WaitForSeconds(0.7f);
            }
        }

        Destroy(SolG_Copy[0]);
        Destroy(SolG_Copy[1]);
        yield return null;
    }

    IEnumerator Straw_Launch(Vector3 SolGryn_Pos, Quaternion SolGryn_Lot, Vector3 Straw_Create_Pos, Quaternion Straw_Lot, Vector3 Peanut_Create_Pos, 
        Quaternion Peanut_Lot, Vector3 Peanut_Dir, float Straw_Lot_time_persist, int Peanut_Launch_Num, float Peanut_Launch_Interval)
    {
        transform.SetPositionAndRotation(SolGryn_Pos, SolGryn_Lot);
      
        change_color = Change_Color_Lerp(new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), Peanut_Launch_Num * Peanut_Launch_Interval + Straw_Lot_time_persist, 0, DisAppear_Effect_1);
        StartCoroutine(change_color);

        Straw_Copy = Instantiate(Straw, Straw_Create_Pos, Quaternion.Euler(0, 0, 0));

        if (Straw_Lot_time_persist != 0)
        {
            float percent = 0;
            while (percent < 1)
            {
                percent += Time.deltaTime / Straw_Lot_time_persist;
                Straw_Copy.transform.rotation = Quaternion.Lerp(Quaternion.Euler(0, 0, 0), Straw_Lot, inclineCurve.Evaluate(percent));
                yield return null;
            }
        }

        backGroundColor.StartCoroutine(backGroundColor.Flash(Color.white, 0.3f, 5));

        for (int i = 0; i < Peanut_Launch_Num; i++)
        {
            Launch_Weapon_For_Move(ref Weapon[1], Peanut_Dir, Peanut_Lot, 15, Peanut_Create_Pos);
            yield return YieldInstructionCache.WaitForSeconds(Peanut_Launch_Interval); // 0.2초는 고정
        }

        StopCoroutine(change_color);
        Destroy(Straw_Copy);
        yield return null;
    }

    IEnumerator Pattern03()
    {
        transform.SetPositionAndRotation(new Vector3(6, 0, 0), Quaternion.identity);

        yield return StartCoroutine(Change_Color_Lerp(SpriteRenderer_Color, new Color(1, 1, 1, 1), 0.33f, 0, DisAppear_Effect_1));
        yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(transform.position.x, transform.position.y - 3, 0), 1, inclineCurve));
        yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(transform.position.x, 11, 0), 1, declineCurve));

        for (int i = -1; i < 2; i++)
        {
            Instantiate(Weapon[4], new Vector3(2.52f * i, -8.31f, 0), Quaternion.identity);
            yield return YieldInstructionCache.WaitForSeconds(1.4f);
        }

        Instantiate(Weapon[3], new Vector3(6.8f, 4.46f, 0), Quaternion.identity);
        yield return YieldInstructionCache.WaitForSeconds(1.5f);

        Instantiate(Weapon[3], new Vector3(-6.8f, 4.46f, 0), Quaternion.identity);
        yield return YieldInstructionCache.WaitForSeconds(1.5f);

        Is_Next_Pattern = false;

        Instantiate(Weapon[3], new Vector3(6.8f, 4.46f, 0), Quaternion.identity);
        Instantiate(Weapon[3], new Vector3(-6.8f, 4.46f, 0), Quaternion.identity);
        
        while (!Is_Next_Pattern)
            yield return null;
        Is_Next_Pattern = false;

        yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(transform.position.x, transform.position.y - 11, 0), 1, declineCurve));
    }
    IEnumerator Pattern04()
    {
        IEnumerator rotate_bullet = Rotate_Bullet(7, 200, 0.02f, 4, Weapon[2]);

        for (int i = 0; i < 5; i++)
        {
            int x = Random.Range(0, 5);
            transform.position = new Vector3(Pattern04_Move[x, 0], Pattern04_Move[x, 1], 0);
           
            yield return Change_Color_Lerp(new Color(1, 1, 1, 0), new Color(1, 1, 1, 0), 0.33f, 1.5f, DisAppear_Effect_2);
            
            StartCoroutine(rotate_bullet);
            Start_Camera_Shake(0.03f, 1.4f, true, false);
            yield return StartCoroutine(Change_Color_Return_To_Origin(new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), 0.7f, false));
            StopCoroutine(rotate_bullet);
        }
        transform.rotation = Quaternion.identity;
        yield return StartCoroutine(Change_Color_Lerp(SpriteRenderer_Color, new Color(1, 1, 1, 1), 1, 0, DisAppear_Effect_1));
        yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(7, 0, 0), 1, declineCurve));
    }

    IEnumerator Pattern05()
    {
        for (int i = 11; i < 14; i++)
        {
            GameObject W1 = Instantiate(Weapon[i], Vector3.zero, Quaternion.identity);
            
            if (i == 13)
            {
                backGroundColor.StartCoroutine(backGroundColor.Flash(new Color(1, 1, 1, 0.7f), 0.3f, 5));
                Start_Camera_Shake(0.03f, 3, true, false);
            }
            else
                Start_Camera_Shake(0.02f, 1, true, false);
            yield return YieldInstructionCache.WaitForSeconds(1f);
            Destroy(W1);
        }

        for (int i = 0; i < 3; i++)
        {
            Instantiate(Weapon[8], new Vector3(0, 2.5f - (2.5f * i), 0), Quaternion.Euler(-90, 0, 0));
            yield return YieldInstructionCache.WaitForSeconds(0.5f);
        }

        Is_Next_Pattern = false;

        Instantiate(Weapon[9], new Vector3(-3, 2, 0), Quaternion.identity);
        Instantiate(Weapon[9], new Vector3(3, 2, 0), Quaternion.identity);

        while(!Is_Next_Pattern)
            yield return null;

        Is_Next_Pattern = false;
        yield return StartCoroutine(Position_Lerp(new Vector3(7, 8, 0), new Vector3(7, 0, 0), 5, declineCurve));
    }

    IEnumerator Pattern06()
    {
        Is_Next_Pattern = false;

        for (int i = 0; i < 4; i++)
        {
            GameObject e = Instantiate(Weapon[10], Vector3.zero, Quaternion.identity);
            e.GetComponent<Monster>().Start_F(new Vector3(bangmeon[0, i] * 7, bangmeon[1, i] * 4, 0), Vector3.zero);
            yield return YieldInstructionCache.WaitForSeconds(1.5f);
        }

        while (!Is_Next_Pattern)
            yield return null;

        Is_Next_Pattern = false;
        yield return null;
    }

    IEnumerator HP_Decrease()
    {
        while(true)
        {
            CurrentHP -= 10;
            if (CurrentHP <= 10)
            {
                OnDie();
                yield break;
            }
            yield return new WaitForSeconds(1f);
        }
    }
    public override void OnDie()
    {
        backGroundColor.Stop_Coroutine();
        GameObject[] e = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] f = GameObject.FindGameObjectsWithTag("Weapon_Devil");
        GameObject[] g = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] h = GameObject.FindGameObjectsWithTag("Item");
        foreach (var u in e)
            Destroy(u);
        foreach (var u in f)
            Destroy(u);
        foreach (var u in g)
            Destroy(u);
        foreach (var u in h)
            Destroy(u);
        StartCoroutine(I_OnDie());
    }
    IEnumerator I_OnDie()
    {
        yield return StartCoroutine(Position_Lerp(new Vector3(7, 5, 0), new Vector3(7, 0, 0), 3, OriginCurve));
        yield return null;
    }
    void Boss_W1(float start_angle, int count, float range_angle, float speed, bool is_Blink)
    {
        float count_per_radian = range_angle / count;
        float intervalAngle = start_angle;
        for (int i = 0; i < count; i++)
        {
            float angle = intervalAngle + (i * count_per_radian);
            float x = Mathf.Cos(angle * Mathf.PI / 180.0f);
            float y = Mathf.Sin(angle * Mathf.PI / 180.0f);
            Launch_Weapon_For_Move(ref Weapon[0], new Vector3(x, y), Quaternion.identity, speed, 4 * Vector3.up);
        }
    }
}