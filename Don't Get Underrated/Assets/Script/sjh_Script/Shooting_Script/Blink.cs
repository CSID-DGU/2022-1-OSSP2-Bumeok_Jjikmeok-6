using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
    // Start is called before the first frame update

    public void Change_D()
    {
        transform.position = transform.position + new Vector3(-0.02f, -0.02f, 0);
    }
    public void Destroy_It()
    {
        Destroy(gameObject);
    }
    // Update is called once per frame
}
