using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DoPhan : Boss_Info
{
    float[] Meteor_Move = new float[9] { 4, 3, 2, 1, 0, -1, -2, -3, -4 };  // ����

    private int Pattern_Num; // ����

    [SerializeField]
    GameObject Charge_Beam; // ����

    [SerializeField]
    GameObject Meteor; // ����

    [SerializeField]
    GameObject Homming_Enemy; // ����

    IEnumerator enemy_spawn; // ����

    IEnumerator meteor_launch;

    IEnumerator change_boss_color;

    IEnumerator thunder;

    IEnumerator charge_beam;

    new private void Awake()
    {
        base.Awake();
        transform.position = new Vector3(11, 0, 0);
        WarningText.color = new Color(WarningText.color.r, WarningText.color.g, WarningText.color.b, 0);
        flashOn = GameObject.FindGameObjectWithTag("Flash").GetComponent<FlashOn>();
        phase = Pattern01();
        Unbeatable = true;
        for (int i = 0; i < 5; i++)
            Pattern_Total.Add(phase);
        Pattern_Num = 0;
    }

    public void OnTriggerEnter2D(Collider2D collision) // �길
    {
        if (collision.CompareTag("Playerrr"))
        {
            collision.GetComponent<PlayerCtrl_Tengai>().TakeDamage(1);
        }
    }

    public override void TakeDamage(float damage) // �길 (�һ��� �������� ���� ����)
    {
        if (Unbeatable)
        {
            CurrentHP -= damage;
            StartCoroutine(Hit());
            if (CurrentHP <= 0)
            {
                OnDie();
            }
        }
    }
    IEnumerator Hit()
    {
        camera_shake = cameraShake.Shake_Act(.03f, .01f, 0.03f, false);
        StartCoroutine(camera_shake);

        spriteRenderer.color = new Color(1, 1, 1, 0.25f);
        yield return new WaitForSeconds(0.07f);
        spriteRenderer.color = new Color(1, 1, 1, 1);
        yield return null;
    }
    public override void OnDie()
    {
        GameObject.FindGameObjectWithTag("Playerrr").GetComponent<PlayerCtrl_Tengai>().Final_Score += 10000;
        Instantiate(When_Dead_Effect, transform.position, Quaternion.identity);

        if (enemy_spawn != null) // ����
            StopCoroutine(enemy_spawn);
        if (meteor_launch != null)
            StopCoroutine(meteor_launch);
        if (change_boss_color != null)
            StopCoroutine(change_boss_color);
        if (thunder != null)
            StopCoroutine(thunder);
        if (charge_beam != null)
            StopCoroutine(charge_beam);

        foreach (var u in Pattern_Total)
        {
            StopCoroutine((IEnumerator)u);
        }
        Destroy(gameObject);

        GameObject.FindGameObjectWithTag("BackGround1").GetComponent<MoveBackGround>().MoveSpeed = GameObject.FindGameObjectWithTag("BackGround1").GetComponent<MoveBackGround>().MoveSpeed * 2f;
        GameObject.FindGameObjectWithTag("BackGround2").GetComponent<MoveBackGround>().MoveSpeed = GameObject.FindGameObjectWithTag("BackGround2").GetComponent<MoveBackGround>().MoveSpeed * 2f;
        
    }
    public void Phase_Start()
    {
        StartCoroutine(Repeat_Phase());
    }
    IEnumerator Repeat_Phase()
    {
        Unbeatable = false;

        while (true)
        {
            Pattern_Num = 3;
            Pattern_Total[Pattern_Num] = Pattern04();
            yield return StartCoroutine(Ready_To_Pattern());
            //Pattern_Num = Random.Range(0, 5);
            //switch (Pattern_Num)
            //{
            //    case 0:
            //        Pattern_Total[Pattern_Num] = Pattern01();
            //        break;
            //    case 1:
            //        Pattern_Total[Pattern_Num] = Pattern02();
            //        break;
            //    case 2:
            //        Pattern_Total[Pattern_Num] = Pattern03();
            //        break;
            //    case 3:
            //        Pattern_Total[Pattern_Num] = Pattern04();
            //        break;
            //    case 4:
            //        Pattern_Total[Pattern_Num] = Pattern05();
            //        break;
            //}
            //yield return StartCoroutine((IEnumerator)Pattern_Total[Pattern_Num]);

            yield return StartCoroutine((IEnumerator)Pattern_Total[Pattern_Num]); // �� �ù� �̷��� ���� ������ �Ѥ�
        }
    }
    
    IEnumerator Ready_To_Pattern()
    {
        var radius = (float)Mathf.Sqrt(Mathf.Pow(0.5f, 2) + Mathf.Pow(-0.5f, 2));
        var start_deg = 90 - (Mathf.Rad2Deg * Mathf.Atan2(-0.5f, 0.5f));
        var center_x = transform.position.x - 0.5f;
        var center_y = transform.position.y + 0.5f;
       
        for (int i = 0; i < 4; i++)
        {
            yield return StartCoroutine(Circle_Move(90, 4, start_deg, radius, radius, center_x, center_y, .5f)); // tuple�� ���� ����� ã��.
        }
        yield break;
    }
    IEnumerator Pattern01()
    {
        yield return StartCoroutine(Boss_Move(StaticData.Sequence_Move));

        Unbeatable = true;

        change_boss_color = Change_Color_Return_To_Origin(Color.white, new Color(159 / 255, 43 / 255, 43 / 255), 0.25f, true);
        StartCoroutine(change_boss_color);

        Launch_Weapon_For_Move(Weapon[0], new Vector3(1, 0.5714f, 0), Quaternion.identity, 2.5f);
        Launch_Weapon_For_Move(Weapon[0], new Vector3(1, -0.5714f, 0), Quaternion.Euler(new Vector3(0, 0, -60)), 2.5f);
        Launch_Weapon_For_Move(Weapon[0], new Vector3(-1, -0.5714f, 0), Quaternion.Euler(new Vector3(0, 0, -180)), 2.5f);
        Launch_Weapon_For_Move(Weapon[0], new Vector3(-1, 0.5714f, 0), Quaternion.Euler(new Vector3(0, 0, 120)), 2.5f);

        yield return YieldInstructionCache.WaitForSeconds(2.5f);

        thunder = flashOn.Thunder();
        StartCoroutine(thunder);

        Launch_Weapon_For_Still(Weapon[2], new Vector3(0, 3, 0), Quaternion.Euler(new Vector3(0, 0, -9)), 2.5f);
        Launch_Weapon_For_Still(Weapon[2], new Vector3(0, -4, 0), Quaternion.Euler(new Vector3(0, 0, -9)), 2.5f);
        Launch_Weapon_For_Still(Weapon[3], new Vector3(-8, 0, 0), Quaternion.Euler(new Vector3(0, 0, -115)), 2.5f);
        Launch_Weapon_For_Still(Weapon[3], new Vector3(6.6f, 0, 0), Quaternion.Euler(new Vector3(0, 0, -115)), 2.5f);

        yield return YieldInstructionCache.WaitForSeconds(2.5f);

        flashOn.Origin();
        StopCoroutine(thunder);
        StopCoroutine(change_boss_color);

        Unbeatable = false;

        yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(7, 0, 0), 1, declineCurve));
    }
    IEnumerator Pattern02()
    {
        GameObject c = Instantiate(Charge_Beam, transform.position + new Vector3(-1.22f, 0, 0), Quaternion.identity);

        Unbeatable = true;

        charge_beam = c.GetComponent<Charge_Beam_Motion>().Change_Size();
        StartCoroutine(charge_beam);

        yield return StartCoroutine(Warning("�� ������ �ݵ�� �׽��ϴ�", .5f));

        StopCoroutine(charge_beam);
        Destroy(c);

        Unbeatable = false;

        for (int i = 0; i < 30; i++)
        {
            Launch_Weapon_For_Move(Weapon[1], Vector3.left, Quaternion.identity, 1);
            Launch_Weapon_For_Move(Weapon[1], new Vector3(-1, 0.5714f, 0), Quaternion.identity, 1);
            Launch_Weapon_For_Move(Weapon[1], new Vector3(-1, -0.5714f, 0), Quaternion.identity, 1);
            Launch_Weapon_For_Move(Weapon[1], new Vector3(-1, 0.2857f, 0), Quaternion.identity, 1);
            Launch_Weapon_For_Move(Weapon[1], new Vector3(-1, -0.2857f, 0), Quaternion.identity, 1);
            yield return YieldInstructionCache.WaitForSeconds(.1f);
        }
        yield return YieldInstructionCache.WaitForSeconds(2f);
        yield break;
    }
    IEnumerator Pattern03()
    {
        yield return StartCoroutine(Position_Lerp(transform.position, Vector3.zero, 1, declineCurve));

        yield return StartCoroutine(Rotate_Bullet(7, 250, .5f, 4, Weapon[4]));

        yield return StartCoroutine(Position_Lerp(Vector3.zero, new Vector3(0, 3, 0), 1, declineCurve));

        yield return StartCoroutine(Circle_Move(270, -1, 0, 7, 3, 0, 0, 2));
    }
    IEnumerator Pattern04() // + �÷��̾ �÷��� �޴ٰ� �����ϴ� �͵� �߰��ؾ��Ѵ�. �ϳ��� ������ ������ ��ȭ ���.
    {

        yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(12, transform.position.y, 0), 1, declineCurve));

        GameObject.FindGameObjectWithTag("Playerrr").GetComponent<PlayerCtrl_Tengai>().Start_Emit();

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
        StopCoroutine(meteor_launch);
        StopCoroutine(enemy_spawn);
        StopCoroutine((IEnumerator)Pattern_Total[Pattern_Num]);

        yield return YieldInstructionCache.WaitForSeconds(2f);

        yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(7, transform.position.y, 0), 1, declineCurve));
        StartCoroutine(Repeat_Phase());

        yield return null;
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
                }
                break;
        }
    }

    IEnumerator Pattern05()
    {
        int Random_Move;
        Color A = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1);
        Color B = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0);
        for (int i = 0; i < 4; i++)
        {
            Random_Move = Random.Range(0, 6);

            yield return StartCoroutine(Change_Color_Lerp(A, B, 0.1f, .2f, DisAppear_Effect_1));

            transform.position = new Vector3(StaticData.Random_Move[Random_Move, 0], StaticData.Random_Move[Random_Move, 1], 0);
            yield return YieldInstructionCache.WaitForEndOfFrame;

            yield return StartCoroutine(Change_Color_Lerp(B, A, 0.1f, .4f, DisAppear_Effect_1));
        }

        yield return YieldInstructionCache.WaitForSeconds(.6f);

        for (int i = 0; i < 7; i++)
        {
            Random_Move = Random.Range(0, 6);
            yield return StartCoroutine(Change_Color_Lerp(A, B, 0.1f, .1f, DisAppear_Effect_1));

            transform.position = new Vector3(StaticData.Random_Move[Random_Move, 0], StaticData.Random_Move[Random_Move, 1], 0);

            yield return StartCoroutine(Change_Color_Lerp(B, A, 0.1f, .1f, DisAppear_Effect_1));
        }

        Unbeatable = true;

        yield return StartCoroutine(StaticFunc.Warning(WarningText, "�÷��̾ �������� �ڵ� �߰��մϴ�. (�ݵ�� ���)", .5f));

        Unbeatable = false;

        for (int i = 0; i < 4; i++)
        {
            Random_Move = Random.Range(0, 4);
            yield return StartCoroutine(Change_Color_Lerp(A, B, 0.1f, .7f, DisAppear_Effect_2));

            if (Random_Move == 3)
                transform.position = GameObject.FindGameObjectWithTag("Playerrr").transform.position;
            else
                transform.position = new Vector3(StaticData.Random_Move[Random_Move, 0], StaticData.Random_Move[Random_Move, 1], 0);

            yield return StartCoroutine(Change_Color_Lerp(B, A, 0.1f, .7f, DisAppear_Effect_2));
        }

        yield return YieldInstructionCache.WaitForSeconds(.5f);

        yield return StartCoroutine(Position_Curve(transform.position, new Vector3(-7, -4, 0), new Vector3(7, 0, 0), 1, declineCurve));
    }
    IEnumerator Boss_Move(float[,] Boss_Move_float)
    {
        for (int i = 0; i < 9; i++)
        {
            yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(Boss_Move_float[i, 0], Boss_Move_float[i, 1], Boss_Move_float[i, 2]), .25f, declineCurve));
        }
        yield break;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
