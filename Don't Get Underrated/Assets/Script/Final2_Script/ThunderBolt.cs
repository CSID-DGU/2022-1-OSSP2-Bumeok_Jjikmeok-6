using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderBolt : Weapon_Devil
{
    // Start is called before the first frame update

    private new void Awake()
    {
        base.Awake();
        if (GameObject.Find("Flash") && GameObject.Find("Flash").TryGetComponent(out ImageColor IC))
            backGroundColor = IC;
        if (GameObject.Find("Weapon_Effect_Sound") && GameObject.Find("Weapon_Effect_Sound").TryGetComponent(out AudioSource AS1))
            EffectSource = AS1;
    }
    void Start()
    {
        Effect_Sound_OneShot(0);
        Start_Camera_Shake(0.006f, 0.3f, false, false);
        Flash(Color.white, 0.1f, 0.5f);
    }
}
