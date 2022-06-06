using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Final1 : Player_Info
{
    [SerializeField]
    KeyCode keyCodeAttack = KeyCode.Space;

    [SerializeField]
    KeyCode keyCodeBoom = KeyCode.Z;

    [SerializeField]
    Text DeathCount_Text;

    [SerializeField]
    Text BoomCount_Text;

    [SerializeField]
    GameObject Emit_Obj;

    [SerializeField]
    GameObject Explode_When_Emit;

    [SerializeField]
    int BoomCount = 3;

    [SerializeField]
    GameObject Naum_Ami;

    [SerializeField]
    public GameObject Power_Slider;

    [SerializeField]
    public GameObject Speed_Slider;

    [SerializeField]
    int deathCount = 0;

    Animator animator; // 애니메이터는 여러개 추가될 수 있어서 상속 생략

    GameObject Emit_Obj_Copy; // 고유

    private bool is_LateUpdate = false;

    private bool is_Update = false;

    public bool is_Power_Up = false;

    public bool is_Speed_Up = false;

    private bool is_boss_first_appear;

    IEnumerator i_start_firing, color_when_unbeatable, i_start_emit;

    public int DeathCount => deathCount;

    private new void Awake()
    {
        base.Awake();
        movement2D = GetComponent<Movement2D>();
        animator = GetComponent<Animator>();
        imageColor = GameObject.FindGameObjectWithTag("Flash").GetComponent<ImageColor>();

        is_Update = false;
        is_LateUpdate = false;
        weapon_able = false;
        movement2D.enabled = false;
        Unbeatable = true;
        is_boss_first_appear = false;
        is_Power_Up = false;
        Final_Stage_1_Score = 0;
        Power_Slider.SetActive(false);
        Speed_Slider.SetActive(false);

        DeathCount_Text.text = "Death Count : " + deathCount;
        DeathCount_Text.color = new Color(DeathCount_Text.color.r, DeathCount_Text.color.g, DeathCount_Text.color.b, 0);

        BoomCount_Text.text ="Boom : " + BoomCount;
        BoomCount_Text.color = new Color(BoomCount_Text.color.r, BoomCount_Text.color.g, BoomCount_Text.color.b, 0);

        My_Name.text = singleTone.id;
        My_Name.color = new Color(My_Name.color.r, My_Name.color.g, My_Name.color.b, 0);

        if (GameObject.Find("Player_Effect_Sound") && GameObject.Find("Player_Effect_Sound").TryGetComponent(out AudioSource AS1))
            EffectSource = AS1;
        if (GameObject.Find("Player_BackGround_Sound") && GameObject.Find("Player_BackGround_Sound").TryGetComponent(out AudioSource AS2))
            BackGroundSource = AS2;
    }
    void Start()
    {
        StartCoroutine(Move_First());
    }
    IEnumerator Move_First()
    {
        animator.SetBool("Dead", false);
        Run_Life_Act_And_Continue(ref color_when_unbeatable, My_Color_When_UnBeatable());

        My_Position = new Vector3(-9, 0, 0);
       
        yield return Move_Straight(My_Position, new Vector3(-4.6f, My_Position.y, My_Position.z), 1.8f, OriginCurve);
        weapon_able = true;
        is_LateUpdate = true;
        is_Update = true;
        movement2D.enabled = true;
        movement2D.MoveSpeed = 10;

        Run_Life_Act(FadeText());
        yield return YieldInstructionCache.WaitForSeconds(2f);

        if (!is_boss_first_appear)
        {
            if (GameObject.Find("EnemyAndBoss") && GameObject.Find("EnemyAndBoss").TryGetComponent(out FinalStage_1_Total FS1_T))
               FS1_T.Boss_First_Appear();
            is_boss_first_appear = true;
        }
        else
            Unbeatable = false;

        Stop_Life_Act(ref color_when_unbeatable);
        My_Color = Color.white;
    }
    IEnumerator FadeText()
    {
        if (DeathCount_Text.color.a >= 1f)
            yield break;
        while (DeathCount_Text.color.a < 1.0f)
        {
            My_Name.color = new Color(My_Name.color.r, My_Name.color.g, My_Name.color.b, My_Name.color.a + Time.deltaTime / 2);
            DeathCount_Text.color = new Color(DeathCount_Text.color.r, DeathCount_Text.color.g, DeathCount_Text.color.b, DeathCount_Text.color.a + Time.deltaTime * 0.5f);
            BoomCount_Text.color = new Color(BoomCount_Text.color.r, BoomCount_Text.color.g, BoomCount_Text.color.b, BoomCount_Text.color.a + (Time.deltaTime * 0.5f));
            yield return null;
        }
    }
    public override void TakeDamage(int damage)
    {
        if (Unbeatable)
            return;
        Unbeatable = true;

        if (Emit_Obj_Copy != null)
        {
            Stop_Life_Act(ref i_start_emit);
            Destroy(Emit_Obj_Copy);
        }

        StopFiring();
        Effect_Sound_OneShot(2);

        Instantiate(When_Dead_Effect, transform.position, Quaternion.identity);
        deathCount += damage;
        animator.SetBool("Dead", true);
       
        weapon_able = false;
        movement2D.enabled = false;
        is_Update = false;
        is_LateUpdate = false;

        DeathCount_Text.text = "Death Count : " + deathCount;

        Run_Life_Act(Damage_After());
    }

    IEnumerator Damage_After()
    {
        yield return MovePath();

        Run_Life_Act(Move_First());
    }
    IEnumerator MovePath() // 여기 수정
    {
        float A = Get_Curve_Distance(My_Position, My_Position + 2.5f * Vector3.left, 
            Get_Center_Vector_For_Curve_Move(My_Position, My_Position + 2.5f * Vector3.left, Vector3.Distance(My_Position, My_Position + 2.5f * Vector3.left) * 0.85f, "clock"));

        yield return Move_Curve(My_Position, My_Position + 2.5f * Vector3.left,
            Get_Center_Vector_For_Curve_Move(My_Position, My_Position + 2.5f * Vector3.left, Vector3.Distance(My_Position, My_Position + 2.5f * Vector3.left) * 0.85f, "clock"), 0.25f, OriginCurve);

        float kuku = ((1.215f * My_Position.x) - My_Position.y - 7) / 1.215f;

        float B = Vector3.Distance(My_Position, new Vector3(kuku, -7, 0));
        yield return Move_Straight(My_Position, new Vector3(kuku, -7, 0), B/A * 0.25f, OriginCurve);
       
    }
    public void Start_Emit()
    {
        Run_Life_Act_And_Continue(ref i_start_emit, I_Start_Emit());
    }
    IEnumerator I_Start_Emit()
    {
        Emit_Obj_Copy = Instantiate(Emit_Obj, My_Position, Quaternion.identity);

        if (Emit_Obj_Copy.TryGetComponent(out Emit_Motion EM))
        {
            if (!EM.Check_Valid_Emit())
                yield break;

            EM.Ready_To_Expand();

            yield return YieldInstructionCache.WaitForSeconds(8f);

            Unbeatable = true;

            Change_BG(Color.white, 1.5f);

            yield return EM.Expand();

            Destroy(Emit_Obj_Copy);

            Flash(Color.white, 0.1f, 2);

            if (GameObject.FindGameObjectWithTag("Boss") && GameObject.FindGameObjectWithTag("Boss").TryGetComponent(out Asura A))
                A.Stop_Meteor();

            Instantiate(Explode_When_Emit, Vector3.zero, Quaternion.identity);
            Instantiate(Naum_Ami, Vector3.zero, Quaternion.identity);
            Camera_Shake(0.025f, 2, true, false);
            Unbeatable = false;
        }
        yield return null;
    }
    public void Power_Up()
    {
        if (Power_Slider.TryGetComponent(out PowerSliderViewer PSV))
        {
            Effect_Sound_OneShot(1);
            Power_Slider.SetActive(true);
            GameObject power_text = Instantiate(Item[0], Vector3.zero, Quaternion.identity);
            power_text.transform.SetParent(transform);
            GameObject power_particle = Instantiate(Item[1], transform.position, Quaternion.Euler(-90, 0, 0));
            power_particle.transform.SetParent(transform);
            power_particle.transform.localRotation = Quaternion.Euler(-90, 0, 0);
            is_Power_Up = true;
            PSV.Stop_To_Decrease();
            PSV.Start_To_Decrease(3);
        }
    }
    public void Speed_Up()
    {
        if (Speed_Slider.TryGetComponent(out SpeedSliderViewer SSV))
        {
            Effect_Sound_OneShot(1);
            Speed_Slider.SetActive(true);
            GameObject speed_text = Instantiate(Item[2], Vector3.zero, Quaternion.identity);
            speed_text.transform.SetParent(transform);
            GameObject speed_particle = Instantiate(Item[3], transform.position, Quaternion.Euler(-90, 0, 0));
            speed_particle.transform.SetParent(transform);
            speed_particle.transform.localRotation = Quaternion.Euler(-90, 0, 0);
            is_Speed_Up = true;
            SSV.Stop_To_Decrease();
            SSV.Start_To_Decrease(3);
        }
    }
    public void Boom_Up()
    {
        Effect_Sound_OneShot(1);
        GameObject boom_text = Instantiate(Item[4], Vector3.zero, Quaternion.identity);
        boom_text.transform.SetParent(transform);
        GameObject boom_particle = Instantiate(Item[5], transform.position, Quaternion.Euler(-90, 0, 0));
        boom_particle.transform.SetParent(transform);
        boom_particle.transform.localRotation = Quaternion.Euler(-90, 0, 0);
        BoomCount++;
        BoomCount_Text.text = "Boom : " + BoomCount;
    }
    void Update()
    {
        if (is_Update)
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            if (is_Speed_Up)
                movement2D.MoveTo(new Vector3(x * 1.5f, y * 1.5f, 0));
            else
                movement2D.MoveTo(new Vector3(x, y, 0));
        }
       
        if (Input.GetKeyDown(keyCodeAttack) && weapon_able)
        {
            animator.SetBool("Launch", true);
            StartFiring();
        }

        else if (Input.GetKeyUp(keyCodeAttack))
        {
            animator.SetBool("Launch", false);
            StopFiring();
        }
        if (Input.GetKeyDown(keyCodeBoom) && weapon_able)
            StartBoom();
    }

    void StartFiring()
    {
        Effect_Sound_Play(0);
        Run_Life_Act_And_Continue(ref i_start_firing, I_Start_Firing());
    }
    void StopFiring()
    {
        Effect_Sound_Stop();
        Stop_Life_Act(ref i_start_firing);
    }
    IEnumerator I_Start_Firing()
    {
        while (true)
        {
            yield return YieldInstructionCache.WaitForSeconds(0.08f);
            if (is_Power_Up)
            {
                Launch_Weapon(ref Weapon[0], new Vector3(1, -0.1f, 0), Quaternion.identity, 18, transform.position + new Vector3(0.77f, -0.3f, 0));
                Launch_Weapon(ref Weapon[0], new Vector3(1, 0.1f, 0), Quaternion.identity, 18, transform.position + new Vector3(0.77f, -0.3f, 0));
            }
            else
                Launch_Weapon(ref Weapon[0], Vector3.right, Quaternion.identity, 18, transform.position + new Vector3(0.77f, -0.3f, 0));
        }
    }
    void StartBoom()
    {
        if (BoomCount > 0)
        {
            BoomCount--;
            BoomCount_Text.text = "Boom : " + BoomCount;
            Instantiate(Weapon[1], new Vector3(0, 7.7f, 0), Quaternion.identity);
        }
    }
    void LateUpdate()
    {
        if (is_LateUpdate)
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, stageData.LimitMin.x, stageData.LimitMax.x),
            Mathf.Clamp(transform.position.y, stageData.LimitMin.y, stageData.LimitMax.y));
        }
    }
}
