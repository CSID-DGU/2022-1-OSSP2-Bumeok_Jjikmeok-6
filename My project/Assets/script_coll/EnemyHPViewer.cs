using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPViewer : MonoBehaviour
{
    // Start is called before the first frame update
    EnemyHP enemyHP;
    Slider hpSlider;

    public void Setup(EnemyHP enemyHP)
    {
        this.enemyHP = enemyHP;
        hpSlider = GetComponent<Slider>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hpSlider.value = enemyHP.CurrentHP / enemyHP.MaxHP;    
    }
}
