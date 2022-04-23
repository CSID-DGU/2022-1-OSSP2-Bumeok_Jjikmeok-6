using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPSliderViewer : MonoBehaviour
{
    // Start is called before the first frame update
    Enemy enemy;
    Slider slider;

    public void SetUp(Enemy enemy)
    {
        this.enemy = enemy;
        slider = GetComponent<Slider>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = enemy.CurrentHP / enemy.MaxHP;
    }
}
