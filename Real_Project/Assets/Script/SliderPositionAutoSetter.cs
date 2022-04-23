using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderPositionAutoSetter : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Vector3 distance = Vector3.up * 2f;
    // ���� ���� �����̴� HP ���� �Ÿ� (�װ� ��� �ؾ��Ѵ�)

    Transform targetTransform;
    RectTransform rectTransform;

    public int i = 0;

    public void Setup(Transform target)
    {
        targetTransform = target; // Slider UI�� �Ѿƴٴ� target ����
        rectTransform = GetComponent<RectTransform>(); // RectTansform ������Ʈ ���� ���
    }
    private void LateUpdate()
    {
        if (targetTransform == null) 
        {
            Destroy(gameObject);
            return;
        }
        // ���� �ı��Ǿ� �Ѿƴٴ� ����� ������� slider UI�� ����
        rectTransform.position = targetTransform.position + distance;
        // �� �ù� Movement2D �־�����
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
