using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asura : Boss_Info
{
    float[] Meteor_Move = new float[9] { 4, 3, 2, 1, 0, -1, -2, -3, -4 };  // 본인

    private int Pattern_Num; // 본인

    private int Pattern2_Meteor_Random_Num;

    int[,] Pattern2_Beam_Spawn = new int[3, 7] { { 4, 4, 4, 3, 3, 3, 3 }, { 3, 3, 3, 4, 3, 3, 3 }, { 3, 3, 3, 3, 4, 4, 4 } };

    public List<For_Continuous_Slerp_Move> DoPhan_Appearance = new List<For_Continuous_Slerp_Move>() 
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
        new Vector3(-6.64f, 1.95f, 0), new Vector3(-3.11f, 0.04f, 0), new Vector3(2.31f, 2.78f, 0), new Vector3(3.4f, -2.91f, 0), new Vector3(-4.92f, -3.07f, 0),
        new Vector3(-7.55f, -1.43f, 0)
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

    IEnumerator enemy_spawn, meteor_launch, change_boss_color, charge_beam, repeat_phase, pattern06_weapon;

    private new void Awake()
    {
        base.Awake();
        transform.position = new Vector3(0, -9, 0);
        transform.localScale = new Vector3(1.2f, 1.2f, 0);
        Unbeatable = true;
        WarningText.color = new Color(WarningText.color.r, WarningText.color.g, WarningText.color.b, 0);
        backGroundColor = GameObject.FindGameObjectWithTag("Flash").GetComponent<BackGroundColor>();
        moveBackGround_1 = GameObject.FindGameObjectWithTag("BackGround1").GetComponent<MoveBackGround>();
        moveBackGround_2 = GameObject.FindGameObjectWithTag("BackGround2").GetComponent<MoveBackGround>();
        playerCtrl_Tengai = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl_Tengai>();

        phase = Pattern01();
       
        for (int i = 0; i < 5; i++)
            Pattern_Total.Add(phase);
        Pattern_Num = 0;
    }

    public override void TakeDamage(float damage) // 얘만 (불상은 데미지를 입지 않음)
    {
        if (!Unbeatable)
        {
            CurrentHP -= damage;
            StartCoroutine(Hit());
            if (CurrentHP <= 0)
            {
                Unbeatable = true;
                OnDie();
            }
        }
    }
    IEnumerator Hit()
    {
        Start_Camera_Shake(0.02f, 0.05f, true, false);

        spriteRenderer.color = new Color(1, 1, 1, 0.25f);
        yield return new WaitForSeconds(0.07f);
        spriteRenderer.color = new Color(1, 1, 1, 1);
        yield return null;
    }
    public override void OnDie()
    {
        transform.position = new Vector3(7, 0, 0);
        SpriteRenderer_Color = new Color(1, 1, 1, 1);
        backGroundColor.Set_BGColor(new Color(1, 1, 1, 0));

        playerCtrl_Tengai.Final_Score += 10000;
        Instantiate(When_Dead_Effect, transform.position, Quaternion.identity);

        Killed_All_Mine();

        StopAllCoroutines();

        StartCoroutine(Boss_Die_After());
    }
    IEnumerator Boss_Die_After()
    {
        backGroundColor.StartCoroutine(backGroundColor.Flash(Color.white, 0.2f, 5));
        StartCoroutine(Shake_Act(0.2f, 0, 10, false));
        moveBackGround_1.StartCoroutine(moveBackGround_1.Decrease_Speed(1, 0));
        yield return moveBackGround_2.StartCoroutine(moveBackGround_2.Decrease_Speed(1, 0));
        yield return YieldInstructionCache.WaitForSeconds(2f);
        
        yield return backGroundColor.StartCoroutine(backGroundColor.Change_Color(backGroundColor.Get_BGColor(), Color.black, 2));
        backGroundColor.Set_BGColor(new Color(0, 0, 0, 1));
        //Destroy(gameObject);
    }
    public void Phase_Start()
    {
        StartCoroutine(Boss_Apprearance());
    }

    IEnumerator Boss_Apprearance()
    {
        StartCoroutine(Size_Change(transform.localScale, transform.localScale * 0.5f, 5, OriginCurve));

        transform.position = new Vector3(0, -8, 0);

        float standard_distance = -1;
        int i = 0;

        foreach (var e in DoPhan_Appearance)
        {
            float first_distance = Get_Slerp_Distance(transform.position, e.Next_Position, Get_Center_Vector(transform.position, e.Next_Position,
                Vector3.Distance(transform.position, e.Next_Position) * 0.85f, e.Dir));
            if (standard_distance == -1)
                standard_distance = first_distance;
            yield return StartCoroutine(Position_Slerp(transform.position, e.Next_Position,
                Get_Center_Vector(transform.position, e.Next_Position, Vector3.Distance(transform.position, e.Next_Position) * 0.85f, e.Dir),
                (0.4f + 0.2f * i++) * (first_distance / standard_distance), OriginCurve, false));
        }

        moveBackGround_1.StartCoroutine(moveBackGround_1.Increase_Speed(1, 8));
        yield return moveBackGround_2.StartCoroutine(moveBackGround_2.Increase_Speed(1, 8));

        transform.position = new Vector3(7, 0, 0);
        transform.localScale = new Vector3(0.6f, 0.6f, 0);

        yield return StartCoroutine(Ready_To_Pattern());
        repeat_phase = Repeat_Phase();
        StartCoroutine(repeat_phase);
    }

    IEnumerator Ready_To_Pattern()
    {
        Unbeatable = true;

        for (int i = 0; i < 2; i++)
            yield return StartCoroutine(Change_Color_Return_To_Origin(Color.white, new Color(159 / 255, 43 / 255, 43 / 255), 0.125f, false));
        
        yield return YieldInstructionCache.WaitForSeconds(0.25f);

        Instantiate(Blink, transform.position, Quaternion.identity);

        yield return backGroundColor.StartCoroutine(backGroundColor.Change_Color_Return_To_Origin(backGroundColor.Get_BGColor(), new Color(1, 1, 1, 1), 1, false));

        Unbeatable = false;
        playerCtrl_Tengai.Unbeatable = false;

        foreach (var e in DoPhan_Ready_To_Pattern_Move)
            yield return StartCoroutine(Position_Lerp(transform.position, transform.position + e, 0.125f, OriginCurve));

        yield return YieldInstructionCache.WaitForSeconds(0.5f);
    }
   
    IEnumerator Repeat_Phase()
    {
        //yield return StartCoroutine(Pattern01());
        //yield return StartCoroutine(Pattern02());
        //yield return StartCoroutine(Pattern03());
        //yield return StartCoroutine(Pattern05());
        //yield return StartCoroutine(Ready_To_Pattern());
        //yield return StartCoroutine(Pattern06());
        yield return StartCoroutine(Ready_To_Pattern());
        yield return StartCoroutine(Pattern01());
        yield return StartCoroutine(Pattern04());
        //while (true)
        //{
        //    Pattern_Num = Random.Range(0, 6);
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
        //        case 5:
        //            Pattern_Total[Pattern_Num] = Pattern06();
        //            break;
        //    }
        //    if (Pattern_Num == 5 || Pattern_Num == 4)
        //        yield return StartCoroutine(Ready_To_Pattern());
        //    yield return StartCoroutine((IEnumerator)Pattern_Total[Pattern_Num]);
        //}
    }
    IEnumerator Pattern01()
    {
        yield return StartCoroutine(Boss_Move());

        Unbeatable = true;

        yield return StartCoroutine(Change_Color_Return_To_Origin(Color.white, new Color(0, 0, 0, 1), 1, false));

        Launch_Weapon_For_Move(ref Weapon[0], new Vector3(1, 0.5714f, 0), Quaternion.identity, 8, transform.position);
        Launch_Weapon_For_Move(ref Weapon[0], new Vector3(1, -0.5714f, 0), Quaternion.Euler(new Vector3(0, 0, -60)), 8, transform.position);
        Launch_Weapon_For_Move(ref Weapon[0], new Vector3(-1, 0.5714f, 0), Quaternion.Euler(new Vector3(0, 0, -60)), 8, transform.position);
        Launch_Weapon_For_Move(ref Weapon[0], new Vector3(-1, -0.5714f, 0), Quaternion.identity, 8, transform.position);

        yield return YieldInstructionCache.WaitForSeconds(1f);

        change_boss_color = Change_Color_Return_To_Origin(Color.white, new Color(159 / 255, 43 / 255, 43 / 255), 0.25f, true);
        StartCoroutine(change_boss_color);
        Start_Camera_Shake(0.03f, 1.5f, true, false);

        Launch_Weapon_For_Move(ref Weapon[1], Vector3.zero, Quaternion.Euler(new Vector3(0, 0, -9)), 0, new Vector3(0, 4, 0));
        Launch_Weapon_For_Move(ref Weapon[1], Vector3.zero, Quaternion.Euler(new Vector3(0, 0, -9)), 0, new Vector3(0, -4.5f, 0));
        Launch_Weapon_For_Move(ref Weapon[2], Vector3.zero, Quaternion.Euler(new Vector3(0, 0, -115)), 0, new Vector3(-8, 0, 0));
        Launch_Weapon_For_Move(ref Weapon[2], Vector3.zero, Quaternion.Euler(new Vector3(0, 0, -115)), 0, new Vector3(6.6f, 0, 0));

        StopCoroutine(change_boss_color);

        yield return backGroundColor.StartCoroutine(backGroundColor.Thunder(5, 0.5f));
        backGroundColor.Origin();

        Unbeatable = false;

        yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(7, 0, 0), 1, declineCurve));
    }
    IEnumerator Pattern02()
    {
        GameObject c = Instantiate(Charge_Beam, transform.position + new Vector3(-1.22f, 0, 0), Quaternion.identity);

        if (c.TryGetComponent(out Charge_Beam_Motion user1))
        {
            Unbeatable = true;

            charge_beam = user1.Change_Size();
            user1.StartCoroutine(charge_beam);

            yield return StartCoroutine(Warning("어디 한 번 지혜를 발휘해봐라", 1f));

            user1.StopCoroutine(charge_beam);
            Destroy(c);
        }

        Unbeatable = false;

        if (playerCtrl_Tengai.transform.position.x >= 3.5f && playerCtrl_Tengai.transform.position.x <= 8f && playerCtrl_Tengai.transform.position.y <= -2.6f)
        {
            Pattern2_Meteor_Random_Num = Random.Range(1, 4);
            switch(Pattern2_Meteor_Random_Num)
            {
                case 1:
                    yield return StartCoroutine(Pattern02_When_Player_Avoid(0));
                    break;
                case 2:
                    yield return StartCoroutine(Pattern02_When_Player_Avoid(1));
                    break;
                case 3:
                    yield return StartCoroutine(Pattern02_When_Player_Avoid(2));
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
                    Launch_Weapon_For_Move(ref Weapon[3], new Vector3(-1, param, 0), Quaternion.identity, 80, transform.position);
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
                Launch_Weapon_For_Move(ref Weapon[Pattern2_Beam_Spawn[kuku, j]], new Vector3(-1, param, 0), Quaternion.identity, 80, transform.position);
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
            if (e.TryGetComponent(out Meteor_Effect user1))
                user1.StartCoroutine(user1.Pattern02_Meteor(Rand));
            else
                Destroy(e);
            yield return YieldInstructionCache.WaitForSeconds(0.2f);
        }
        yield return null;
    }
    IEnumerator Pattern03()
    {
        yield return StartCoroutine(Position_Lerp(transform.position, Vector3.zero, 1, declineCurve));

        yield return StartCoroutine(Rotate_Bullet(7, 250, .5f, 4, Weapon[5]));

        yield return StartCoroutine(Position_Lerp(Vector3.zero, new Vector3(0, 3, 0), 1, declineCurve));

        yield return StartCoroutine(Circle_Move(270, -1, 0, 7, 3, 0, 0, 2));
    }
    IEnumerator Pattern04() // + 플레이어가 시련을 겪다가 방출하는 것도 추가해야한다. 하나라도 데미지 맞으면 정화 취소.
    {
        Unbeatable = true;
        yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(12, transform.position.y, 0), 1, declineCurve));

        playerCtrl_Tengai.Start_Emit();

        enemy_spawn = Enemy_Spawn();
        StartCoroutine(enemy_spawn);

        for (int i = 0; i < 7; i++)
        {
            int Random_Num = Random.Range(0, 4);
            if (meteor_launch != null)
                StopCoroutine(meteor_launch);
            switch (Random_Num)
            {
                case 0:
                    meteor_launch = Meteor_Launch(9, 1);
                    yield return StartCoroutine(meteor_launch);
                    break;
                case 1:
                    meteor_launch = Meteor_Launch(6, 1);
                    yield return StartCoroutine(meteor_launch);
                    break;
                case 2:
                    meteor_launch = Meteor_Launch(3, 3);
                    yield return StartCoroutine(meteor_launch);
                    break;
                case 3:
                    meteor_launch = Meteor_Launch(1, 8);
                    yield return StartCoroutine(meteor_launch);
                    break;
            }
            yield return YieldInstructionCache.WaitForSeconds(1.5f);
        }
        if (meteor_launch != null)
            StopCoroutine(meteor_launch);

        if (enemy_spawn != null)
            StopCoroutine(enemy_spawn);

        Killed_All_Mine();

        Unbeatable = false;
        yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(7, transform.position.y, 0), 1, declineCurve));
        yield return YieldInstructionCache.WaitForSeconds(0.5f);
    }
    public void Stop_Meteor()
    {
        StartCoroutine(I_Stop_Meteor());
    }
    IEnumerator I_Stop_Meteor()
    {
        if (meteor_launch != null)
            StopCoroutine(meteor_launch);

        if (enemy_spawn != null)
            StopCoroutine(enemy_spawn);

        if ((IEnumerator)Pattern_Total[Pattern_Num] != null)
            StopCoroutine((IEnumerator)Pattern_Total[Pattern_Num]);

        if (repeat_phase != null)
            StopCoroutine(repeat_phase);

        yield return YieldInstructionCache.WaitForSeconds(2f);

        Unbeatable = false;
        StartCoroutine(Shake_Act(0.3f, 0, 2, false));
        yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(7, 0, 0), 2, declineCurve));
        
        yield return YieldInstructionCache.WaitForSeconds(2f); // 플레이어에게 무차별 폭격 받는 곳
        repeat_phase = Repeat_Phase();
        StartCoroutine(repeat_phase);
        yield return null;
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
                    if (e.TryGetComponent(out Meteor_Effect user1))
                        user1.StartCoroutine(user1.Meteor_Launch_Act(Meteor_Move[i], R1, R2, R3));
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
                        if (e.TryGetComponent(out Meteor_Effect user1))
                            user1.StartCoroutine(user1.Meteor_Launch_Act(Meteor_Move[j], R1, R2, R3));
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
                    if (e.TryGetComponent(out Meteor_Effect user1))
                        user1.StartCoroutine(user1.Meteor_Launch_Act(Meteor_Move[i], R1, R2, R3));
                    else
                        Destroy(e);
                }
                break;
        }
        yield return null;
    }

    IEnumerator Pattern06()
    {
        moveBackGround_1.StartCoroutine(moveBackGround_1.Increase_Speed(0.3f, 16));
        yield return moveBackGround_2.StartCoroutine(moveBackGround_2.Increase_Speed(0.3f, 16));

        yield return StartCoroutine(Change_Color_Return_To_Origin(Color.white, new Color(159 / 255, 43 / 255, 43 / 255), 0.8f, false));

        transform.position = new Vector3(7, 0, 0);
        pattern06_weapon = Pattern06_Weapon();
        StartCoroutine(pattern06_weapon);

        yield return StartCoroutine(Position_Lerp(new Vector3(7, 0, 0), new Vector3(7, 3, 0), 1.5f, De_In_Curve));
        for (int i = 0; i < 4; i++)
        {
            yield return StartCoroutine(Position_Lerp(new Vector3(7, 3, 0), new Vector3(7, -3, 0), 1.5f, De_In_Curve));
            yield return StartCoroutine(Position_Lerp(new Vector3(7, -3, 0), new Vector3(7, 3, 0), 1.5f, De_In_Curve));
        }

        StopCoroutine(pattern06_weapon);
        int flag = 1;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
                Launch_Weapon_For_Move(ref Weapon[6], Vector3.left, Quaternion.identity, 12, new Vector3(7, 5 - (1.5f * j) + (flag * 0.5f), 0));
            flag = -flag;
            yield return YieldInstructionCache.WaitForSeconds(1f);
        }
    }

    IEnumerator Pattern06_Weapon()
    {
        float Randomly = transform.position.y;
        while(true)
        {
            float ee = transform.position.y - Randomly;
            Vector3 normal_dir = new Vector3(-1, ee, 0).normalized;

            Launch_Weapon_For_Move(ref Weapon[6], normal_dir, Quaternion.identity, 10, transform.position + new Vector3(-0.5f, 2f, 0));
            Launch_Weapon_For_Move(ref Weapon[6], normal_dir, Quaternion.identity, 10, transform.position + new Vector3(-0.5f, -2f, 0));

            Randomly = transform.position.y;
            yield return YieldInstructionCache.WaitForSeconds(Time.deltaTime * 20);
        }
    }

    IEnumerator Pattern05()
    {
        int Random_Move;
        Color Alpha_1 = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1);
        Color Alpha_0 = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0);
        for (int i = 0; i < 15; i++)
        {
            Random_Move = Random.Range(0, 6);

            yield return StartCoroutine(Change_Color_Lerp(Alpha_1, Alpha_0, 0.1f, .2f, DisAppear_Effect_1));

            transform.position = DoPhan_Pattern05_Move[Random_Move];

            yield return StartCoroutine(Change_Color_Lerp(Alpha_0, Alpha_1, 0.1f, .4f, DisAppear_Effect_1));
        }
        yield return YieldInstructionCache.WaitForSeconds(0.5f);

        yield return StartCoroutine(Position_Slerp(transform.position, new Vector3(7, 0, 0),
            Vector3.zero, 2, declineCurve, false));
    }
    IEnumerator Boss_Move()
    {
        foreach (var e in DoPhan_Pattern01_Move)
            yield return StartCoroutine(Position_Lerp(transform.position, e, .25f, declineCurve));
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}