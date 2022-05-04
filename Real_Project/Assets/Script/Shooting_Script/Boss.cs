using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : HP_Info
{
    // Start is called before the first frame update


    [SerializeField]
    AnimationCurve curve;


    [SerializeField]
    GameObject Die_Explosion;

    float[,] Boss_Random_Move = new float[9, 3] { { 7, 2, 0 }, { -7, 2, 0 }, { -7, -2, 0 }, { 7, -2, 0 }, { 3.5f, 1, 0 },
    { -3.5f, 1, 0 }, { -3.5f, -1, 0 }, { 3.5f, 1, 0 }, { 0, 0, 0 }};

    PlayerControl playerControl;

    public bool UnBeatable = true;

    float boomDelay = 0.5f;

    SpriteRenderer spriteRenderer;

    Rigidbody2D rb;

    [SerializeField]
    GameObject[] Boss_Weapon;

    public float speed = 15;
    public float rotateSpeed = 200f;


    IEnumerator phase;

    ArrayList Phase_Total;

    new private void Awake()
    {
        base.Awake();
        playerControl = GameObject.FindGameObjectWithTag("Playerrr").GetComponent<PlayerControl>();
        rb = GetComponent<Rigidbody2D>();
        CurrentHP = MaxHP;
        spriteRenderer = GetComponent<SpriteRenderer>();
        Phase_Total = new ArrayList();
        phase = Phase01();
        Phase_Total.Add(phase);

        phase = Phase02();
        Phase_Total.Add(phase);

        phase = Phase03();
        Phase_Total.Add(phase);

        phase = Phase04();
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
        while (true)
        {
            yield return StartCoroutine(Ready_To_Phase());
            int Random_Num = Random.Range(0, 4);

            switch (Random_Num)
            {
                case 0:
                    Phase_Total[Random_Num] = Phase01();
                    break;
                case 1:
                    Phase_Total[Random_Num] = Phase02();
                    break;
                case 2:
                    Phase_Total[Random_Num] = Phase03();
                    break;
                case 3:
                    Phase_Total[Random_Num] = Phase04();
                    break;
            }
            yield return StartCoroutine((IEnumerator)Phase_Total[Random_Num]);
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
        yield return StartCoroutine(Boss_Move(Boss_Random_Move));

        Instantiate(Boss_Weapon[0], transform.position, Quaternion.identity);

        GameObject L2 = Instantiate(Boss_Weapon[0], transform.position, Quaternion.Euler(new Vector3(0, 0, -60)));
        L2.GetComponent<Movement2D>().MoveTo(new Vector3(1, -0.5714f, 0));

        GameObject L3 = Instantiate(Boss_Weapon[0], transform.position, Quaternion.Euler(new Vector3(0, 0, -180)));
        L3.GetComponent<Movement2D>().MoveTo(new Vector3(-1, -0.5714f, 0));

        GameObject L4 = Instantiate(Boss_Weapon[0], transform.position, Quaternion.Euler(new Vector3(0, 0, 120)));
        L4.GetComponent<Movement2D>().MoveTo(new Vector3(-1, 0.5714f, 0));

        yield return YieldInstructionCache.WaitForSeconds(2f);

        GameObject L5 = Instantiate(Boss_Weapon[2], new Vector3(0, 3, 0), Quaternion.Euler(new Vector3(0, 0, -9)));
        GameObject L6 = Instantiate(Boss_Weapon[2], new Vector3(0, -4, 0), Quaternion.Euler(new Vector3(0, 0, -9)));
        GameObject L7 = Instantiate(Boss_Weapon[3], new Vector3(-8, 0, 0), Quaternion.Euler(new Vector3(0, 0, -115)));
        GameObject L8 = Instantiate(Boss_Weapon[3], new Vector3(6.6f, 0, 0), Quaternion.Euler(new Vector3(0, 0, -115)));
        yield return YieldInstructionCache.WaitForSeconds(2.5f);

        Destroy(L5); Destroy(L6); Destroy(L7); Destroy(L8);

        while (true)
        {
            transform.position += Vector3.right * (Time.deltaTime * 8f);
            yield return YieldInstructionCache.WaitForEndOfFrame;

            if (transform.position.x >= 7)
            {
                yield return YieldInstructionCache.WaitForEndOfFrame;
                yield break;
            }
        }
    }
    IEnumerator Phase02()
    {
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
        
        float boomDelay = 0.5f;
        float current = 0;
        float percent = 0;
        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / boomDelay;
            transform.position = Vector3.Slerp(transform.position, new Vector3(0, 0, 0), curve.Evaluate(percent));
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
        yield return YieldInstructionCache.WaitForSeconds(3f);
        yield break;
    }
    IEnumerator Phase04()
    {
        float percent = 0;
        while(percent < 1)
        {
            percent += Time.deltaTime * 2;
            transform.localScale = new Vector3(transform.localScale.x + (8f * Time.deltaTime),
                transform.localScale.y + (8f * Time.deltaTime), 0);
            yield return YieldInstructionCache.WaitForEndOfFrame;
        }

        percent = 0;
        while (percent < 1)
        {
            percent += (Time.deltaTime * 2.5f);
            transform.localScale = new Vector3(transform.localScale.x - (16f * Time.deltaTime),
                transform.localScale.y - (16f * Time.deltaTime), 0);
            yield return YieldInstructionCache.WaitForEndOfFrame;
        }
        percent = 0;
        while (percent < 1)
        {
            percent += (Time.deltaTime / 3);
            Vector2 direction = (Vector2)(GameObject.FindGameObjectWithTag("Playerrr").transform.position) - rb.position;
            direction.Normalize();

            float rotateAmount = Vector3.Cross(direction, transform.up).z;

            rb.angularVelocity = -rotateAmount * rotateSpeed;

            rb.velocity = transform.up * speed;
            yield return YieldInstructionCache.WaitForEndOfFrame;
        }
        rb.angularVelocity = 0;
        rb.velocity = new Vector2(0, 0);
        transform.position = new Vector3(6, 1, 0);

        percent = 0;
        while (percent < 1)
        {
            percent += (Time.deltaTime * 1.25f);
            transform.localScale = new Vector3(transform.localScale.x + (3f * Time.deltaTime),
                transform.localScale.y + (3f * Time.deltaTime), 0);
            yield return YieldInstructionCache.WaitForEndOfFrame;
        }
        yield return YieldInstructionCache.WaitForEndOfFrame;
        yield break;
    }
    IEnumerator Boss_Move(float[,] Boss_Move_float)
    {
        for (int i = 0; i < 9; i++)
        {
            float current = 0;
            float percent = 0;
            while (percent < 1)
            {
                current += Time.deltaTime;
                percent = current / boomDelay;

                transform.position = Vector3.Lerp(transform.position, new Vector3(Boss_Move_float[i, 0], Boss_Move_float[i, 1], Boss_Move_float[i, 2]), curve.Evaluate(percent));
                yield return YieldInstructionCache.WaitForEndOfFrame;
            }
        }
        yield break;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(UnBeatable);
    }
}
