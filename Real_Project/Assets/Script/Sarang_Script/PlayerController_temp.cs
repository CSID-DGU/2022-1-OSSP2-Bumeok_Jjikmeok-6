using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController_temp : MonoBehaviour
{
    public float IsDoubleClick = 0.25f;
    private bool IsOneClick = false;
    private double Timer = 0;
    private bool onUpdate = true; 

    [SerializeField]
    float maxWalkSpeed = 0.02f;

    [SerializeField]
    Camera getCamera;
   
    // Start is called before the first frame update
    void Start()
    {
      
    }

    IEnumerator Move_Delay()
    {
        maxWalkSpeed *= 2;
        StartCoroutine("Check_Button_Off");
        yield return new WaitForSeconds(3f);
        maxWalkSpeed = 0;
        yield return new WaitForSeconds(2f);
        maxWalkSpeed = 0.02f;
        onUpdate = true;
        yield return null;
    }
    IEnumerator Check_Button_Off()
    {
        while(true)
        {
            if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
                yield break;
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
                    StartCoroutine(Move_Delay());
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
            Debug.Log(pos);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);
            Debug.Log(hit);
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
