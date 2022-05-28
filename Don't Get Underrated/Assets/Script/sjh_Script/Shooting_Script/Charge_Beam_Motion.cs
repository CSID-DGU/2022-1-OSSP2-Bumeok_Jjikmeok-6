using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge_Beam_Motion : MonoBehaviour
{
    // Start is called before the first frame update

    ParticleSystem ps;
    Asura asura;
    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        asura = GameObject.FindGameObjectWithTag("Boss").GetComponent<Asura>();
    }
    private void Update()
    {
        Debug.Log(asura.transform.position);
        Debug.Log(ps.transform.position);
        ps.transform.rotation = Quaternion.LookRotation(asura.transform.position - ps.transform.position);
        Debug.Log(ps.transform.rotation);
    }
}
