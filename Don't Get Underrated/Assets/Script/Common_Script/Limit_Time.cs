using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Limit_Time : MonoBehaviour
{
    TextMeshProUGUI Limit_Time_Text;
    Image image;

    FlashOn flashOn;

    IEnumerator flash_on;

    IEnumerator disappear;

    [SerializeField]
    GameObject Time_Over_Text;

    [SerializeField]
    int Option_Limit_Time;

    private void Awake()
    {
        Limit_Time_Text = GetComponent<TextMeshProUGUI>();
        Limit_Time_Text.text = "제한시간 : " + Option_Limit_Time;
        image = GameObject.Find("Flash_TimeOut").GetComponent<Image>();
        flashOn = image.GetComponent<FlashOn>();
    }

    private void Start()
    {
        StartCoroutine(Descent_Time());
    }

    IEnumerator Descent_Time()
    {
        while(true)
        {
            if (Option_Limit_Time <= 10)
            {
                if (flash_on != null)
                    StopCoroutine(flash_on);
                flash_on = flashOn.Change_Color_Return_To_Origin(new Color(1, 0, 0, 0), new Color(1, 0, 0, 0.5f), .5f, false);
                StartCoroutine(flash_on);
                // 씬 전환
            }
            if (Option_Limit_Time <= 0)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl_Sarang>().Stop_Coroutine();
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl_Sarang>().Destroy_sliderClone();

                GameObject[] e = GameObject.FindGameObjectsWithTag("Interrupt");
                GameObject[] q = GameObject.FindGameObjectsWithTag("Student");
                GameObject ww = GameObject.Find("Student_Gaze");
                GameObject rr = GameObject.Find("Targetting_Object");


                if (ww != null)
                {
                    ww.GetComponent<Student_Gaze_Info>().Stop_Coroutine();
                    Destroy(ww);
                }

                if (rr != null)
                    Destroy(rr);
                    

                foreach (var u in e)
                {
                    u.GetComponent<Interrupt>().Stop_Coroutine();
                    disappear = u.GetComponent<Interrupt>().Change_Color_Lerp(new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), 1.5f, 0.1f, null);
                    StartCoroutine(disappear);
                }
                
                foreach (var u in q)
                {
                    u.GetComponent<Student_Move>().Stop_Coroutine();
                    disappear = u.GetComponent<Student_Move>().Change_Color_Lerp(new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), 1.5f, 0.1f, null);
                    StartCoroutine(disappear);
                }

                yield return YieldInstructionCache.WaitForSeconds(3f);
                Instantiate(Time_Over_Text, new Vector3(0.54f, -0.14f, 0), Quaternion.identity);
                yield break;
                // 씬 전환 
            }
            yield return YieldInstructionCache.WaitForSeconds(1f);
            Option_Limit_Time -= 1;
        }
    }

    


    void Update()
    {
        Limit_Time_Text.text = "제한시간 : " + Option_Limit_Time;
    }
}
