using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor_Button : MonoBehaviour // 각 층마다 윗층, 아랫층 이동에 대한 버튼의 위치를 재설정해주는 스크립트
{
    public void Start_Down_Or_UP(bool is_Down, float Change_X, float Change_Y)
    {
        if (is_Down && gameObject.activeInHierarchy)
            transform.position = new Vector3(Change_X - 4, Change_Y, 0);
        else
            transform.position = new Vector3(Change_X + 4, Change_Y, 0);
    }
}