using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHPSliderViewer : MonoBehaviour
{
    // Start is called before the first frame update

    Boss boss;
    Slider slider;

    bool OnUpdate = false;

    public void Kuku()
    {
        Debug.Log("int KUKU = ");
    }

    public void F_HPFull(Boss boss)
    {
        this.boss = boss;
        slider = GetComponent<Slider>();
        StartCoroutine("I_HPFull");
    }
    public IEnumerator I_HPFull()
    {
        for (int i = 0; i < boss.MaxHP; i++)
        {
            yield return new WaitForSeconds(0.04f);
            slider.value = i / boss.MaxHP;
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
            slider.value = boss.CurrentHP / boss.MaxHP;
       
    }
}
