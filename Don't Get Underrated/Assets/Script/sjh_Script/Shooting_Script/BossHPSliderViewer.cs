using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHPSliderViewer : Slider_Viewer
{
    // Start is called before the first frame update

    Boss_Info boss_info;

    bool OnUpdate = false;

    private new void Awake()
    {
        base.Awake();
        slider.value = 0;
    }
    public void F_HPFull(Boss_Info boss_info)
    {
        this.boss_info = boss_info;
       
        StartCoroutine("I_HPFull");
    }
    IEnumerator I_HPFull()
    {
        while(slider.value < 1)
        {
            slider.value += Time.deltaTime / 2;
            yield return null;
        }
        OnUpdate = true;
    }
    // Update is called once per frame
    void LateUpdate()
    {
         if (OnUpdate)
            slider.value = boss_info.CurrentHP / boss_info.MaxHP;
    }
}
