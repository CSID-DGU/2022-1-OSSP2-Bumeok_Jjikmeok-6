using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolGryn_Copy : Enemy_Info
{
    // Start is called before the first frame update
    private new void Awake()
    {
        base.Awake();

    }
    public void Move_Lerp_Distance(Vector3 late_vector)
    {
        StartCoroutine(Position_Lerp(transform.position, late_vector, 0.15f, OriginCurve));        
    }

    // Update is called once per frame

}
