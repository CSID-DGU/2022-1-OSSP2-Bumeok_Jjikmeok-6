using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderBolt : Weapon_Devil
{
    // Start is called before the first frame update

    BackGroundColor backGroundColor;

    private new void Awake()
    {
        base.Awake();
        backGroundColor = GameObject.Find("Flash").GetComponent<BackGroundColor>();

    }
    void Start()
    {
        backGroundColor.StartCoroutine(backGroundColor.Flash(new Color(1, 1, 1, 1), 0.1f, 7));
        Start_Camera_Shake(0.006f, 0.3f, true, false);
    }
}
