using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolGryn : Boss_Info
{

    // <a href='https://kr.freepik.com/vectors/light'>Light 벡터는 upklyak - kr.freepik.com가 제작함</a>

    [SerializeField]
    private GameObject SolGryn_HP;

    [SerializeField]
    private GameObject PalJeongDo_Thunder;

    [SerializeField]
    private GameObject Straw;

    private Player_Final2 himaController;

    private ImageColor PalJeongDo;

    private GameObject Straw_Copy;

    private SpriteColor spriteColor;

    private List<GameObject> SolG_Copy;

    private IEnumerator pattern, change_color, rotate_bullet;

    private int[,] bangmeon = new int[2, 4] { { 1, -1, -1, 1 }, { 1, 1, -1, -1 } };



    [SerializeField]
    List<Vector3> Pattern06_Monster_Move;


    private bool is_Next_Pattern = false;
    private new void OnTriggerEnter2D(Collider2D collision) // 얘만
    {
        base.OnTriggerEnter2D(collision);
    }
    public bool Is_Next_Pattern
    {
        get { return is_Next_Pattern; }
        set { is_Next_Pattern = value; }
    }

    private float[,] Pattern04_Move =
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

        if (GameObject.Find("Flash") && GameObject.Find("Flash").TryGetComponent(out ImageColor i1))
            imageColor = i1;
        if (GameObject.Find("PalJeongDo") && GameObject.Find("PalJeongDo").TryGetComponent(out ImageColor i2))
            PalJeongDo = i2;
        if (GameObject.FindGameObjectWithTag("Player") && GameObject.FindGameObjectWithTag("Player").TryGetComponent(out Player_Final2 i3))
            himaController = i3;

        if (GameObject.Find("Jebal") && GameObject.Find("Jebal").TryGetComponent(out SpriteColor s1))
            spriteColor = s1;

        SolGryn_HP.SetActive(false);
        trailRenderer.enabled = false;
        pattern = null;

        SolG_Copy = new List<GameObject>();
    }
    public void WelCome()
    {
        Run_Life_Act(TeoKisis());
    }

    private void Launch_SoyBean()
    {
        Instantiate(Weapon[7], My_Position, Quaternion.identity);
    }
    private IEnumerator Blink_Bullet()
    {
        My_Scale = new Vector3(0.7f, 0.7f, 0);  // 임시
        for (int i = 0; i < 10; i++)
        {
            Boss_W1(72 + (20 * i), 25, 360, 6, true);
            yield return YieldInstructionCache.WaitForSeconds(0.6f);
        }
        yield return null;
    }
    private IEnumerator TeoKisis()
    {
        yield return Move_Straight(new Vector3(0, 7, 0), new Vector3(0, 0, 0), 7f, declineCurve);

        Camera_Shake(0.01f, 2, true, false);
        yield return Change_My_Color_And_Back(Color.white, new Color(1, 69 / 255, 69 / 255, 1), 1, false);

        for (int i = 0; i < 4; i++)
            SolG_Copy.Add(Instantiate(Weapon[5], My_Position, Quaternion.identity));

        for (int i = 0; i < 4; i++)
        {
            if (SolG_Copy[i].TryGetComponent(out SolGryn_Copy SC1))
                SC1.Move_Straight(new Vector3(bangmeon[0, i] * 7, bangmeon[1, i] * 2.5f, 0));
        }

        yield return Change_BG_And_Wait(Color.white, 0.5f);
        PalJeongDo.Set_BGColor(Color.white);
        yield return Change_BG_And_Wait(Color.clear, 1);

        yield return YieldInstructionCache.WaitForSeconds(2f);

        yield return Change_BG_And_Wait(new Color(0, 0, 0, 0.6f), 2);

        for (int i = -1; i < 2; i++)
            Instantiate(PalJeongDo_Thunder, My_Position + (5 * new Vector3(i, 0, 0)), Quaternion.identity);

        yield return Change_BG_And_Wait(Color.white, 0.5f);
        PalJeongDo.Set_BGColor(Color.red);
        yield return Change_BG_And_Wait(Color.clear, 1);

        yield return YieldInstructionCache.WaitForSeconds(1f);

        foreach (var u in SolG_Copy)
            Destroy(u);

        yield return Change_My_Size(My_Scale, My_Scale / 2, 0.5f, OriginCurve);

        spriteColor.Change_C(Color.white, 1);
        yield return Change_My_Size(My_Scale, My_Scale * 20, 1f, inclineCurve);

        PalJeongDo.Set_BGColor(Color.clear);
        yield return YieldInstructionCache.WaitForSeconds(1.5f); // 이동 후 카메라 정지 + 1.5초 정지

        spriteColor.Change_C(Color.black, 1);
        yield return YieldInstructionCache.WaitForSeconds(1.5f);  // 검정색 플래시 후 1.5초 정지

        spriteColor.Change_C(Color.clear, 1);
        yield return Boss_Pattern();
    }
    private void Continue_Camera_Shake()
    {
        Camera_Shake(0.002f, 1, false, true);
    }
    private IEnumerator Boss_Pattern()
    {
        himaController.IsMove = true;
        himaController.Unbeatable = false;
        My_Position = new Vector3(7, 4, 0);
        My_Scale = new Vector3(0.7f, 0.7f, 0);

        SolGryn_HP.SetActive(true);
        if (SolGryn_HP.TryGetComponent(out BossHPSliderViewer B1))
            B1.F_HPFull(this);

        Run_Life_Act(HP_Decrease());

        Continue_Camera_Shake();

        yield return Move_Curve(My_Position, new Vector3(-4, 4, 0),
            Get_Center_Vector(My_Position, new Vector3(-4, 4, 0), Vector3.Distance(My_Position, new Vector3(-4, 4, 0)) * 0.85f, "anti_clock"), 4, OriginCurve);

        List<IEnumerator> Pattern_Collect;


        while (true)
        {
            Pattern_Collect = new List<IEnumerator>() { Pattern01(), Pattern02(), Pattern03(), Pattern04(), Pattern05(), Pattern06() };
            for (int i = 0; i < Pattern_Collect.Count; i++)
            {
                pattern = Pattern_Collect[i];
                yield return pattern;
            }
        }
    }
    private IEnumerator Pattern01() // 완료
    {
        float[,] u1 = new float[3, 2] { { -4f, -0.4f }, { 4f, -0.4f }, { 4f, -6f } };
        float[,] u2 = new float[3, 2] { { 0, 2 }, { 0, -3.35f }, { -4, -3.35f } };

        trailRenderer.enabled = true;
        if (TryGetComponent(out TrailCollisions TC))
            TC.Draw_Collision_Line();

        My_Position = new Vector3(-4, 4, 0);
        Flash(Color.white, 0, 0.5f);
        yield return Change_My_Color(My_Color, Color.white, 0.33f, 0, DisAppear_Effect_1);

        for (int i = 0; i < 3; i++)
            yield return Move_Straight(My_Position, new Vector3(u1[i, 0], u1[i, 1], 0), 0.12f, declineCurve);

        My_Position = new Vector3(4, 2, 0);
        Flash(Color.white, 0, 0.5f);
        yield return Change_My_Color(new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), 0.33f, 0, DisAppear_Effect_1);

        for (int i = 0; i < 3; i++)
            yield return Move_Straight(My_Position, new Vector3(u2[i, 0], u2[i, 1], 0), 0.12f, declineCurve);

        Run_Life_Act(Change_My_Color(My_Color, new Color(1, 1, 1, 0), 0.4f, 0, DisAppear_Effect_1));
        yield return Trail_Color_Change_And_Back(Color.red, Color.green, 0.33f, 3);

        GameObject nachi_x_g_1 = Instantiate(Weapon[6], new Vector3(6.3f, 1.87f, 0), Quaternion.identity);
        GameObject nachi_x_g_2 = Instantiate(Weapon[6], new Vector3(-5.61f, 1.64f, 0), Quaternion.identity);

        Run_Life_Act(Blink_Bullet());

        if (nachi_x_g_1.TryGetComponent(out Nachi_X NX_1) && nachi_x_g_2.TryGetComponent(out Nachi_X NX_2))
        {
            NX_1.StartCoroutine(NX_1.Move(-1));
            yield return NX_2.StartCoroutine(NX_2.Move(1));
        }
        yield return Trail_Color_Change(Color.red, Color.clear, 0.5f);
        trailRenderer.enabled = false;

        if (GameObject.Find("TrailCollider"))
            Destroy(GameObject.Find("TrailCollider"));

        My_Color = Color.white;
        My_Position = new Vector3(7, 4, 0);
        yield return Move_Straight(My_Position, new Vector3(7, 0, 0), 1f, inclineCurve);

    }
    private IEnumerator Pattern02()
    {
        My_Position = new Vector3(-7, 2.5f, 0);

        yield return Straw_Launch(new Vector3(-7, -2.5f, 0), Quaternion.Euler(0, 0, -90), new Vector3(-4.5f, -2.5f, 0), Quaternion.Euler(0, 0, -90),
          new Vector3(-3.1f, -2.5f, 0), Quaternion.identity, Vector3.right, 0.3f, 11, 0.3f);

        yield return Straw_Launch(new Vector3(7, -2.5f, 0), Quaternion.Euler(0, 0, 90), new Vector3(4.6f, -2.5f, 0), Quaternion.Euler(0, 0, 90),
          new Vector3(3.2f, -2.5f, 0), Quaternion.identity, Vector3.left, 0.3f, 11, 0.3f);

        yield return Straw_Launch(new Vector3(0, 4.3f, 0), Quaternion.Euler(0, 0, 180), new Vector3(0, 1.92f, 0), Quaternion.identity,
          new Vector3(0, 0.61f, 0), Quaternion.Euler(0, 0, 90), Vector3.down, 0, 7, 0.1f);

        yield return Straw_Launch(new Vector3(3.11f, 4.3f, 0), Quaternion.Euler(0, 0, 180), new Vector3(3.11f, 1.92f, 0), Quaternion.identity,
          new Vector3(3.11f, 0.61f, 0), Quaternion.Euler(0, 0, 90), Vector3.down, 0, 7, 0.1f);

        yield return Straw_Launch(new Vector3(-3.11f, 4.3f, 0), Quaternion.Euler(0, 0, 180), new Vector3(-3.11f, 1.92f, 0), Quaternion.identity,
          new Vector3(-3.11f, 0.61f, 0), Quaternion.Euler(0, 0, 90), Vector3.down, 0, 7, 0.1f);

        yield return Straw_Launch(new Vector3(1.5f, 4.3f, 0), Quaternion.Euler(0, 0, 180), new Vector3(1.5f, 1.92f, 0), Quaternion.identity,
          new Vector3(1.5f, 0.61f, 0), Quaternion.Euler(0, 0, 90), Vector3.down, 0, 7, 0.1f);

        yield return Straw_Launch(new Vector3(-1.5f, 4.3f, 0), Quaternion.Euler(0, 0, 180), new Vector3(-1.5f, 1.92f, 0), Quaternion.identity,
          new Vector3(-1.5f, 0.61f, 0), Quaternion.Euler(0, 0, 90), Vector3.down, 0, 7, 0.1f);

        transform.SetPositionAndRotation(new Vector3(0, 1.35f, 0), Quaternion.identity);
        yield return Change_My_Color(My_Color, Color.white, 1f, 0, DisAppear_Effect_1);
        yield return Move_Straight(My_Position, new Vector3(My_Position.x, My_Position.y - 1.35f, 0), 0.5f, inclineCurve);

        SolG_Copy = new List<GameObject>();
        for (int i = 0; i < 2; i++)
            SolG_Copy.Add(Instantiate(Weapon[5], My_Position, Quaternion.identity));


        if (SolG_Copy[0].TryGetComponent(out SolGryn_Copy SC1) && SolG_Copy[1].TryGetComponent(out SolGryn_Copy SC2))
        {
            SC1.Move_Slerp_Distance(new Vector3(6.5f, 2.68f, 0), "anti_clock");
            SC2.Move_Slerp_Distance(new Vector3(-6.5f, 2.68f, 0), "clock");

            yield return Move_Straight(My_Position, new Vector3(My_Position.x, 2.68f, 0), 0.75f, declineCurve);

            for (int i = 0; i < 4; i++)
            {
                SC1.Shake_Act(); SC2.Shake_Act();
                yield return Shake_Act(0.2f, 0.2f, 0.5f, false);

                int Rand = Random.Range(0, 3);

                if (Rand == 0)
                    SC1.Launch_SoyBean();
                else if (Rand == 1)
                    SC2.Launch_SoyBean();
                else
                    Launch_SoyBean();
                yield return YieldInstructionCache.WaitForSeconds(0.5f);
            }
        }
        Destroy(SolG_Copy[0]);
        Destroy(SolG_Copy[1]);
        My_Color = Color.white;
        My_Position = new Vector3(7, 4, 0);
        yield return Move_Straight(My_Position, new Vector3(7, 0, 0), 1f, inclineCurve);
        yield return null;
    }
    private IEnumerator Straw_Launch(Vector3 SolGryn_Pos, Quaternion SolGryn_Lot, Vector3 Straw_Create_Pos, Quaternion Straw_Lot, Vector3 Peanut_Create_Pos, 
        Quaternion Peanut_Lot, Vector3 Peanut_Dir, float Straw_Lot_time_persist, int Peanut_Launch_Num, float Peanut_Launch_Interval)
    {
        transform.SetPositionAndRotation(SolGryn_Pos, SolGryn_Lot);

        Straw_Copy = Instantiate(Straw, Straw_Create_Pos, Quaternion.Euler(0, 0, 0));

        if (Straw_Lot_time_persist != 0)
        {
            yield return Change_My_Color(My_Color, Color.white, 0.1f, 0, DisAppear_Effect_1);
            float percent = 0;
            while (percent < 1)
            {
                percent += Time.deltaTime / Straw_Lot_time_persist;
                Straw_Copy.transform.rotation = Quaternion.Lerp(Quaternion.Euler(0, 0, 0), Straw_Lot, inclineCurve.Evaluate(percent));
                yield return null;
            }
            yield return Flash_And_Wait(Color.white, 0.1f, 0.4f);
        }
        else
            Flash(Color.white, 0.1f, 0.5f);

        Run_Life_Act_And_Continue(ref change_color, Change_My_Color(Color.white, Color.clear, Peanut_Launch_Num * Peanut_Launch_Interval + Straw_Lot_time_persist, 0, DisAppear_Effect_1));

        for (int i = 0; i < Peanut_Launch_Num; i++)
        {
            Launch_Weapon(ref Weapon[1], Peanut_Dir, Peanut_Lot, 15, Peanut_Create_Pos);
            yield return YieldInstructionCache.WaitForSeconds(Peanut_Launch_Interval); // 0.2초는 고정
        }

        Stop_Life_Act(ref change_color);
        Destroy(Straw_Copy);
        yield return null;
    }


    private IEnumerator Pattern03()
    {
        transform.SetPositionAndRotation(new Vector3(6, 0, 0), Quaternion.identity);

        yield return Change_My_Color(My_Color, Color.white, 0.33f, 0, DisAppear_Effect_1);
        yield return Move_Straight(My_Position, new Vector3(My_Position.x, My_Position.y - 3, 0), 1, inclineCurve);
        yield return Move_Straight(My_Position, new Vector3(My_Position.x, 11, 0), 1, declineCurve);

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

        yield return Move_Straight(My_Position, Vector3.zero, 1, declineCurve);
    }
    private IEnumerator Pattern04()
    {
        for (int i = 0; i < 5; i++)
        {
            int x = Random.Range(0, 5);
            My_Position = new Vector3(Pattern04_Move[x, 0], Pattern04_Move[x, 1], 0);
           
            yield return Change_My_Color(Color.clear, Color.clear, 0.33f, 1.5f, DisAppear_Effect_2);

            Run_Life_Act_And_Continue(ref rotate_bullet, Rotate_Bullet(7, 200, 0.02f, 3, Weapon[2]));

            Camera_Shake(0.02f, 1.4f, true, false);
            yield return Change_My_Color_And_Back(Color.clear, Color.white, 0.7f, false);
            Stop_Life_Act(ref rotate_bullet);
        }

        transform.rotation = Quaternion.identity;
        My_Color = Color.white;
        My_Position = new Vector3(7, 4, 0);
        yield return Change_My_Color(My_Color, Color.white, 0.5f, 0, DisAppear_Effect_1);
        yield return Move_Straight(My_Position, new Vector3(7, 0, 0), 1f, inclineCurve);
    }
    private IEnumerator Pattern05()
    {
        yield return Move_Straight(My_Position, new Vector3(My_Position.x, My_Position.y - 3, 0), 0.7f, inclineCurve);
        yield return Move_Straight(My_Position, new Vector3(My_Position.x, 11, 0), 0.7f, declineCurve);
        for (int i = 11; i < 14; i++)
        {
            GameObject W1 = Instantiate(Weapon[i], Vector3.zero, Quaternion.identity);
            
            if (i == 13)
            {
                Flash(new Color(1, 1, 1, 0.8f), 0, 1.8f);
                Camera_Shake(0.02f, 3, true, false);
            }
            else
                Camera_Shake(0.015f, 1, true, false);

            yield return YieldInstructionCache.WaitForSeconds(1f);
            Destroy(W1);
        }

        for (int i = 0; i < 3; i++)
        {
            Instantiate(Weapon[8], new Vector3(0, 2.5f - (2.5f * i), 0), Quaternion.Euler(-90, 0, 0));
            yield return YieldInstructionCache.WaitForSeconds(0.4f);
        }

        Is_Next_Pattern = false;

        Instantiate(Weapon[9], new Vector3(-3, 2, 0), Quaternion.identity);
        Instantiate(Weapon[9], new Vector3(3, 2, 0), Quaternion.identity);

        while(!Is_Next_Pattern)
            yield return null;

        Is_Next_Pattern = false;
        yield return Move_Straight(My_Position, new Vector3(7, 0, 0), 5, declineCurve);
    }

    private void Launch_Monster(Color Target_Player_Or_None, Vector3 Move_Monster, int Player_Or_None)
    {
        GameObject UnderTail = Instantiate(Weapon[10], Vector3.zero, Quaternion.identity);
        if (UnderTail.TryGetComponent(out Monster M1))
        {
            Flash(Target_Player_Or_None, 0, 0.6f);
            M1.Start_Attack(Move_Monster, Player_Or_None);
        }
    }

    private IEnumerator Pattern06()
    {
        int Player_Or_None;

        List<Color> CHK_Player_Target = new List<Color>() { Color.green, Color.blue };
        for (int i = 0; i < 4; i++)
        {
            Player_Or_None = Random.Range(0, 2);
            Launch_Monster(CHK_Player_Target[Player_Or_None], new Vector3(bangmeon[0, i] * 7, bangmeon[1, i] * 4, 0), Player_Or_None);
            yield return YieldInstructionCache.WaitForSeconds(1.2f);
        }

        yield return YieldInstructionCache.WaitForSeconds(1);

        for (int i = 0; i < 3; i++)
        {
            Player_Or_None = Random.Range(0, 2);

            for (int j = 0; j < 4; j++)
                Launch_Monster(CHK_Player_Target[Player_Or_None], new Vector3(bangmeon[0, j] * 7, bangmeon[1, j] * 4, 0), Player_Or_None);

            yield return YieldInstructionCache.WaitForSeconds(1.5f);
        }

        yield return YieldInstructionCache.WaitForSeconds(1);
        
        for (int i = 0; i < Pattern06_Monster_Move.Count; i++)
        {
            Player_Or_None = Random.Range(0, 2);
            Launch_Monster(CHK_Player_Target[Player_Or_None], Pattern06_Monster_Move[i], Player_Or_None);
            yield return YieldInstructionCache.WaitForSeconds(0.8f);
        }

        for (int i = Pattern06_Monster_Move.Count - 2; i >= 0; i--)
        {
            Player_Or_None = Random.Range(0, 2);
            Launch_Monster(CHK_Player_Target[Player_Or_None], Pattern06_Monster_Move[i], Player_Or_None);
            yield return YieldInstructionCache.WaitForSeconds(0.8f);
        }
        yield return YieldInstructionCache.WaitForSeconds(1);
        yield return null;
    }

    private IEnumerator HP_Decrease()
    {
        while(true)
        {
            CurrentHP -= 3;
            if (CurrentHP <= 10)
            {
                OnDie();
                yield break;
            }
            yield return YieldInstructionCache.WaitForSeconds(1f);
        }
    }
    public override void OnDie()
    {
        CurrentHP = 0;
        SolGryn_HP.SetActive(false);

        Killed_All_Mine();
        Init_Back_And_Camera();

        StartCoroutine(I_OnDie());
    }

    private IEnumerator I_OnDie()
    {
        GameObject.Find("Main Camera").GetComponent<UB.Simple2dWeatherEffects.Standard.D2FogsPE>().enabled = false;
        GameObject.Find("Main Camera").GetComponent<Camera>().backgroundColor = Color.green;
        Flash(Color.white, 1, 2);
        yield return Move_Straight(new Vector3(7, 5, 0), new Vector3(7, 0, 0), 10, OriginCurve);
        yield return null;
    }
    private void Boss_W1(float start_angle, int count, float range_angle, float speed, bool is_Blink)
    {
        float count_per_radian = range_angle / count;
        float intervalAngle = start_angle;
        for (int i = 0; i < count; i++)
        {
            float angle = intervalAngle + (i * count_per_radian);
            float x = Mathf.Cos(angle * Mathf.PI / 180.0f);
            float y = Mathf.Sin(angle * Mathf.PI / 180.0f);
            Launch_Weapon(ref Weapon[0], new Vector3(x, y), Quaternion.identity, speed, 4 * Vector3.up);
        }
    }
}