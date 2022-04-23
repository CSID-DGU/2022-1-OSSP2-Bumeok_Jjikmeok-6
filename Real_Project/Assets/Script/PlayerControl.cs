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
    KeyCode keyCodeAttack = KeyCode.Space;

    [SerializeField]
    KeyCode keyCodeBoom = KeyCode.Z;

    [SerializeField]
    Slider slider;

    SpriteRenderer spriteRenderer;

    [SerializeField]
    TextMeshProUGUI PlayerScore;

    [SerializeField]
    StageData stageData;

    new private void Awake()
    {
        base.Awake();
        movement2D = GetComponent<Movement2D>();
        weapon = GetComponent<Weapon>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        slider.value = CurrentHP / MaxHP;
        StartCoroutine("Hit");
        if (CurrentHP <= 0)
        {
            Destroy(gameObject);
        }
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
                    slider.value = i / MaxHP;
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
    }
    void LateUpdate()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, stageData.LimitMin.x, stageData.LimitMax.x),
           Mathf.Clamp(transform.position.y, stageData.LimitMin.y, stageData.LimitMax.y));
    }
}
