using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerControl : MonoBehaviour
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
    AnimationCurve curve;

    public bool Unbeatable_Player = false;

    [SerializeField]
    KeyCode keyCodeAttack = KeyCode.Space;

    [SerializeField]
    KeyCode keyCodeBoom = KeyCode.Z;

    SpriteRenderer spriteRenderer;

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

    private void Awake()
    {
        movement2D = GetComponent<Movement2D>();
        weapon = GetComponent<Weapon>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        is_Update = false;
        is_LateUpdate = false;
        weapon.enabled = false;
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
                yield return new WaitForSeconds(3f);
                yield break;
            }
        }
    }
    IEnumerator FadeText()
    {
        while (PlayerScore.color.a < 1.0f)
        {
            LifeTime_Text.color = new Color(LifeTime_Text.color.r, LifeTime_Text.color.g, LifeTime_Text.color.b, LifeTime_Text.color.a + (Time.deltaTime / 2.0f));
            PlayerScore.color = new Color(PlayerScore.color.r, PlayerScore.color.g, PlayerScore.color.b, PlayerScore.color.a + (Time.deltaTime / 2.0f));
            yield return null;
        }
    }
    IEnumerator Damage_After()
    {
        weapon.enabled = false;
        is_LateUpdate = false;
        is_Update = false;
        movement2D.enabled = true;
        yield return null;

        yield return StartCoroutine("MovePath");
        movement2D.enabled = false;
        yield return null;

        yield return StartCoroutine("Move_first");
        yield break;
    }
    IEnumerator MovePath()
    {

        float Params = 0.7f;
        while (true)
        {
            if (transform.position.y <= -7)
            {
                yield break;
            }
            movement2D.MoveSpeed = 3f / Mathf.Pow(Params, 2);
            Params -= 0.004f;
            movement2D.MoveTo(new Vector3(-0.7f, -1, 0));
            
            yield return null;
        }
    }
    public void TakeDamage()
    {
        LifeTime--;

        LifeTime_Text.text = "Life x  : " + LifeTime;
        if (LifeTime <= 0)
        {
            OnDie();
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


    public void OnDie()
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
