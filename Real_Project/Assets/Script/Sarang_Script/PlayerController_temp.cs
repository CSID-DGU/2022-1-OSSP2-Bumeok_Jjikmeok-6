using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController_temp : Player_Info
{
    public float IsDoubleClick = 0.25f;
    private bool IsOneClick = false;
    private double Timer = 0;
    private bool Dash_Able = true;
    private bool FixedSlider = false;
    private IEnumerator m_Coroutine;

    [SerializeField]
    float PlayerWalkSpeed = 0.02f;

    [SerializeField]
    GameObject Move_Slider;

    [SerializeField]
    Transform canvasTransform;

    [SerializeField]
    GameObject Lazor;

    [SerializeField]
    GameObject Student_Gaze;

    private new void Awake()
    {
        // Student_Gaze.SetActive(false);
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

            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);

            if (hit.collider != null && hit.transform.gameObject.name != "Person")
            {
                if (Input.GetMouseButton(0))
                {
                    GameObject targetStudent = hit.transform.gameObject;

                    if (!FixedSlider)
                    {
                        Student_Gaze.transform.position = targetStudent.transform.position + new Vector3(0, 2, 0);
                        FixedSlider = true;
                    }

                    Launch_Weapon_For_Move(Lazor, targetStudent.transform.position, transform.position);

                    if (!Student_Gaze.activeSelf)
                        Student_Gaze.SetActive(true);
                    if (Student_Gaze.GetComponent<Student_Gaze_Info>().Block_HP(targetStudent.transform.position))
                    {
                        yield return StartCoroutine(Second_Phase(targetStudent));
                    }
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    Student_Gaze.GetComponent<Student_Gaze_Info>().Empty_HP();
                    FixedSlider = false;
                    Student_Gaze.SetActive(false);
                }
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
            key = 1;
            transform.position = new Vector3(transform.position.x + PlayerWalkSpeed * key, transform.position.y, transform.position.z);

        }
        if (Input.GetKey(KeyCode.A))
        {
            key = -1;
            transform.position = new Vector3(transform.position.x + PlayerWalkSpeed * key, transform.position.y, transform.position.z);
        }

        if (key != 0)
        {
            transform.localScale = new Vector3(key * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
    IEnumerator Second_Phase(GameObject targetStudent_t)
    {
        IEnumerator lazor_after_second = Lazor_After_Second(targetStudent_t);
        StartCoroutine(lazor_after_second);
        yield return StartCoroutine(Student_Gaze.GetComponent<Student_Gaze_Info>().Competition(targetStudent_t));
        StopCoroutine(lazor_after_second);
        FixedSlider = false;
        yield return new WaitForSeconds(0.1f);
    }
    IEnumerator Lazor_After_Second(GameObject targetStudent_t)
    {
        while(true)
        {
            if (targetStudent_t != null)
                Launch_Weapon_For_Move(Lazor, targetStudent_t.transform.position, transform.position);
            yield return YieldInstructionCache.WaitForEndOfFrame;
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
        StartCoroutine(Check_Button_Off(sliderClone));

        yield return new WaitForSeconds(3f);

        Destroy(sliderClone);
        PlayerWalkSpeed = 0;
        yield return new WaitForSeconds(2f);

        PlayerWalkSpeed = 0.02f;
        Dash_Able = true;
        yield return null;
    }
    IEnumerator Check_Button_Off(GameObject e)
    {
        while (true)
        {
            if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
            {
                PlayerWalkSpeed = 0.02f;
                Destroy(e);
                Dash_Able = true;
                StopCoroutine(m_Coroutine);
                yield break;
            }
            yield return null;
        }
    }
    void Update()
    {

    }
}