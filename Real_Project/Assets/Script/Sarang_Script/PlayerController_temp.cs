using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController_temp : MonoBehaviour
{
    public float IsDoubleClick = 0.25f;
    private bool IsOneClick = false;
    private double Timer = 0;
    private bool onUpdate = true;
    private IEnumerator m_Coroutine;

    [SerializeField]
    float maxWalkSpeed = 0.02f;

    [SerializeField]
    GameObject Move_Slider;

    [SerializeField]
    Transform canvasTransform;

    [SerializeField]
    GameObject Lazor;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
      
    }

    IEnumerator Move_Delay()
    {
        Debug.Log("왜");
        GameObject sliderClone = Instantiate(Move_Slider, transform.position, Quaternion.identity);

        sliderClone.transform.SetParent(canvasTransform); // Slider UI 오브젝트를 parent("Canvas")의 자식으로 설정
        sliderClone.transform.position = transform.position + new Vector3(0, 2, 0);
        sliderClone.transform.localScale = Vector3.one; // 계층 설정으로 바뀐 크기를 기존의 크기로 재설정
        sliderClone.GetComponent<Gaze_Info>().HP_Down();
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
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);

            if (hit.collider != null && hit.transform.gameObject.GetComponent<Square>().Correct_Color())
            {
                Instantiate(Lazor, transform.position, Quaternion.identity);
                Lazor.GetComponent<Movement2D>().MoveTo(new Vector3(hit.transform.gameObject.transform.position.x - transform.position.x,
                    hit.transform.gameObject.transform.position.y - transform.position.y, 0));
            }

            GameObject[] e = GameObject.FindGameObjectsWithTag("Enemy");
            foreach(var u in e)
            {
                u.GetComponent<Square>().Change_color(255, 255, 255);
            }
            if (hit.collider != null)
            {
                Debug.Log(hit.transform.gameObject);
                hit.transform.gameObject.GetComponent<Square>().Change_color(57, 192, 212);
            }

        }        
    }
}
