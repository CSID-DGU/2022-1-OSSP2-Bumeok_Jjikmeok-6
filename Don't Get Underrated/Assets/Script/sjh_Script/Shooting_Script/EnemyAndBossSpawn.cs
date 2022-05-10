using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAndBossSpawn : MonoBehaviour
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
    void Start()
    {
        StartCoroutine(Deley());
    }
    IEnumerator Deley()
    {  
        yield return StartCoroutine(First_Boss_Appear());
    }
   
    IEnumerator First_Boss_Appear()
    {
        
        yield return StartCoroutine(Boss.GetComponent<DoPhan>().Position_Lerp(Boss.transform.position, new Vector3(6.3f, Boss.transform.position.y, Boss.transform.position.z), 0.4f, OriginCurve));
        BossHPSliderPrefab.SetActive(true);
        BossHPSliderLeft.SetActive(true);
        BossHPSliderPrefab.GetComponent<BossHPSliderViewer>().F_HPFull(Boss.GetComponent<DoPhan>());
        yield return new WaitForSeconds(2f);
        Boss.GetComponent<DoPhan>().Phase_Start();
    }
 

    // Update is called once per frame
    void Update()
    {
        
    }
}
