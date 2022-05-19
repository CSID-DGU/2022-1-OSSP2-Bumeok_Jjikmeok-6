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

    List<GameObject> SolG_Copy;

    [SerializeField]
    GameObject Time_Out_For_Damage;

    IEnumerator change_color;

    IEnumerator nachi_x_i_1, nachi_x_i_2;

    float[,] move_random =
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
        PalJeongDo.Set_BGColor(new Color(1, 1, 1, 0));
        himaController = GameObject.FindGameObjectWithTag("Player").GetComponent<HimaController>();
        trailRenderer = GetComponent<TrailRenderer>();
        SolGryn_HP.SetActive(false);
        trailRenderer.enabled = false;
        trailRenderer = GetComponent<TrailRenderer>();
        SolG_Copy = new List<GameObject>();
        transform.position = new Vector3(0, -15.81f, 0);
        transform.localScale = new Vector3(1.5f, 1.5f, 0);
        for (int i = 0; i < 5; i++)
            Pattern_Total.Add(phase);
    }
    public void OnTriggerEnter2D(Collider2D collision) // 얘만
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("야");
            collision.GetComponent<HimaController>().TakeDamage(1);
        }
    }
    public void WelCome()
    {
       // StartCoroutine(Blink_Bullet());

       // StartCoroutine(Straw_Start());

       // StartCoroutine(Time_Out());
        
        // 이 쪽은 아님

        
        // StartCoroutine(I_WelCome());
        //StartCoroutine(Boss_Pattern());
        StartCoroutine(TeoKisis());
        // 여긴 진짜 작업공간
    }
    
    void Launch_SoyBean()
    {
        if (GameObject.FindGameObjectWithTag("Player"))
            Instantiate(Weapon[7], transform.position, Quaternion.identity);
    }
    IEnumerator Blink_Bullet()
    {
        transform.position = new Vector3(-7, 5, 0);
        transform.localScale = new Vector3(1.5f, 1.5f, 0);  // 임시
        for (int i = 0; i < 10; i++)
        {
            StartCoroutine(Boss_W1(72 + (20 * i), 25, 360, 2, true));
            yield return YieldInstructionCache.WaitForSeconds(0.5f);
        }
        yield return null;
    }
    IEnumerator TeoKisis()
    {
        yield return StartCoroutine(Position_Lerp(new Vector3(0, 7, 0), new Vector3(0, 0, 0), 7f, declineCurve));
        yield return YieldInstructionCache.WaitForSeconds(1f);

        camera_shake = cameraShake.Shake_Act(.01f, .01f, 1, true);
        StartCoroutine(camera_shake); // 등장할 때 한번 흔들어 재껴줘야함
        yield return StartCoroutine(Change_Color_Return_To_Origin(Color.white, new Color(1, 69 / 255, 69 / 255, 1), 1, false));
        StopCoroutine(camera_shake);

        for (int i = 0; i < 4; i++)
            SolG_Copy.Add(Instantiate(Weapon[5], transform.position, Quaternion.identity));

        SolG_Copy[0].GetComponent<SolGryn_Copy>().Move_Lerp_Distance(new Vector3(7, 2.5f, 0));
        SolG_Copy[1].GetComponent<SolGryn_Copy>().Move_Lerp_Distance(new Vector3(7, -2.5f, 0));
        SolG_Copy[2].GetComponent<SolGryn_Copy>().Move_Lerp_Distance(new Vector3(-7, 2.5f, 0));
        SolG_Copy[3].GetComponent<SolGryn_Copy>().Move_Lerp_Distance(new Vector3(-7, -2.5f, 0));

        yield return YieldInstructionCache.WaitForSeconds(2f);

        yield return StartCoroutine(backGroundColor.Change_Color(backGroundColor.Get_BGColor(), new Color(1, 1, 1, 1), 1));
        yield return StartCoroutine(PalJeongDo.Change_Color(PalJeongDo.Get_BGColor(), new Color(1, 1, 1, 1), 1));
        yield return StartCoroutine(backGroundColor.Change_Color(backGroundColor.Get_BGColor(), new Color(1, 1, 1, 0), 2));

        yield return YieldInstructionCache.WaitForSeconds(1f);

        yield return StartCoroutine(PalJeongDo.Change_Color(PalJeongDo.Get_BGColor(), new Color(1, 1, 1, 0.5f), 2)); // 효과음 넣어야한다 (낮아지는 소리)
        GameObject.Find("Flash").transform.SetAsLastSibling();

        Instantiate(PalJeongDo_Thunder, transform.position, Quaternion.identity);
        Instantiate(PalJeongDo_Thunder, transform.position + 5 * Vector3.left, Quaternion.identity);
        Instantiate(PalJeongDo_Thunder, transform.position + 5 * Vector3.right, Quaternion.identity);
        yield return StartCoroutine(backGroundColor.Change_Color(backGroundColor.Get_BGColor(), new Color(1, 1, 1, 1), 0.2f));
        StartCoroutine(PalJeongDo.Change_Color(PalJeongDo.Get_BGColor(), new Color(1, 0, 0, 1), 0.5f));
        yield return StartCoroutine(backGroundColor.Change_Color(backGroundColor.Get_BGColor(), new Color(1, 1, 1, 0), 1));

        yield return YieldInstructionCache.WaitForSeconds(1f);

        foreach (var u in SolG_Copy)
            Destroy(u);

        yield return StartCoroutine(Size_Change(transform.localScale, transform.localScale / 2, 0.5f, OriginCurve));
        StartCoroutine(backGroundColor.Change_Color(backGroundColor.Get_BGColor(), new Color(1, 1, 1, 1), 1));
        yield return StartCoroutine(Size_Change(transform.localScale, transform.localScale * 20, 1f, inclineCurve));

        yield return YieldInstructionCache.WaitForSeconds(1.5f); // 이동 후 카메라 정지 + 1.5초 정지

        StartCoroutine(PalJeongDo.Change_Color(PalJeongDo.Get_BGColor(), new Color(1, 1, 1, 0), 0.5f));
        yield return StartCoroutine(backGroundColor.Change_Color(backGroundColor.Get_BGColor(), new Color(0, 0, 0, 1), 1));
        Debug.Log(PalJeongDo.Get_BGColor());

        yield return YieldInstructionCache.WaitForSeconds(1.5f);  // 검정색 플래시 후 1.5초 정지

        StartCoroutine(backGroundColor.Change_Color(backGroundColor.Get_BGColor(), new Color(1, 1, 1, 0), 1));
        yield return StartCoroutine(Boss_Pattern());

    }
    IEnumerator Boss_Pattern()
    {
        yield return StartCoroutine(Pattern01());
        //himaController.IsMove = true;
        //transform.position = new Vector3(7, 4, 0);
        //transform.localScale = new Vector3(1.4f, 1.4f, 0);
        //yield return YieldInstructionCache.WaitForSeconds(.5f);

        //SolGryn_HP.SetActive(true);
        //SolGryn_HP.GetComponent<BossHPSliderViewer>().F_HPFull(gameObject.GetComponent<SolGryn>());
        //StartCoroutine(HP_Decrease());

        //camera_shake = cameraShake.Shake_Act(.01f, .01f, 1, true);
        //StartCoroutine(camera_shake);

        //yield return StartCoroutine(Position_Slerp_Temp(transform.position, new Vector3(-4, 4, 0),
        //    Get_Center_Vector(transform.position, new Vector3(-4, 4, 0), Vector3.Distance(transform.position, new Vector3(-4, 4, 0)) * 0.85f, "anti_clock"), 4, OriginCurve, false));

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
        //    yield return StartCoroutine((IEnumerator)Pattern_Total[Pattern_Num]);
        //}
       
        ////yield return StartCoroutine(First_Move());

        ////yield return StartCoroutine(Pattern_1());
        ////yield return StartCoroutine(Pattern_2());
        //yield return StartCoroutine(Pattern_3());
        //yield return StartCoroutine(Pattern_4());
    }
    IEnumerator Pattern01()
    {
        transform.position = new Vector3(-4, 4, 0);
        yield return StartCoroutine(Change_Color_Lerp(SpriteRenderer_Color, new Color(1, 1, 1, 1), 0.33f, 0, DisAppear_Effect_1));


        trailRenderer.enabled = true;
        yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(-4f, -0.4f, 0), 0.1f, declineCurve));
        yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(4f, -0.4f, 0), 0.1f, declineCurve));
        yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(4f, -6f, 0), 0.1f, declineCurve));

        transform.position = new Vector3(4, 2, 0);
        yield return StartCoroutine(Change_Color_Lerp(new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), 0.5f, 0, DisAppear_Effect_1));

        yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(0, 2, 0), 0.1f, declineCurve));
        yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(0, -3.35f, 0), 0.1f, declineCurve));
        yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(-4f, -3.35f, 0), 0.1f, declineCurve));
        yield return StartCoroutine(Change_Color_Lerp(SpriteRenderer_Color, new Color(1, 1, 1, 0), 0.33f, 0, DisAppear_Effect_1));
        yield return StartCoroutine(Nachi_Color_Change(Color.red, Color.blue, 0.5f, true));


        GameObject nachi_x_g_1 = Instantiate(Weapon[6], new Vector3(6.3f, 1.87f, 0), Quaternion.identity);
        GameObject nachi_x_g_2 = Instantiate(Weapon[6], new Vector3(-5.61f, 1.64f, 0), Quaternion.identity);

        nachi_x_i_1 = nachi_x_g_1.GetComponent<Nachi_X>().Move(-1);
        nachi_x_i_2 = nachi_x_g_2.GetComponent<Nachi_X>().Move(1);

        StartCoroutine(Blink_Bullet());
        StartCoroutine(nachi_x_i_1);
        yield return StartCoroutine(nachi_x_i_2);
        StopCoroutine(nachi_x_i_1);
        
        //yield return StartCoroutine(Nachi_Color_Change(Color.red, new Color(1, 0, 0, 0), 1, false));

        yield return null;
    }
    IEnumerator Nachi_Color_Change(Color Origin_C, Color Change_C, float time_persist, bool is_Continue)
    {
        Debug.Log("왜 바뀜?");
        for (int i = 0; i < 3; i++)
        {
            percent = 0;
            while (percent < 1)
            {
                percent += Time.deltaTime / time_persist;
                trailRenderer.endColor = Color.Lerp(Origin_C, Change_C, percent);
                trailRenderer.startColor = Color.Lerp(Origin_C, Change_C, percent);
                yield return null;
            }
            if (!is_Continue)
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
          new Vector3(-3.1f, -2.5f, 0), Quaternion.Euler(0, 0, 0), Vector3.right, 0.3f, 10, 0.2f));

        yield return StartCoroutine(Straw_Launch(new Vector3(7, -2.5f, 0), Quaternion.Euler(0, 0, 90), new Vector3(4.6f, -2.5f, 0), Quaternion.Euler(0, 0, 90),
          new Vector3(3.2f, -2.5f, 0), Quaternion.Euler(0, 0, 0), Vector3.left, 0.3f, 10, 0.2f));

        yield return StartCoroutine(Straw_Launch(new Vector3(-3.11f, 4.3f, 0), Quaternion.Euler(0, 0, 180), new Vector3(-3.11f, 1.92f, 0), Quaternion.Euler(0, 0, 0),
          new Vector3(-3.11f, 0.61f, 0), Quaternion.Euler(0, 0, 90), Vector3.down, 0, 7, 0.1f));

        yield return StartCoroutine(Straw_Launch(new Vector3(0, 4.3f, 0), Quaternion.Euler(0, 0, 180), new Vector3(0, 1.92f, 0), Quaternion.Euler(0, 0, 0),
          new Vector3(0, 0.61f, 0), Quaternion.Euler(0, 0, 90), Vector3.down, 0, 7, 0.1f));

        yield return StartCoroutine(Straw_Launch(new Vector3(3.11f, 4.3f, 0), Quaternion.Euler(0, 0, 180), new Vector3(3.11f, 1.92f, 0), Quaternion.Euler(0, 0, 0),
          new Vector3(3.11f, 0.61f, 0), Quaternion.Euler(0, 0, 90), Vector3.down, 0, 7, 0.1f));

        yield return StartCoroutine(Straw_Launch(new Vector3(-1.5f, 4.3f, 0), Quaternion.Euler(0, 0, 180), new Vector3(-1.5f, 1.92f, 0), Quaternion.Euler(0, 0, 0),
          new Vector3(-1.5f, 0.61f, 0), Quaternion.Euler(0, 0, 90), Vector3.down, 0, 7, 0.1f));

        yield return StartCoroutine(Straw_Launch(new Vector3(1.5f, 4.3f, 0), Quaternion.Euler(0, 0, 180), new Vector3(1.5f, 1.92f, 0), Quaternion.Euler(0, 0, 0),
          new Vector3(1.5f, 0.61f, 0), Quaternion.Euler(0, 0, 90), Vector3.down, 0, 7, 0.1f));


        transform.position = new Vector3(0, 1.35f, 0);
        transform.rotation = Quaternion.identity;
        change_color = Change_Color_Lerp(SpriteRenderer_Color, new Color(1, 1, 1, 1), 1f, 0, DisAppear_Effect_1);
        yield return StartCoroutine(change_color);
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
                solGryn_Copy_1.Shake_ItSelf();
                solGryn_Copy_2.Shake_ItSelf();
                yield return StartCoroutine(Shake_Act(0.2f, 0.2f, 0.5f, false));

                solGryn_Copy_1.Launch_SoyBean();
                solGryn_Copy_2.Launch_SoyBean();
                Launch_SoyBean();
                yield return YieldInstructionCache.WaitForSeconds(0.7f);
            }
        }

        yield return null;
    }

    IEnumerator Straw_Launch(Vector3 SolGryn_Pos, Quaternion SolGryn_Lot, Vector3 Straw_Create_Pos, Quaternion Straw_Lot, Vector3 Peanut_Create_Pos, Quaternion Peanut_Lot, Vector3 Peanut_Dir, float Straw_Lot_time_persist,
        int Peanut_Launch_Num, float Peanut_Launch_Interval)
    {
        transform.position = SolGryn_Pos;
        transform.rotation = SolGryn_Lot;
        Straw_Copy = Instantiate(Straw, Straw_Create_Pos, Quaternion.Euler(0, 0, 0));

        change_color = Change_Color_Lerp(new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), Peanut_Launch_Num * Peanut_Launch_Interval + Straw_Lot_time_persist, 0, DisAppear_Effect_1);
        StartCoroutine(change_color);

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

        StartCoroutine(backGroundColor.Change_Color_Return_To_Origin(backGroundColor.Get_BGColor(), new Color(1, 1, 1, 1), 0.2f, false));

        for (int i = 0; i < Peanut_Launch_Num; i++)
        {
            camera_shake = cameraShake.Shake_Act(.03f, .03f, 0.1f, false);
            StartCoroutine(camera_shake);
            Launch_Weapon_For_Move_Blink(Weapon[1], Peanut_Dir, Peanut_Lot, 15, false, Peanut_Create_Pos);
            yield return YieldInstructionCache.WaitForSeconds(Peanut_Launch_Interval); // 0.2초는 고정
        }

        StopCoroutine(change_color);
        Destroy(Straw_Copy);
        yield return null;
    }

    IEnumerator Pattern03()
    {
        transform.position = new Vector3(6, 0, 0);
        yield return StartCoroutine(Change_Color_Lerp(SpriteRenderer_Color, new Color(1, 1, 1, 1), 0.33f, 0, DisAppear_Effect_1));

        transform.rotation = Quaternion.Euler(0, 0, 0);
        cameraShake.Shake_Act(.9f, .6f, 1, false);

        yield return YieldInstructionCache.WaitForSeconds(0.5f);
        yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(transform.position.x, transform.position.y - 3, 0), 1, inclineCurve));
        yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(transform.position.x, 11, 0), 1, declineCurve));

        Instantiate(Weapon[4], new Vector3(2.52f, -8.31f, 0), Quaternion.identity);
        yield return YieldInstructionCache.WaitForSeconds(1.4f);

        Instantiate(Weapon[4], new Vector3(0, -8.31f, 0), Quaternion.identity);
        yield return YieldInstructionCache.WaitForSeconds(1.4f);

        Instantiate(Weapon[4], new Vector3(-2.52f, -8.31f, 0), Quaternion.identity);
        yield return YieldInstructionCache.WaitForSeconds(1);

        GameObject W1 = Instantiate(Weapon[3], transform.position, Quaternion.identity);
        W1.GetComponent<DynaBlade>().Dyna_Start(false);
        yield return YieldInstructionCache.WaitForSeconds(1.5f);

        cameraShake.mainCamera.transform.position = new Vector3(0, 0, -10);
        cameraShake.mainCamera.transform.rotation = Quaternion.identity;
        cameraShake.mainCamera.transform.localScale = new Vector3(1, 1, 1);
        yield return null;
        GameObject W2 = Instantiate(Weapon[3], transform.position, Quaternion.identity);
        W2.GetComponent<DynaBlade>().Dyna_Start(true);
        yield return YieldInstructionCache.WaitForSeconds(1.5f);

        cameraShake.mainCamera.transform.position = new Vector3(0, 0, -10);
        cameraShake.mainCamera.transform.rotation = Quaternion.identity;
        cameraShake.mainCamera.transform.localScale = new Vector3(1, 1, 1);
        yield return null;
        GameObject W3 = Instantiate(Weapon[3], transform.position, Quaternion.identity);
        GameObject W4 = Instantiate(Weapon[3], transform.position, Quaternion.identity);
        W3.GetComponent<DynaBlade>().Dyna_Start(false);
        W4.GetComponent<DynaBlade>().Dyna_Start(true);
        yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(transform.position.x, transform.position.y - 11, 0), 1, declineCurve));
    }

    IEnumerator Pattern04()
    {
        IEnumerator rotate_bullet = Rotate_Bullet(7, 200, 0.02f, 4, Weapon[2]);

        while (true)
        {
            yield return Change_Color_Lerp(new Color(1, 1, 1, 0), new Color(1, 1, 1, 0), 0.33f, 1.5f, DisAppear_Effect_2);

            camera_shake = cameraShake.Shake_Act(.035f, .2f, 1, true);
            StartCoroutine(camera_shake);
            StartCoroutine(rotate_bullet);

            int x = Random.Range(0, 5);
            transform.position = new Vector3(move_random[x, 0], move_random[x, 1], 0);

            yield return StartCoroutine(Change_Color_Return_To_Origin(new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), 0.67f, false));
            StopCoroutine(camera_shake);
            cameraShake.Origin_Camera();
            StopCoroutine(rotate_bullet);

            yield return null;
        }
    }

    IEnumerator Pattern05()
    {
        Instantiate(Weapon[8], new Vector3(0, 2.5f, 0), Quaternion.Euler(-90, 0, 0));
        yield return YieldInstructionCache.WaitForSeconds(0.5f);

        GameObject a = Instantiate(Time_Out_For_Damage, new Vector3(0, 2.5f, 0), Quaternion.identity);

        Instantiate(Weapon[8], Vector3.zero, Quaternion.Euler(-90, 0, 0));
        yield return YieldInstructionCache.WaitForSeconds(0.5f);

        GameObject b = Instantiate(Time_Out_For_Damage, Vector3.zero, Quaternion.identity);

        Instantiate(Weapon[8], new Vector3(0, -2.5f, 0), Quaternion.Euler(-90, 0, 0));
        yield return YieldInstructionCache.WaitForSeconds(0.5f);

        GameObject c = Instantiate(Time_Out_For_Damage, new Vector3(0, -2.5f, 0), Quaternion.identity);
        yield return YieldInstructionCache.WaitForSeconds(0.5f);

        Destroy(a); Destroy(b); Destroy(c);

        yield return null;

    }



    IEnumerator HP_Decrease()
    {
        while(true)
        {
            CurrentHP -= 10;
            yield return new WaitForSeconds(1f);
        }
    }
   
    
    
    IEnumerator Move_Round_Trip(float x_f, float y_f, float x_l, float y_l)
    {
        bool move_dir = false;
        while (true)
        {
            if (move_dir)
                yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(x_f, y_f, 0), 1, De_In_Curve));
            else
                yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(x_l, y_l, 0), 1, De_In_Curve));

            move_dir = !move_dir;
        }
    }
    IEnumerator Rotate(int Degree)
    {
        bool rotate_dir = true;
        for (int i = 0; i < Degree; i++)
        {
            if (i == Degree - 1)
            {
                i = 0;
                rotate_dir = !rotate_dir;
            }
            if (rotate_dir)
                transform.Rotate(Vector3.forward * 100 *  Time.deltaTime);
            else
                transform.Rotate(Vector3.back * 100 * Time.deltaTime);
            yield return null;
        }
    }
    IEnumerator Boss_W1(float start_angle, int count, float range_angle, float speed, bool is_Blink)
    {
        float count_per_radian = range_angle / count;
        float intervalAngle = start_angle;
        for (int i = 0; i < count; i++)
        {
            float angle = intervalAngle + (i * count_per_radian);
            float x = Mathf.Cos(angle * Mathf.PI / 180.0f);
            float y = Mathf.Sin(angle * Mathf.PI / 180.0f);
            Launch_Weapon_For_Move_Blink(Weapon[0], new Vector3(x, y), Quaternion.identity, speed, is_Blink, transform.position);
        }
        yield return null;
    }
}
