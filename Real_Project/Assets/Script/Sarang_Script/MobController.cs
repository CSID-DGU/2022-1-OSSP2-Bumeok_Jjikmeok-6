using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobController : MonoBehaviour
{

    float speedx = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int key = 0;
        float trans_x;

        if (this.transform.localScale.x > 0) key = 1;
        if (this.transform.localScale.x < 0) key = -1;

        if(Mathf.Abs(this.transform.position.x) > 9)
        {

        }

        trans_x = this.transform.position.x + speedx * key;

        this.transform.position = new Vector3(trans_x, this.transform.position.y, this.transform.position.z);

    }
}
