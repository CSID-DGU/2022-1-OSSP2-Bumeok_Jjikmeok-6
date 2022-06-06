using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart_Gaze_Viewer : Slider_Viewer // �÷��̾��� ��Ʈ ������
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
    public void When_Player_Defeat(int Ratio) // �÷��̾ ���ͷ�Ʈ���� ���� �� : 0.1 * ���ͷ�Ʈ�� ����ŭ ��Ʈ ������ ����
    {
        if (Check_Valid_Slider() && player_stage3 != null && !player_stage3.Is_Fever)
            slider.value -= 0.1f * Ratio;
    }
    public void Ordinary_Case() // ������ ��� : ��Ʈ������ 0.05 ����
    {
        if (Check_Valid_Slider() && player_stage3 != null && !player_stage3.Is_Fever)
            slider.value += 0.05f;
    }
    public void When_Interrupt_Defeat(int Ratio) // �÷��̾ ���ͷ�Ʈ���� �̰��� �� : 0.1 * ���ͷ�Ʈ�� ����ŭ ��Ʈ ������ ����
    { 
        if (Check_Valid_Slider() && player_stage3 != null && !player_stage3.Is_Fever)
            slider.value += 0.1f * Ratio;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (Check_Valid_Slider() && player_stage3 != null && player_stage3.is_Domain)
        {
            if (player_stage3.Is_Fever) // �ǹ� Ÿ�� ���� �� --> fever_decrease(��) ���� �� ���ִ� ��Ʈ���������� 0���� ���ҽ�Ŵ
            {
                slider.value -= Time.deltaTime * fever_decrease;
                if (slider.value <= 0)
                {
                    player_stage3.Is_Fever = false;
                    player_stage3.Out_Fever(); // ���� �������� �÷��̾��� �ǹ�Ÿ�� ����
                }
            }
            else
            {
                if (slider.value >= 1 && !player_stage3.Is_Fever) // ��Ʈ�������� �� á���� �ǹ�Ÿ�� ����
                {
                    slider.value = 0.98f; // ���� ������ ���� ���� ��¦ ���ҽ�����
                    player_stage3.Is_Fever = true;
                    player_stage3.animator.SetBool("BulSang", true); // �÷��̾��� ��� ��ȭ (�һ�����)
                    player_stage3.Enter_Fever(); // �÷��̾��� �ǹ�Ÿ�� ����
                }
            }
        }
    }
}
