using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 보통 자연스럽게 보이는 카메라 흔들림 효과의 세기는 0.02입니다.
public class Asura : Boss_Info
{
    float[] Meteor_Move = new float[9] { 4, 3, 2, 1, 0, -1, -2, -3, -4 };  // 본인

    private int Total_Pattern_Num = 0; // 본인

    private int Pattern2_Meteor_Random_Num;

    int[,] Pattern2_Beam_Spawn = new int[3, 7] { { 4, 4, 4, 3, 3, 3, 3 }, { 3, 3, 3, 4, 3, 3, 3 }, { 3, 3, 3, 3, 4, 4, 4 } };

    // Snow Particle - by https://notyu.tistory.com/60

    private new void OnTriggerEnter2D(Collider2D collision) // 얘만
    {
        base.OnTriggerEnter2D(collision);
    }

    [SerializeField]
    List<For_Continuous_Slerp_Move> DoPhan_Appearance = new List<For_Continuous_Slerp_Move>() 
    {  new For_Continuous_Slerp_Move(new Vector3(-6.61f, 2.21f, 0), "anti_clock"),  new For_Continuous_Slerp_Move(new Vector3(-5f, 3.66f, 0), "anti_clock"), 
        new For_Continuous_Slerp_Move(new Vector3(6.85f, -2.9f, 0), "clock"), new For_Continuous_Slerp_Move(new Vector3(-6.52f, -0.6f, 0), "anti_clock"), 
        new For_Continuous_Slerp_Move(new Vector3(7, 0, 0), "anti_clock" )
    };

    [SerializeField]
    List<Vector3> DoPhan_Pattern01_Move = new List<Vector3>()  { 
        new Vector3(7, 2, 0), new Vector3(-7, 2, 0), new Vector3(-7, -2, 0), new Vector3(7, -2, 0), new Vector3(3.5f, 1, 0),
        new Vector3(-3.5f, 1, 0), new Vector3(-3.5f, -1, 0), new Vector3(3.5f, 1, 0), new Vector3(0, 0, 0 )
    };

    [SerializeField]
    List<Vector3> DoPhan_Pattern05_Move = new List<Vector3>()  { 
        new Vector3(-6.56f, 1.72f, 0), new Vector3(-3.42f, 1.72f, 0), new Vector3(0.28f, 1.72f, 0), new Vector3(3.7f, 1.72f, 0), new Vector3(5, 1.72f, 0),
        new Vector3(-6.56f, 0, 0), new Vector3(-3.42f, 0, 0), new Vector3(0.28f, 0, 0), new Vector3(3.7f, 0, 0), new Vector3(5, 0, 0),
        new Vector3(-6.56f, -1.72f, 0), new Vector3(-3.42f, -1.72f, 0), new Vector3(0.28f, -1.72f, 0), new Vector3(3.7f, -1.72f, 0), new Vector3(5, -1.72f, 0)
    };

    [SerializeField]
    List<Vector3> DoPhan_Ready_To_Pattern_Move = new List<Vector3>()
    {
        new Vector3(-1, 1, 0), new Vector3(2, 0, 0), new Vector3(-1, -2, 0), new Vector3(-1, 1, 0), new Vector3(2, 0, 0),
        new Vector3(-1, 1, 0), new Vector3(0, -1, 0)
    };

    [SerializeField]
    GameObject Charge_Beam; // 본인

    [SerializeField]
    GameObject Meteor1;

    [SerializeField]
    GameObject Meteor2; // 본인

    [SerializeField]
    GameObject Homming_Enemy; // 본인

    [SerializeField]
    GameObject Blink;

    PlayerCtrl_Tengai playerCtrl_Tengai;

    MoveBackGround moveBackGround_1, moveBackGround_2;

    IEnumerator enemy_spawn, meteor_launch, change_boss_color, repeat_phase, pattern06_weapon, pattern;

