using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Limit_Time : MonoBehaviour
{
    TextMeshProUGUI Limit_Time_Text;
    Image image;

    BackGroundColor flashOn;

    IEnumerator flash_on;

    IEnumerator disappear;

    IEnumerator descent_time;

    [SerializeField]
    GameObject Time_Over_Text;

    [SerializeField]
    int Option_Limit_Time;

    private void Awake()
    {
        Limit_Time_Text = GetComponent<TextMeshProUGUI>();
        Limit_Time_Text.text = "제한시간 : " + Option_Limit_Time;
        image = GameObject.Find("Flash_TimeOut").GetComponent<Image>();
        flashOn = image.GetComponent<BackGroundColor>();
    }

    private void Start()
    {
        descent_time = Descent_Time();
        StartCoroutine(descent_time);
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
                    Destroy(ww);

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
                    u.GetComponent<Student>().Stop_Coroutine();
                    disappear = u.GetComponent<Student>().Change_Color_Lerp(new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), 1.5f, 0.1f, null);
                    StartCoroutine(disappear);
                }

                yield return YieldInstructionCache.WaitForSeconds(3f);
                Instantiate(Time_Over_Text, new Vector3(GameObject.FindGameObjectWithTag("Player").transform.position.x, 0, 0), Quaternion.identity);
                yield break;
                // 씬 전환 
            }
            yield return YieldInstructionCache.WaitForSeconds(1f);
            Option_Limit_Time -= 1;
        }
    }

    public void When_Walk_Floor()
    {
        if (descent_time != null)
            StopCoroutine(descent_time);
    }

    public void Final_Walk_Floor()
    {
        descent_time = Descent_Time();
        StartCoroutine(descent_time);
    }


    void Update()
    {
        Limit_Time_Text.text = "제한시간 : " + Option_Limit_Time;
    }
}
