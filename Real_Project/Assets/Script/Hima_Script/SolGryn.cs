using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolGryn : HP_Info
{
    // Start is called before the first frame update

    // Update is called once per frame

    [SerializeField]
    GameObject[] Weapon;

    [SerializeField]
    GameObject SolGryn_HP;
  
    CameraShake cameraShake;

    [SerializeField]
    AnimationCurve animationCurve;

    [SerializeField]
    GameObject Disappear_Effect;

    SpriteRenderer spriteRenderer;

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
        cameraShake = GetComponent<CameraShake>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        CurrentHP = MaxHP;
       
        SolGryn_HP.SetActive(false);
    }

    void Start()
    {
        transform.position = new Vector3(7, 3, 0);
        StartCoroutine(Boss_Pattern());
    }
    IEnumerator Boss_Pattern()
    {
        yield return new WaitForSeconds(1f);
        SolGryn_HP.SetActive(true);
        SolGryn_HP.GetComponent<BossHPSliderViewer>().F_HPFull(gameObject.GetComponent<SolGryn>());

        yield return new WaitForSeconds(3f);

        StartCoroutine(HP_Decrease());
        yield return StartCoroutine(First_Move());

        yield return StartCoroutine(Pattern_1());
        yield return StartCoroutine(Pattern_2());
        yield return StartCoroutine(Pattern_3());
    }
    IEnumerator HP_Decrease()
    {
        while(true)
        {
            CurrentHP -= 10;
            yield return new WaitForSeconds(1f);
        }
    }
    IEnumerator Pattern_3()
    {
        IEnumerator rotate_Attack = Rotate_Attack();
        while (true)
        {
            
            yield return null;
            yield return StartCoroutine(Disappear());
            StartCoroutine(rotate_Attack);
            int x = Random.Range(0, 5);
            yield return StartCoroutine(SolGryn_Color(move_random[x, 0], move_random[x, 1]));
            StopCoroutine(rotate_Attack);
            yield return null;
        }
    }
    IEnumerator SolGryn_Color(float x, float y)
    {
        transform.position = new Vector3(x, y, 0);
        while (spriteRenderer.color.a < 1.0f)
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a + (Time.deltaTime * 1.5f));
            yield return null;
        }
        while (spriteRenderer.color.a > 0f)
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a - (Time.deltaTime * 1.5f));
            yield return null;
        }
        yield break;
    }
    IEnumerator Rotate_Attack()
    {
        Debug.Log("Èì");
        float rot_Speed = 7;
        int MAX = 4;
        int Index = 0;
        while (true)
        {
            transform.Rotate(Vector3.forward * rot_Speed * 200 * Time.deltaTime);
            Index++;
            if (Index >= MAX)
            {
                Index = 0;
                GameObject T1 = Instantiate(Weapon[2]);
                T1.transform.position = transform.position;
                T1.transform.rotation = transform.rotation;
            }
            yield return null;
        }
    }
    IEnumerator Pattern_1()
    {
        IEnumerator move_Second = Move_Second(7, -2, 7, 2);
        IEnumerator rotate = Rotate();
        StartCoroutine(move_Second);
        StartCoroutine(rotate);
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(Boss_W1(90 + (i * 20), 10, 180));
        }
        StopCoroutine(move_Second);
        StopCoroutine(rotate);
        yield break;
    }
    IEnumerator Pattern_2()
    {
        IEnumerator move_Second = Move_Second(-2f, 2, 2f, 2);
        IEnumerator rotate = Rotate();
        yield return StartCoroutine(Disappear());
        yield return StartCoroutine(Appear(-2, 2));
        StartCoroutine(move_Second);
        StartCoroutine(rotate);
       
       
        for (int i = 0; i < 3; i++)
        {
            StartCoroutine(Boss_W1(162 + (i * 20), 10, 180));
            yield return new WaitForSeconds(0.8f);
            Instantiate(Weapon[1], transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.4f);
        }
        StopCoroutine(move_Second);
        StopCoroutine(rotate);
        yield break;
    }
    IEnumerator Disappear()
    {
        Instantiate(Disappear_Effect, transform.position, Quaternion.identity);
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0);
        yield return new WaitForSeconds(1.5f);
    }
    IEnumerator Appear(float x, float y)
    {
        transform.position = new Vector3(x, y, 0);
        transform.Rotate(0, 0, 70);
        Instantiate(Disappear_Effect, transform.position, Quaternion.identity);
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1);
        yield return null;
    }
    IEnumerator First_Move()
    {
        float temp = .01f;
        while (transform.position.y >= -3)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - (Mathf.Pow(5, temp) * Time.deltaTime), transform.position.z);
            temp += 0.025f;
            yield return null;
        }
        temp = .01f;
        StartCoroutine(Boss_W1(72, 7, 150));
        cameraShake.Shake();
        while (transform.position.x >= -7 && transform.position.y <= 3)
        {
            transform.position = new Vector3(transform.position.x - (Mathf.Pow(5, temp) * Time.deltaTime), transform.position.y + (0.4286f * Mathf.Pow(5, temp) * Time.deltaTime), transform.position.z);
            temp += 0.015f;
            yield return null;
        }
        temp = .01f;
        StartCoroutine(Boss_W1(252, 7, 150));
        cameraShake.Shake();
        while (transform.position.y >= -3)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - (Mathf.Pow(5, temp) * Time.deltaTime), transform.position.z);
            temp += 0.025f;
            yield return null;
        }
        temp = .01f;
        StartCoroutine(Boss_W1(-18, 7, 150));
        cameraShake.Shake();
        while (transform.position.x <= 7 && transform.position.y <= 3)
        {
            transform.position = new Vector3(transform.position.x + (Mathf.Pow(5, temp) * Time.deltaTime), transform.position.y + (0.4286f * Mathf.Pow(5, temp) * Time.deltaTime), transform.position.z);
            temp += 0.015f;
            yield return null;
        }
        StartCoroutine(Boss_W1(162, 7, 150));
        cameraShake.Shake();
        float journeyTime = 0.45f;
        float startTime = Time.time;
        while (true)
        {
            if (transform.position.x <= -6.5f && transform.position.y <= -3.5f)
            {
                startTime = Time.time;
                break;
            }
            Vector3 center = (new Vector3(7, 4, 0) + new Vector3(-7, -4, 0)) * 0.5f;
            center -= new Vector3(0, 7f, 0);
            Vector3 riseRelCenter = new Vector3(7, 4, 0) - center;
            Vector3 setRelCenter = new Vector3(-7, -4, 0) - center;
            float fracComplete = (Time.time - startTime) / journeyTime;
            transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, fracComplete);
            transform.position += center;
            yield return null;
        }
        while (true)
        {
            if (transform.position.x >= 6.5f && transform.position.y >= -.5f)
                break;
            Vector3 center = (new Vector3(-7, -4, 0) + new Vector3(7, 0, 0)) * 0.5f;
            center -= new Vector3(0, -7f, 0);
            Vector3 riseRelCenter = new Vector3(-7, -4, 0) - center;
            Vector3 setRelCenter = new Vector3(7, 0, 0) - center;
            float fracComplete = (Time.time - startTime) / journeyTime;
            transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, fracComplete);
            transform.position += center;
            yield return null;
        }
    }
    IEnumerator Move_Second(float x_f, float y_f, float x_l, float y_l)
    {
        float current;
        Vector3 startPosition;
        Vector3 endPosition;
        bool move_dir = false;
        while (true)
        {
            startPosition = transform.position;
            if (move_dir)
                endPosition = new Vector3(x_f, y_f, 0);
            else
                endPosition = new Vector3(x_l, y_l, 0);
            current = 0;
            while (current < 1)
            {
                current += Time.deltaTime;
                transform.position = Vector3.Lerp(startPosition, endPosition, animationCurve.Evaluate(current));
                yield return null;
            }
            move_dir = !move_dir;
        }
    }
    IEnumerator Rotate()
    {
        int MAX = 180;
        bool rotate_dir = true;
        for (int i = 0; i < MAX; i++)
        {
            if (i == MAX - 1)
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
    IEnumerator Boss_W1(float start_angle, int count, float range_angle)
    {
        float count_per_radian = range_angle / count;
        float intervalAngle = start_angle;
        for (int i = 0; i < count; i++)
        {
            GameObject clone = Instantiate(Weapon[0], transform.position, Quaternion.identity);
            float angle = intervalAngle + (i * count_per_radian);
            float x = Mathf.Cos(angle * Mathf.PI / 180.0f);
            float y = Mathf.Sin(angle * Mathf.PI / 180.0f);
            clone.GetComponent<Movement2D>().MoveSpeed = 8;
            clone.GetComponent<Movement2D>().MoveTo(new Vector3(x, y));
            yield return null;
        }
    }
    void Update()
    {

    }
}
