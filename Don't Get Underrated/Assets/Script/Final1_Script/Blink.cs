using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
    public void Change_D()
    {
        transform.position = transform.position + new Vector3(-0.02f, -0.02f, 0);
    }
    public void Destroy_It()
    {
        Destroy(gameObject);
    }
}