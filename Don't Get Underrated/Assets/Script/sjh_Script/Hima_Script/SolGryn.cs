using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolGryn : Boss_Info
{
    // Start is called before the first frame update

    // Update is called once per frame

    // <a href='https://kr.freepik.com/vectors/light'>Light 벡터는 upklyak - kr.freepik.com가 제작함</a>

    [SerializeField]
    GameObject SolGryn_HP;

    [SerializeField]
    GameObject PalJeongDo_Thunder;

    HimaController himaController;

    BackGroundColor PalJeongDo;

    TrailRenderer trailRenderer;

    IEnumerator nachi_x_i_1, nachi_x_i_2;

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
        
        backGroundColor = GameObject.Find("Flash").GetComponent<BackGroundColor>();
        PalJeongDo = GameObject.Find("PalJeongDo").GetComponent<BackGroundColor>();
        PalJeongDo.Set_BGColor(new Color(1, 1, 1, 0));
        himaController = GameObject.FindGameObjectWithTag("Player").GetComponent<HimaController>();
        trailRenderer = GetComponent<TrailRenderer>();
        SolGryn_HP.SetActive(false);
        trailRenderer.enabled = false;
        trailRenderer = GetComponent<TrailRenderer>();
        transform.position = new Vector3(0, -15.81f, 0);
        transform.localScale = new Vector3(1.5f, 1.5f, 0);
    }
    public void OnTriggerEnter2D(Collider2D collision) // 얘만
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("야");
            collision.GetComponent<HimaController>().TakeDamage(1);
        }
    }
    public void WelCome()
    {
        // StartCoroutine(I_WelCome());
        //StartCoroutine(Boss_Pattern());
        StartCoroutine(TeoKisis());
    }
    IEnumerator TeoKisis()
    {
        yield return StartCoroutine(Position_Lerp(new Vector3(0, 7, 0), new Vector3(0, 0, 0), 7f, declineCurve));
        yield return YieldInstructionCache.WaitForSeconds(1f);

        camera_shake = cameraShake.Shake_Act(.01f, .01f, 1, true);
        StartCoroutine(camera_shake); // 등장할 때 한번 흔들어 재껴줘야함
        yield return StartCoroutine(Change_Color_Return_To_Origin(Color.white, new Color(1, 69 / 255, 69 / 255, 1), 1, false));
        StopCoroutine(camera_shake);

        GameObject a = Instantiate(Weapon[5], transform.position, Quaternion.identity);
        GameObject b = Instantiate(Weapon[5], transform.position, Quaternion.identity);
        GameObject c = Instantiate(Weapon[5], transform.position, Quaternion.identity);
        GameObject d = Instantiate(Weapon[5], transform.position, Quaternion.identity);

        a.GetComponent<SolGryn_Copy>().Move_Lerp_Distance(new Vector3(7, 2.5f, 0));
        b.GetComponent<SolGryn_Copy>().Move_Lerp_Distance(new Vector3(7, -2.5f, 0));
        c.GetComponent<SolGryn_Copy>().Move_Lerp_Distance(new Vector3(-7, 2.5f, 0));
        d.GetComponent<SolGryn_Copy>().Move_Lerp_Distance(new Vector3(-7, -2.5f, 0));

        yield return YieldInstructionCache.WaitForSeconds(2f);

        yield return StartCoroutine(backGroundColor.Change_Color(backGroundColor.Get_BGColor(), new Color(1, 1, 1, 1), 1));
        yield return StartCoroutine(PalJeongDo.Change_Color(PalJeongDo.Get_BGColor(), new Color(1, 1, 1, 1), 1));
        yield return StartCoroutine(backGroundColor.Change_Color(backGroundColor.Get_BGColor(), new Color(1, 1, 1, 0), 2));

        yield return YieldInstructionCache.WaitForSeconds(1f);

        yield return StartCoroutine(PalJeongDo.Change_Color(PalJeongDo.Get_BGColor(), new Color(1, 1, 1, 0.5f), 2)); // 효과음 넣어야한다 (낮아지는 소리)
        GameObject.Find("Flash").transform.SetAsLastSibling();

        Instantiate(PalJeongDo_Thunder, transform.position, Quaternion.identity);
        yield return StartCoroutine(backGroundColor.Change_Color(backGroundColor.Get_BGColor(), new Color(1, 1, 1, 1), 0.2f));
        StartCoroutine(PalJeongDo.Change_Color(PalJeongDo.Get_BGColor(), new Color(1, 0, 0, 1), 0.5f));
        yield return StartCoroutine(backGroundColor.Change_Color(backGroundColor.Get_BGColor(), new Color(1, 1, 1, 0), 1));

        yield return YieldInstructionCache.WaitForSeconds(2f);

        Destroy(a); Destroy(b); Destroy(c); Destroy(d);

        yield return StartCoroutine(Size_Change(transform.localScale, transform.localScale / 2, 0.5f, OriginCurve));
        StartCoroutine(backGroundColor.Change_Color(backGroundColor.Get_BGColor(), new Color(1, 1, 1, 1), 1));
        yield return StartCoroutine(Size_Change(transform.localScale, transform.localScale * 20, 1f, inclineCurve));

        yield return YieldInstructionCache.WaitForSeconds(1.5f); // 이동 후 카메라 정지 + 1.5초 정지

        StartCoroutine(PalJeongDo.Change_Color(PalJeongDo.Get_BGColor(), new Color(1, 1, 1, 0), 0.5f));
        yield return StartCoroutine(backGroundColor.Change_Color(backGroundColor.Get_BGColor(), new Color(0, 0, 0, 1), 1));
        Debug.Log(PalJeongDo.Get_BGColor());

        yield return YieldInstructionCache.WaitForSeconds(1.5f);  // 검정색 플래시 후 1.5초 정지

        StartCoroutine(backGroundColor.Change_Color(backGroundColor.Get_BGColor(), new Color(1, 1, 1, 0), 1));
        yield return StartCoroutine(Boss_Pattern());

    }
    IEnumerator Nachi_Color_Change(Color Origin_C, Color Change_C, float time_persist, bool is_Continue)
    {
        for (int i = 0; i < 3; i++)
        {
            percent = 0;
            while (percent < 1)
            {
                percent += Time.deltaTime / time_persist;
                trailRenderer.endColor = Color.Lerp(Origin_C, Change_C, percent);
                trailRenderer.startColor = Color.Lerp(Origin_C, Change_C, percent);
                yield return null;
            }
            if (!is_Continue)
                yield break;
            percent = 0;
            while (percent < 1)
            {
                percent += Time.deltaTime / time_persist;
                trailRenderer.endColor = Color.Lerp(Change_C, Origin_C, percent);
                trailRenderer.startColor = Color.Lerp(Change_C, Origin_C, percent);
                yield return null;
            }
        }
    }
    IEnumerator Boss_Pattern()
    {
        himaController.IsMove = true;
        transform.position = new Vector3(7, 4, 0);
        transform.localScale = new Vector3(1.4f, 1.4f, 0);
        yield return YieldInstructionCache.WaitForSeconds(.5f);

        SolGryn_HP.SetActive(true);
        SolGryn_HP.GetComponent<BossHPSliderViewer>().F_HPFull(gameObject.GetComponent<SolGryn>());
        StartCoroutine(HP_Decrease());

        camera_shake = cameraShake.Shake_Act(.02f, .02f, 1, true);
        StartCoroutine(camera_shake);

        // -3.79 -0.4

        yield return StartCoroutine(Position_Slerp_Temp(transform.position, new Vector3(-4, 4, 0), 
            Get_Center_Vector(transform.position, new Vector3(-4, 4, 0), Vector3.Distance(transform.position, new Vector3(-4, 4, 0)) * 0.85f, "anti_clock"), 4, OriginCurve, false));

        trailRenderer.enabled = true;
        yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(-4f, -0.4f, 0), 0.1f, declineCurve));
        yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(4f, -0.4f, 0), 0.1f, declineCurve));
        yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(4f, -6f, 0), 0.1f, declineCurve));

        transform.position = new Vector3(4, 2, 0);
        yield return StartCoroutine(Change_Color_Lerp(new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), 0.5f, 0, DisAppear_Effect_1));

        yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(0, 2, 0), 0.1f, declineCurve));
        yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(0, -3.35f, 0), 0.1f, declineCurve));
        yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(-4f, -3.35f, 0), 0.1f, declineCurve));
        yield return StartCoroutine(Change_Color_Lerp(SpriteRenderer_Color, new Color(1, 1, 1, 0), 0.33f, 0, DisAppear_Effect_1));
        yield return StartCoroutine(Nachi_Color_Change(Color.red, Color.blue, 0.5f, true));


        GameObject nachi_x_g_1 = Instantiate(Weapon[6], new Vector3(6.3f, 1.87f, 0), Quaternion.identity);
        GameObject nachi_x_g_2 = Instantiate(Weapon[6], new Vector3(-5.61f, 1.64f, 0), Quaternion.identity);

        nachi_x_i_1 = nachi_x_g_1.GetComponent<Nachi_X>().Move(-1);
        nachi_x_i_2 = nachi_x_g_2.GetComponent<Nachi_X>().Move(1);

        StartCoroutine(nachi_x_i_1);
        yield return StartCoroutine(nachi_x_i_2);
        yield return StartCoroutine(Nachi_Color_Change(Color.red, new Color(1, 0, 0, 0), 1, false));

        ////yield return StartCoroutine(First_Move());

        ////yield return StartCoroutine(Pattern_1());
        ////yield return StartCoroutine(Pattern_2());
        //yield return StartCoroutine(Pattern_3());
        //yield return StartCoroutine(Pattern_4());
    }
    IEnumerator First_Move()
    {

        yield return null;
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
        transform.rotation = Quaternion.Euler(0, 0, 0);
        cameraShake.Shake_Act(.9f, .6f, 1, false);

        yield return YieldInstructionCache.WaitForSeconds(0.5f);
        yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(transform.position.x, transform.position.y - 3, 0), 1, inclineCurve));
        yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(transform.position.x, 11, 0), 1, declineCurve));

        Instantiate(Weapon[4], new Vector3(2.52f, -8.31f, 0), Quaternion.identity);
        yield return YieldInstructionCache.WaitForSeconds(1.4f);

        Instantiate(Weapon[4], new Vector3(0, -8.31f, 0), Quaternion.identity);
        yield return YieldInstructionCache.WaitForSeconds(1.4f);

        Instantiate(Weapon[4], new Vector3(-2.52f, -8.31f, 0), Quaternion.identity);
        yield return YieldInstructionCache.WaitForSeconds(1);

        GameObject W1 = Instantiate(Weapon[3], transform.position, Quaternion.identity);
        W1.GetComponent<DynaBlade>().Dyna_Start(false);
        yield return YieldInstructionCache.WaitForSeconds(1.5f);

        cameraShake.mainCamera.transform.position = new Vector3(0, 0, -10);
        cameraShake.mainCamera.transform.rotation = Quaternion.identity;
        cameraShake.mainCamera.transform.localScale = new Vector3(1, 1, 1);
        yield return null;
        GameObject W2 = Instantiate(Weapon[3], transform.position, Quaternion.identity);
        W2.GetComponent<DynaBlade>().Dyna_Start(true);
        yield return YieldInstructionCache.WaitForSeconds(1.5f);

        cameraShake.mainCamera.transform.position = new Vector3(0, 0, -10);
        cameraShake.mainCamera.transform.rotation = Quaternion.identity;
        cameraShake.mainCamera.transform.localScale = new Vector3(1, 1, 1);
        yield return null;
        GameObject W3 = Instantiate(Weapon[3], transform.position, Quaternion.identity);
        GameObject W4 = Instantiate(Weapon[3], transform.position, Quaternion.identity);
        W3.GetComponent<DynaBlade>().Dyna_Start(false);
        W4.GetComponent<DynaBlade>().Dyna_Start(true);
        yield return YieldInstructionCache.WaitForSeconds(2);

    }
    IEnumerator Pattern_4()
    {
        IEnumerator rotate_bullet = Rotate_Bullet(7, 200, 0.02f, 4, Weapon[2]);

        while (true)
        {
            yield return Change_Color_Lerp(new Color(1, 1, 1, 0), new Color(1, 1, 1, 0), 0.33f, 1.5f, DisAppear_Effect_2);

            camera_shake = cameraShake.Shake_Act(.035f, .2f, 1, true);
            StartCoroutine(camera_shake);
            StartCoroutine(rotate_bullet);

            int x = Random.Range(0, 5);
            transform.position = new Vector3(move_random[x, 0], move_random[x, 1], 0);

            yield return StartCoroutine(Change_Color_Return_To_Origin(new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), 0.67f, false));
            StopCoroutine(camera_shake);
            cameraShake.Origin_Camera();
            StopCoroutine(rotate_bullet);
            
            yield return null;
        }
    }
    IEnumerator Pattern_1()
    {
        IEnumerator move_Second = Move_Round_Trip(7, -2, 7, 2);
        IEnumerator rotate = Rotate(150);
        StartCoroutine(move_Second);
        StartCoroutine(rotate);

        for (int i = 0; i < 7; i++)
        {
            StartCoroutine(cameraShake.Shake_Act(.07f, .2f, 0.1f, false));
            StartCoroutine(Boss_W1(72 + (i * 20), 25, 360));
            yield return new WaitForSeconds(0.6f);
        }

        StopCoroutine(move_Second);
        StopCoroutine(rotate);
        yield return StartCoroutine(Change_Color_Lerp(new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), 0.33f, 0, DisAppear_Effect_1));
        yield break;
    }
    IEnumerator Pattern_2() // 각도 수정 & 
    {
        transform.position = new Vector3(-2, 2, 0);
        StartCoroutine(Change_Color_Lerp(new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), 0.33f, 0, DisAppear_Effect_1));

        IEnumerator move_Second = Move_Round_Trip(-2f, 2, 2f, 2);
        IEnumerator rotate = Rotate(150);
        StartCoroutine(move_Second);
        StartCoroutine(rotate);


        for (int i = 0; i < 6; i++)
        {
            StartCoroutine(cameraShake.Shake_Act(.07f, .2f, 0.2f, false));

            StartCoroutine(Boss_W1(110 + (i * 20), 30, 360));
            yield return new WaitForSeconds(0.4f); // 탄알 발사

            Instantiate(Weapon[1], transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.4f); // 땅콩 발사
        }

        StopCoroutine(move_Second);
        StopCoroutine(rotate);
        yield break;
    }
    IEnumerator Move_Round_Trip(float x_f, float y_f, float x_l, float y_l)
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
            Launch_Weapon_For_Move(Weapon[0], new Vector3(x, y), Quaternion.identity, 2f);
        }
        yield return null;
    }
}
