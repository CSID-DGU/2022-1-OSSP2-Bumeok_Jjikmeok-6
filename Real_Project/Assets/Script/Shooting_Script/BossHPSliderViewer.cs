using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHPSliderViewer : MonoBehaviour
{
    // Start is called before the first frame update

    HP_Info hp_info;
    Slider slider;

    bool OnUpdate = false;


    public void F_HPFull(HP_Info hp_info)
    {
        this.hp_info = hp_info;
        slider = GetComponent<Slider>();
        StartCoroutine("I_HPFull");
    }
    IEnumerator I_HPFull()
    {
        Debug.Log(hp_info.CurrentHP);
        for (int i = 0; i < hp_info.MaxHP; i++)
        {
            yield return null;
            slider.value = i / hp_info.MaxHP;
        }
        OnUpdate = true;
    }


    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
         if (OnUpdate)
            slider.value = hp_info.CurrentHP / hp_info.MaxHP;
       
    }
}
