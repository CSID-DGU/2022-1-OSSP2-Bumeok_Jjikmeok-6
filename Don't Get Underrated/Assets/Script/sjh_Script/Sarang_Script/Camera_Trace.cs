using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Trace : MonoBehaviour // 이건 카메라가 플레이어 추적하는 코드
{
    // Start is called before the first frame update

    private GameObject player;

    private bool Floor_On;

    private void Awake()
    {
        Floor_On = true;
    }
    private void Update()
    {
        if (Floor_On)
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
    public void Final_Walk_Floor(int Floor_Num)
    {
        transform.position = new Vector3(0, 40 * (Floor_Num - 1), -10);
        Floor_On = true;
    }
}
