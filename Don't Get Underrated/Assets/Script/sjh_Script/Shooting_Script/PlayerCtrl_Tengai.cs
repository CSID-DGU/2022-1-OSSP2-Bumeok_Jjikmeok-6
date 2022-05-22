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
    GameObject TalkPanel;

    [SerializeField]
    int BoomCount = 3;

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
        backGroundColor = GameObject.FindGameObjectWithTag("Flash").GetComponent<BackGroundColor>();

        is_Update = false;
        is_LateUpdate = false;
        weapon_able = false;
        movement2D.enabled = false;
        Unbeatable = true;
        boss_intend = false;
        is_Power_Up = false;
        Final_Score = 0;
        //TalkPanel.SetActive(true);

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
        color_when_unbeatable = Color_When_UnBeatable();
        StartCoroutine(color_when_unbeatable);

        transform.position = new Vector3(-9, 0, 0);

        yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(-4.6f, transform.position.y, transform.position.z), 2.5f, OriginCurve));

        weapon_able = true;
        is_LateUpdate = true;
        is_Update = true;
        movement2D.enabled = true;
        movement2D.MoveSpeed = 10;
        

        StartCoroutine(FadeText());
        yield return new WaitForSeconds(2f);
        //TalkPanel.SetActive(true);
        if (!boss_intend)
        {
            GameObject.Find("EnemyAndBoss").GetComponent<FinalStage_1_Total>().Boss_First_Appear();
            boss_intend = true;
        }
        else
            Unbeatable = false;

        StopCoroutine(color_when_unbeatable);
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
        Debug.Log("흠");
        LifeTime -= damage;

        Unbeatable = true;
        weapon_able = false;
        is_LateUpdate = false;
        movement2D.enabled = false;
        is_Update = false;

        LifeTime_Text.text = "Life x  : " + LifeTime;

        if (Emit_Obj_Copy != null)
        {
            if (i_start_emit != null)
                StopCoroutine(i_start_emit);
            if (emit_expand_circle != null)
                StopCoroutine(emit_expand_circle);
            if (emit_change_size != null)
                StopCoroutine(emit_change_size);
            Destroy(Emit_Obj_Copy);
        }

        if (LifeTime <= 0)
        {
            OnDie();
        }

        StartCoroutine(Damage_After());
    }
    IEnumerator Damage_After()
    {
        //GameObject e = Instantiate(When_Dead_Effect, transform.position, Quaternion.identity);
        yield return null;

        yield return StartCoroutine(MovePath());
        //Destroy(e);

        yield return StartCoroutine(Move_First());
        yield break;
    }
    IEnumerator MovePath() // 여기 수정
    {
        float A = Get_Slerp_Distance(transform.position, transform.position + 2.5f * Vector3.left, Get_Center_Vector(transform.position, transform.position + 2.5f * Vector3.left, Vector3.Distance(transform.position, transform.position + 2.5f * Vector3.left) * 0.85f, "clock"));

        yield return StartCoroutine(Position_Slerp_Temp(transform.position, transform.position + 2.5f * Vector3.left, Get_Center_Vector(transform.position, transform.position + 2.5f * Vector3.left, Vector3.Distance(transform.position, transform.position + 2.5f * Vector3.left) * 0.85f, "clock"), 0.3f, OriginCurve, false));

        float kuku = ((1.215f * transform.position.x) - transform.position.y - 7) / 1.215f;

        float B = Vector3.Distance(transform.position, new Vector3(kuku, -7, 0));
        yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(kuku, -7, 0), B/A * 0.3f, OriginCurve));
    }

    public void Start_Emit()
    {
        i_start_emit = I_Start_Emit();
        StartCoroutine(i_start_emit);
    }
    IEnumerator I_Start_Emit()
    {
        Emit_Obj_Copy = Instantiate(Emit_Obj, transform.position, Quaternion.identity);

        emit_expand_circle = Emit_Obj_Copy.GetComponent<Emit_Motion>().Emit_Expand_Circle();
        emit_change_size = Emit_Obj_Copy.GetComponent<Emit_Motion>().Emit_Change_Size();

        StartCoroutine(emit_change_size);

        yield return YieldInstructionCache.WaitForSeconds(5f);

        Unbeatable = true;
        yield return null;

        StopCoroutine(emit_change_size);
        yield return StartCoroutine(emit_expand_circle);

        Destroy(Emit_Obj_Copy);

        StartCoroutine(backGroundColor.Flash(new Color(1, 1, 1, 1), 0.1f, 5));

        GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] meteor = GameObject.FindGameObjectsWithTag("Meteor");
        GameObject[] weapon_devil = GameObject.FindGameObjectsWithTag("Weapon_Devil");
        foreach (var e in enemy)
        {
            Destroy(e);
        }
        foreach (var e in meteor)
        {
            Destroy(e);
        }
        foreach (var e in weapon_devil)
        {
            Destroy(e);
        }

        GameObject.FindGameObjectWithTag("Boss").GetComponent<DoPhan>().Stop_Meteor();

        GameObject u = Instantiate(When_Dead_Effect, Vector3.zero, Quaternion.identity);

        StartCoroutine(cameraShake.Shake_Act(0.4f, 0.4f, 0.3f, false));
        yield return YieldInstructionCache.WaitForSeconds(2f);

        Destroy(u);
    }
    public override void OnDie()
    {
        Destroy(gameObject); // 씬 추가해야한다.
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
            if (x < 0 || y < 0)
            {
                animator.SetBool("HasExit", true);
            }
            else
                animator.SetBool("HasExit", false);
        }
        PlayerScore.text = "점수 : " + Final_Score;
        if (Input.GetKeyDown(keyCodeAttack) && weapon_able)
        {
            StartFiring();
        }
        else if (Input.GetKeyUp(keyCodeAttack) || !weapon_able)
        {
            StopFiring();
        }
        if (Input.GetKeyDown(keyCodeBoom))
        {
            StartBoom();
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
            Instantiate(Weapon[0], transform.position, Quaternion.identity);
        }
    }
    void StartBoom()
    {
        if (BoomCount > 0)
        {
            BoomCount--;
            BoomCountText.text = "폭탄 : " + BoomCount;
            Instantiate(Weapon[1], transform.position, Quaternion.identity);
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
