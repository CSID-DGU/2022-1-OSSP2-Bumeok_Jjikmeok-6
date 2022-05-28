using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart_Gaze_Viewer : Slider_Viewer
{
    // Start is called before the first frame update

    PlayerCtrl_Sarang playerCtrl_Sarang;

    private new void Awake()
    {
        base.Awake();
        if (GameObject.FindGameObjectWithTag("Player").TryGetComponent(out PlayerCtrl_Sarang user))
            playerCtrl_Sarang = user;
        slider.value = 0.5f;
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
                slider.value -= Time.deltaTime / 40;
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
