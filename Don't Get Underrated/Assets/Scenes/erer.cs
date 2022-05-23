using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class erer : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    GameObject Wow;
    void Start()
    {
        Debug.Log(Wow);
        if (Wow == null)
        {
            Debug.Log("¿ì¿Í!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
