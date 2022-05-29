using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart_Gaze_Viewer : Slider_Viewer
{
    // Start is called before the first frame update

    PlayerCtrl_Sarang playerCtrl_Sarang;
    float fever_decrease;


    private new void Awake()
    {
        base.Awake();
        if (GameObject.FindGameObjectWithTag("Player").TryGetComponent(out PlayerCtrl_Sarang PC_S))
            playerCtrl_Sarang = PC_S;
        slider.value = 0.5f;
        fever_decrease = StaticFunc.Reverse_Time(40);
    }

    public void Decrease_HP(float ratio)
    {
        if (playerCtrl_Sarang != null && !playerCtrl_Sarang.Is_Fever)
            slider.value -= ratio;
    }
    public void When_Player_Defeat()
    {
        if (playerCtrl_Sarang != null && !playerCtrl_Sarang.Is_Fever)
            slider.value -= 0.1f;
    }
    public void Ordinary_Case()
    {
        if (playerCtrl_Sarang != null && !playerCtrl_Sarang.Is_Fever)
            slider.value += 0.05f;
    }
    public void When_Interrupt_Defeat()
    { 
        if (playerCtrl_Sarang != null && !playerCtrl_Sarang.Is_Fever)
            slider.value += 0.1f;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (playerCtrl_Sarang != null)
        {
            if (playerCtrl_Sarang.Is_Fever && playerCtrl_Sarang.is_Domain)
            {
                slider.value -= Time.deltaTime * fever_decrease;
                if (slider.value <= 0.1)
                    playerCtrl_Sarang.Out_Fever();
            }
            else
            {
                if (slider.value >= 0.4)
                {
                    slider.value = 0.38f;
                    playerCtrl_Sarang.Is_Fever = true;
                    playerCtrl_Sarang.transform.localScale = new Vector3(2, 2, 1);
                    playerCtrl_Sarang.animator.SetBool("BulSang", true);
                    playerCtrl_Sarang.Enter_Fever();
                }
            }
        }
    }
}
