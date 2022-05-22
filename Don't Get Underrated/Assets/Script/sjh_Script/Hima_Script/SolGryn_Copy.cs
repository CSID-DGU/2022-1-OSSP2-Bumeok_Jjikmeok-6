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

    public void Move_Slerp_Distance(Vector3 Common, string dir)
    {
        StartCoroutine(Position_Slerp_Temp(transform.position, Common,
              Get_Center_Vector(transform.position, Common, Vector3.Distance(transform.position, Common) * 0.85f, dir), 1, declineCurve, false));

    }
    public void Shake_ItSelf()
    {
        StartCoroutine(Shake_Act(0.2f, 0.2f, 0.5f, false));
    }

    public void Launch_SoyBean()
    {
        //if (GameObject.FindGameObjectWithTag("Player"))
        Instantiate(Weapon[0], transform.position, Quaternion.identity);
    }
    

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    // Update is called once per frame

}
