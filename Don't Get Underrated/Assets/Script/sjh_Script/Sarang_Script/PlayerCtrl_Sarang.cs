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

    private IEnumerator sibal;

    private IEnumerator move_delay;

    private IEnumerator first_phase;

    private IEnumerator score_up;

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

    GameObject Fever_Particle_Clone;

    GameObject sliderClone;

    Animator animator;

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
        sibal = null;
        move_delay = null;
        Student_Gaze.SetActive(false);
        Targetting_Object.SetActive(false);
        StudentLayerMask = 1 << LayerMask.NameToLayer("Student");
        animator = GetComponent<Animator>();
        flashOn = GameObject.Find("Flash_Interrupt").GetComponent<FlashOn>();
    }

    // Start is called before the first frame update
    void Start()
    {
        first_phase = First_Phase();
        StartCoroutine(first_phase);
    }
    IEnumerator First_Phase()
    {
        if (sibal != null)
            StopCoroutine(sibal);

        while (true)
        {
            Debug.Log("얘!");
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
                    sibal = Sibal();
                    yield return StartCoroutine(sibal);
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

    IEnumerator Sibal()
    {
        if (first_phase != null)
            StopCoroutine(first_phase);

        if (move_delay != null)
            StopCoroutine(move_delay); // 보류 코드

        while(true)
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f, StudentLayerMask);

            Debug.Log("왜 안 되냐?");
            if (hit.collider != null && hit.transform.gameObject.CompareTag("Student"))
            {
                Debug.Log("지금 학생이어야 하는데");
                if (Student_Clone == null)
                    Student_Clone = hit.transform.gameObject;
                if (Input.GetMouseButton(0))
                {
                    float dir_Change = transform.position.x - Student_Clone.transform.position.x;
                    transform.localScale = new Vector3((dir_Change / Mathf.Abs(dir_Change)) * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                    Targetting_Object.GetComponent<Targetting_Effect>().Init();
                    Targetting_Object.SetActive(false);
                    FixedTarget = false;
                    if (score_up == null)
                    {
                        score_up = Score_UP();
                        StartCoroutine(score_up);
                    }
                    if (!FixedSlider)
                    {
                        Debug.Log("설마 여기가 안 되나");
                        Student_Gaze.transform.position = Student_Clone.transform.position + new Vector3(0, 2, 0);
                        FixedSlider = true;

                        Student_Clone.GetComponent<Student_Move>().Be_Attacked();
                        Student_Clone.GetComponent<Student_Move>().Stop_Move(); // 공격 받는 중 + 움직임 정지
                    }
                    Heart_Slider.GetComponent<Heart_Gaze_Viewer>().Decrease_HP(Decrease_HP_ratio);
                    Lazor_In_First_Phase(Weapon[0], Student_Clone.transform.position, transform.position);

                    if (!Student_Gaze.activeSelf)
                        Student_Gaze.SetActive(true);

                    if (Student_Gaze.GetComponent<Student_Gaze_Info>().Block_HP(Student_Clone.transform.position))
                    {
                        yield return StartCoroutine(Second_Phase(Student_Clone));
                    }
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    Student_Gaze.GetComponent<Student_Gaze_Info>().Empty_HP();
                    Student_Gaze.SetActive(false);
                    FixedSlider = false;

                    if (Student_Clone != null)
                    {
                        Student_Clone.GetComponent<Student_Move>().NotBe_Attacked();
                        Student_Clone.GetComponent<Student_Move>().Start_Move();

                        Student_Clone = null;
                    }
                   
                    if (score_up != null)
                        StopCoroutine(score_up);

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
                    Student_Clone.GetComponent<Student_Move>().NotBe_Attacked();
                    Student_Clone.GetComponent<Student_Move>().Start_Move();

                    Student_Clone = null;
                }

                if (score_up != null)
                    StopCoroutine(score_up);

                first_phase = First_Phase();
                StartCoroutine(first_phase);
                yield return YieldInstructionCache.WaitForSeconds(0.2f);
                yield break;
            }
            yield return null;
            
        }
    }

    private void Lazor_In_First_Phase(GameObject weapon, Vector3 target, Vector3 self)
    {
        Launch_Weapon_For_Move(weapon, target, self);
    }
    public void Stop_Coroutine()
    {
        StopAllCoroutines();
    }
    
    IEnumerator Second_Phase(GameObject targetStudent_t)
    {
        if (sibal != null)
            StopCoroutine(sibal);

        if (move_delay != null)
            StopCoroutine(move_delay);

        if (first_phase != null)
            StopCoroutine(first_phase);

        if (score_up != null)
            StopCoroutine(score_up);

        IEnumerator lazor_after_second = Lazor_In_Second_Phase(targetStudent_t);

        StartCoroutine(lazor_after_second);
       
        yield return StartCoroutine(Student_Gaze.GetComponent<Student_Gaze_Info>().Competition(targetStudent_t));
        StopCoroutine(lazor_after_second);

        Student_Gaze.GetComponent<Student_Gaze_Info>().Empty_HP();
        Student_Gaze.SetActive(false);
        FixedSlider = false;

        if (targetStudent_t != null)
        {
            targetStudent_t.GetComponent<Student_Move>().NotBe_Attacked();
            targetStudent_t.GetComponent<Student_Move>().Start_Move();

            Destroy(targetStudent_t);
        }
        yield return YieldInstructionCache.WaitForSeconds(1f); // 플레이어가 하트 게이지를 얻거나, 쓰러지는 시간임
        first_phase = First_Phase();
        StartCoroutine(first_phase);
        yield break;
    }
    IEnumerator Lazor_In_Second_Phase(GameObject targetStudent_t)
    {
        while(true)
        {
            if (targetStudent_t != null)
                Lazor_In_First_Phase(Weapon[0], targetStudent_t.transform.position, transform.position);
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
        PlayerWalkSpeed *= 2;

        IEnumerator check_button_off = Check_Button_Off(sliderClone);
        StartCoroutine(check_button_off);

        yield return new WaitForSeconds(3f);

        Destroy(sliderClone);
        StopCoroutine(check_button_off);

        Dash_Able = false;
        Move_Able = false;
        if (first_phase != null)
            StopCoroutine(first_phase);
        animator.SetTrigger("Tired");
        PlayerWalkSpeed = 0;
        yield return new WaitForSeconds(2.5f);

        animator.SetBool("IsWalk", false);
        yield return YieldInstructionCache.WaitForSeconds(0.1f);

        Dash_Able = true;
        Move_Able = true;
        first_phase = First_Phase();
        StartCoroutine(first_phase);
        PlayerWalkSpeed = 0.03f;
        yield return null;
    }
    IEnumerator Check_Button_Off(GameObject e)
    {
        while (true)
        {
            if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
            {
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

    public void Fever_Time()
    {
        StartCoroutine(flashOn.Flash(new Color(1, 1, 1, 1), 0.2f, 5));
        Fever_Particle_Clone = Instantiate(Fever_Particle, transform.position - Vector3.down * 1.5f, Quaternion.Euler(-90, 0, 0));
    }

    void Update()
    {
        Main3_Score_Text.text = "점수 : " + Main_3_Score;
    }
    private void LateUpdate()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, stageData.LimitMin.x, stageData.LimitMax.x),
        Mathf.Clamp(transform.position.y, stageData.LimitMin.y, stageData.LimitMax.y));
        if (Fever_Particle_Clone != null)
            Fever_Particle_Clone.transform.position = new Vector3(transform.position.x, Fever_Particle_Clone.transform.position.y, Fever_Particle_Clone.transform.position.z);
    }
}