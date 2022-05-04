using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Boss : HP_Info
{
    // Start is called before the first frame update

    [SerializeField]
    AnimationCurve curve;


    [SerializeField]
    GameObject Die_Explosion;

    float[,] Boss_Sequence_Move = new float[9, 3] { { 7, 2, 0 }, { -7, 2, 0 }, { -7, -2, 0 }, { 7, -2, 0 }, { 3.5f, 1, 0 },
    { -3.5f, 1, 0 }, { -3.5f, -1, 0 }, { 3.5f, 1, 0 }, { 0, 0, 0 }};

    float[,] Boss_Random_Move = new float[6, 2] { { -6.64f, 1.95f }, { -3.11f, 0.04f }, { 2.31f, 2.78f }, { 3.4f, -2.91f }, { -4.92f, -3.07f },
    { -7.55f, -1.43f }};

    float[] Meteor_Move = new float[9] { 4, 3, 2, 1, 0, -1, -2, -3, -4 };

    PlayerControl playerControl;

    public bool UnBeatable = true;

    float percent = 0;

    SpriteRenderer spriteRenderer;

    Rigidbody2D rb;

    [SerializeField]
    GameObject[] Boss_Weapon;

    [SerializeField]
    GameObject Boss_Disappear_1;

    [SerializeField]
    GameObject Boss_Disappear_2;

    [SerializeField]
    TextMeshProUGUI TraceWarningText;

    [SerializeField]
    GameObject Charge_Beam;

    [SerializeField]
    GameObject Meteor;

    [SerializeField]
    GameObject Meteor_Line;

    public float speed = 15;
    public float rotateSpeed = 200f;

    IEnumerator phase;

    ArrayList Phase_Total;

    new private void Awake()
    {
        base.Awake();
        CurrentHP = MaxHP;

        playerControl = GameObject.FindGameObjectWithTag("Playerrr").GetComponent<PlayerControl>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        TraceWarningText.color = new Color(TraceWarningText.color.r, TraceWarningText.color.g, TraceWarningText.color.b, 0);
        

        Phase_Total = new ArrayList();
        phase = Phase01();
        Phase_Total.Add(phase);

        phase = Phase02();
        Phase_Total.Add(phase);

        phase = Phase03();
        Phase_Total.Add(phase);

        phase = Phase04();
        Phase_Total.Add(phase);

        phase = Phase05();
        Phase_Total.Add(phase);

    }

    public void OnTriggerEnter2D(Collider2D collision)
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

    public void TakeDamage(float damage)
    {
        if (!UnBeatable)
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
    }
    public void OnDie()
    {
        playerControl.Score += 10000;
        Instantiate(Die_Explosion, transform.position, Quaternion.identity);
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
        StartCoroutine("Repeat_Phase");
    }
    IEnumerator Repeat_Phase()
    {

        UnBeatable = false;
        yield return YieldInstructionCache.WaitForEndOfFrame;
        //while (true)
        //{
        //    yield return StartCoroutine(Ready_To_Phase());
        //    int Random_Num = Random.Range(0, 5);

        //    switch (Random_Num)
        //    {
        //        case 0:
        //            Phase_Total[Random_Num] = Phase01();
        //            break;
        //        case 1:
        //            Phase_Total[Random_Num] = Phase02();
        //            break;
        //        case 2:
        //            Phase_Total[Random_Num] = Phase03();
        //            break;
        //        case 3:
        //            Phase_Total[Random_Num] = Phase04();
        //            break;
        //        case 4:
        //            Phase_Total[Random_Num] = Phase05();
        //            break;
        //    }
        //    yield return StartCoroutine((IEnumerator)Phase_Total[Random_Num]);
        //}
        yield return StartCoroutine(Phase04());
    }
    IEnumerator Ready_To_Phase()
    {
        var radius = (float)Mathf.Sqrt(Mathf.Pow(0.5f, 2) + Mathf.Pow(-0.5f, 2));
        var start_deg = 90 - (Mathf.Rad2Deg * Mathf.Atan2(-0.5f, 0.5f));
        var center_x = transform.position.x - 0.5f;
        var center_y = transform.position.y + 0.5f;
        for (int i = 0; i < 4; i++)
        {
            Debug.Log(i);
            for (int th = 0; th < 90; th++)
            {
                var rad = Mathf.Deg2Rad * (4 * th + start_deg);
                var x = radius * Mathf.Sin(rad);
                var y = radius * Mathf.Cos(rad);

                transform.position = new Vector3(center_x + x, center_y + y, 0);
                yield return YieldInstructionCache.WaitForEndOfFrame;
            }
        }
        yield break;
    }
    IEnumerator Phase01()
    {
        
        yield return StartCoroutine(Boss_Move(Boss_Sequence_Move));
        IEnumerator change_boss_color = Change_Boss_Color();
        StartCoroutine(change_boss_color);
        GameObject L1 = Instantiate(Boss_Weapon[0], transform.position, Quaternion.identity);


        GameObject L2 = Instantiate(Boss_Weapon[0], transform.position, Quaternion.Euler(new Vector3(0, 0, -60)));
        L2.GetComponent<Movement2D>().MoveTo(new Vector3(1, -0.5714f, 0));

        GameObject L3 = Instantiate(Boss_Weapon[0], transform.position, Quaternion.Euler(new Vector3(0, 0, -180)));
        L3.GetComponent<Movement2D>().MoveTo(new Vector3(-1, -0.5714f, 0));

        GameObject L4 = Instantiate(Boss_Weapon[0], transform.position, Quaternion.Euler(new Vector3(0, 0, 120)));
        L4.GetComponent<Movement2D>().MoveTo(new Vector3(-1, 0.5714f, 0));

        yield return YieldInstructionCache.WaitForSeconds(2f);
        Destroy(L1); Destroy(L2); Destroy(L3); Destroy(L4);
        IEnumerator thunder = GameObject.FindGameObjectWithTag("Flash").GetComponent<FlashOn>().Thunder();
        StartCoroutine(thunder);

        GameObject L5 = Instantiate(Boss_Weapon[2], new Vector3(0, 3, 0), Quaternion.Euler(new Vector3(0, 0, -9)));
        GameObject L6 = Instantiate(Boss_Weapon[2], new Vector3(0, -4, 0), Quaternion.Euler(new Vector3(0, 0, -9)));
        GameObject L7 = Instantiate(Boss_Weapon[3], new Vector3(-8, 0, 0), Quaternion.Euler(new Vector3(0, 0, -115)));
        GameObject L8 = Instantiate(Boss_Weapon[3], new Vector3(6.6f, 0, 0), Quaternion.Euler(new Vector3(0, 0, -115)));

        yield return YieldInstructionCache.WaitForSeconds(2.5f);
        Destroy(L5); Destroy(L6); Destroy(L7); Destroy(L8);
        StopCoroutine(change_boss_color);
        GameObject.FindGameObjectWithTag("Flash").GetComponent<FlashOn>().Origin();
        StopCoroutine(thunder);
        percent = 0;
        Vector3 temp_location = transform.position;
        while (percent < 1)
        {
            percent += Time.deltaTime;
            transform.position = Vector3.Lerp(temp_location, new Vector3(7, 0, 0), curve.Evaluate(percent));
            yield return YieldInstructionCache.WaitForEndOfFrame;
        }
    }
    IEnumerator Change_Boss_Color()
    {
        while(true)
        {
            percent = 0;
            while (percent < 1)
            {
                percent += Time.deltaTime * 4;
                spriteRenderer.color = Color.Lerp(Color.white, new Color(159 / 255, 43 / 255, 43 / 255), percent);
                yield return null;
            }
            percent = 0;
            while (percent < 1)
            {
                percent += Time.deltaTime * 4;
                spriteRenderer.color = Color.Lerp(new Color(159 / 255, 43 / 255, 43 / 255), Color.white, percent);
                yield return null;
            }
        }
    }
    IEnumerator Phase02()
    {
        GameObject c = Instantiate(Charge_Beam, transform.position + new Vector3(-1.22f, 0, 0), Quaternion.identity);

        IEnumerator charge_beam = c.GetComponent<Charge_Beam_Motion>().Change_Size();
        StartCoroutine(charge_beam);
        yield return StartCoroutine(Warning("이 공격은 반드시 죽습니다"));
        StopCoroutine(charge_beam);
        Destroy(c);
        for (int i = 0; i < 20; i++)
        {
            Instantiate(Boss_Weapon[1], transform.position, Quaternion.identity);

            GameObject C2 = Instantiate(Boss_Weapon[1], transform.position, Quaternion.identity);
            C2.GetComponent<Movement2D>().MoveTo(new Vector3(-1, 0.5714f, 0));

            GameObject C3 = Instantiate(Boss_Weapon[1], transform.position, Quaternion.identity);
            C3.GetComponent<Movement2D>().MoveTo(new Vector3(-1, -0.5714f, 0));

            GameObject C4 = Instantiate(Boss_Weapon[1], transform.position, Quaternion.identity);
            C4.GetComponent<Movement2D>().MoveTo(new Vector3(-1, 0.2857f, 0));

            GameObject C5 = Instantiate(Boss_Weapon[1], transform.position, Quaternion.identity);
            C5.GetComponent<Movement2D>().MoveTo(new Vector3(-1, -0.2857f, 0));

            yield return YieldInstructionCache.WaitForSeconds(0.15f);
        }
        yield return YieldInstructionCache.WaitForSeconds(2f);
        yield break;
    }
    IEnumerator Phase03()
    {
        percent = 0;
        Vector3 temp_position = transform.position;
        while (percent < 1)
        {
            percent += Time.deltaTime;
            transform.position = Vector3.Lerp(temp_position, new Vector3(0, 0, 0), curve.Evaluate(percent));
            yield return YieldInstructionCache.WaitForEndOfFrame;
        }
        float rot_Speed = 7;
        percent = 0;
        while (percent < 1)
        {
            percent += (Time.deltaTime / 2);
            transform.Rotate(Vector3.forward * rot_Speed * 250 * Time.deltaTime);
            for (int i = 0; i < 4; i++)
            {
                if (i == 3)
                {
                    GameObject T1 = Instantiate(Boss_Weapon[4]);
                    T1.transform.position = transform.position;
                    T1.transform.rotation = transform.rotation;
                    yield return YieldInstructionCache.WaitForEndOfFrame;
                }
            }
        }
        transform.rotation = Quaternion.Euler(0, 0, 0);
        yield return YieldInstructionCache.WaitForEndOfFrame;


        percent = 0;
        temp_position = transform.position;
        while (percent < 1)
        {
            percent += Time.deltaTime;
            transform.position = Vector3.Lerp(temp_position, new Vector3(0, 3, 0), curve.Evaluate(percent));
            yield return YieldInstructionCache.WaitForEndOfFrame;
        }
        for (int th = 0; th < 270; th++)
        {
            
            float rad = Mathf.Deg2Rad * (-th);
            
            float x = 7 * Mathf.Sin(rad);
            float y = 3 * Mathf.Cos(rad);

            transform.position = new Vector3(x, y, 0);
            yield return YieldInstructionCache.WaitForSeconds(Time.deltaTime * 2);
        }
        yield break;

    }
    IEnumerator Phase04()
    {
        while(true)
        {
            int Random_Num = Random.Range(0, 4);
            Debug.Log(Random_Num);
            switch (Random_Num)
            {
                case 0:
                    yield return StartCoroutine(Meteor_Launch(9, 1));
                    break;
                case 1:
                    yield return StartCoroutine(Meteor_Launch(6, 1));
                    break;
                case 2:
                    yield return StartCoroutine(Meteor_Launch(1, 8));
                    break;
                case 3:
                    yield return StartCoroutine(Meteor_Launch(3, 3));
                    break;
            }
            yield return new WaitForSeconds(3f);

        }
    }
    IEnumerator Meteor_Launch(int Meteor_Num, int Launch_Count)
    {

        Meteor_Move = StaticStript.ShuffleList(Meteor_Move);
        switch (Launch_Count)
        {
            case 1:
                for (int i = 0; i < Meteor_Num; i++)
                {
                    GameObject e = Instantiate(Meteor_Line, new Vector3(0, Meteor_Move[i], 0), Quaternion.identity);
                    StartCoroutine(e.GetComponent<Meteor_Line>().Change_Color());
                }
                yield return YieldInstructionCache.WaitForSeconds(2f);
                for (int i = 0; i < Meteor_Num; i++)
                {
                    Instantiate(Meteor, new Vector3(7, Meteor_Move[i], 0), Quaternion.identity);
                }
                yield return YieldInstructionCache.WaitForSeconds(.5f);
                break;

            case 3:
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 3 * i; j < 3 * (i + 1); j++)
                    {
                        GameObject e = Instantiate(Meteor_Line, new Vector3(0, Meteor_Move[j], 0), Quaternion.identity);
                        StartCoroutine(e.GetComponent<Meteor_Line>().Change_Color());
                    }
                    yield return YieldInstructionCache.WaitForSeconds(2f);
                    for (int j = 3 * i; j < 3 * (i + 1); j++)
                    {
                        Instantiate(Meteor, new Vector3(7, Meteor_Move[j], 0), Quaternion.identity);
                    }
                    yield return YieldInstructionCache.WaitForSeconds(.5f);
                }
                break;

            case 8:
                for (int i = 0; i < 8; i++)
                {
                    GameObject e = Instantiate(Meteor_Line, new Vector3(0, Meteor_Move[i], 0), Quaternion.identity);
                    StartCoroutine(e.GetComponent<Meteor_Line>().Change_Color());
                    yield return YieldInstructionCache.WaitForSeconds(2f);
                    Instantiate(Meteor, new Vector3(7, Meteor_Move[i], 0), Quaternion.identity);
                    yield return YieldInstructionCache.WaitForEndOfFrame;
                }
                break;
        }
        yield break;
    }
    

    IEnumerator Phase05()
    {
        for (int i = 0; i < 4; i++)
        {
            int Random_Move = Random.Range(0, 6);

            yield return StartCoroutine(Disappear_Color(0, -10, .2f, false));

            transform.position = new Vector3(Boss_Random_Move[Random_Move, 0], Boss_Random_Move[Random_Move, 1], 0);
            yield return YieldInstructionCache.WaitForEndOfFrame;

            yield return StartCoroutine(Disappear_Color(1, 10, .4f, false));
        }
        yield return YieldInstructionCache.WaitForSeconds(.7f);

        for (int i = 0; i < 7; i++)
        {
            int Random_Move = Random.Range(0, 6);
            yield return StartCoroutine(Disappear_Color(0, -10, .1f, false));

            transform.position = new Vector3(Boss_Random_Move[Random_Move, 0], Boss_Random_Move[Random_Move, 1], 0);
            yield return YieldInstructionCache.WaitForEndOfFrame;

            yield return StartCoroutine(Disappear_Color(1, 10, .1f, false));
        }

        yield return StartCoroutine(Warning("플레이어를 랜덤으로 자동 추격합니다. (반드시 즉사)"));

        for (int i = 0; i < 4; i++)
        {
            int Random_Move = Random.Range(0, 4);
            yield return StartCoroutine(Disappear_Color(0, -10, .7f, true));

            if (Random_Move == 3)
                transform.position = GameObject.FindGameObjectWithTag("Playerrr").transform.position;
            else
                transform.position = new Vector3(Boss_Random_Move[Random_Move, 0], Boss_Random_Move[Random_Move, 1], 0);
            yield return YieldInstructionCache.WaitForEndOfFrame;
            yield return StartCoroutine(Disappear_Color(1, 10, .7f, true));
        }
        yield return YieldInstructionCache.WaitForSeconds(.5f);

        Vector3 temp_position = transform.position;
        percent = 0;
        float Length = Mathf.Sqrt(Mathf.Pow(transform.position.x - 7, 2) + Mathf.Pow(transform.position.y, 2));
        float journeyTime = 1f * Mathf.Sqrt(212) / Length; // 3차원 좌표 (-7, -4, 0), (7, 0, 0) 사이를 선형보간하는 시간을 1초로 기준을 두고 계산

        while (true)
        {
            if (percent / journeyTime >= 1)
                break;
            percent += Time.deltaTime;
            Vector3 center = (temp_position + new Vector3(7, 0, 0)) * 0.5f;
            center -= new Vector3(0, -7f, 0);
            Vector3 riseRelCenter = temp_position - center;
            Vector3 setRelCenter = new Vector3(7, 0, 0) - center;
            
            transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, curve.Evaluate(percent / journeyTime));
            
            transform.position += center;
            yield return null;
        }
    }
    IEnumerator Warning(string temp)
    {
        TraceWarningText.text = temp;
        while (TraceWarningText.color.a < 1.0f)
        {
            TraceWarningText.color = new Color(TraceWarningText.color.r, TraceWarningText.color.g, TraceWarningText.color.b, TraceWarningText.color.a + Time.deltaTime / 2);
            yield return null;
        }
        while (TraceWarningText.color.a > 0.0f)
        {
            TraceWarningText.color = new Color(TraceWarningText.color.r, TraceWarningText.color.g, TraceWarningText.color.b, TraceWarningText.color.a - Time.deltaTime / 2);
            yield return null;
        }
    }
    IEnumerator Disappear_Color(float color_a, float time_in_de, float wait_for_next, bool check)
    {
        if (!check)
            Instantiate(Boss_Disappear_1, transform.position, Quaternion.identity);
        else
            Instantiate(Boss_Disappear_2, transform.position, Quaternion.identity);
        while (true)
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a + (Time.deltaTime * time_in_de));
            if (color_a == 0 && spriteRenderer.color.a <= 0)
                break;
            else if (color_a == 1 && spriteRenderer.color.a >= 1)
                break;
            yield return YieldInstructionCache.WaitForEndOfFrame;
        }
        yield return YieldInstructionCache.WaitForSeconds(wait_for_next);
    }
    IEnumerator Boss_Move(float[,] Boss_Move_float)
    {
        for (int i = 0; i < 9; i++)
        {
            float current = 0; 
            percent = 0;
            Vector3 temp_position = transform.position;
            while (percent < 1)
            {
                current += Time.deltaTime;
                percent = current * 4;

                transform.position = Vector3.Lerp(temp_position, new Vector3(Boss_Move_float[i, 0], Boss_Move_float[i, 1], Boss_Move_float[i, 2]), curve.Evaluate(percent));
                yield return null;
            }
        }
        yield break;
    }
}
