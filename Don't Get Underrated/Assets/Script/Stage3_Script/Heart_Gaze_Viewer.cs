using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart_Gaze_Viewer : Slider_Viewer
{
    // Start is called before the first frame update

    Player_Stage3 playerCtrl_Sarang;

    [SerializeField]
    float fever_decrease;
    private new void Awake()
    {
        base.Awake();
        if (GameObject.FindGameObjectWithTag("Player").TryGetComponent(out Player_Stage3 PC_S))
            playerCtrl_Sarang = PC_S;
        fever_decrease = StaticFunc.Reverse_Time(fever_decrease);
    }
    public void When_Player_Defeat(int Ratio)
    {
        if (Check_Valid_Slider() && playerCtrl_Sarang != null && !playerCtrl_Sarang.Is_Fever)
            slider.value -= 0.1f * Ratio;
    }
    public void Ordinary_Case()
    {
        if (Check_Valid_Slider() && playerCtrl_Sarang != null && !playerCtrl_Sarang.Is_Fever)
            slider.value += 0.05f;
    }
    public void When_Interrupt_Defeat(int Ratio)
    { 
        if (Check_Valid_Slider() && playerCtrl_Sarang != null && !playerCtrl_Sarang.Is_Fever)
            slider.value += 0.1f * Ratio;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (Check_Valid_Slider() && playerCtrl_Sarang != null && playerCtrl_Sarang.is_Domain)
        {
            if (playerCtrl_Sarang.Is_Fever)
            {
                slider.value -= Time.deltaTime * fever_decrease;
                if (slider.value <= 0)
                {
                    playerCtrl_Sarang.Is_Fever = false;
                    playerCtrl_Sarang.Out_Fever();
                }
            }
            else
            {
                if (slider.value >= 1 && !playerCtrl_Sarang.Is_Fever)
                {
                    slider.value = 0.98f;
                    playerCtrl_Sarang.Is_Fever = true;
                    playerCtrl_Sarang.animator.SetBool("BulSang", true);
                    playerCtrl_Sarang.Enter_Fever();
                }
            }
        }
    }
}
