using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalStage_1_Total : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    GameObject Boss;

    [SerializeField]
    GameObject BossHPSliderPrefab;

    [SerializeField]
    GameObject BossHPSliderLeft;

    [SerializeField]
    AnimationCurve OriginCurve;

    private void Awake()
    {
        BossHPSliderPrefab.SetActive(false);
        BossHPSliderLeft.SetActive(false);
    }

    public void Boss_First_Appear() // 플레이어 --> 보스
    {
        Boss.transform.position = new Vector3(0, -7, 0);
        BossHPSliderPrefab.SetActive(true);
        BossHPSliderPrefab.GetComponent<BossHPSliderViewer>().F_HPFull(Boss.GetComponent<DoPhan>());
        BossHPSliderLeft.SetActive(true);
        Boss.GetComponent<DoPhan>().Phase_Start();

    }
}