    private new void Awake()
    {
        base.Awake();
        My_Position = new Vector3(0, -9, 0);
        My_Scale = new Vector3(1.2f, 1.2f, 0);
        Unbeatable = true;
        WarningText.color = new Color(WarningText.color.r, WarningText.color.g, WarningText.color.b, 0);
        imageColor = GameObject.FindGameObjectWithTag("Flash").GetComponent<ImageColor>();
        moveBackGround_1 = GameObject.FindGameObjectWithTag("BackGround1").GetComponent<MoveBackGround>();
        moveBackGround_2 = GameObject.FindGameObjectWithTag("BackGround2").GetComponent<MoveBackGround>();
        playerCtrl_Tengai = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl_Tengai>();
        pattern = null;
        Total_Pattern_Num = 0;
    }

    public override void TakeDamage(float damage) // 얘만 (불상은 데미지를 입지 않음)
    {
        if (!Unbeatable)
        {
            CurrentHP -= damage;
            Run_Life_Act(Hit());
            if (CurrentHP <= 0)
            {
                Unbeatable = true;
                OnDie();
            }
        }
    }
    IEnumerator Hit()
    {
        Camera_Shake(0.005f, 0.05f, true, false);

        My_Color = new Color(1, 1, 1, 0.25f);
        yield return new WaitForSeconds(0.07f);
        My_Color = Color.white;
        yield return null;
    }
    public override void OnDie()
    {
        My_Position = new Vector3(7, 0, 0);
        My_Color = Color.white;
        imageColor.Set_BGColor(Color.clear);

        playerCtrl_Tengai.Final_Score += 10000;
        Instantiate(When_Dead_Effect, My_Position, Quaternion.identity);

        Killed_All_Mine();

        Run_Life_Act(Boss_Die_After());
    }
    IEnumerator Boss_Die_After()
    {
        Flash(Color.white, 0.2f, 5);
        Run_Life_Act(Shake_Act(0.2f, 0, 10, false));
        moveBackGround_1.Decrease_Speed_F(8, 0);
        yield return moveBackGround_2.Decrease_Speed_W(8, 0);
        yield return YieldInstructionCache.WaitForSeconds(2f);

        yield return Change_BG_And_Wait(Color.black, 2);
        //Destroy(gameObject);
    }
    public void Phase_Start()
    {
        Run_Life_Act(Boss_Apprearance());
    }

    IEnumerator Boss_Apprearance()
    {
        //Run_Life_Act(Change_My_Size(My_Scale, My_Scale * 0.5f, 5, OriginCurve));

        //My_Position = new Vector3(0, -8, 0);

        //float standard_distance = -1;
        //int i = 0;

        //foreach (var e in DoPhan_Appearance)
        //{
        //    float first_distance = Get_Curve_Distance(My_Position, e.Next_Position, Get_Center_Vector(My_Position, e.Next_Position,
        //        Vector3.Distance(My_Position, e.Next_Position) * 0.85f, e.Dir));
        //    if (standard_distance == -1)
        //        standard_distance = first_distance;
        //    yield return Move_Curve(My_Position, e.Next_Position,
        //        Get_Center_Vector(My_Position, e.Next_Position, Vector3.Distance(My_Position, e.Next_Position) * 0.85f, e.Dir),
        //        (0.4f + 0.2f * i++) * (first_distance / standard_distance), OriginCurve);
        //}

        //moveBackGround_1.Increase_Speed_F(8, 10);
        //yield return moveBackGround_2.Increase_Speed_W(8, 10);
        playerCtrl_Tengai.Unbeatable = false;

        My_Position = new Vector3(7, 0, 0);
        My_Scale = new Vector3(0.6f, 0.6f, 0);

        yield return Ready_To_Pattern();
        Run_Life_Act_And_Continue(ref repeat_phase, Repeat_Phase());
    }

