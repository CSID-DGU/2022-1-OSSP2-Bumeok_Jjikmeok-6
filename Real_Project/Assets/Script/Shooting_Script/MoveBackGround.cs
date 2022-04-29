using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackGround : MonoBehaviour
{

    [SerializeField]
    GameObject Back_another;

    float move_value = 17.69f;

    // Start is called before the first frame update

    void Start()
    {
        StartCoroutine("Move_BackGround");
    }
    IEnumerator Move_BackGround()
    {
        while(true)
        {
            transform.position += Vector3.left * (Time.deltaTime * 5);
            yield return null;
            if (transform.position.x <= -move_value)
            {
                transform.position = Back_another.transform.position + (Vector3.right * move_value);
            }
        }
     
    }

    // Update is called once per frame
    void Update()
    {

    }
}
