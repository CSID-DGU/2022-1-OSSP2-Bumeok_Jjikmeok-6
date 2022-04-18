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
        targetTransform = target; // Slider UI가 쫓아다닐 target 설정
        rectTransform = GetComponent<RectTransform>(); // RectTansform 컴포넌트 정보 얻기
    }
    private void LateUpdate()
    {
        if  (targetTransform == null) // 아니 씨발.... 너무 어렵잖아...
            // targetTansform == null이라는 뜻은 적이 사라져서 적의 위치가 없다는 뜻임.
            // 그러면 분명 적을 삭제하는 코드가 있거나, 화면 밖으로 나갔을 때 실행되는 코드인데....
            // 적을 삭제하는건 
        {
            Destroy(gameObject);
            return;

        } // 적이 파괴되어 쫓아다닐 대상이 사라지면 slider UI도 삭제
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