    IEnumerator Ready_To_Pattern()
    {
        Unbeatable = true;

        for (int i = 0; i < 2; i++)
            yield return Change_My_Color_And_Back(Color.white, new Color(159 / 255, 43 / 255, 43 / 255), 0.125f, false);
        
        yield return YieldInstructionCache.WaitForSeconds(0.25f);

        Instantiate(Blink, My_Position, Quaternion.identity);

        yield return Change_BG_And_Wait(Color.white, 0.8f);
        yield return Change_BG_And_Wait(Color.clear, 0.8f);

        //Unbeatable = false;
        // 이 부분도 제발 나중에 수정 좀 해


        foreach (var e in DoPhan_Ready_To_Pattern_Move)
            yield return Move_Straight(My_Position, My_Position + e, 0.125f, OriginCurve);

        yield return YieldInstructionCache.WaitForSeconds(0.25f);
    }
   
    IEnumerator Repeat_Phase()
    {
        yield return Pattern01();
        // yield return Pattern02();
        // yield return Pattern03();
        //yield return Pattern05();
        // yield return Ready_To_Pattern();
        //yield return Pattern06();
        //yield return Ready_To_Pattern();
        pattern = Pattern02();
        yield return pattern;
        pattern = Pattern05();
        yield return pattern;
        pattern = Pattern04();
        yield return pattern;
        //while (true)
        //{
        //    Total_Pattern_Num = Random.Range(0, 6);
        //    switch (Total_Pattern_Num)
        //    {
        //        case 0:
        //            pattern = Pattern01();
        //            break;
        //        case 1:
        //            pattern = Pattern02();
        //            break;
        //        case 2:
        //            pattern = Pattern03();
        //            break;
        //        case 3:
        //            pattern = Pattern04();
        //            break;
        //        case 4:
        //            pattern = Pattern05();
        //            break;
        //        case 5:
        //            pattern = Pattern06();
        //            break;
        //    }
        //    if (Total_Pattern_Num == 5 || Total_Pattern_Num == 4)
        //        yield return Ready_To_Pattern();
        //    yield return pattern;
        //}
    }
    IEnumerator Pattern01()
    {
        yield return Move_Straight(My_Position, DoPhan_Pattern01_Move[0], 0.5f, declineCurve);
        trailRenderer.enabled = true;
        yield return Boss_Move();

        Unbeatable = true;

        yield return Trail_Color_Change_And_Back(Color.white, Color.green, 0.5f, 2);

        trailRenderer.enabled = false;

        yield return Move_Straight(My_Position, Vector3.zero, 2, declineCurve);

        Launch_Weapon(ref Weapon[0], new Vector3(1, 0.5714f, 0), Quaternion.identity, 8, My_Position);
        Launch_Weapon(ref Weapon[0], new Vector3(1, -0.5714f, 0), Quaternion.Euler(new Vector3(0, 0, -60)), 8, My_Position);
        Launch_Weapon(ref Weapon[0], new Vector3(-1, 0.5714f, 0), Quaternion.Euler(new Vector3(0, 0, -60)), 8, My_Position);
        Launch_Weapon(ref Weapon[0], new Vector3(-1, -0.5714f, 0), Quaternion.identity, 8, My_Position);

        yield return YieldInstructionCache.WaitForSeconds(1f);

        Run_Life_Act_And_Continue(ref change_boss_color, Change_My_Color_And_Back(Color.white, Color.black, 0.25f, true));

        Camera_Shake(0.03f, 1.5f, true, false);

        Launch_Weapon(ref Weapon[1], Vector3.zero, Quaternion.Euler(new Vector3(0, 0, -9)), 0, new Vector3(0, 4, 0));
        Launch_Weapon(ref Weapon[1], Vector3.zero, Quaternion.Euler(new Vector3(0, 0, -9)), 0, new Vector3(0, -4.5f, 0));
        Launch_Weapon(ref Weapon[2], Vector3.zero, Quaternion.Euler(new Vector3(0, 0, -115)), 0, new Vector3(-8, 0, 0));
        Launch_Weapon(ref Weapon[2], Vector3.zero, Quaternion.Euler(new Vector3(0, 0, -115)), 0, new Vector3(6.6f, 0, 0));

        for (int i = 0; i < 5; i++)
            yield return Flash_And_Wait(Random.ColorHSV(), 0.1f, 0.5f);

        Unbeatable = false;

        Stop_Life_Act(ref change_boss_color);

        yield return Move_Straight(My_Position, new Vector3(7, 0, 0), 1, declineCurve);     
    }
    IEnumerator Pattern02()
    {
        GameObject Light_To_Death = Instantiate(Charge_Beam, My_Position, Quaternion.identity);
        if (Light_To_Death.TryGetComponent(out ParticleSystem PS))
        {
            Unbeatable = true;
            yield return Warning(Color.red, "어디 한 번 지혜를 발휘해봐라", 1f);

            if (PS.IsAlive())
                Destroy(PS.gameObject);
        }

        Unbeatable = false;

        if (playerCtrl_Tengai.My_Position.x >= 3.5f && playerCtrl_Tengai.My_Position.x <= 8f && playerCtrl_Tengai.My_Position.y <= -2.6f)
        {
            Pattern2_Meteor_Random_Num = Random.Range(1, 4);
            switch(Pattern2_Meteor_Random_Num)
            {
                case 1:
                    yield return Pattern02_When_Player_Avoid(0);
                    break;
                case 2:
                    yield return Pattern02_When_Player_Avoid(1);
                    break;
                case 3:
                    yield return Pattern02_When_Player_Avoid(2);
                    break;
            }
        }
        else
        {
            for (int i = 0; i < 30; i++)
            {
                float param = 0.5714f;
                for (int j = 0; j < 7; j++)
                {
                    Launch_Weapon(ref Weapon[3], new Vector3(-1, param, 0), Quaternion.identity, 80, My_Position);
                    param -= 0.1904f;
                }
                yield return YieldInstructionCache.WaitForSeconds(.1f);
            }
        }
        yield return YieldInstructionCache.WaitForSeconds(2f);
        yield break;
    }
    IEnumerator Pattern02_When_Player_Avoid(int kuku)
    {
        StartCoroutine(Pattern02_Meteor_Down(kuku));

        for (int i = 0; i < 30; i++)
        {
            float param = 0.5714f;
            for (int j = 0; j < 7; j++)
            {
                Launch_Weapon(ref Weapon[Pattern2_Beam_Spawn[kuku, j]], new Vector3(-1, param, 0), Quaternion.identity, 80, My_Position);
                param -= 0.1904f;
            }
            yield return YieldInstructionCache.WaitForSeconds(.1f);
        }
        yield return null;
    }
    IEnumerator Pattern02_Meteor_Down(int kuku)
    { 
        yield return YieldInstructionCache.WaitForSeconds(0.5f);
        for (int i = 0; i < 7; i++)
        {
            int Rand = Random.Range(4, 9);
            GameObject e = Instantiate(Meteor1, new Vector3(Rand, 5.6f, 0), Quaternion.identity);
            if (e.TryGetComponent(out Meteor_Effect ME))
                ME.Pattern02_Meteor_Launch(Rand);
            else
                Destroy(e);
            yield return YieldInstructionCache.WaitForSeconds(0.2f);
        }
        yield return null;
    }
    IEnumerator Pattern03()
    {
        yield return Move_Straight(My_Position, Vector3.zero, 1, declineCurve);

        yield return Rotate_Bullet(7, 250, .5f, 4, Weapon[5]);

        yield return Move_Straight(Vector3.zero, new Vector3(0, 3, 0), 1, declineCurve);

        yield return Move_Circle(270, -1, 0, 7, 3, 0, 0, 2);
    }
    IEnumerator Pattern04() // + 플레이어가 시련을 겪다가 방출하는 것도 추가해야한다. 하나라도 데미지 맞으면 정화 취소.
    {
        Unbeatable = true;
        yield return Move_Straight(My_Position, new Vector3(12, My_Position.y, 0), 1, declineCurve);

        playerCtrl_Tengai.Start_Emit();

        Run_Life_Act_And_Continue(ref enemy_spawn, Enemy_Spawn());

        for (int i = 0; i < 14; i++)
        {
            int Random_Num = Random.Range(0, 4);
            switch (Random_Num)
            {
                case 0:
                    meteor_launch = Meteor_Launch(9, 1);
                    yield return meteor_launch;
                    break;
                case 1:
                    meteor_launch = Meteor_Launch(6, 1);
                    yield return meteor_launch;
                    break;
                case 2:
                    meteor_launch = Meteor_Launch(3, 3);
                    yield return meteor_launch;
                    break;
                case 3:
                    meteor_launch = Meteor_Launch(1, 8);
                    yield return meteor_launch;
                    break;
            }
            yield return YieldInstructionCache.WaitForSeconds(2f);
        }

        Stop_Life_Act(ref enemy_spawn);

        Unbeatable = false;
        yield return Move_Straight(My_Position, new Vector3(7, My_Position.y, 0), 1, declineCurve);
        yield return YieldInstructionCache.WaitForSeconds(0.5f);
        yield return null;
    }
    public void Stop_Meteor()
    {
        Run_Life_Act(I_Stop_Meteor());
    }
    IEnumerator I_Stop_Meteor()
    {
        Stop_Life_Act(ref meteor_launch);
        Stop_Life_Act(ref enemy_spawn);
        Stop_Life_Act(ref pattern);
        Stop_Life_Act(ref repeat_phase);

        yield return YieldInstructionCache.WaitForSeconds(2f);

        Unbeatable = false;
        Run_Life_Act(Shake_Act(0.3f, 0, 2, false));
        yield return Move_Straight(My_Position, new Vector3(7, 0, 0), 2, declineCurve);

        for (int i = 0; i < 3; i++)
            yield return Warning(Color.blue, "CHANCE!!!", 1f);
        yield return YieldInstructionCache.WaitForSeconds(2f); // 플레이어에게 무차별 폭격 받는 곳
        //Run_Life_Act_And_Continue(ref repeat_phase, Repeat_Phase());
        // 제발 이 부분도 수정 좀
        //yield return null;
    }
    IEnumerator Enemy_Spawn()
    {
        while(true)
        {
            Instantiate(Homming_Enemy, new Vector3(4, 0, 0), Quaternion.identity);
            yield return YieldInstructionCache.WaitForSeconds(3f);
        }
    }
    IEnumerator Meteor_Launch(int Meteor_Num, int Launch_Count)
    {
        float R1 = Random.Range(.15f, 1); float R2 = Random.Range(.15f, 1); float R3 = Random.Range(.15f, 1);
        Meteor_Move = StaticFunc.ShuffleList(Meteor_Move);
        switch (Launch_Count)
        {   
            case 1:
                for (int i = 0; i < Meteor_Num; i++)
                {
                    GameObject e = Instantiate(Meteor2, new Vector3(10, Meteor_Move[i], 0), Quaternion.identity);
                    if (e.TryGetComponent(out Meteor_Effect ME1))
                        ME1.Pattern04_Meteor_Launch(Meteor_Move[i], R1, R2, R3);
                    else
                        Destroy(e);
                    yield return YieldInstructionCache.WaitForSeconds(0.2f);
                }
                break;

            case 3:
                for (int i = 0; i < 3; i++)
                {
                    R1 = Random.Range(.15f, 1); R2 = Random.Range(.15f, 1);  R3 = Random.Range(.15f, 1);
                    for (int j = 3 * i; j < 3 * (i + 1); j++)
                    {
                        GameObject e = Instantiate(Meteor2, new Vector3(10, Meteor_Move[j], 0), Quaternion.identity);
                        if (e.TryGetComponent(out Meteor_Effect ME3))
                            ME3.Pattern04_Meteor_Launch(Meteor_Move[j], R1, R2, R3);
                        else
                            Destroy(e);
                    }
                    yield return YieldInstructionCache.WaitForSeconds(0.5f);
                }
                break;

            case 8:
                for (int i = 0; i < 8; i++)
                {
                    GameObject e = Instantiate(Meteor2, new Vector3(10, Meteor_Move[i], 0), Quaternion.identity);
                    if (e.TryGetComponent(out Meteor_Effect ME8))
                        ME8.Pattern04_Meteor_Launch(Meteor_Move[i], R1, R2, R3);
                    else
                        Destroy(e);
                }
                break;
        }
        yield return null;
    }

