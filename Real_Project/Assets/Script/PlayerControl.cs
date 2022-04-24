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

    //Animator animator;

    new private void Awake()
    {
        base.Awake();
        Instantiate(eee, new Vector3(0, 0, 0), Quaternion.identity);
        movement2D = GetComponent<Movement2D>();
        weapon = GetComponent<Weapon>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        //animator = GetComponent<Animator>();
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
        CurrentHP -= attack_rate;
        Debug.Log(CurrentHP);
        HPslider.value = CurrentHP / MaxHP;
        if (CurrentHP <= 0)
        {
            Destroy(gameObject);
        }
        StartCoroutine("OnTrap");
    }
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
        while (true)
        {
            transform.position += Vector3.right * (Time.deltaTime * 1.5f);
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
            //animator.SetTrigger("Change");
        }
        else
        {
            //animator.SetTrigger("Origin");
        }
    }
    void LateUpdate()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, stageData.LimitMin.x, stageData.LimitMax.x),
           Mathf.Clamp(transform.position.y, stageData.LimitMin.y, stageData.LimitMax.y));
    }
}
