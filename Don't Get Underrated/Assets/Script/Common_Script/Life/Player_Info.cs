using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player_Info : Life
{
    [SerializeField]
    protected Text My_Name; // 인게임 내에 출력되는 플레이어의 id

    [SerializeField]
    protected GameObject[] Item; // 플레이어가 사용하는 아이템

    protected bool weapon_able; // 플레이어의 무기 사용 여부에 대한 flag

    private int main_stage_1_score, main_stage_2_score, main_stage_3_score, final_stage_1_score, final_stage_2_score; // 플레이어가 갖는 스테이지에 대한 점수

    protected virtual new void Awake()
    {
        base.Awake();
    }

    public int Main_Stage_1_Score
    {
        set { main_stage_1_score = value; }
        get { return main_stage_1_score; }
    }
    public int Main_Stage_2_Score
    {
        set { main_stage_2_score = value; }
        get { return main_stage_2_score; }
    }
    public int Main_Stage_3_Score
    {
        set { main_stage_3_score = value; }
        get { return main_stage_3_score; }
    }
    public int Final_Stage_1_Score
    {
        set { final_stage_1_score = value; }
        get { return final_stage_1_score; }
    }
    public int Final_Stage_2_Score
    {
        set { final_stage_2_score = value; }
        get { return final_stage_2_score; }
    }
    public override void TakeDamage(int damage) // 현재 플레이어는 생명에 대한 정의를 내리지 못해 무적 상태일 때만 이를 무시하며,
        // 아닐 시 죽도록 처리
    {
        if (Unbeatable)
            return;
        OnDie();
    }

    public override void Stop_When_Network_Stop()
    {
        base.Stop_When_Network_Stop();
    }
    public override void OnDie()
    {
        base.OnDie();
    }
}