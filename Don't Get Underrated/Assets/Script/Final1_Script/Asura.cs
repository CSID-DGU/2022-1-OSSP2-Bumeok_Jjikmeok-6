using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Asura : Boss_Info // 최종 (1)의 보스
{
    [SerializeField]
    float[] Pattern04_Meteor_Move = new float[9] { 4.9f, 3.3f, 1.7f, 0.1f, -1.5f, -3.1f, -4.7f, 0.1f, -3.1f };  // 패턴 4에서 운석이 나오는 위치

    [SerializeField]
    float[,] Pattern02_Meteor_Move = new float[3, 4] { { 5, 6, 7, 8 }, { 3, 4, 7, 8 }, { 3, 4, 5, 6 } }; // 패턴 2에서 운석이 나오는 위치

    [SerializeField]
    float backGround_Speed = 16; // 뒷 배경이 움직이는 기본 속도

    [SerializeField]
    List<Set_Slerp_Move> Asura_Appearance = new List<Set_Slerp_Move>() 
    {  new Set_Slerp_Move(new Vector3(-6.61f, 2.21f, 0), "anti_clock"),  new Set_Slerp_Move(new Vector3(-5f, 3.66f, 0), "anti_clock"), 
        new Set_Slerp_Move(new Vector3(6.85f, -2.9f, 0), "clock"), new Set_Slerp_Move(new Vector3(-6.52f, -0.6f, 0), "anti_clock"), 
        new Set_Slerp_Move(new Vector3(7, 0, 0), "anti_clock" )
    }; // 처음 등장할 때의 이동 위치 설정

    [SerializeField]
    List<Vector3> Asura_Pattern01_Move = new List<Vector3>()  { 
        new Vector3(7, 2, 0), new Vector3(-7, 2, 0), new Vector3(3, -4, 0), new Vector3(0, 4, 0), new Vector3(-3, 4, 0), new Vector3(7, 2, 0)
    };  // 패턴 1 시작 시의 이동 위치 설정

    [SerializeField]
    List<Vector3> Asura_Pattern05_Move = new List<Vector3>()  { 
        new Vector3(-6.56f, 1.72f, 0), new Vector3(-3.42f, 1.72f, 0), new Vector3(0.28f, 1.72f, 0), new Vector3(3.7f, 1.72f, 0), new Vector3(5, 1.72f, 0),
        new Vector3(-6.56f, 0, 0), new Vector3(-3.42f, 0, 0), new Vector3(0.28f, 0, 0), new Vector3(3.7f, 0, 0), new Vector3(5, 0, 0),
        new Vector3(-6.56f, -1.72f, 0), new Vector3(-3.42f, -1.72f, 0), new Vector3(0.28f, -1.72f, 0), new Vector3(3.7f, -1.72f, 0), new Vector3(5, -1.72f, 0)
    }; // 패턴 5의 순간이동 시의 이동 위치 설정

    [SerializeField]
    List<Vector3> Asura_Ready_To_Pattern_Move = new List<Vector3>()
    {
        new Vector3(-1, 1, 0), new Vector3(2, 0, 0), new Vector3(-1, -2, 0), new Vector3(-1, 1, 0), new Vector3(2, 0, 0),
        new Vector3(-1, 1, 0), new Vector3(0, -1, 0)
    }; // 보스의 패턴 준비 중의 이동 위치 설정

    [SerializeField]
    GameObject Charge_Beam; // 즉사기를 위한 차지 파티클

    [SerializeField]
    GameObject Meteor1; // 패턴 2에서 쓰이는 운석

    [SerializeField]
    GameObject Meteor2; // 패턴 4에서 쓰이는 운석

    [SerializeField]
    GameObject Homming_Enemy; // 패턴 4에서 스폰하는 적

    [SerializeField]
    GameObject Blink; // 패턴 준비 시의 블링크

    Player_Final1 player_final1; // 최종 1에서의 플레이어 정보

    MoveBackGround moveBackGround_1, moveBackGround_2; // 보스가 뒷배경을 움직이는 주체이므로, 이를 설정

    IEnumerator enemy_spawn, meteor_launch, change_boss_color, repeat_pattern, pattern06_weapon, pattern, shake_act;

    int Total_Pattern_Num = 0; // 본인

    int[,] Pattern02_Beam_Spawn = new int[3, 7] { { 4, 4, 4, 3, 3, 3, 3 }, { 3, 3, 3, 4, 3, 3, 3 }, { 3, 3, 3, 3, 4, 4, 4 } }; // 패턴 2에서 플레이어가
    // 외곽으로 즉사기를 피했을 때 나오는 빔 (4 --> 무지개 색 빔. 3 --> 일반 빨간색 빔)

    private new void Awake()
    {
        base.Awake();
        My_Position = new Vector3(0, -9, 0);
        My_Scale = new Vector3(1.2f, 1.2f, 0);
        Unbeatable = true;
        WarningText.color = new Color(WarningText.color.r, WarningText.color.g, WarningText.color.b, 0);
        pattern = null;
        Total_Pattern_Num = 0;

        imageColor = GameObject.FindGameObjectWithTag("Flash").GetComponent<ImageColor>();
        moveBackGround_1 = GameObject.FindGameObjectWithTag("BackGround1").GetComponent<MoveBackGround>();
        moveBackGround_2 = GameObject.FindGameObjectWithTag("BackGround2").GetComponent<MoveBackGround>();
        player_final1 = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Final1>();
        if (GameObject.Find("Boss_Effect_Sound") && GameObject.Find("Boss_Effect_Sound").TryGetComponent(out AudioSource AS1))
            EffectSource = AS1;
        if (GameObject.Find("Boss_BackGround_Sound") && GameObject.Find("Boss_BackGround_Sound").TryGetComponent(out AudioSource AS2))
            BackGroundSource = AS2;
    }
    private new void OnTriggerEnter2D(Collider2D collision) // 얘만
    {
        base.OnTriggerEnter2D(collision);
    }

    public override void TakeDamage(float damage)
    {
        float Rand = Random.Range(0.0f, 1.0f);
        if (!Unbeatable) // 무적 상태일 때만 적용
        {
            Effect_Sound_OneShot(15);
            CurrentHP -= damage;

            if (Rand <= 0.01f)
                player_final1.Speed_Up(); // 1퍼센트의 확률로 플레이어에게 스피드 업 아이템 부여
            if (Rand > 0.01f && Rand <= 0.02f)
                player_final1.Power_Up(); // 1퍼센트의 확률로 플레이어에게 파워 업 아이템 부여
            if (Rand > 0.02f && Rand <= 0.023f)
                player_final1.Boom_Up(); // 0.5퍼센트의 확률로 플레이어에게 폭탄 증가 아이템 부여

            Run_Life_Act(Hit());
            if (CurrentHP <= 0) // 현재 HP가 0이면 죽도록
            {
                Unbeatable = true;
                OnDie();
            }
        }
    }
    IEnumerator Hit() // 플레이어의 공격에 맞을 때의 로직. (카메라를 살짝 흔들고, 본인의 색상의 채도를 줄여 맞았다는 것을 플레이어가 확인하도록)
    {
        Camera_Shake(0.009f, 0.05f, true, false);

        My_Color = new Color(1, 1, 1, 0.25f);
        yield return YieldInstructionCache.WaitForSeconds(0.07f);
        My_Color = Color.white;
        yield return null;
    }

    public override void OnDie()
    {
        singleTone.Music_Decrease = false;
        Effect_Sound_Stop();
        Effect_Sound_OneShot(16);
        player_final1.Unbeatable = true;
        My_Position = new Vector3(7, 0, 0);
        My_Scale = new Vector3(0.6f, 0.6f, 0);
        WarningText.color = Color.clear;
        imageColor.Set_BGColor(Color.clear); // 사운드 중지, 플레이어 무적화, 본인의 원래 모습으로 초기화.

        singleTone.final_stage_1_score = player_final1.DeathCount;
        //Debug.Log(singleTone.final_stage_1_score); // 플레이어의 데스 카운트를 최종(1)의 스코어로 반영

        Instantiate(When_Dead_Effect, My_Position, Quaternion.identity); // 본인이 죽었을 때의 이펙트 생성

        Killed_All_Mine();

        Run_Life_Act(Boss_Die_After()); // Boss_Die_After 로직 수행
    }
    IEnumerator Boss_Die_After() // ondie 함수 이후 호출되는 로직
    { // 배경 움직임 감소 --> 배경 검은색으로 변화
        Flash(Color.white, 0.2f, 5);
        Run_Life_Act(Decrease_BackGround_Sound(6));
        Run_Life_Act(Shake_Act(0.2f, 0, 10, false));

        moveBackGround_1.Decrease_Speed_Func(8, 0);
        yield return moveBackGround_2.Decrease_Speed_And_Wait(8, 0);

        yield return YieldInstructionCache.WaitForSeconds(2f);

        yield return Change_BG_And_Wait(Color.black, 2);

        singleTone.Music_Decrease = true;
        singleTone.SceneNumManage++;
        SceneManager.LoadScene(singleTone.SceneNumManage);

        yield break;
    }
    public void Pattern_Start() // 페이즈 시작
    {
        Run_Life_Act(Boss_Apprearance());
    }

    IEnumerator Boss_Apprearance() // 보스의 등장 시의 로직
    {
        Run_Life_Act(Change_My_Size(My_Scale, My_Scale * 0.5f, 5, OriginCurve));

        My_Position = new Vector3(0, -8, 0); // 처음 위치를 0, -8, 0으로 설정

        float standard_distance = -1;
        int i = 0;

        foreach (var move in Asura_Appearance) // 보스가 처음 등장할 때의 곡선 이동
        {
            float first_distance = Get_Curve_Distance(My_Position, move.Next_Position, Get_Center_Vector_For_Curve_Move(My_Position, move.Next_Position,
                Vector3.Distance(My_Position, move.Next_Position) * 0.85f, move.Dir));
            if (standard_distance == -1)
                standard_distance = first_distance;
            yield return Move_Curve(My_Position, move.Next_Position,
                Get_Center_Vector_For_Curve_Move(My_Position, move.Next_Position, Vector3.Distance(My_Position, move.Next_Position) * 0.85f, move.Dir),
                (0.4f + 0.2f * i++) * (first_distance / standard_distance), OriginCurve);
        }

        moveBackGround_1.Increase_Speed_Func(8, backGround_Speed);
        yield return moveBackGround_2.Increase_Speed_And_Wait(8, backGround_Speed);

        BackGround_Sound_Play(0);
        player_final1.Unbeatable = false;
        My_Position = new Vector3(7, 0, 0);
        My_Scale = new Vector3(0.6f, 0.6f, 0);

        yield return Ready_To_Pattern();
        Run_Life_Act_And_Continue(ref repeat_pattern, Repeat_Pattern());

        yield return null;
    }

    IEnumerator Ready_To_Pattern() // 보스가 패턴을 시작하기 전 행위를 취하는 로직
    {
        Unbeatable = true;

        Effect_Sound_Play(0);
        for (int i = 0; i < 2; i++)
            yield return Change_My_Color_And_Back(Color.white, new Color(159 / 255, 43 / 255, 43 / 255), 0.125f, false);
        Effect_Sound_Stop();

        yield return YieldInstructionCache.WaitForSeconds(0.25f);

        Instantiate(Blink, My_Position, Quaternion.identity);
        Effect_Sound_OneShot(1);
        yield return Change_BG_And_Wait(Color.white, 0.8f);
        yield return Change_BG_And_Wait(Color.clear, 0.8f);

        Unbeatable = false;

        foreach (var e in Asura_Ready_To_Pattern_Move)
            yield return Move_Straight(My_Position, My_Position + e, 0.125f, OriginCurve);

        yield return YieldInstructionCache.WaitForSeconds(0.25f);
    }
   
    IEnumerator Repeat_Pattern() // 보스의 공격 패턴 반복 (랜덤 패턴) 
    {
        while (true)
        {
            Total_Pattern_Num = Random.Range(0, 6);
            switch (Total_Pattern_Num)
            {
                case 0: pattern = Pattern01(); break;
                case 1: pattern = Pattern02(); break;
                case 2: pattern = Pattern03(); break;
                case 3: pattern = Pattern04(); break;
                case 4: pattern = Pattern05(); break;
                case 5: pattern = Pattern06(); break;
            }
            if (Total_Pattern_Num == 3 || Total_Pattern_Num == 5) // 패턴 4, 패턴 6 시작 전에는 패턴 준비 행위를 취하도록 추가 설정
                yield return Ready_To_Pattern();
            yield return pattern;
        }
    }
    IEnumerator Pattern01() // 패턴 1
    {
        Unbeatable = true;

        yield return Change_My_Size(My_Scale, 1.5f * My_Scale, 0.2f, OriginCurve);
        yield return Move_Straight(My_Position, Asura_Pattern01_Move[0], 0.2f, declineCurve);

        Effect_Sound_OneShot(2);
        trailRenderer.enabled = true;
        yield return Boss_Move();

        for (int i = 0; i < 2; i++)
        {
            Effect_Sound_Play(3);
            yield return Flash_And_Wait(Color.black, 0, 0.8f);
        }
           
        trailRenderer.enabled = false;

        yield return Change_My_Size(My_Scale, My_Scale / 1.5f, 0.1f, OriginCurve);
        yield return Move_Straight(My_Position, Vector3.zero, 0.2f, declineCurve);

        Effect_Sound_Play(4);
        Launch_Weapon(ref Weapon[0], new Vector3(1, 0.5714f, 0), Quaternion.identity, 12, My_Position);
        Launch_Weapon(ref Weapon[0], new Vector3(1, -0.5714f, 0), Quaternion.Euler(new Vector3(0, 0, -60)), 12, My_Position);
        Launch_Weapon(ref Weapon[0], new Vector3(-1, 0.5714f, 0), Quaternion.Euler(new Vector3(0, 0, -60)), 12, My_Position);
        Launch_Weapon(ref Weapon[0], new Vector3(-1, -0.5714f, 0), Quaternion.identity, 12, My_Position);
        
        yield return YieldInstructionCache.WaitForSeconds(0.5f);

        Effect_Sound_Play(5);
        Run_Life_Act_And_Continue(ref change_boss_color, Change_My_Color_And_Back(Color.white, Color.black, 0.25f, true));

        Camera_Shake(0.025f, 1.5f, true, false);

        Launch_Weapon(ref Weapon[1], Vector3.zero, Quaternion.Euler(new Vector3(0, 0, -9)), 0, new Vector3(0, 4, 0));
        Launch_Weapon(ref Weapon[1], Vector3.zero, Quaternion.Euler(new Vector3(0, 0, -9)), 0, new Vector3(0, -4.5f, 0));
        Launch_Weapon(ref Weapon[2], Vector3.zero, Quaternion.Euler(new Vector3(0, 0, -115)), 0, new Vector3(-8, 0, 0));
        Launch_Weapon(ref Weapon[2], Vector3.zero, Quaternion.Euler(new Vector3(0, 0, -115)), 0, new Vector3(6.6f, 0, 0));

        for (int i = 0; i < 5; i++)
            yield return Flash_And_Wait(Random.ColorHSV(), 0.1f, 0.5f);

        Effect_Sound_Stop();

        Stop_Life_Act(ref change_boss_color);
        My_Color = Color.white;
        Unbeatable = false;

        yield return Move_Straight(My_Position, new Vector3(7, 0, 0), 1, declineCurve); 
    }
    IEnumerator Pattern02() // 패턴 2 - 즉사기 발사
    {
        GameObject Light_To_Death = Instantiate(Charge_Beam, My_Position, Quaternion.identity);
        
        if (Light_To_Death.TryGetComponent(out ParticleSystem PS)) // 차지 파티클이 존재할 시에만 차지 모션 적용
        {
            Effect_Sound_Play(6);
            Unbeatable = true;
            yield return Warning(Color.red, "선정과 지혜를 갖춘 사람은 실로 열반에 가까워지리.", 1);

            if (PS.IsAlive())
                Destroy(PS.gameObject);
        }

        Unbeatable = false;

        Effect_Sound_Play(7);
        if (player_final1.My_Position.x >= 3 && player_final1.My_Position.x <= 8f && player_final1.My_Position.y <= -2.2f)
            // 플레이어가 해당 위치로 이동하여 즉사기를 피하면 Pattern02_When_Player_Avoid() 로직으로 이동
        {
            int Avoid_Pos = Random.Range(0, 3);
            yield return Pattern02_When_Player_Avoid(Avoid_Pos);
        }
        else // 그게 아니라면 7갈래 방향으로 빨간 즉사기 발사
        {
            for (int i = 0; i < 30; i++)
            {
                float param = 0.5714f;
                for (int j = 0; j < 7; j++)
                {
                    Launch_Weapon(ref Weapon[3], new Vector3(-1, param, 0), Quaternion.identity, 100, My_Position);
                    param -= 0.1904f;
                }
                yield return YieldInstructionCache.WaitForSeconds(0.1f);
            }
        }
        Effect_Sound_Stop();
        yield break;
    }
    IEnumerator Pattern02_When_Player_Avoid(int Avoid_Pos)
    {
        Run_Life_Act(Pattern02_Meteor_Down(Avoid_Pos));
        // 보스는 추가로 플레이어가 피한 위치에 운석 발사를 해야하므로 Pattern02_Meteor_Down() 로직을 처리한다

        for (int i = 0; i < 30; i++)
        {
            float param = 0.5714f;
            for (int j = 0; j < 7; j++)
            {
                Launch_Weapon(ref Weapon[Pattern02_Beam_Spawn[Avoid_Pos, j]], new Vector3(-1, param, 0), Quaternion.identity, 100, My_Position);
                param -= 0.1904f;
            }
            yield return YieldInstructionCache.WaitForSeconds(0.1f);
        }
        // Pattern02_Beam_Spawn 배열에 따라 빔의 색깔을 랜덤으로 설정후, 즉사기 발사
        yield return null;
    }
    IEnumerator Pattern02_Meteor_Down(int Avoid_Pos)
    { 
        yield return YieldInstructionCache.WaitForSeconds(0.4f);
        for (int i = 0; i < 10; i++)
        {
            int Rand = Random.Range(0, 4);
            GameObject e = Instantiate(Meteor1, new Vector3(Pattern02_Meteor_Move[Avoid_Pos, Rand], 5.6f, 0), Quaternion.identity);
            if (e.TryGetComponent(out Meteor_Effect ME))
                ME.Pattern02_Meteor_Launch(Pattern02_Meteor_Move[Avoid_Pos, Rand]);
            else
                Destroy(e);
            yield return YieldInstructionCache.WaitForSeconds(0.1f);
            // Pattern02_Meteor_Move = { 5, 6, 7, 8 }, { 3, 4, 7, 8 }, { 3, 4, 5, 6 }
            // Avoid_Pos == 0 -->  x 좌표 중 5, 6, 7, 8에 랜덤으로 운석 발사
            // Avoid_Pos == 1 -->  x 좌표 중 3, 4, 7, 8에 랜덤으로 운석 발사
            // Avoid_Pos == 2 -->  x 좌표 중 3, 4, 5, 6에 랜덤으로 운석 발사
        }
        yield return null;
    }
    IEnumerator Pattern03() // 패턴 3
    {
        Effect_Sound_OneShot(9);
        yield return Move_Straight(My_Position, Vector3.zero, 0.7f, declineCurve);

        Effect_Sound_Play(8);
        yield return Rotate_Bullet(7, 250, .5f, 4, Weapon[5]);
        Effect_Sound_Stop();

        Effect_Sound_OneShot(9);
        yield return Move_Straight(Vector3.zero, new Vector3(0, 3, 0), 1, declineCurve);

        yield return Move_Circle(270, -1, 0, 7, 3, 0, 0, 1);
    }
    IEnumerator Pattern04() // 패턴 4
    {
        Unbeatable = true;
        yield return Warning(Color.red, "인내심을 가져라.\n모든 것은 적당한 때에 결국 네게 올테니.", 1f);

        Effect_Sound_OneShot(9);
        yield return Move_Straight(My_Position, new Vector3(12, My_Position.y, 0), 1, declineCurve);
        player_final1.Start_Emit(); // 플레이어가 기를 모아 필살기를 사용할 수 있도록 함. 

        Run_Life_Act_And_Continue(ref enemy_spawn, Enemy_Spawn());

        for (int i = 0; i < 10; i++) // 총 10번 동안 운석 발사 실시
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
            yield return YieldInstructionCache.WaitForSeconds(1.5f);
        }

        Stop_Life_Act(ref enemy_spawn);

        Unbeatable = false;
        yield return Move_Straight(My_Position, new Vector3(7, My_Position.y, 0), 1, declineCurve);
        yield return null;
    }
    public void Stop_Meteor() // 플레이어가 8초간 모든 운석을 피하고 필살기를 썼을 떄의 로직 처리
    {
        Killed_All_Mine();
        Run_Life_Act(I_Stop_Meteor());
    }
    IEnumerator I_Stop_Meteor()
    {
        yield return YieldInstructionCache.WaitForSeconds(2f);

        Unbeatable = false;
        Run_Life_Act_And_Continue(ref shake_act, Shake_Act(0.3f, 0, 20, false));
        yield return Move_Straight(My_Position, new Vector3(7, 0, 0), 2, declineCurve);

        for (int i = 0; i < 3; i++)
            yield return Warning(Color.blue, "CHANCE!!!", 1.5f);

        // 해당 시간 동안은 플레이어가 무차별적으로 보스를 공격할 수 있음

        Stop_Life_Act(ref shake_act);
        transform.localRotation = Quaternion.identity;
        Unbeatable = true;

        Effect_Sound_Play(13);
        for (int i = 0; i < 2; i++)
        {
            yield return Change_BG_And_Wait(new Color(1, 0, 0, 0.7f), 0.3f);
            yield return Change_BG_And_Wait(Color.clear, 0.3f);
        }
        Effect_Sound_Stop();

        Unbeatable = false;
        Run_Life_Act_And_Continue(ref repeat_pattern, Repeat_Pattern());
        yield return null;
    }
    IEnumerator Enemy_Spawn() // 패턴 4에서 1초 간격으로 적 스폰
    {
        while(true)
        {
            Instantiate(Homming_Enemy, new Vector3(4, 0, 0), Quaternion.identity);
            yield return YieldInstructionCache.WaitForSeconds(1);
        }
    }
    IEnumerator Meteor_Launch(int Meteor_Num, int Launch_Count) // 패턴 4에서 운석을 발사하는 로직
    {
        float R1 = Random.Range(.15f, 1); float R2 = Random.Range(.15f, 1); float R3 = Random.Range(.15f, 1);
        Pattern04_Meteor_Move = StaticFunc.ShuffleList(Pattern04_Meteor_Move);
        switch (Launch_Count)
        {   
            case 1:
                for (int i = 0; i < Meteor_Num; i++) // 0.2초 간격으로 9개 내지 6개 운석 발사
                {
                    GameObject e = Instantiate(Meteor2, new Vector3(10, Pattern04_Meteor_Move[i], 0), Quaternion.identity);
                    if (e.TryGetComponent(out Meteor_Effect ME1))
                        ME1.Pattern04_Meteor_Launch(Pattern04_Meteor_Move[i], R1, R2, R3);
                    else
                        Destroy(e);
                    yield return YieldInstructionCache.WaitForSeconds(0.2f);
                }
                break;

            case 3:
                for (int i = 0; i < 3; i++) // 0.5초 간격으로 3번 * 3개의 운석 동시 발사
                {
                    R1 = Random.Range(.15f, 1); R2 = Random.Range(.15f, 1);  R3 = Random.Range(.15f, 1);
                    for (int j = 3 * i; j < 3 * (i + 1); j++)
                    {
                        GameObject e = Instantiate(Meteor2, new Vector3(10, Pattern04_Meteor_Move[j], 0), Quaternion.identity);
                        if (e.TryGetComponent(out Meteor_Effect ME3))
                            ME3.Pattern04_Meteor_Launch(Pattern04_Meteor_Move[j], R1, R2, R3);
                        else
                            Destroy(e);
                    }
                    yield return YieldInstructionCache.WaitForSeconds(0.5f);
                }
                break;

            case 8: // 8개의 운석 동시 발사
                for (int i = 0; i < 8; i++)
                {
                    GameObject e = Instantiate(Meteor2, new Vector3(10, Pattern04_Meteor_Move[i], 0), Quaternion.identity);
                    if (e.TryGetComponent(out Meteor_Effect ME8))
                        ME8.Pattern04_Meteor_Launch(Pattern04_Meteor_Move[i], R1, R2, R3);
                    else
                        Destroy(e);
                }
                break;
        }
        yield return null;
    }

    IEnumerator Pattern06() // 패턴 6
    {
        Unbeatable = true;

        moveBackGround_1.Increase_Speed_Func(5, backGround_Speed * 3);
        yield return moveBackGround_2.Increase_Speed_And_Wait(5, backGround_Speed * 3);

        Effect_Sound_Play(10);
        yield return Change_My_Color_And_Back(Color.white, Color.black, 0.8f, false);
        Unbeatable = false;

        Run_Life_Act_And_Continue(ref pattern06_weapon, Pattern06_Weapon());

        Effect_Sound_Play(11);
        yield return Move_Straight(new Vector3(7, 0, 0), new Vector3(7, 3, 0), 1.2f, De_In_Curve);
        for (int i = 0; i < 7; i++)
        {
            yield return Move_Straight(new Vector3(7, 3, 0), new Vector3(7, -3, 0), 1.2f, De_In_Curve);
            yield return Move_Straight(new Vector3(7, -3, 0), new Vector3(7, 3, 0), 1.2f, De_In_Curve);
        }

        Stop_Life_Act(ref pattern06_weapon);

        Effect_Sound_Play(10);
        Run_Life_Act(Change_My_Color_And_Back(Color.white, Color.black, 1f, false));
        yield return Move_Straight(My_Position, new Vector3(7, 0, 0), 1f, declineCurve);
        Effect_Sound_Stop();

        int flag = 1;
        for (int i = 0; i < 12; i++) // 0.7초 간격으로 총알이 일정 상, 하 간격(현재는 2.8)을 두고 발사되는 로직
        {
            Effect_Sound_OneShot(17);
            for (int j = 0; j < 9; j++)
                Launch_Weapon(ref Weapon[6], Vector3.left, Quaternion.identity, 15, new Vector3(7, 6 - (2.8f * j) + (flag * 0.5f), 0));
            flag = -flag;
            yield return YieldInstructionCache.WaitForSeconds(0.7f);
        }

        Unbeatable = true;
        moveBackGround_1.Decrease_Speed_Func(5, backGround_Speed);
        yield return moveBackGround_2.Decrease_Speed_And_Wait(5, backGround_Speed);
        Unbeatable = false;
    }

    IEnumerator Pattern06_Weapon() // 상, 하로 움직이면서 웨이브 형식으로 총알 발사
    {
        float Randomly = My_Position.y;
        while(true)
        {
            float Y_displacement = My_Position.y - Randomly;
            Vector3 normal_dir = new Vector3(-1, Y_displacement, 0).normalized;

            Launch_Weapon(ref Weapon[6], normal_dir, Quaternion.identity, 13, My_Position + new Vector3(-0.5f, 2.5f, 0));
            Launch_Weapon(ref Weapon[6], normal_dir, Quaternion.identity, 13, My_Position + new Vector3(-0.5f, -2.5f, 0));

            Randomly = My_Position.y;
            yield return YieldInstructionCache.WaitForSeconds(0.05f);
        }
    }

    IEnumerator Pattern05() // 패턴 5 - 보스의 여러번 순간이동
    {
        int Random_Move;
        float Random_Time = 0.2f;

        Color Alpha_1 = new Color(My_Color.r, My_Color.g, My_Color.b, 1);
        Color Alpha_0 = new Color(My_Color.r, My_Color.g, My_Color.b, 0);
        for (int i = 0; i < 15; i++)
        {
            if (i == 6)
            {
                Effect_Sound_Stop();
                yield return YieldInstructionCache.WaitForSeconds(1f);
                Random_Time = 0.05f;
            }
            Random_Move = Random.Range(0, 6);
            Effect_Sound_OneShot(9);
            yield return Change_My_Color(Alpha_1, Alpha_0, 0.1f, Random_Time, DisAppear_Effect_1);

            My_Position = Asura_Pattern05_Move[Random_Move];

            Effect_Sound_OneShot(9);
            yield return Change_My_Color(Alpha_0, Alpha_1, 0.1f, Random_Time, DisAppear_Effect_2);
        }
        yield return Move_Curve(My_Position, new Vector3(7, 0, 0), Vector3.zero, 1, declineCurve);
    }
    IEnumerator Boss_Move()
    {
        foreach (var e in Asura_Pattern01_Move)
            yield return Move_Straight(My_Position, e, 0.25f, declineCurve);
    }
}