using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor_Button : MonoBehaviour
{
    public void Start_Down_Or_UP(bool is_Down, int Change_Y)
    {
        if (is_Down && gameObject.activeInHierarchy)
            transform.position = new Vector3(-7, Change_Y, 0);
        else
            transform.position = new Vector3(43, Change_Y, 0);
    }
}
