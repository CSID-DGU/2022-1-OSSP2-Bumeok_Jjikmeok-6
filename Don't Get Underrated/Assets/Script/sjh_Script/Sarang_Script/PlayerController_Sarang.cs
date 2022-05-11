using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController_Sarang : Player_Info
{
    public float IsDoubleClick = 0.25f;
    private bool IsOneClick = false;
    private double Timer = 0;
    private bool Dash_Able = true;
    private bool FixedSlider = false;
    private bool FixedTarget = false;
    private IEnumerator m_Coroutine;
    int StudentLayerMask;  // Player 레이어만 충돌 체크함

    [SerializeField]
    float PlayerWalkSpeed = 0.03f;

    [SerializeField]
    GameObject Move_Slider;

    [SerializeField]
    Transform canvasTransform;

    [SerializeField]
    GameObject Student_Gaze;

    [SerializeField]
    GameObject Targetting_Object;

    Animator animator;

    private new void Awake()
    {
        base.Awake();
        Student_Gaze.SetActive(false);
        Targetting_Object.SetActive(false);
        StudentLayerMask = 1 << LayerMask.NameToLayer("Student");
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_Coroutine = Second_Phase(null);
        
        StartCoroutine(First_Phase());
    }
    IEnumerator First_Phase()
    {

        while (true)
        {
            if (Dash_Able)
                Dash();
            Move(); 
            
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f, StudentLayerMask);
            

            if (hit.collider != null && hit.transform.gameObject.name != "Person")
            {

                if (!FixedTarget)
                {
                    Targetting_Object.SetActive(true);
                    Targetting_Object.GetComponent<Targetting_Effect>().Scale_Color(hit.transform.gameObject);
                    FixedTarget = true;
                }
              
                if (Input.GetMouseButton(0))
                {
                    float dir_Change = transform.position.x - hit.transform.gameObject.transform.position.x;
                    transform.localScale = new Vector3((dir_Change / Mathf.Abs(dir_Change)) * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                    Targetting_Object.GetComponent<Targetting_Effect>().Init();
                    Targetting_Object.SetActive(false);
                    FixedTarget = false;
                   
                    if (!FixedSlider)
                    {
                        Student_Gaze.transform.position = hit.transform.gameObject.transform.position + new Vector3(0, 2, 0);
                        FixedSlider = true;

                        hit.transform.gameObject.GetComponent<Student_Random_Move>().Be_Attacked();
                        hit.transform.gameObject.GetComponent<Student_Random_Move>().Stop_Move(); // 공격 받는 중 + 움직임 정지
                    }

                    Lazor_In_First_Phase(Weapon[0], hit.transform.gameObject.transform.position, transform.position);

                    if (!Student_Gaze.activeSelf)
                        Student_Gaze.SetActive(true);

                    if (Student_Gaze.GetComponent<Student_Gaze_Info>().Block_HP(hit.transform.gameObject.transform.position))
                    {
                        yield return StartCoroutine(Second_Phase(hit.transform.gameObject));
                    }
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    Student_Gaze.GetComponent<Student_Gaze_Info>().Empty_HP();
                    FixedSlider = false;

                    hit.transform.gameObject.GetComponent<Student_Random_Move>().NotBe_Attacked();
                    hit.transform.gameObject.GetComponent<Student_Random_Move>().Start_Move();

                    Student_Gaze.SetActive(false);
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

    private void Lazor_In_First_Phase(GameObject weapon, Vector3 target, Vector3 self)
    {
        Launch_Weapon_For_Move(weapon, target, self);
    }
    
    IEnumerator Second_Phase(GameObject targetStudent_t)
    {
        IEnumerator lazor_after_second = Lazor_In_Second_Phase(targetStudent_t);

        StartCoroutine(lazor_after_second);
        yield return StartCoroutine(Student_Gaze.GetComponent<Student_Gaze_Info>().Competition(targetStudent_t));
        StopCoroutine(lazor_after_second);

        if (targetStudent_t != null)
        {
            targetStudent_t.GetComponent<Student_Random_Move>().NotBe_Attacked();
            targetStudent_t.GetComponent<Student_Random_Move>().Start_Move();
        }
     
        FixedSlider = false;
        yield return new WaitForSeconds(0.1f);
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
        GameObject sliderClone = Instantiate(Move_Slider, transform.position, Quaternion.identity);

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
        animator.SetTrigger("Tired");
        PlayerWalkSpeed = 0;
        yield return new WaitForSeconds(2.5f);

        PlayerWalkSpeed = 0.03f;
        Dash_Able = true;
        animator.SetBool("IsWalk", false);
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
                StopCoroutine(m_Coroutine);
                yield break;
            }
            yield return null;
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

                m_Coroutine = Move_Delay();
                StartCoroutine(m_Coroutine);
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
    void Update()
    {

    }
}