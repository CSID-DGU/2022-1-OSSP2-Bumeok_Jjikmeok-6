using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderPositionAutoSetter : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Vector3 distance = Vector3.down * 35.0f;
    Transform targetTransform;
    RectTransform rectTransform;

    public void Setup(Transform target)
    {
        targetTransform = target; // Slider UI�� �Ѿƴٴ� target ����
        rectTransform = GetComponent<RectTransform>(); // RectTansform ������Ʈ ���� ���
    }
    private void LateUpdate()
    {
        if  (targetTransform == null) // �ƴ� ����.... �ʹ� ����ݾ�...
            // targetTansform == null�̶�� ���� ���� ������� ���� ��ġ�� ���ٴ� ����.
            // �׷��� �и� ���� �����ϴ� �ڵ尡 �ְų�, ȭ�� ������ ������ �� ����Ǵ� �ڵ��ε�....
            // ���� �����ϴ°� 
        {
            Destroy(gameObject);
            return;

        } // ���� �ı��Ǿ� �Ѿƴٴ� ����� ������� slider UI�� ����
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(targetTransform.position);
        rectTransform.position = screenPosition + distance;
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
