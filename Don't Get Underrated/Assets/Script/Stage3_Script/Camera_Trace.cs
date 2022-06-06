using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Trace : MonoBehaviour // 카메라가 플레이어를 추적하는 스크립트 (메인 카메라에 적용)
{
    private GameObject player;

    private bool Floor_On;

    private void Awake()
    {
        Floor_On = true;
    }
    private void Update()
    {
        if (Floor_On) // 플레이어가 복도에 있을 때만 추적. 아닐 때, 즉 계단을 오르고 있을 때는 일시적으로 추적 멈춤
        {
            if (player)
            {
                Vector3 newPos = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
                transform.position = newPos;
            }
            else
                player = GameObject.FindGameObjectWithTag("Player");
        }
    }
    public void DoNot_Trace_Player()
    {
        Floor_On = false;
    }
    public void Trace_Player(int Floor_Num)
    {
        transform.position = new Vector3(0, 40 * (Floor_Num - 1), -10);
        Floor_On = true;
    }
}
