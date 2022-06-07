using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Limit_Time : MonoBehaviour // 제한 시간에 대한 스크립트. 게임의 종료를 담당
{
    [SerializeField]
    GameObject Time_Over_Text;

    [SerializeField]
    int Option_Limit_Time;

    TextMeshProUGUI Limit_Time_Text;

    Image image;

    ImageColor flashOn;

    IEnumerator flash_on, decrease_time;

    int wow_Time;

    SpriteColor spriteColor;

    private void Awake()
    {
        Limit_Time_Text = GetComponent<TextMeshProUGUI>();
        image = GameObject.Find("Flash_TimeOut").GetComponent<Image>();
        if (GameObject.Find("Total_Sprite") && GameObject.Find("Total_Sprite").TryGetComponent(out SpriteColor s1))
            spriteColor = s1;
        flashOn = image.GetComponent<ImageColor>();
        flash_on = null;
        decrease_time = null;
        wow_Time = Option_Limit_Time;
        Limit_Time_Text.text = "제한시간 : " + wow_Time;
    }

    private void Start()
    {
        decrease_time = Decrease_Time();
        StartCoroutine(decrease_time); // 제한 시간 감소 시작
    }

    IEnumerator Decrease_Time()
    {
        while (true)
        {
            if (wow_Time <= 10) // 제한 시간이 10초 남았을 때
            {
                if (flash_on != null)
                    flashOn.StopCoroutine(flash_on);
                flash_on = flashOn.Change_Origin_BG(new Color(1, 0, 0, 0.5f), 0.4f); // 0.4초 간 빨간색 배경으로 서서히 변환 후 원위치 
                flashOn.StartCoroutine(flash_on);
            }
            if (wow_Time <= 0) // 제한시간이 다 됐을 때
            {
                singleTone.Music_Decrease = false;
                Player_Stage3 player_stage3;

                if (GameObject.FindGameObjectWithTag("Player").TryGetComponent(out Player_Stage3 PC_S))
                    player_stage3 = PC_S;
                else
                    yield break;

                player_stage3.Stop_Walk(); // 플레이어의 행동 중지

                GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");
                GameObject[] student = GameObject.FindGameObjectsWithTag("Student");
                GameObject student_gaze = GameObject.Find("Student_Gaze");
                GameObject targetting_object = GameObject.Find("Targetting_Object");

                if (student_gaze != null && student_gaze.TryGetComponent(out Student_Gaze_Info SGI))
                {
                    SGI.Change_Red_Slider();
                    Destroy(student_gaze);
                } // 학생 게이지 초기화 후 학생 게이지 삭제

                if (targetting_object != null)
                    Destroy(targetting_object);
                    
                foreach (var e in enemy)
                {
                    if (e.TryGetComponent(out Interrupt I1))
                    {
                        I1.Stop_Lazor();
                        I1.Disappear();
                    } // 인터럽트의 레이저 중지 후 사라짐
                }
                
                foreach (var e in student)
                {
                    if (e.TryGetComponent(out Student S1))
                    {
                        S1.Stop_Coroutine();
                        S1.Disappear();
                    }  // 돌아다니는 학생의 행동 중지 후 사라짐
                }

                yield return YieldInstructionCache.WaitForSeconds(3f);
                singleTone.main_stage_3_score = player_stage3.Main_Stage_3_Score;

                Instantiate(Time_Over_Text, new Vector3(player_stage3.transform.position.x, player_stage3.transform.position.y + 2, 0), Quaternion.identity);
                yield return YieldInstructionCache.WaitForSeconds(3f);

                if (spriteColor != null)
                    yield return spriteColor.StartCoroutine(spriteColor.Change_Color_Real_Time(Color.black, 2));
                
                singleTone.Music_Decrease = true;
                singleTone.SceneNumManage++;
                SceneManager.LoadScene(singleTone.SceneNumManage);
                yield break;
                // 씬 전환 
            }
            yield return YieldInstructionCache.WaitForSeconds(1f);
            wow_Time -= 1;
        }
    }
    public void Stop_Time_Decrease()
    {
        if (decrease_time != null)
            StopCoroutine(decrease_time);
    }

    public void Start_Time_Decrease()
    {
        decrease_time = Decrease_Time();
        StartCoroutine(decrease_time);
    }

    void Update()
    {
        Limit_Time_Text.text = "제한시간 : " + wow_Time;
    }
}
