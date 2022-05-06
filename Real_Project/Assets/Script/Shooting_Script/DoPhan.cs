using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DoPhan : Boss_Info
{
    float[] Meteor_Move = new float[9] { 4, 3, 2, 1, 0, -1, -2, -3, -4 };  // 본인

    public bool Boss_UnBeatable = true; // 본인

    private int Phase_Num; // 본인

    [SerializeField]
    GameObject Charge_Beam; // 본인

    [SerializeField]
    GameObject Meteor; // 본인

    [SerializeField]
    GameObject Homming_Enemy; // 본인

    IEnumerator enemy_spawn; // 본인

    new private void Awake()
    {
        base.Awake();
        phase = Phase01();

        for (int i = 0; i < 5; i++)
            Phase_Total.Add(phase);
        // Make Array of Phase
    }

    public void OnTriggerEnter2D(Collider2D collision) // 얘만
    {
        if (collision.CompareTag("Playerrr"))
        {
            if (!collision.GetComponent<PlayerControl>().Unbeatable_Player)
            {
                collision.GetComponent<PlayerControl>().Unbeatable_Player = true;
                collision.GetComponent<PlayerControl>().TakeDamage();
            }
        }
    }

    public void TakeDamage(float damage) // 얘만 (불상은 데미지를 입지 않음)
    {
        if (!Boss_UnBeatable)
        {
            CurrentHP -= damage;
            StartCoroutine("Hit");
            if (CurrentHP <= 0)
            {
                OnDie();
            }
        }
    }
    IEnumerator Hit()
    {
        spriteRenderer.color = Color.blue;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
        yield return null;
    }
    public new void OnDie()
    {
        GameObject.FindGameObjectWithTag("Playerrr").GetComponent<PlayerControl>().Score += 10000;
        Instantiate(Boss_Explosion_When_Die, transform.position, Quaternion.identity);

        foreach (var u in Phase_Total)
        {
            StopCoroutine((IEnumerator)u);
        }

        GameObject.FindGameObjectWithTag("BackGround1").GetComponent<MoveBackGround>().MoveSpeed = GameObject.FindGameObjectWithTag("BackGround1").GetComponent<MoveBackGround>().MoveSpeed * 2f;
        GameObject.FindGameObjectWithTag("BackGround2").GetComponent<MoveBackGround>().MoveSpeed = GameObject.FindGameObjectWithTag("BackGround2").GetComponent<MoveBackGround>().MoveSpeed * 2f;
        Destroy(gameObject);
    }
    public void Phase_Start()
    {
        StartCoroutine(Repeat_Phase());
    }
    IEnumerator Repeat_Phase()
    {
        Boss_UnBeatable = false;

        while (true)
        {
            yield return StartCoroutine(Ready_To_Phase());
            Phase_Num = Random.Range(0, 5);
            switch (Phase_Num)
            {
                case 0:
                    Phase_Total[Phase_Num] = Phase01();
                    break;
                case 1:
                    Phase_Total[Phase_Num] = Phase02();
                    break;
                case 2:
                    Phase_Total[Phase_Num] = Phase03();
                    break;
                case 3:
                    Phase_Total[Phase_Num] = Phase04();
                    break;
                case 4:
                    Phase_Total[Phase_Num] = Phase05();
                    break;
            }
            //yield return StartCoroutine((IEnumerator)Phase_Total[Phase_Num]);
            yield return StartCoroutine(Phase05());
        }
    }
    
    IEnumerator Ready_To_Phase()
    {
        var radius = (float)Mathf.Sqrt(Mathf.Pow(0.5f, 2) + Mathf.Pow(-0.5f, 2));
        var start_deg = 90 - (Mathf.Rad2Deg * Mathf.Atan2(-0.5f, 0.5f));
        var center_x = transform.position.x - 0.5f;
        var center_y = transform.position.y + 0.5f;
       
        for (int i = 0; i < 4; i++)
        {
            yield return StartCoroutine(Circle_Move(90, 4, start_deg, radius, radius, center_x, center_y, .5f)); // tuple로 묶는 방법을 찾자.
        }
        yield break;
    }
    IEnumerator Phase01()
    {
        yield return StartCoroutine(Boss_Move(StaticData.Sequence_Move));

        IEnumerator change_boss_color = Change_Color_Return_To_Origin(Color.white, new Color(159 / 255, 43 / 255, 43 / 255), 4);
        StartCoroutine(change_boss_color);

        Launch_Weapon_For_Move(Boss_Weapon[0], new Vector3(1, 0.5714f, 0), Quaternion.identity, 2.5f);
        Launch_Weapon_For_Move(Boss_Weapon[0], new Vector3(1, -0.5714f, 0), Quaternion.Euler(new Vector3(0, 0, -60)), 2.5f);
        Launch_Weapon_For_Move(Boss_Weapon[0], new Vector3(-1, -0.5714f, 0), Quaternion.Euler(new Vector3(0, 0, -180)), 2.5f);
        Launch_Weapon_For_Move(Boss_Weapon[0], new Vector3(-1, 0.5714f, 0), Quaternion.Euler(new Vector3(0, 0, 120)), 2.5f);

        yield return YieldInstructionCache.WaitForSeconds(2.5f);

        IEnumerator thunder = GameObject.FindGameObjectWithTag("Flash").GetComponent<FlashOn>().Thunder();
        StartCoroutine(thunder);

        Launch_Weapon_For_Still(Boss_Weapon[2], new Vector3(0, 3, 0), Quaternion.Euler(new Vector3(0, 0, -9)), 2.5f);
        Launch_Weapon_For_Still(Boss_Weapon[2], new Vector3(0, -4, 0), Quaternion.Euler(new Vector3(0, 0, -9)), 2.5f);
        Launch_Weapon_For_Still(Boss_Weapon[3], new Vector3(-8, 0, 0), Quaternion.Euler(new Vector3(0, 0, -115)), 2.5f);
        Launch_Weapon_For_Still(Boss_Weapon[3], new Vector3(6.6f, 0, 0), Quaternion.Euler(new Vector3(0, 0, -115)), 2.5f);

        yield return YieldInstructionCache.WaitForSeconds(2.5f);

       
        GameObject.FindGameObjectWithTag("Flash").GetComponent<FlashOn>().Origin();
        StopCoroutine(thunder);
        StopCoroutine(change_boss_color);

        yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(7, 0, 0), 1, curve));
    }
    IEnumerator Phase02()
    {
        GameObject c = Instantiate(Charge_Beam, transform.position + new Vector3(-1.22f, 0, 0), Quaternion.identity);

        IEnumerator charge_beam = c.GetComponent<Charge_Beam_Motion>().Change_Size();
        StartCoroutine(charge_beam);

        yield return StartCoroutine(Warning("이 공격은 반드시 죽습니다", .5f));

        StopCoroutine(charge_beam);
        Destroy(c);

        for (int i = 0; i < 30; i++)
        {
            Launch_Weapon_For_Move(Boss_Weapon[1], Vector3.left, Quaternion.identity, 1);
            Launch_Weapon_For_Move(Boss_Weapon[1], new Vector3(-1, 0.5714f, 0), Quaternion.identity, 1);
            Launch_Weapon_For_Move(Boss_Weapon[1], new Vector3(-1, -0.5714f, 0), Quaternion.identity, 1);
            Launch_Weapon_For_Move(Boss_Weapon[1], new Vector3(-1, 0.2857f, 0), Quaternion.identity, 1);
            Launch_Weapon_For_Move(Boss_Weapon[1], new Vector3(-1, -0.2857f, 0), Quaternion.identity, 1);
            yield return YieldInstructionCache.WaitForSeconds(.1f);
        }
        yield return YieldInstructionCache.WaitForSeconds(2f);
        yield break;
    }
    IEnumerator Phase03()
    {
        yield return StartCoroutine(Position_Lerp(transform.position, Vector3.zero, 1, curve));

        yield return StartCoroutine(Rotate_Bullet(7, 250, .5f, 4, Boss_Weapon[4]));

        yield return StartCoroutine(Position_Lerp(Vector3.zero, new Vector3(0, 3, 0), 1, curve));

        yield return StartCoroutine(Circle_Move(270, -1, 0, 7, 3, 0, 0, 2));
    }
    IEnumerator Phase04() // + 플레이어가 시련을 겪다가 방출하는 것도 추가해야한다. 하나라도 데미지 맞으면 정화 취소.
    {
        yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(12, transform.position.y, 0), 1, curve));

        GameObject.FindGameObjectWithTag("Playerrr").GetComponent<PlayerControl>().Start_Emit();

        enemy_spawn = Enemy_Spawn();
        StartCoroutine(enemy_spawn);

        for (int i = 0; i < 7; i++)
        {
            int Random_Num = Random.Range(0, 4);
            switch (Random_Num)
            {
                case 0:
                    yield return StartCoroutine(Meteor_Launch(9, 1));
                    break;
                case 1:
                    yield return StartCoroutine(Meteor_Launch(6, 1));
                    break;
                case 2:
                    yield return StartCoroutine(Meteor_Launch(3, 3));
                    break;
                case 3:
                    yield return StartCoroutine(Meteor_Launch(1, 8));
                    break;
            }
            yield return YieldInstructionCache.WaitForSeconds(3f);
        }
        yield return YieldInstructionCache.WaitForSeconds(1f);
        
    }
    public void Stop_Meteor()
    {
        StartCoroutine(I_Stop_Meteor());
    }
    IEnumerator I_Stop_Meteor()
    {
        StopCoroutine(enemy_spawn);
        StopCoroutine((IEnumerator)Phase_Total[Phase_Num]);

        yield return YieldInstructionCache.WaitForSeconds(3f);

        yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(7, transform.position.y, 0), 1, curve));

        StartCoroutine(Repeat_Phase());
        yield return YieldInstructionCache.WaitForEndOfFrame;
    }
    IEnumerator Enemy_Spawn()
    {
        while(true)
        {
            Instantiate(Homming_Enemy, new Vector3(4, 0, 0), Quaternion.identity);
            yield return YieldInstructionCache.WaitForSeconds(4f);
        }
    }
    IEnumerator Meteor_Launch(int Meteor_Num, int Launch_Count)
    {
        float R1 = Random.Range(.15f, 1);
        float R2 = Random.Range(.15f, 1);
        float R3 = Random.Range(.15f, 1);
        Meteor_Move = StaticFunc.ShuffleList(Meteor_Move);
        switch (Launch_Count)
        {
            case 1:
                for (int i = 0; i < Meteor_Num; i++)
                {
                    GameObject e = Instantiate(Meteor, new Vector3(8, Meteor_Move[i], 0), Quaternion.identity);
                    StartCoroutine(e.GetComponent<Meteor_Effect>().Meteor_Launch_Act(Meteor_Move[i], R1, R2, R3));
                }
                yield return YieldInstructionCache.WaitForSeconds(2f);
                break;

            case 3:
                for (int i = 0; i < 3; i++)
                {
                    R1 = Random.Range(.15f, 1);
                    R2 = Random.Range(.15f, 1);
                    R3 = Random.Range(.15f, 1);
                    for (int j = 3 * i; j < 3 * (i + 1); j++)
                    {
                        GameObject e = Instantiate(Meteor, new Vector3(8, Meteor_Move[j], 0), Quaternion.identity);
                        StartCoroutine(e.GetComponent<Meteor_Effect>().Meteor_Launch_Act(Meteor_Move[j], R1, R2, R3));
                    }
                    yield return YieldInstructionCache.WaitForSeconds(.5f);
                }
                break;

            case 8:
                for (int i = 0; i < 8; i++)
                {
                    GameObject e = Instantiate(Meteor, new Vector3(8, Meteor_Move[i], 0), Quaternion.identity);
                    StartCoroutine(e.GetComponent<Meteor_Effect>().Meteor_Launch_Act(Meteor_Move[i], R1, R2, R3));
                    yield return YieldInstructionCache.WaitForSeconds(.4f);
                }
                break;
        }
        yield break;
    }

    IEnumerator Phase05()
    {
        int Random_Move;
        Color A = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1);
        Color B = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0);
        for (int i = 0; i < 4; i++)
        {
            Random_Move = Random.Range(0, 6);

            yield return StartCoroutine(Change_Color_Temporary(A, B, 10, .2f, Boss_Disappear_1));

            transform.position = new Vector3(StaticData.Random_Move[Random_Move, 0], StaticData.Random_Move[Random_Move, 1], 0);
            yield return YieldInstructionCache.WaitForEndOfFrame;

            yield return StartCoroutine(Change_Color_Temporary(B, A, 10, .4f, Boss_Disappear_1));
        }
        yield return YieldInstructionCache.WaitForSeconds(.6f);

        for (int i = 0; i < 7; i++)
        {
            Random_Move = Random.Range(0, 6);
            yield return StartCoroutine(Change_Color_Temporary(A, B, 10, .1f, Boss_Disappear_1));

            transform.position = new Vector3(StaticData.Random_Move[Random_Move, 0], StaticData.Random_Move[Random_Move, 1], 0);

            yield return StartCoroutine(Change_Color_Temporary(B, A, 10, .1f, Boss_Disappear_1));
        }

        yield return StartCoroutine(StaticFunc.Warning(WarningText, "플레이어를 랜덤으로 자동 추격합니다. (반드시 즉사)", .5f));

        for (int i = 0; i < 4; i++)
        {
            Random_Move = Random.Range(0, 4);
            yield return StartCoroutine(Change_Color_Temporary(A, B, 10, .7f, Boss_Disappear_2));

            if (Random_Move == 3)
                transform.position = GameObject.FindGameObjectWithTag("Playerrr").transform.position;
            else
                transform.position = new Vector3(StaticData.Random_Move[Random_Move, 0], StaticData.Random_Move[Random_Move, 1], 0);

            yield return StartCoroutine(Change_Color_Temporary(B, A, 10, .7f, Boss_Disappear_2));
        }

        yield return YieldInstructionCache.WaitForSeconds(.5f);

        yield return StartCoroutine(Position_Curve(transform.position, new Vector3(-7, -4, 0), new Vector3(7, 0, 0), 1, curve));
    }
    IEnumerator Boss_Move(float[,] Boss_Move_float)
    {
        for (int i = 0; i < 9; i++)
        {
            yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(Boss_Move_float[i, 0], Boss_Move_float[i, 1], Boss_Move_float[i, 2]), 4, curve));
        }
        yield break;
    }
}
