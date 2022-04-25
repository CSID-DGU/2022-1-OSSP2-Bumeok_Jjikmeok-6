using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerControl : HP_Info
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

    [SerializeField]
    Slider HPslider;

    SpriteRenderer spriteRenderer;

    [SerializeField]
    TextMeshProUGUI PlayerScore;

    [SerializeField]
    StageData stageData;

    [SerializeField]
    GameObject eee;

    Animator animator;

    //[SerializeField]
    //int LifeTime = 5;
    new private void Awake()
    {
        base.Awake();
        Instantiate(eee, new Vector3(0, 0, 0), Quaternion.identity);
        movement2D = GetComponent<Movement2D>();
        weapon = GetComponent<Weapon>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        movement2D.enabled = false;
        weapon.enabled = false;
        score = 0;
        PlayerScore.text = "점수 : " + score;
        PlayerScore.color = new Color(PlayerScore.color.r, PlayerScore.color.g, PlayerScore.color.b, 0);
    }
    void Start()
    {
        Move_First();
    }
    public void Move_First()
    {
        StartCoroutine("Move_first");
    }
    public void TakeDamage(float attack_rate)
    {
        //LifeTime--;
        //if (LifeTime <= 0)
        //{
        //    Debug.Log("앙?");
        //    Destroy(gameObject);
        //}

        //StartCoroutine("Wow");
        CurrentHP -= attack_rate;
        Debug.Log(CurrentHP);
        HPslider.value = CurrentHP / MaxHP;
        if (CurrentHP <= 0)
        {
            Destroy(gameObject);
        }
        StartCoroutine("OnTrap");
    }
    //IEnumerator Wow()
    //{

    //    Debug.Log("응?");
    //    float boomDelay = 0.5f;
    //    float current = 0;
    //    float percent = 0;
    //    while (percent < 1)
    //    {
    //        Debug.Log(percent);
    //        current += Time.deltaTime;
    //        percent = current / boomDelay;
    //        transform.position = Vector3.Slerp(transform.position, new Vector3(transform.position.x - 0.5f, transform.position.y - 0.5f, 0), curve.Evaluate(percent));
    //        yield return null;
    //    }
    //}
    IEnumerator OnTrap()
    {
        yield return StartCoroutine("Hit");
        yield return StartCoroutine("UnBeatable_Apply");
    }
    IEnumerator UnBeatable_Apply()
    {
        Unbeatable_Player = true;
        yield return null;

        int countTime = 0;

        while(countTime < 10)
        {
            if (countTime % 2 == 0)
                spriteRenderer.color = new Color32(255, 255, 255, 90);
            else
                spriteRenderer.color = new Color32(255, 255, 255, 150);
            countTime++;

            yield return new WaitForSeconds(0.2f);
        }
        spriteRenderer.color = new Color32(255, 255, 255, 255);

        Unbeatable_Player = false;
        yield return null;
    }
    IEnumerator Hit()
    {
        spriteRenderer.color = Color.blue;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }
    IEnumerator Move_first()
    {
        transform.position = new Vector3(-9, 0, 0);
        yield return null;
        while (true)
        {
            transform.position += Vector3.right * (Time.deltaTime * 2f);
            yield return null;
            if (transform.position.x >= -4.6)
            {
                movement2D.enabled = true;
                weapon.enabled = true;
                StartCoroutine("FadeText");
                for (int i = 0; i <= CurrentHP; i++)
                {
                    HPslider.value = i / MaxHP;
                    yield return new WaitForSeconds(0.02f);
                }
                yield return new WaitForSeconds(3f);
                yield break;
            }
        }
    }
    IEnumerator FadeText()
    {
        while (PlayerScore.color.a < 1.0f)
        {
            PlayerScore.color = new Color(PlayerScore.color.r, PlayerScore.color.g, PlayerScore.color.b, PlayerScore.color.a + (Time.deltaTime / 2.0f));
            yield return null;
        }
    }

    public bool OnDie()
    {
        return false;
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
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, stageData.LimitMin.x, stageData.LimitMax.x),
           Mathf.Clamp(transform.position.y, stageData.LimitMin.y, stageData.LimitMax.y));
    }
}
