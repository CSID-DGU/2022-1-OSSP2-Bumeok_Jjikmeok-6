using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D : MonoBehaviour // 생명, 무기가 '이동'할 때 사용하는 스크립트. (리지드바디를 이용하여 이동할 때는 해당 스크립트를 사용하지 않는다.)
{
    [SerializeField]
    private float moveSpeed = 0.0f;

    [SerializeField]
    private Vector3 moveDirection = Vector3.zero;

    public float MoveSpeed // 생명, 무기의 이동 속도 설정
    {
        set { moveSpeed = value; }
        get { return moveSpeed; }
    }
    public Vector3 MoveDirection // 생명, 무기의 이동 방향 설정
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
        transform.position += moveDirection * moveSpeed * Time.deltaTime; // 이동 방향 + 이동 속도만큼 해당 물체들을 이동시킨다.
    }
}
