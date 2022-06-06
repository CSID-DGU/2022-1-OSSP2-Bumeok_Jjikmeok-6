using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D : MonoBehaviour // ����, ���Ⱑ '�̵�'�� �� ����ϴ� ��ũ��Ʈ. (������ٵ� �̿��Ͽ� �̵��� ���� �ش� ��ũ��Ʈ�� ������� �ʴ´�.)
{
    [SerializeField]
    private float moveSpeed = 0.0f;

    [SerializeField]
    private Vector3 moveDirection = Vector3.zero;

    public float MoveSpeed // ����, ������ �̵� �ӵ� ����
    {
        set { moveSpeed = value; }
        get { return moveSpeed; }
    }
    public Vector3 MoveDirection // ����, ������ �̵� ���� ����
    {
        set { moveDirection = value;  }
        get { return moveDirection; }
    }
    public void MoveTo(Vector3 direction)
    {
        moveDirection = direction;
    } 

    void LateUpdate()
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime; // �̵� ���� + �̵� �ӵ���ŭ �ش� ��ü���� �̵���Ų��.
    }
}
