using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart_Gaze_Viewer : Slider_Viewer
{
    // Start is called before the first frame update

    private new void Awake()
    {
        base.Awake();
        slider.value = 1;
    }

    public void Decrease_HP(float ratio)
    {
        slider.value -= ratio;
    }
    public void When_Player_Defeat()
    {
        slider.value -= 0.1f;
    }
    public void When_Interrupt_Defeat()
    {
        slider.value += 0.1f;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
