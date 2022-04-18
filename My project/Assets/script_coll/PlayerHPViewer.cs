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
        // �� �װ� ���ǹ� �Լ� --> �ڷ�ƾ�� ���� ���� ���� �ցٴµ�.... ������ ��� ü���� ���� �� ��� �޾ƾ� �Ѵٴ°�
        // �ﰢ������ Ȯ���� �ʿ䰡 �־ �����ϰ� update�� �ϴ� �� ����.
    }
}
