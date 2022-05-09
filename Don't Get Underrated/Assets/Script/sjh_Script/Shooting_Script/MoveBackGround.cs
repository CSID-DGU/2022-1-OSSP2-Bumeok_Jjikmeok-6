using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackGround : MonoBehaviour
{

    [SerializeField]
    GameObject Back_another;

    float move_value = 17.69f;

    Vector3 moveDirection = Vector3.left;

    float moveSpeed = 6f;

    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }

    // Start is called before the first frame update

    void Start()
    {
        
    }
    //IEnumerator Move_BackGround()
    //{
    //    while(true)
    //    {
    //        transform.position += Vector3.left * (Time.deltaTime * 5);
    //        yield return null;
    //        if (transform.position.x <= -move_value)
    //        {
    //            transform.position = Back_another.transform.position + (Vector3.right * move_value);
    //        }
    //    }
     
    //}

    // Update is called once per frame
    void Update()
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
        if (transform.position.x <= -move_value)
        {
            transform.position = Back_another.transform.position + (Vector3.right * move_value);
        }
    }
}
