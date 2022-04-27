using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderPositionAutoSetter : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Vector3 distance = Vector3.up * 2f;
    // 적과 적의 슬라이더 HP 간의 거리 (네가 계산 해야한다)

    Transform targetTransform;
    RectTransform rectTransform;

    public int i = 0;

    public void Setup(Transform target)
    {
        targetTransform = target; // Slider UI가 쫓아다닐 target 설정
        rectTransform = GetComponent<RectTransform>(); // RectTansform 컴포넌트 정보 얻기
    }
    private void LateUpdate()
    {
        if (targetTransform == null) 
        {
            Destroy(gameObject);
            return;
        }
        // 적이 파괴되어 쫓아다닐 대상이 사라지면 slider UI도 삭제
        rectTransform.position = targetTransform.position + distance;
        // 아 시발 Movement2D 있었구나
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
