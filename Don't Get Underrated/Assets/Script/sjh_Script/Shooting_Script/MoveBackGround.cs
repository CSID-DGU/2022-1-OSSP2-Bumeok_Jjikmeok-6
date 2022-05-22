using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackGround : MonoBehaviour
{

    [SerializeField]
    GameObject Back_another;

    float move_value = 17.69f;

    Vector3 moveDirection = Vector3.left;

    [SerializeField]
    float moveSpeed= 9f;

    float inGameSpeed;

    public float InGameSpeed
    {
        get { return inGameSpeed; }
        set { inGameSpeed = value; }
    }
    private void Awake()
    {
        inGameSpeed = 0;   
    }

    public IEnumerator Increase_Speed(float time_persist, float Speed_Limit)
    {
        while(true)
        {
            inGameSpeed += Time.deltaTime / time_persist;
            if (inGameSpeed >= Speed_Limit)
                yield break;
            yield return null;
        }
    }

    public IEnumerator Decrease_Speed(float time_persist, float Speed_Limit)
    {
        while (true)
        {
            inGameSpeed -= Time.deltaTime / time_persist;
            if (inGameSpeed <= Speed_Limit)
                yield break;
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += moveDirection * inGameSpeed * Time.deltaTime;
        if (transform.position.x <= -move_value)
        {
            transform.position = Back_another.transform.position + (Vector3.right * move_value);
        }
    }
}
