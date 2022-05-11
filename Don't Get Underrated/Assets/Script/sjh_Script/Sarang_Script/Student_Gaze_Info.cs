using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Student_Gaze_Info : MonoBehaviour
{
    // Start is called before the first frame update

    Slider slider;

    Camera selectedCamera;

    public bool CanWarp = false;

    ArrayList Interrupt_Num_On_Active; // 플레이어와 실제 경쟁 중인 인터럽트

    ArrayList Interrupt_Num_On_NonActive; // 맵을 떠도는 인터럽트

    ArrayList Stop_Interrupt_Arr_IEnum; // IEnumrator를 담는 배열

    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.value = 0;
        Interrupt_Num_On_NonActive = new ArrayList();
        Interrupt_Num_On_Active = new ArrayList();
        Stop_Interrupt_Arr_IEnum = new ArrayList();
    }
    void Start()
    {
        selectedCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }
    public bool Block_HP(Vector3 TempPosition)
    {
        slider.value += Time.deltaTime / 2;
        if (slider.value >= 0.5f && Interrupt_Num_On_NonActive.Count >= 1 && Interrupt_Num_On_Active.Count == 0)
        {
            Stop_Interrupt_Arr_IEnum = new ArrayList();
            foreach (var interrupt in Interrupt_Num_On_NonActive)
            {
                GameObject copy_interrupt = (GameObject)interrupt;

                copy_interrupt.GetComponent<Interrupt_Random_Move>().Stop_Move();
                Interrupt_Num_On_Active.Add(copy_interrupt);

                IEnumerator temp_s = copy_interrupt.GetComponent<Interrupt_Random_Move>().Trigger_Lazor(TempPosition);
                Stop_Interrupt_Arr_IEnum.Add(temp_s);

                StartCoroutine(temp_s);
            }
            return true;
        }
        return false;
    }
    
    public void Empty_HP()
    {
        slider.value = 0;
    }
    public IEnumerator Competition(GameObject student)
    {
        StartCoroutine(GameObject.FindGameObjectWithTag("Flash").GetComponent<FlashOn>().Flash(new Color(1, 1, 1, 1), 0.05f, 5));

        while(true)
        {
            slider.value -= Time.deltaTime / (5 - Interrupt_Num_On_NonActive.Count);

            if (Input.GetMouseButtonDown(0))
                slider.value += (Time.deltaTime * 15);

            if (slider.value <= 0 || slider.value >= 1)
            {
                foreach (var stop in Stop_Interrupt_Arr_IEnum)
                {
                    StopCoroutine((IEnumerator)stop);
                }
                if (slider.value <= 0)
                {
                    foreach (var u in Interrupt_Num_On_Active)
                    {
                        GameObject interrupt = (GameObject)u;
                        interrupt.GetComponent<Interrupt_Random_Move>().Start_Move();
                    }
                }
                else if (slider.value >= 1)
                {
                    foreach (var u in Interrupt_Num_On_Active)
                    {
                        GameObject copy_interrupt = (GameObject)u;
                        Destroy(copy_interrupt);
                    }
                    slider.value = 0;

                    Destroy(student);
                }
                
                gameObject.SetActive(false);
                Stop_Interrupt_Arr_IEnum = new ArrayList();
                Interrupt_Num_On_Active = new ArrayList();
                Interrupt_Num_On_NonActive = new ArrayList();
                yield return null;
                yield break;
            }
            yield return null;
        }
    }
    void CheckInterrupt()
    {
        GameObject[] e = GameObject.FindGameObjectsWithTag("Interrupt");
        if (e != null)
        {
            Interrupt_Num_On_NonActive = new ArrayList();
            foreach (var u in e)
            {
                Vector3 screenPoint = selectedCamera.WorldToViewportPoint(u.transform.position);
                bool OnScreen = screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

                if (OnScreen)
                {
                    Interrupt_Num_On_NonActive.Add(u);
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        CheckInterrupt();
    } // 학생이 레이저 빔을 과제, 시험같은 방해 오브젝트 및 플레이어에게 맞았을 때 처리한 코드
}