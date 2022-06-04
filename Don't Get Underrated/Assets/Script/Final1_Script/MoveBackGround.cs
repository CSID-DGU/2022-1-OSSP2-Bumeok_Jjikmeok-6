using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackGround : MonoBehaviour
{

    [SerializeField]
    GameObject Back_another;

    float move_value = 17.778f;

    Vector3 moveDirection = Vector3.left;

    IEnumerator increase_speed, decrease_speed;

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

    IEnumerator Increase_Speed(float time_persist, float Speed_Limit)
    {
        float temp_Speed = inGameSpeed;
        float percent = 0;
        float inverse_time_persist = StaticFunc.Reverse_Time(time_persist);
        while (percent < 1)
        {
            percent += Time.deltaTime * inverse_time_persist;
            inGameSpeed = Mathf.Lerp(temp_Speed, Speed_Limit, percent);
            yield return null;
        }
    }
    IEnumerator Decrease_Speed(float time_persist, float Speed_Limit)
    {
        float temp_Speed = inGameSpeed;
        float percent = 0;
        float inverse_time_persist = StaticFunc.Reverse_Time(time_persist);
        while (percent < 1)
        {
            percent += Time.deltaTime * inverse_time_persist;
            inGameSpeed = Mathf.Lerp(temp_Speed, Speed_Limit, percent);
            yield return null;
        }
    }
    public void Increase_Speed_Func(float time_persist, float Speed_Limit)
    {
        if (increase_speed != null)
            StopCoroutine(increase_speed);
        increase_speed = Increase_Speed(time_persist, Speed_Limit);
        StartCoroutine(increase_speed);
    }
    public void Decrease_Speed_Func(float time_persist, float Speed_Limit)
    {
        if (decrease_speed != null)
            StopCoroutine(decrease_speed);
        decrease_speed = Decrease_Speed(time_persist, Speed_Limit);
        StartCoroutine(decrease_speed);
    }

    public IEnumerator Increase_Speed_And_Wait(float time_persist, float Speed_Limit)
    {
        if (increase_speed != null)
            StopCoroutine(increase_speed);
        increase_speed = Increase_Speed(time_persist, Speed_Limit);
        yield return StartCoroutine(increase_speed);
    }
    public IEnumerator Decrease_Speed_And_Wait(float time_persist, float Speed_Limit)
    {
        if (decrease_speed != null)
            StopCoroutine(decrease_speed);
        decrease_speed = Decrease_Speed(time_persist, Speed_Limit);
        yield return StartCoroutine(decrease_speed);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += inGameSpeed * Time.deltaTime * moveDirection;
        if (transform.position.x <= -move_value)
            transform.position = Back_another.transform.position + (Vector3.right * move_value);
    }
}
