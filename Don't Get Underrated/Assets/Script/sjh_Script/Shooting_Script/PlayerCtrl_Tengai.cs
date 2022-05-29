using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerCtrl_Tengai : Player_Info
{
    // Start is called before the first frame update

    [SerializeField]
    KeyCode keyCodeAttack = KeyCode.Space;

    [SerializeField]
    KeyCode keyCodeBoom = KeyCode.Z;

    [SerializeField]
    TextMeshProUGUI PlayerScore;

    [SerializeField]
    TextMeshProUGUI LifeTime_Text;

    [SerializeField]
    TextMeshProUGUI BoomCountText;

    [SerializeField]
    GameObject Emit_Obj;

    [SerializeField]
    GameObject Explode;

    Emit_Motion emit_Motion;

    [SerializeField]
    int BoomCount = 3;

    [SerializeField]
    GameObject Boom_Effect;

    [SerializeField]
    GameObject Boom_Up_Text;

    [SerializeField]
    GameObject Power_Effect;

    [SerializeField]
    GameObject Power_Up_Text;

    [SerializeField]
    GameObject naum_ani;

    Animator animator; // 애니메이터는 여러개 추가될 수 있어서 상속 생략

    GameObject Emit_Obj_Copy; // 고유

    bool is_LateUpdate = false;

    bool is_Update = false;

    bool is_Power_Up = false;

    bool boss_intend;

    IEnumerator emit_expand_circle, emit_change_size, i_start_emit, i_start_firing, color_when_unbeatable;

    private new void Awake()
    {
        base.Awake();
        movement2D = GetComponent<Movement2D>();
        animator = GetComponent<Animator>();
        backGroundColor = GameObject.FindGameObjectWithTag("Flash").GetComponent<ImageColor>();

        is_Update = false;
        is_LateUpdate = false;
        weapon_able = false;
        movement2D.enabled = false;
        Unbeatable = true;
        boss_intend = false;
        is_Power_Up = false;
        Final_Score = 0;
        emit_Motion = null;

        PlayerScore.text = "점수 : " + Final_Score;
        PlayerScore.color = new Color(PlayerScore.color.r, PlayerScore.color.g, PlayerScore.color.b, 0);

        LifeTime_Text.text = "Life x  : " + LifeTime;
        LifeTime_Text.color = new Color(LifeTime_Text.color.r, LifeTime_Text.color.g, LifeTime_Text.color.b, 0);

        BoomCountText.text = "폭탄 : " + BoomCount;
        BoomCountText.color = new Color(BoomCountText.color.r, BoomCountText.color.g, BoomCountText.color.b, 0);
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
       
        yield return Move_Straight(My_Position, new Vector3(-4.6f, My_Position.y, My_Position.z), 2.5f, OriginCurve);
           
        weapon_able = true;
        is_LateUpdate = true;
        is_Update = true;
        movement2D.enabled = true;
        movement2D.MoveSpeed = 10;
        

        Run_Life_Act(FadeText());
        yield return new WaitForSeconds(2f);

        if (!boss_intend)
        {
            GameObject.Find("EnemyAndBoss").GetComponent<FinalStage_1_Total>().Boss_First_Appear();
            boss_intend = true;
        }
        else
            Unbeatable = false;

        Stop_Life_Act(ref color_when_unbeatable);
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }
    IEnumerator FadeText()
    {
        if (PlayerScore.color.a >= 1f)
            yield break;
        while (PlayerScore.color.a < 1.0f)
        {
            LifeTime_Text.color = new Color(LifeTime_Text.color.r, LifeTime_Text.color.g, LifeTime_Text.color.b, LifeTime_Text.color.a + Time.deltaTime / 2);
            PlayerScore.color = new Color(PlayerScore.color.r, PlayerScore.color.g, PlayerScore.color.b, PlayerScore.color.a + Time.deltaTime / 2);
            BoomCountText.color = new Color(BoomCountText.color.r, BoomCountText.color.g, BoomCountText.color.b, BoomCountText.color.a + (Time.deltaTime / 2.0f));
            yield return null;
        }
    }
    public override void TakeDamage(int damage)
    {
        if (Unbeatable)
            return;
        Instantiate(When_Dead_Effect, transform.position, Quaternion.identity);
        LifeTime -= damage;
        animator.SetBool("Dead", true);
       
        Unbeatable = true;
        weapon_able = false;
        is_LateUpdate = false;
        movement2D.enabled = false;
        is_Update = false;

        LifeTime_Text.text = "Life x  : " + LifeTime;

        if (Emit_Obj_Copy != null && emit_Motion != null && i_start_emit != null)
        {
            StopCoroutine(i_start_emit);
            Destroy(Emit_Obj_Copy);
        }

        if (LifeTime <= 0)
            OnDie();

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
            Get_Center_Vector(My_Position, My_Position + 2.5f * Vector3.left, Vector3.Distance(My_Position, My_Position + 2.5f * Vector3.left) * 0.85f, "clock"));

        yield return Move_Curve(My_Position, My_Position + 2.5f * Vector3.left, 
            Get_Center_Vector(My_Position, My_Position + 2.5f * Vector3.left, Vector3.Distance(My_Position, My_Position + 2.5f * Vector3.left) * 0.85f, "clock"), 0.3f, OriginCurve);

        float kuku = ((1.215f * My_Position.x) - My_Position.y - 7) / 1.215f;

        float B = Vector3.Distance(My_Position, new Vector3(kuku, -7, 0));
        yield return Move_Straight(My_Position, new Vector3(kuku, -7, 0), B/A * 0.3f, OriginCurve);
       
    }
    public void Start_Emit()
    {
        Unbeatable = true;
        Run_Life_Act_And_Continue(ref i_start_emit, I_Start_Emit());
    }
    IEnumerator I_Start_Emit()
    {
        yield return null;
        Emit_Obj_Copy = Instantiate(Emit_Obj, My_Position, Quaternion.identity);

        emit_Motion = null;

        if (Emit_Obj_Copy.TryGetComponent(out Emit_Motion EM))
        {
            emit_Motion = EM;
            emit_expand_circle = emit_Motion.Emit_Expand_Circle();
            emit_change_size = emit_Motion.Emit_Change_Size();
        }
        else
            yield break;

        emit_Motion.StartCoroutine(emit_change_size);

        yield return YieldInstructionCache.WaitForSeconds(5f);

        Unbeatable = true;

        emit_Motion.StopCoroutine(emit_change_size);

        Change_BG(Color.white, 2);
        yield return emit_expand_circle;

        Destroy(Emit_Obj_Copy);

        Flash(new Color(1, 1, 1, 1), 0.1f, 2);

        GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] meteor = GameObject.FindGameObjectsWithTag("Meteor");
        GameObject[] weapon_devil = GameObject.FindGameObjectsWithTag("Weapon_Devil");
        foreach (var e in enemy)
            Destroy(e);
        foreach (var e in meteor)
            Destroy(e);
        foreach (var e in weapon_devil)
            Destroy(e);

        GameObject.FindGameObjectWithTag("Boss").GetComponent<Asura>().Stop_Meteor();

        Instantiate(Explode, Vector3.zero, Quaternion.identity);
        Instantiate(naum_ani, Vector3.zero, Quaternion.identity);
        Camera_Shake(0.025f, 2, true, false);
    }
    public override void OnDie()
    {
        Destroy(gameObject); // 게임오버 씬 + 에니메이션 추가해야한다.
        return;
    }

    // Update is called once per frame
    void Update()
    {
        if (is_Update)
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            movement2D.MoveTo(new Vector3(x, y, 0));
        }
        PlayerScore.text = "점수 : " + Final_Score;
        if (Input.GetKeyDown(keyCodeAttack) && weapon_able)
        {
            animator.SetBool("Launch", true);
            StartFiring();
        }
        else if (Input.GetKeyUp(keyCodeAttack) || !weapon_able)
        {
            animator.SetBool("Launch", false);
            StopFiring();
        }
        if (Input.GetKeyDown(keyCodeBoom))
            StartBoom();
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameObject e = Instantiate(Boom_Up_Text, Vector3.zero, Quaternion.identity);
            e.transform.SetParent(transform);
            GameObject f = Instantiate(Boom_Effect, transform.position, Quaternion.Euler(-90, 0, 0));
            f.transform.SetParent(transform);
            f.transform.localRotation = Quaternion.Euler(-90, 0, 0);
            BoomCount++;
            BoomCountText.text = "폭탄 : " + BoomCount;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameObject e = Instantiate(Power_Up_Text, Vector3.zero, Quaternion.identity);
            e.transform.SetParent(transform);
            GameObject f = Instantiate(Power_Effect, transform.position, Quaternion.Euler(-90, 0, 0));
            f.transform.SetParent(transform);
            f.transform.localRotation = Quaternion.Euler(-90, 0, 0);
            is_Power_Up = true;
        }
    }
    void StartFiring()
    {
        i_start_firing = I_Start_Firing();
        StartCoroutine(i_start_firing);
    }
    void StopFiring()
    {
        if (i_start_firing != null)
            StopCoroutine(i_start_firing);
    }
    IEnumerator I_Start_Firing()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
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
            BoomCountText.text = "폭탄 : " + BoomCount;
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
