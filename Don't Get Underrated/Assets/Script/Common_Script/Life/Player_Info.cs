using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player_Info : Life
{
    [SerializeField]
    protected Text My_Name;

    [SerializeField]
    protected GameObject[] Item;

    protected bool weapon_able;

    private int main_stage_1_score, main_stage_2_score, main_stage_3_score, final_stage_1_score, final_stage_2_score;

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
    public override void TakeDamage(int damage)
    {
        if (Unbeatable)
            return;
        OnDie();
    }
    public override void OnDie()
    {
        base.OnDie();
    }
}