using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderBolt : Weapon_Devil
{
    // Start is called before the first frame update

    private new void Awake()
    {
        base.Awake();

    }
    void Start()
    {
        Start_Camera_Shake(0.006f, 0.3f, false, false);
    }
}
