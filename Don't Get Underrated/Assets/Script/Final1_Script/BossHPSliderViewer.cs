using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHPSliderViewer : Slider_Viewer
{
    Boss_Info boss_info;

    bool OnUpdate = false;

    private new void Awake()
    {
        base.Awake();
    }
    public void F_HPFull(Boss_Info boss_info)
    {
        this.boss_info = boss_info;
       
        StartCoroutine(I_HPFull());
    }
    IEnumerator I_HPFull()
    {
        if (!Check_Valid_Slider())
            yield break;
        while(slider.value < 1)
        {
            slider.value += Time.deltaTime * 0.5f;
            yield return null;
        }
        OnUpdate = true;
    }
    // Update is called once per frame
    void LateUpdate()
    {
         if (OnUpdate && Check_Valid_Slider())
            slider.value = boss_info.CurrentHP / boss_info.MaxHP;
    }
}
