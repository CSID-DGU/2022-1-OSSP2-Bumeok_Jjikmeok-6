using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController_temp : MonoBehaviour
{
    public float IsDoubleClick = 0.25f;
    private bool IsOneClick = false;
    private double Timer = 0;
    private bool onUpdate = true;
    private bool FixedSlider = false;
    private IEnumerator m_Coroutine;
    private IEnumerator i_Coroutine;


    [SerializeField]
    float maxWalkSpeed = 0.02f;

    [SerializeField]
    GameObject Move_Slider;

    [SerializeField]
    Transform canvasTransform;

    [SerializeField]
    GameObject Lazor;

    [SerializeField]
    GameObject Student_Gaze;

    private void Awake()
    {
       // Student_Gaze.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        i_Coroutine = Replace_Update();
        m_Coroutine = Auto_HP(null);
        StartCoroutine(i_Coroutine);
    }
    IEnumerator Replace_Update()
    {
        
        while(true)
        {
            if (onUpdate)
            {
                if (IsOneClick && ((Time.time - Timer) > IsDoubleClick))
                {
                    IsOneClick = false;
                }

                if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A))
                {
                    if (!IsOneClick) // 싱글클릭
                    {
                        Timer = Time.time;
                        IsOneClick = true;
                    }

                    else if (IsOneClick && ((Time.time - Timer) < IsDoubleClick)) // 더블클릭
                    {
                        onUpdate = false;
                        m_Coroutine = Move_Delay();
                        StartCoroutine(m_Coroutine);
                        IsOneClick = false;
                    }
                } // 더블클릭 했을 때 아얘 동작하지 않도록 해야한다
            }
            int key = 0;

            if (Input.GetKey(KeyCode.D))
            {
                key = 1;
                transform.position = new Vector3(transform.position.x + maxWalkSpeed * key, transform.position.y, transform.position.z);

            }
            if (Input.GetKey(KeyCode.A))
            {
                key = -1;
                transform.position = new Vector3(transform.position.x + maxWalkSpeed * key, transform.position.y, transform.position.z);
            }

            if (key != 0)
            {
                transform.localScale = new Vector3(key * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
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

                    Lazor.GetComponent<Movement2D_Variation>().MoveTo(new Vector3(targetStudent.transform.position.x - transform.position.x,
                           targetStudent.transform.position.y - transform.position.y, 0));
                    Instantiate(Lazor, transform.position, Quaternion.identity);
                    
                    if (!Student_Gaze.activeSelf)
                        Student_Gaze.SetActive(true);
                    //Debug.Log(Student_Gaze.transform.position);
                    if (Student_Gaze.GetComponent<Student_Gaze_Info>().Block_HP(targetStudent.transform.position))
                    {
                        StopCoroutine(m_Coroutine);
                        m_Coroutine = Auto_HP(targetStudent);
                        StartCoroutine(m_Coroutine);
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
    IEnumerator Auto_HP(GameObject targetStudent_t)
    {
        StopCoroutine(i_Coroutine);
        yield return StartCoroutine(Student_Gaze.GetComponent<Student_Gaze_Info>().Conflict(targetStudent_t));
        FixedSlider = false;
        yield return new WaitForSeconds(0.1f); // 이 한줄 코드 때문에 10시에 끝날거 12시에 끝났다
       
        StartCoroutine(i_Coroutine);

        yield return null;
    }

    IEnumerator Move_Delay()
    {
        GameObject sliderClone = Instantiate(Move_Slider, transform.position, Quaternion.identity);

        sliderClone.transform.SetParent(canvasTransform); // Slider UI 오브젝트를 parent("Canvas")의 자식으로 설정
        sliderClone.transform.position = transform.position + new Vector3(0, 2, 0);
        sliderClone.transform.localScale = Vector3.one; // 계층 설정으로 바뀐 크기를 기존의 크기로 재설정
        sliderClone.GetComponent<Move_Gaze_Info>().HP_Down();
        maxWalkSpeed *= 2;
        StartCoroutine(Check_Button_Off(sliderClone));
            
        yield return new WaitForSeconds(3f);

        Destroy(sliderClone);
        maxWalkSpeed = 0;
        yield return new WaitForSeconds(2f);

        maxWalkSpeed = 0.02f;
        onUpdate = true;
        yield return null;
    }
    IEnumerator Check_Button_Off(GameObject e)
    {
        while (true)
        {
            if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
            {
                maxWalkSpeed = 0.02f;
                Destroy(e);
                onUpdate = true;
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
