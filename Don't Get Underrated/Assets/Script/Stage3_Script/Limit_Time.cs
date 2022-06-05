using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Limit_Time : MonoBehaviour
{
    TextMeshProUGUI Limit_Time_Text;
    Image image;

    ImageColor flashOn;

    IEnumerator flash_on;

    IEnumerator descent_time;

    [SerializeField]
    GameObject Time_Over_Text;

    [SerializeField]
    int Option_Limit_Time;

    int wow_Time;

    private void Awake()
    {
        Limit_Time_Text = GetComponent<TextMeshProUGUI>();
        wow_Time = Option_Limit_Time;
        Limit_Time_Text.text = "제한시간 : " + wow_Time;
        image = GameObject.Find("Flash_TimeOut").GetComponent<Image>();
        flashOn = image.GetComponent<ImageColor>();
    }

    private void Start()
    {
        descent_time = Descent_Time();
        StartCoroutine(descent_time);
    }

    IEnumerator Descent_Time()
    {
        while (true)
        {
            if (wow_Time <= 10)
            {
                if (flash_on != null)
                    flashOn.StopCoroutine(flash_on);
                flash_on = flashOn.Change_Origin_BG(new Color(1, 0, 0, 0.5f), .5f);
                flashOn.StartCoroutine(flash_on);
            }
            if (wow_Time <= 0)
            {
                Player_Stage3 playerCtrl_Sarang;

                if (GameObject.FindGameObjectWithTag("Player").TryGetComponent(out Player_Stage3 PC_S))
                    playerCtrl_Sarang = PC_S;
                else
                    yield break;

                playerCtrl_Sarang.Stop_Walk();

                GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");
                GameObject[] student = GameObject.FindGameObjectsWithTag("Student");
                GameObject student_gaze = GameObject.Find("Student_Gaze");
                GameObject targetting_object = GameObject.Find("Targetting_Object");

                if (student_gaze != null && student_gaze.TryGetComponent(out Student_Gaze_Info SGI))
                {
                    SGI.Change_Red_Slider();
                    Destroy(student_gaze);
                }

                if (targetting_object != null)
                    Destroy(targetting_object);
                    
                foreach (var e in enemy)
                {
                    if (e.TryGetComponent(out Interrupt I1))
                    {
                        I1.Stop_Coroutine();
                        I1.Disappear();
                    }
                }
                
                foreach (var e in student)
                {
                    if (e.TryGetComponent(out Student S1))
                    {
                        S1.Stop_Coroutine();
                        S1.Disappear();
                    }
                }

                yield return YieldInstructionCache.WaitForSeconds(3f);
                Instantiate(Time_Over_Text, new Vector3(playerCtrl_Sarang.transform.position.x, playerCtrl_Sarang.transform.position.y + 2, 0), Quaternion.identity);
                yield break;
                // 씬 전환 
            }
            yield return YieldInstructionCache.WaitForSeconds(1f);
            wow_Time -= 1;
        }
    }
    public void Stop_Time_Persist()
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
        Limit_Time_Text.text = "제한시간 : " + wow_Time;
    }
}
