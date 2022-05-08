using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolGryn : Boss_Info
{
    // Start is called before the first frame update

    // Update is called once per frame
    
    [SerializeField]
    GameObject SolGryn_HP;
  
    CameraShake cameraShake;

    [SerializeField]
    AnimationCurve inclineCurve;

    [SerializeField]
    AnimationCurve De_In_Curve;

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

        cameraShake = GetComponent<CameraShake>();

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
        IEnumerator rotate_bullet = Rotate_Bullet(7, 200, 0.02f, 4, Boss_Weapon[2]);
        while (true)
        {
            yield return Change_Color_Temporary(new Color(1, 1, 1, 0), new Color(1, 1, 1, 0), 3, 1.5f, Boss_Disappear_2);

            StartCoroutine(rotate_bullet);
            int x = Random.Range(0, 5);

            transform.position = new Vector3(move_random[x, 0], move_random[x, 1], 0);
            yield return StartCoroutine(Change_Color_Return_To_Origin(new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), 1.5f, false));
            StopCoroutine(rotate_bullet);
            yield return null;
        }
    }
    IEnumerator Pattern_1()
    {
        IEnumerator move_Second = Move_Second(7, -2, 7, 2);
        IEnumerator rotate = Rotate(180);
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
        IEnumerator rotate = Rotate(180);
      
        yield return Change_Color_Temporary(new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), 3, 1.5f, Boss_Disappear_1);
        transform.position = new Vector3(-2, 2, 0);
        yield return Change_Color_Temporary(new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), 3, 0, Boss_Disappear_1);

        StartCoroutine(move_Second);
        StartCoroutine(rotate);
       
       
        for (int i = 0; i < 3; i++)
        {
            StartCoroutine(Boss_W1(162 + (i * 20), 10, 180));
            yield return new WaitForSeconds(0.8f);
            Instantiate(Boss_Weapon[1], transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.4f);
        }

        StopCoroutine(move_Second);
        StopCoroutine(rotate);
        yield break;
    }
    IEnumerator First_Move()
    {
        yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(transform.position.x, -3, 0), 2, inclineCurve));
        yield return StartCoroutine(Boss_W1(72, 7, 150));
        cameraShake.Shake();

        yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(-7, 3, 0), 2, inclineCurve));
        yield return StartCoroutine(Boss_W1(252, 7, 150));
        cameraShake.Shake();
       
        yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(-7, -3, 0), 2, inclineCurve));
        yield return StartCoroutine(Boss_W1(-18, 7, 150));
        cameraShake.Shake();

        yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(7, 3, 0), 2, inclineCurve));
        yield return StartCoroutine(Boss_W1(162, 7, 150));
        cameraShake.Shake();

        yield return StartCoroutine(Position_Curve(transform.position, new Vector3(-7, -4, 0), 2.2f, "up"));
        yield return StartCoroutine(Position_Curve(transform.position, new Vector3(7, 0, 0), 2.2f, "down"));
    }
    IEnumerator Move_Second(float x_f, float y_f, float x_l, float y_l)
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
    IEnumerator Boss_W1(float start_angle, int count, float range_angle)
    {
        float count_per_radian = range_angle / count;
        float intervalAngle = start_angle;
        for (int i = 0; i < count; i++)
        {
            float angle = intervalAngle + (i * count_per_radian);
            float x = Mathf.Cos(angle * Mathf.PI / 180.0f);
            float y = Mathf.Sin(angle * Mathf.PI / 180.0f);
            Launch_Weapon_For_Move(Boss_Weapon[0], new Vector3(x, y), Quaternion.identity, 2f);
            yield return null;
        }
    }
    void Update()
    {

    }
}
