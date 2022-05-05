using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyAndBossSpawn : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    GameObject Boss;


    [SerializeField]
    GameObject BossHPSliderPrefab;

    [SerializeField]
    GameObject BossHPSliderLeft;

    private void Awake()
    {
        BossHPSliderPrefab.SetActive(false);
        BossHPSliderLeft.SetActive(false);
    }
    void Start()
    {
        Random_made();
    }

    public void Random_made()
    {
        StartCoroutine(Deley());
    }
    IEnumerator Deley()
    {  
        yield return StartCoroutine(First_Boss_Appear());
    }
   
    IEnumerator First_Boss_Appear()
    {
        while (true)
        {
            if (Boss == null)
                break;
            Boss.transform.position += Vector3.left * (Time.deltaTime * 4f);
            yield return null;
            if (Boss.transform.position.x <= 6.3f)
            {
                BossHPSliderPrefab.SetActive(true);
                BossHPSliderLeft.SetActive(true);
                BossHPSliderPrefab.GetComponent<BossHPSliderViewer>().F_HPFull(Boss.GetComponent<Boss>());
                yield return new WaitForSeconds(2f);
                Boss.GetComponent<Boss>().Phase_Start();
                yield break;
            }
            yield return null;
        }
    }
 

    // Update is called once per frame
    void Update()
    {
        
    }
}
