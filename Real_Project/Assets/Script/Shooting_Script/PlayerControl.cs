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

    float[,] Jebal = new float[41, 2]
    {
        { -0.06f, 0.12f },
        { -0.11f, 0.11f },
        { -0.06f, 0.12f },
        { -0.17f, 0.11f },
        { -0.060000f, 0.060000f },
        { -0.180000f, 0.170000f },
        { -0.050000f, 0.000000f },
        { -0.120000f, 0.060000f },
        { -0.060000f, 0.120000f },
        { -0.060000f, 0.000000f },
        { -0.170000f, 0.060000f },
        { -0.230000f, 0.050000f },
        { -0.060000f, 0.060000f },
        { -0.060000f, 0.060000f },
        { -0.110000f, 0.060000f },
        { -0.060000f, 0.000000f },
        { -0.170000f, 0.050000f },
        { -0.230000f, 0.000000f },
        { -0.240000f, 0.060000f },
        { -0.050000f, 0.000000f },
        { -0.120000f, 0.000000f },
        { -0.060000f, 0.000000f },
        { -0.170000f, 0.000000f },
        { -0.060000f, 0.000000f },
        { -0.110000f, 0.000000f },
        { -0.120000f, 0.000000f },
        { -0.120000f, 0.000000f },
        { -0.050000f, 0.000000f },
        { -0.060000f, -0.110000f },
        { -0.060000f, 0.000000f },
        { -0.120000f, -0.120000f },
        { -0.050000f, -0.110000f },
        { -0.060000f, 0.000000f },
        { -0.120000f, -0.120000f },
        { -0.050000f, -0.120000f },
        { -0.060000f, -0.050000f },
        { -0.120000f, -0.120000f },
        { -0.110000f, -0.170000f },
        { -0.180000f, -0.120000f },
        { -0.060000f, -0.110000f },
        { -0.110000f, -0.120000f },
    };
    [SerializeField]
    int LifeTime = 5;

    private void Awake()
    {
        movement2D = GetComponent<Movement2D>();
        weapon = GetComponent<Weapon>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        movement2D.enabled = false;
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
            transform.position += Vector3.right * (Time.deltaTime * 2f);
            yield return null;
            if (transform.position.x >= -4.6)
            {
                movement2D.enabled = true;
                weapon.enabled = true;
                is_LateUpdate = true;
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
        movement2D.enabled = false;
        weapon.enabled = false;
        is_LateUpdate = false;
        yield return null;
        yield return StartCoroutine("MovePath");
        yield return StartCoroutine("Move_first");
        yield break;
    }
    IEnumerator MovePath()
    {
        yield return null;
        for (int i = 0; i < 41; i++)
        {
            Vector3 temp = transform.position;
            while (true)
            {
                transform.position += new Vector3(Jebal[i, 0], Jebal[i, 1], 0) * (Time.deltaTime * ((float)i * 4 + 20f));
                yield return null;

                if (temp.x - transform.position.x >= Jebal[i, 0] && temp.y - transform.position.y <= Jebal[i, 1])
                {
                    break;
                }
                if (transform.position.y <= -7)
                {
                    yield break;
                }

            }
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
        PlayerScore.text = "점수 : " + Score;
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        movement2D.MoveTo(new Vector3(x, y, 0));
        if (Input.GetKeyDown(keyCodeAttack) && weapon.enabled)
        {
            weapon.StartFiring();
        }
        else if (Input.GetKeyUp(keyCodeAttack) && weapon.enabled)
        {
            weapon.StopFiring();
        }
        if (Input.GetKeyDown(keyCodeBoom))
        {
            weapon.StartBoom();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            animator.SetBool("HasExit", true);
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow) && Input.GetKeyUp(KeyCode.UpArrow))
        {
            animator.SetBool("HasExit", false);
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
