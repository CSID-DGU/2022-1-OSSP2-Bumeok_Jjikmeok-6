using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerControl : Player_Info
{
    // Start is called before the first frame update

    Movement2D movement2D;

    Weapon weapon;

    int score;

    public int Score
    {
        set { score = value; }
        get { return score; }
    }

    [SerializeField]
    AnimationCurve curve_For_Boom;

    public bool Unbeatable_Player = false;

    [SerializeField]
    KeyCode keyCodeAttack = KeyCode.Space;

    [SerializeField]
    KeyCode keyCodeBoom = KeyCode.Z;

    [SerializeField]
    TextMeshProUGUI PlayerScore;

    [SerializeField]
    TextMeshProUGUI LifeTime_Text;

    [SerializeField]
    StageData stageData;

    Animator animator;

    bool is_LateUpdate = false;

    bool is_Update = false;

    [SerializeField]
    int LifeTime = 5;

    [SerializeField]
    GameObject Dead_Particle;

    [SerializeField]
    GameObject Emit_Obj;

    GameObject Emit_Obj_Copy;

    IEnumerator params_enum;

    private new void Awake()
    {
        base.Awake();
        movement2D = GetComponent<Movement2D>();
        weapon = GetComponent<Weapon>();
        
        animator = GetComponent<Animator>();
        is_Update = false;
        is_LateUpdate = false;
        weapon.enabled = false;
        movement2D.enabled = false;
        //Unbeatable_Player = true;
        score = 0;
        PlayerScore.text = "점수 : " + score;
        PlayerScore.color = new Color(PlayerScore.color.r, PlayerScore.color.g, PlayerScore.color.b, 0);
        LifeTime_Text.text = "Life x  : " + LifeTime;
        LifeTime_Text.color = new Color(LifeTime_Text.color.r, LifeTime_Text.color.g, LifeTime_Text.color.b, 0);
    }
    void Start()
    {
        Move_First();
    }
    public void Move_First()
    {
        StartCoroutine("Move_first");
    }
    IEnumerator Move_first()
    {
        StartCoroutine("UnBeatable_Apply");
        transform.position = new Vector3(-9, 0, 0);
        Unbeatable_Player = true;
        yield return null;
        while (true)
        {
            transform.position += Vector3.right * (Time.deltaTime * 2);
            yield return null;
            if (transform.position.x >= -4.6)
            {
                weapon.enabled = true;
                is_LateUpdate = true;
                is_Update = true;
                movement2D.enabled = true;
                
                movement2D.MoveSpeed = 10;
                yield return null;

                StartCoroutine("FadeText");
                yield return new WaitForSeconds(2f);
                Unbeatable_Player = false;
                yield return null;
                yield break;
            }
        }
    }
    IEnumerator FadeText()
    {
        while (PlayerScore.color.a < 1.0f)
        {
            LifeTime_Text.color = new Color(LifeTime_Text.color.r, LifeTime_Text.color.g, LifeTime_Text.color.b, LifeTime_Text.color.a + Time.deltaTime / 2);
            PlayerScore.color = new Color(PlayerScore.color.r, PlayerScore.color.g, PlayerScore.color.b, PlayerScore.color.a + Time.deltaTime / 2);
            yield return null;
        }
    }
    IEnumerator Damage_After()
    {
       
        weapon.enabled = false;
        is_LateUpdate = false;
        movement2D.enabled = false;
        is_Update = false;
        GameObject e = Instantiate(Dead_Particle, transform.position, Quaternion.identity);
        yield return null;

        yield return StartCoroutine(MovePath());
        Destroy(e);

        yield return StartCoroutine("Move_first");
        yield break;
    }
    IEnumerator MovePath()
    {
        Vector3 temp_position = transform.position;
        Vector3 Last_Position;
        float percent = 0;
        float Fall_X = 0;
        float Fall_Y = 0;
        float Params = 0;
        while (percent < 1)
        {
            Last_Position = transform.position;
            percent += Time.deltaTime * 2;
            Vector3 center = (temp_position + new Vector3(temp_position.x - 2.5f, temp_position.y, temp_position.z)) * 0.5f;
            center -= Vector3.up;
            Vector3 riseRelCenter = temp_position - center;
            Vector3 setRelCenter = new Vector3(temp_position.x - 2.5f, temp_position.y, temp_position.z) - center;

            transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, percent);

            transform.position += center;
            Fall_X = transform.position.x - Last_Position.x;
            Fall_Y = transform.position.y - Last_Position.y;
            yield return null;
        }
        Vector2 Normal = new Vector2(Fall_X, Fall_Y).normalized;


        while (true)
        {
            if (transform.position.y <= -7)
            {
                yield break;
            }
            transform.position = new Vector3(transform.position.x + (Normal.x * Time.deltaTime * (6 + Params)), transform.position.y + (Normal.y * Time.deltaTime * (6 + Params)), 0);

            Params += 0.03f;
            yield return null;
        }
    }
    public void TakeDamage()
    {
        //if (Unbeatable_Player)
        //    return;
        LifeTime--;

        LifeTime_Text.text = "Life x  : " + LifeTime;
        if (LifeTime <= 0)
        {
            OnDie();
        }

        if (Emit_Obj_Copy != null)
        {
            StopCoroutine(params_enum);
            Destroy(Emit_Obj_Copy);
        }

        StartCoroutine("Damage_After");
    }
    IEnumerator UnBeatable_Apply()
    {

        int countTime = 0;

        while (countTime < 20)
        {
            if (countTime % 2 == 0)
                spriteRenderer.color = new Color32(255, 255, 255, 90);
            else
                spriteRenderer.color = new Color32(255, 255, 255, 180);
            countTime++;

            yield return new WaitForSeconds(0.2f);
        }
        spriteRenderer.color = new Color32(255, 255, 255, 255);

        Unbeatable_Player = false;
        yield return null;
    }

    public void Start_Emit()
    {
        StartCoroutine(I_Start_Emit());
    }
    IEnumerator I_Start_Emit()
    {
        Unbeatable_Player = true;
        yield return null;
        Emit_Obj_Copy = Instantiate(Emit_Obj, transform.position, Quaternion.identity);
        params_enum = Emit_Obj_Copy.GetComponent<Emit_Motion>().Change_Size();
        
        StartCoroutine(params_enum);
        yield return YieldInstructionCache.WaitForSeconds(5f);

        StopCoroutine(params_enum);

        yield return StartCoroutine(Emit_Obj_Copy.GetComponent<Emit_Motion>().Expand_Circle());
        Destroy(Emit_Obj_Copy);

        GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] meteor = GameObject.FindGameObjectsWithTag("Meteor");
        GameObject[] meteor_line = GameObject.FindGameObjectsWithTag("Meteor_Line");
        GameObject[] meteor_traffic = GameObject.FindGameObjectsWithTag("Meteor_Traffic");
        foreach (var e in enemy)
        {
            Destroy(e);
        }
        foreach (var e in meteor)
        {
            Destroy(e);
        }
        foreach (var e in meteor_line)
        {
            Destroy(e);
        }
        foreach (var e in meteor_traffic)
        {
            Destroy(e);
        }

        StartCoroutine(GameObject.FindGameObjectWithTag("Flash").GetComponent<FlashOn>().White_Flash());
        GameObject.FindGameObjectWithTag("Boss").GetComponent<DoPhan>().Stop_Meteor();
        GameObject u = Instantiate(Dead_Particle, Vector3.zero, Quaternion.identity);
        yield return YieldInstructionCache.WaitForSeconds(2f);
        Destroy(u);
    }


    public override void OnDie()
    {
        Destroy(gameObject);
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
        PlayerScore.text = "점수 : " + Score;
        if (Input.GetKeyDown(keyCodeAttack) && weapon.enabled)
        {
            weapon.StartFiring();
        }
        else if (Input.GetKeyUp(keyCodeAttack) || !weapon.enabled)
        {
            weapon.StopFiring();
        }
        if (Input.GetKeyDown(keyCodeBoom))
        {
            weapon.StartBoom();
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
