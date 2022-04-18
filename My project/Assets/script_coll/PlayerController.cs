using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private StageData stageData;

    // Start is called before the first frame update
    Movement2D movement2D;
    Weapon weapon;
    Animator animator;

    [SerializeField]
    KeyCode keyCodeAttack = KeyCode.Space;

    [SerializeField]
    KeyCode keyCodeBoom = KeyCode.Z;

    bool isDie = false;

    int score;
    public int Score // private 변수인 score를 어떻게 보면 클래스 식으로 (set, get) 처리해줄 수 있다
    {
        set => score = Mathf.Max(0, value);
        get => score;
    }


    void Start()
    {
        
    }
    private void Awake()
    {
        movement2D = GetComponent<Movement2D>();
        weapon = GetComponent<Weapon>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDie == true)
        {
            return; // 플레이어의 이동 전부 종료
        }
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        movement2D.MoveTo(new Vector3(x, y, 0));
        if (Input.GetKeyDown(keyCodeAttack))
        {
            weapon.StartFiring();
        }
        else if (Input.GetKeyUp(keyCodeAttack))
        {
            weapon.StopFiring();
        }
        if (Input.GetKeyDown(keyCodeBoom))
        {
            weapon.StartBoom();
        }
    }
    public void OnDie()
    {
        movement2D.MoveTo(Vector3.zero);
        animator.SetTrigger("OnDie");
        Destroy(GetComponent<CircleCollider2D>());
        isDie = true;
    }
    public void OnDieEvent()
    {
        PlayerPrefs.SetInt("Score", score);
        SceneManager.LoadScene("GameOver");
    }
    public void LateUpdate()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, stageData.LimitMin.x, stageData.LimitMax.x),
            Mathf.Clamp(transform.position.y, stageData.LimitMin.y, stageData.LimitMax.y));
    }
}
