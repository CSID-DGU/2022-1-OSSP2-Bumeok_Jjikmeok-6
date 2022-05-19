using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerCtrl_Sarang : Player_Info
{
    public float IsDoubleClick = 0.25f;

    private bool IsOneClick = false;

    private double Timer = 0;

    private bool Dash_Able = true;

    private bool Move_Able = true;

    private bool FixedSlider = false;

    private bool FixedTarget = false;

    private bool is_Domain = false;

    private IEnumerator second_phase;

    private IEnumerator move_delay;

    private IEnumerator first_phase;

    private IEnumerator score_up;

    private int Floor_Player_Place;

    int StudentLayerMask;  // Player 레이어만 충돌 체크함

    [SerializeField]
    float PlayerWalkSpeed = 0.03f;

    [SerializeField]
    float Decrease_HP_ratio = 0.0006f;

    [SerializeField]
    GameObject Move_Slider;

    [SerializeField]
    Transform canvasTransform;

    [SerializeField]
    GameObject Student_Gaze;

    [SerializeField]
    GameObject Targetting_Object;

    [SerializeField]
    GameObject Heart_Slider;

    [SerializeField]
    TextMeshProUGUI Main3_Score_Text;

    [SerializeField]
    GameObject Fever_Particle;

    [SerializeField]
    GameObject Down_Floor;

    [SerializeField]
    GameObject Up_Floor;

    GameObject Fever_Particle_Clone;

    GameObject sliderClone;

    public Animator animator;

    GameObject Student_Clone;

    public void Destroy_sliderClone()
    {
        if (sliderClone != null)
            Destroy(sliderClone);
    }

    private new void Awake()
    {
        base.Awake();
        score_up = null;
        first_phase = null;
        second_phase = null;
        move_delay = null;
        is_Domain = true;
        transform.position = new Vector3(0, -2.5f, 0);
        Floor_Player_Place = 1;
        Student_Gaze.SetActive(false);
        Targetting_Object.SetActive(false);
        Down_Floor.SetActive(false);
        Up_Floor.SetActive(false);
        stageData.LimitMin = new Vector2(-3, -6);
        stageData.LimitMax = new Vector2(40, 8);
        StudentLayerMask = 1 << LayerMask.NameToLayer("Student");
        animator = GetComponent<Animator>();
        backGroundColor = GameObject.Find("Flash_Interrupt").GetComponent<BackGroundColor>();
    }

    // Start is called before the first frame update
    void Start()
    {
        first_phase = First_Phase();
        StartCoroutine(first_phase);
    }
    IEnumerator First_Phase()
    {
        if (second_phase != null)
            StopCoroutine(second_phase);

        while (true)
        {
            if (Dash_Able)
                Dash();
            if (Move_Able)
                Move(); 
            
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f, StudentLayerMask);

            if (hit.collider != null && hit.transform.gameObject.CompareTag("Student"))
            {
                if (!FixedTarget)
                {
                    Targetting_Object.SetActive(true);
                    Targetting_Object.GetComponent<Targetting_Effect>().Scale_Color(hit.transform.gameObject);
                    FixedTarget = true;
                }
                if (Input.GetMouseButtonDown(0))
                {
                    second_phase = Second_Phase();
                    yield return StartCoroutine(second_phase);
                }
            }
            else
            {
                Targetting_Object.GetComponent<Targetting_Effect>().Init();
                Targetting_Object.SetActive(false);
                FixedTarget = false;
            }
                
            yield return null;
        }
    }

    IEnumerator Second_Phase()
    {
        if (first_phase != null)
            StopCoroutine(first_phase);

        if (move_delay != null)
            StopCoroutine(move_delay); // 보류 코드

        while(true)
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f, StudentLayerMask);

            if (hit.collider != null && hit.transform.gameObject.CompareTag("Student"))
            {
                if (Student_Clone == null)
                    Student_Clone = hit.transform.gameObject;
                if (Input.GetMouseButton(0))
                {
                    float dir_Change = transform.position.x - Student_Clone.transform.position.x;
                    transform.localScale = new Vector3((dir_Change / Mathf.Abs(dir_Change)) * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                    Targetting_Object.GetComponent<Targetting_Effect>().Init();
                    Targetting_Object.SetActive(false);
                    FixedTarget = false;
                 
                    if (!FixedSlider)
                    {
                        Student_Gaze.transform.position = Student_Clone.transform.position + new Vector3(0, 2, 0);
                        FixedSlider = true;

                        Student_Clone.GetComponent<Student>().Be_Attacked();
                        Student_Clone.GetComponent<Student>().Stop_Move(); // 공격 받는 중 + 움직임 정지
                    }
                    Heart_Slider.GetComponent<Heart_Gaze_Viewer>().Decrease_HP(Decrease_HP_ratio);
                    Lazor_In_Second_Phase(Weapon[0], Student_Clone.transform.position, transform.position);

                    if (!Student_Gaze.activeSelf)
                        Student_Gaze.SetActive(true);

                    int temp = Student_Gaze.GetComponent<Student_Gaze_Info>().Check_Student_HP(Student_Clone.transform.position);
                    if (temp == 0)
                        yield return StartCoroutine(Third_Phase(Student_Clone));
                    else if (temp == 1)
                    {
                        Debug.Log("흠");
                        Heart_Slider.GetComponent<Heart_Gaze_Viewer>().Ordinary_Case();
                        Student_Gaze.GetComponent<Student_Gaze_Info>().Empty_HP();
                        Student_Gaze.SetActive(false);
                        FixedSlider = false;

                        if (Student_Clone != null)
                            Destroy(Student_Clone);


                    }
                    else if (temp == 3)
                    {
                        if (score_up == null)
                        {
                            score_up = Score_UP();
                            StartCoroutine(score_up);
                        }
                    }
                       
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    Student_Gaze.GetComponent<Student_Gaze_Info>().Empty_HP();
                    Student_Gaze.SetActive(false);
                    FixedSlider = false;

                    if (Student_Clone != null)
                    {
                        Student_Clone.GetComponent<Student>().NotBe_Attacked();
                        Student_Clone.GetComponent<Student>().Start_Move();

                        Student_Clone = null;
                    }
                   
                    if (score_up != null)
                    {
                        StopCoroutine(score_up);
                        score_up = null;
                    }

                     first_phase = First_Phase();
                     StartCoroutine(first_phase);
                 
                    yield return YieldInstructionCache.WaitForSeconds(0.2f);
                    yield break;
                }
            }
            else
            {
                Student_Gaze.GetComponent<Student_Gaze_Info>().Empty_HP();
                Student_Gaze.SetActive(false);
                FixedSlider = false;

                if (Student_Clone != null)
                {
                    Student_Clone.GetComponent<Student>().NotBe_Attacked();
                    Student_Clone.GetComponent<Student>().Start_Move();

                    Student_Clone = null;
                }

                if (score_up != null)
                {
                    StopCoroutine(score_up);
                    score_up = null;
                }

                first_phase = First_Phase();
                StartCoroutine(first_phase);
                yield return YieldInstructionCache.WaitForSeconds(0.2f);
                yield break;
            }
            yield return null;
            
        }
    }

    private void Lazor_In_Second_Phase(GameObject weapon, Vector3 target, Vector3 self)
    {
        Launch_Weapon_For_Move_Blink(weapon, target - self, Quaternion.identity, 8, false, transform.position);
    }
    public void Stop_Coroutine()
    {
        StopAllCoroutines();
    }
    
    IEnumerator Third_Phase(GameObject targetStudent_t)
    {
        if (second_phase != null)
            StopCoroutine(second_phase);

        if (move_delay != null)
            StopCoroutine(move_delay);

        if (first_phase != null)
            StopCoroutine(first_phase);

        if (score_up != null)
        {
            StopCoroutine(score_up);
            score_up = null;
        }

        IEnumerator lazor_in_second_phase = Lazor_In_Second_Phase(targetStudent_t);

        StartCoroutine(lazor_in_second_phase);
       
        yield return StartCoroutine(Student_Gaze.GetComponent<Student_Gaze_Info>().Competition(targetStudent_t));
        StopCoroutine(lazor_in_second_phase);

        Student_Gaze.GetComponent<Student_Gaze_Info>().Empty_HP();
        Student_Gaze.SetActive(false);
        FixedSlider = false;

        if (targetStudent_t != null)
        {
            targetStudent_t.GetComponent<Student>().NotBe_Attacked();
            targetStudent_t.GetComponent<Student>().Start_Move();

            Destroy(targetStudent_t);
        }
        yield return YieldInstructionCache.WaitForSeconds(2f); // 플레이어가 하트 게이지를 얻거나, 쓰러지는 시간임
        animator.SetBool("IsDead", false);
        first_phase = First_Phase();
        StartCoroutine(first_phase);
        yield break;
    }
    IEnumerator Lazor_In_Second_Phase(GameObject targetStudent_t)
    {
        while(true)
        {
            if (targetStudent_t != null)
                Lazor_In_Second_Phase(Weapon[0], targetStudent_t.transform.position, transform.position);
            yield return null;
        }
    }

    IEnumerator Move_Delay()
    {
        sliderClone = Instantiate(Move_Slider, transform.position, Quaternion.identity);

        sliderClone.transform.SetParent(canvasTransform); 
        sliderClone.transform.position = transform.position + new Vector3(0, 2, 0);
        sliderClone.transform.localScale = Vector3.one; 
        sliderClone.GetComponent<Move_Gaze_Info>().HP_Down();
        Debug.Log("뜀");
        animator.SetBool("IsRun", true);
        PlayerWalkSpeed *= 2;

        IEnumerator check_button_off = Check_Button_Off(sliderClone);
        StartCoroutine(check_button_off);

        yield return new WaitForSeconds(3f);

        Destroy(sliderClone);
        StopCoroutine(check_button_off);

        Dash_Able = false;
        Move_Able = false;
        PlayerWalkSpeed = 0;
        if (first_phase != null)
            StopCoroutine(first_phase);
        animator.SetTrigger("Tired");
        yield return new WaitForSeconds(2.5f);

        animator.SetBool("IsWalk", false);
        animator.SetBool("IsRun", false);
        yield return YieldInstructionCache.WaitForSeconds(0.1f);

        Dash_Able = true;
        Move_Able = true;
        PlayerWalkSpeed = 0.03f;
        first_phase = First_Phase();
        StartCoroutine(first_phase);
        PlayerWalkSpeed /= 2;
        yield return null;
    }
    IEnumerator Check_Button_Off(GameObject e)
    {
        while (true)
        {
            if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
            {
                animator.SetBool("IsRun", false);
                PlayerWalkSpeed = 0.03f;
                Destroy(e);
                Dash_Able = true;
                if (move_delay != null)
                    StopCoroutine(move_delay);
                yield break;
            }
            yield return null;
        }
    }
    IEnumerator Score_UP()
    {
       while(true)
       {
            Main_3_Score += 3;
            yield return YieldInstructionCache.WaitForSeconds(0.125f);
       }
    }

    private void Dash()
    {
        if (IsOneClick && ((Time.time - Timer) > IsDoubleClick))
        {
            IsOneClick = false;
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A))
        {
            if (!IsOneClick)
            {
                Timer = Time.time;
                IsOneClick = true;
            }

            else if (IsOneClick && ((Time.time - Timer) < IsDoubleClick))
            {
                Dash_Able = false;
                IsOneClick = false;

                move_delay = Move_Delay();
                StartCoroutine(move_delay);
            }
        }
    }
    private void Move()
    {
        int key = 0;
        if (Input.GetKey(KeyCode.D))
        {
            animator.SetBool("IsWalk", true);
            key = 1;
            transform.position = new Vector3(transform.position.x + PlayerWalkSpeed * key, transform.position.y, transform.position.z);

        }
        if (Input.GetKey(KeyCode.A))
        {
            animator.SetBool("IsWalk", true);
            key = -1;
            transform.position = new Vector3(transform.position.x + PlayerWalkSpeed * key, transform.position.y, transform.position.z);
        }

        if (key != 0)
        {
            transform.localScale = new Vector3(-key * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
            animator.SetBool("IsWalk", false);
    }

    public void Down_Or_Up(int param)
    {
        if (param == 0) // 아래 층 이동
        {
            Floor_Player_Place--;
            StartCoroutine(I_Move_Floor(38, "down"));
        }
        else if (param == 1) // 위 층 이동
        {
            Floor_Player_Place++;
            StartCoroutine(I_Move_Floor(0, "up"));
        }
    }

    IEnumerator I_Move_Floor(int Change_X, string CHK)
    {
        if (second_phase != null)
            StopCoroutine(second_phase);
        if (move_delay != null)
            StopCoroutine(move_delay);
        if (first_phase != null)
            StopCoroutine(first_phase);
        if (score_up != null)
            StopCoroutine(score_up);

        Dash_Able = false;
        Move_Able = false;
        is_Domain = false;
        PlayerWalkSpeed = 0;

        if (Down_Floor.activeSelf)
        {
            Down_Floor.GetComponent<Floor_Button>().Stop_Down_Or_UP();
            Down_Floor.SetActive(false);
        }
        if (Up_Floor.activeSelf)
        {
            Up_Floor.GetComponent<Floor_Button>().Stop_Down_Or_UP();
            Up_Floor.SetActive(false);
        }

        animator.SetBool("IsWalk", false);
        animator.SetBool("IsRun", false);
        animator.SetBool("IsDead", false);
        yield return null;

        animator.SetBool("IsWalk", true);

        GameObject.Find("Main Camera").GetComponent<Camera_Trace>().When_Walk_Floor();
        GameObject.FindGameObjectWithTag("LimitTimeText").GetComponent<Limit_Time>().When_Walk_Floor();

        IEnumerator qq = backGroundColor.Change_Color(backGroundColor.Get_BGColor(), new Color(0, 0, 0, 1), 2);

        StartCoroutine(qq);
        if (CHK == "up")
        {
            stageData.LimitMax = stageData.LimitMax + new Vector2(0, 40);
            stageData.LimitMin = stageData.LimitMin + new Vector2(0, 40);
            yield return StartCoroutine(Position_Lerp(transform.position, transform.position + new Vector3(6, 2, 0), 2, OriginCurve));
        }
        else if (CHK == "down")
        {
            stageData.LimitMax = stageData.LimitMax + new Vector2(0, -40);
            stageData.LimitMin = stageData.LimitMin + new Vector2(0, -40);
            yield return StartCoroutine(Position_Lerp(transform.position, transform.position + new Vector3(-6, -2, 0), 2, OriginCurve));
        }
           
        StopCoroutine(qq);

        yield return YieldInstructionCache.WaitForSeconds(1f);

        transform.position = new Vector3(Change_X, -2.5f + 40 * (Floor_Player_Place - 1), 0);
        GameObject.Find("Main Camera").GetComponent<Camera_Trace>().Final_Walk_Floor(Floor_Player_Place);
        GameObject.FindGameObjectWithTag("LimitTimeText").GetComponent<Limit_Time>().Final_Walk_Floor();
        yield return StartCoroutine(backGroundColor.Change_Color(backGroundColor.Get_BGColor(), new Color(1, 1, 1, 0.5f), 0.5f));
        yield return StartCoroutine(backGroundColor.Change_Color(backGroundColor.Get_BGColor(), new Color(1, 1, 1, 0), 0.2f));

        animator.SetBool("IsWalk", false);

        first_phase = First_Phase();
        StartCoroutine(first_phase);

        is_Domain = true;
        Dash_Able = true;
        Move_Able = true;
        PlayerWalkSpeed = 0.03f;

        yield return null;
    }
    public void Up()
    {
        is_Domain = false;
        StartCoroutine(Position_Lerp(transform.position, transform.position + new Vector3(6, 2, 0), 3, OriginCurve));
    }
    public void Fever_Time()
    {
        StartCoroutine(backGroundColor.Flash(new Color(1, 1, 1, 1), 0.2f, 5));
        Fever_Particle_Clone = Instantiate(Fever_Particle, transform.position - Vector3.down * 1.5f, Quaternion.Euler(-90, 0, 0));
    }

    void Update()
    {
        Main3_Score_Text.text = "점수 : " + Main_3_Score;
    }
    private void LateUpdate()
    {
        if (is_Domain)
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, stageData.LimitMin.x, stageData.LimitMax.x),
            Mathf.Clamp(transform.position.y, stageData.LimitMin.y, stageData.LimitMax.y));
            if (transform.position.x <= -3)
            {
                if (Floor_Player_Place != 1)
                {
                    Down_Floor.SetActive(true);
                    Down_Floor.GetComponent<Floor_Button>().Start_Down_Or_UP(true, 40 * (Floor_Player_Place - 1));
                }
            }
            else
            {
                if (Down_Floor.activeSelf)
                {
                    Down_Floor.GetComponent<Floor_Button>().Stop_Down_Or_UP();
                    Down_Floor.SetActive(false);
                }
            }
            if (transform.position.x >= 40)
            {
                if (Floor_Player_Place != 4)
                {
                    Up_Floor.SetActive(true);
                    Up_Floor.GetComponent<Floor_Button>().Start_Down_Or_UP(false, 40 * (Floor_Player_Place - 1));
                }
            }
            else
            {
                if (Up_Floor.activeSelf)
                {
                    Up_Floor.GetComponent<Floor_Button>().Stop_Down_Or_UP();
                    Up_Floor.SetActive(false);
                }
            }
        }
        if (Fever_Particle_Clone != null)
            Fever_Particle_Clone.transform.position = new Vector3(transform.position.x, Fever_Particle_Clone.transform.position.y, Fever_Particle_Clone.transform.position.z);

      
    }
}