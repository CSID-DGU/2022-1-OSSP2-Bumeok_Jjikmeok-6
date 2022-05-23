using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    float moveSpeed = 0.0f;

    bool is_Blink = false;
    public bool Is_Blink
    {
        get { return is_Blink; }
        set { is_Blink = value; }
    }
    [SerializeField]
    Vector3 moveDirection = Vector3.zero; // ó������ �̷��� �ʱ�ȭ �Ѱǵ�, SerializeField ������ inspector �ȿ����� ������ �����ϴ�

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
    } // ��.....MoveTo���� ��ġ�� �� �������ְ�

    void LateUpdate() // ���⼭ ��ġ�� �̵���Ű�� �ű�
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != null && collision.CompareTag("Student") && collision.gameObject.TryGetComponent(out Student user1))
        {
            if (user1.get_Color() == new Color(0, 0, 1, 1))
                Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject != null && collision.gameObject.CompareTag("Player") && collision.gameObject.TryGetComponent(out HimaController user2))
        {
            user2.TakeDamage(1);
        }
    }
}
