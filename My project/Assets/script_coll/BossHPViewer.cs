using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHPViewer : MonoBehaviour
{

    [SerializeField]
    BossHP bossHP;
    Slider sliderHP;

    private void Awake()
    {
        sliderHP = GetComponent<Slider>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        sliderHP.value = bossHP.CurrentHP / bossHP.MaxHP;
    }
}
