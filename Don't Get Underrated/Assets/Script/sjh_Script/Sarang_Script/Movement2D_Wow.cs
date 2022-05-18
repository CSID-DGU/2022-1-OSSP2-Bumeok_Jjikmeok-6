using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D_Wow : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    float moveSpeed = 0.0f;


    [SerializeField]
    Vector3 moveDirection = Vector3.zero; // ó������ �̷��� �ʱ�ȭ �Ѱǵ�, SerializeField ������ inspector �ȿ����� ������ �����ϴ�

    public float MoveSpeed
    {
        set { moveSpeed = value; }
        get { return moveSpeed; }
    }


    public void MoveTo(Vector3 direction1)
    {
        moveDirection = direction1;
    } // ��.....MoveTo���� ��ġ�� �� �������ְ�
    
    // Update is called once per frame
    void LateUpdate() // ���⼭ ��ġ�� �̵���Ű�� �ű�
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Student") && collision.gameObject.GetComponent<Student_Move>().get_Color() == new Color(0, 0, 1, 1))
            Destroy(gameObject);
    }
}
