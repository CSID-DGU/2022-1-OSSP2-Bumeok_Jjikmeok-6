using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    float moveSpeed = 0.0f;


    [SerializeField]
    Vector3 moveDirection = Vector3.zero; // 처음에야 이렇게 초기화 한건데, SerializeField 때문에 inspector 안에서도 수정이 가능하다


    void Start()
    {

    }

    public void MoveTo(Vector3 direction)
    {
        moveDirection = direction;
    } // 흠.....MoveTo에서 위치를 딱 설정해주고

    // Update is called once per frame
    void LateUpdate() // 여기서 위치를 이동시키는 거군
    {

        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
}
