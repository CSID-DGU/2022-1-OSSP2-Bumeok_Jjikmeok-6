using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPViewer : MonoBehaviour
{
    [SerializeField]
    PlayerHP playerHP;

    [SerializeField]
    Slider sliderHP;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        sliderHP.value = playerHP.CurrentHp / playerHP.MaxHp;
        // 뭐 네가 조건문 함수 --> 코루틴을 만들어서 해줄 수는 있곘는데.... 어차피 사람 체력은 닿을 때 계속 달아야 한다는걸
        // 즉각적으로 확인할 필요가 있어서 안전하게 update로 하는 것 같다.
    }
}