    IEnumerator Pattern06()
    {
        moveBackGround_1.Increase_Speed_F(8, 16);
        yield return moveBackGround_2.Increase_Speed_W(8, 16);

        yield return Change_My_Color_And_Back(Color.white, Color.black, 0.8f, false);

        My_Position = new Vector3(7, 0, 0);
        Run_Life_Act_And_Continue(ref pattern06_weapon, Pattern06_Weapon());

        yield return Move_Straight(new Vector3(7, 0, 0), new Vector3(7, 3, 0), 1.5f, De_In_Curve);
        for (int i = 0; i < 4; i++)
        {
            yield return Move_Straight(new Vector3(7, 3, 0), new Vector3(7, -3, 0), 1.5f, De_In_Curve);
            yield return Move_Straight(new Vector3(7, -3, 0), new Vector3(7, 3, 0), 1.5f, De_In_Curve);
        }

        Stop_Life_Act(ref pattern06_weapon);
        yield return YieldInstructionCache.WaitForSeconds(1f);
        int flag = 1;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 9; j++)
                Launch_Weapon(ref Weapon[6], Vector3.left, Quaternion.identity, 12, new Vector3(7, 6 - (2.8f * j) + (flag * 0.5f), 0));
            flag = -flag;
            yield return YieldInstructionCache.WaitForSeconds(1f);
        }
        moveBackGround_1.Decrease_Speed_F(4, 12);
        yield return moveBackGround_2.Decrease_Speed_W(4, 12);
    }

    IEnumerator Pattern06_Weapon()
    {
        float Randomly = My_Position.y;
        while(true)
        {
            float ee = My_Position.y - Randomly;
            Vector3 normal_dir = new Vector3(-1, ee, 0).normalized;

            Launch_Weapon(ref Weapon[6], normal_dir, Quaternion.identity, 10, My_Position + new Vector3(-0.5f, 2.5f, 0));
            Launch_Weapon(ref Weapon[6], normal_dir, Quaternion.identity, 10, My_Position + new Vector3(-0.5f, -2.5f, 0));

            Randomly = My_Position.y;
            yield return YieldInstructionCache.WaitForSeconds(0.05f);
        }
    }

    IEnumerator Pattern05()
    {
        int Random_Move;
        float Random_Time = 0.2f;

        Color Alpha_1 = new Color(My_Color.r, My_Color.g, My_Color.b, 1);
        Color Alpha_0 = new Color(My_Color.r, My_Color.g, My_Color.b, 0);
        for (int i = 0; i < 15; i++)
        {
            if (i == 6)
            {
                yield return YieldInstructionCache.WaitForSeconds(2f);
                Random_Time = 0.05f;
            }
            Random_Move = Random.Range(0, 6);
            yield return Change_My_Color(Alpha_1, Alpha_0, 0.1f, Random_Time, DisAppear_Effect_1);

            My_Position = DoPhan_Pattern05_Move[Random_Move];

            yield return Change_My_Color(Alpha_0, Alpha_1, 0.1f, Random_Time, DisAppear_Effect_2);
        }
        yield return Move_Curve(My_Position, new Vector3(7, 0, 0), Vector3.zero, 2, declineCurve);
    }
    IEnumerator Boss_Move()
    {
        foreach (var e in DoPhan_Pattern01_Move)
            yield return Move_Straight(My_Position, e, .25f, declineCurve);
    }
}