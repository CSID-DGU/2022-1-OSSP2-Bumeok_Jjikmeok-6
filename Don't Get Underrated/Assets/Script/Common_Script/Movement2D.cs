using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float moveSpeed = 0.0f;
    [SerializeField]
    private Vector3 moveDirection = Vector3.zero; // 처음에야 이렇게 초기화 한건데, SerializeField 때문에 inspector 안에서도 수정이 가능하다

    public float MoveSpeed
    {
        set { moveSpeed = value; }
        get { return moveSpeed; }
    }
    public Vector3 MoveDirection
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
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
}
