using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor_Button : MonoBehaviour // �� ������ ����, �Ʒ��� �̵��� ���� ��ư�� ��ġ�� �缳�����ִ� ��ũ��Ʈ
{
    public void Start_Down_Or_UP(bool is_Down, float Change_X, float Change_Y)
    {
        if (is_Down && gameObject.activeInHierarchy)
            transform.position = new Vector3(Change_X - 4, Change_Y, 0);
        else
            transform.position = new Vector3(Change_X + 4, Change_Y, 0);
    }
}