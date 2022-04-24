using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    float moveSpeed = 0.0f;


    [SerializeField]
    Vector3 moveDirection = Vector3.zero; // ó������ �̷��� �ʱ�ȭ �Ѱǵ�, SerializeField ������ inspector �ȿ����� ������ �����ϴ�


    void Start()
    {

    }

    public void MoveTo(Vector3 direction)
    {
        moveDirection = direction;
    } // ��.....MoveTo���� ��ġ�� �� �������ְ�

    // Update is called once per frame
    void LateUpdate() // ���⼭ ��ġ�� �̵���Ű�� �ű�
    {

        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
}
