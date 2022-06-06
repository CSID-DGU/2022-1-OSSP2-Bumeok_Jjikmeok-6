using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart_Gaze_Viewer : Slider_Viewer // 플레이어의 하트 게이지
{

    [SerializeField]
    float fever_decrease = 10;

    Player_Stage3 player_stage3;
   
    private new void Awake()
    {
        base.Awake();
        if (GameObject.FindGameObjectWithTag("Player").TryGetComponent(out Player_Stage3 PC_S))
            player_stage3 = PC_S;
        fever_decrease = StaticFunc.Reverse_Time(fever_decrease);
    }
    public void When_Player_Defeat(int Ratio) // 플레이어가 인터럽트에게 졌을 때 : 0.1 * 인터럽트의 수만큼 하트 게이지 감소
    {
        if (Check_Valid_Slider() && player_stage3 != null && !player_stage3.Is_Fever)
            slider.value -= 0.1f * Ratio;
    }
    public void Ordinary_Case() // 평상시의 경우 : 하트게이지 0.05 증가
    {
        if (Check_Valid_Slider() && player_stage3 != null && !player_stage3.Is_Fever)
            slider.value += 0.05f;
    }
    public void When_Interrupt_Defeat(int Ratio) // 플레이어가 인터럽트에게 이겼을 때 : 0.1 * 인터럽트의 수만큼 하트 게이지 증가
    { 
        if (Check_Valid_Slider() && player_stage3 != null && !player_stage3.Is_Fever)
            slider.value += 0.1f * Ratio;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (Check_Valid_Slider() && player_stage3 != null && player_stage3.is_Domain)
        {
            if (player_stage3.Is_Fever) // 피버 타임 돌입 시 --> fever_decrease(초) 동안 꽉 차있던 하트게이지에서 0으로 감소시킴
            {
                slider.value -= Time.deltaTime * fever_decrease;
                if (slider.value <= 0)
                {
                    player_stage3.Is_Fever = false;
                    player_stage3.Out_Fever(); // 감소 시켰으면 플레이어의 피버타임 해제
                }
            }
            else
            {
                if (slider.value >= 1 && !player_stage3.Is_Fever) // 하트게이지가 꽉 찼으면 피버타임 돌입
                {
                    slider.value = 0.98f; // 에러 방지를 위해 값을 살짝 감소시켰음
                    player_stage3.Is_Fever = true;
                    player_stage3.animator.SetBool("BulSang", true); // 플레이어의 모습 변화 (불상으로)
                    player_stage3.Enter_Fever(); // 플레이어의 피버타임 돌입
                }
            }
        }
    }
}
