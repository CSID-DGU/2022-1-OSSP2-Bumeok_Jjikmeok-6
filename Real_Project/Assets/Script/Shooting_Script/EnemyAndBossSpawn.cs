using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyAndBossSpawn : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    GameObject enemy;

    [SerializeField]
    GameObject enemyHPSliderPrefab;

    [SerializeField]
    GameObject Boss;

    [SerializeField]
    Transform canvasTransform;

    [SerializeField]
    TextMeshProUGUI BossWarningText;

    [SerializeField]
    GameObject BossHPSliderPrefab;

    [SerializeField]
    GameObject BossHPSliderLeft;

  


    int EnemyLimit = 10;

    private void Awake()
    {
        BossWarningText.color = new Color(BossWarningText.color.r, BossWarningText.color.g, BossWarningText.color.b, 0);
        BossHPSliderPrefab.SetActive(false);
        BossHPSliderLeft.SetActive(false);
    }

    public void Random_made()
    {
        StartCoroutine(Deley());
    }
    IEnumerator Deley()
    {  
        //yield return StartCoroutine(Enemy_Random_made());
        //yield return StartCoroutine(Warning());
        yield return StartCoroutine(First_Boss_Appear());
    }

    IEnumerator Enemy_Random_made()
    {
        yield return new WaitForSeconds(7f);
        int CNT = 0;
        while (true)
        {
            float yy = Random.Range(-4, 2);
            GameObject enemyClone = Instantiate(enemy, new Vector3(7.5f, yy, 1), Quaternion.identity);
            SpawnEnemyHPSlider(enemyClone);
            yield return new WaitForSeconds(0.9f);

            CNT++;
            if (CNT >= EnemyLimit)
            {
                yield break;
            }
        }
    }
    IEnumerator Warning()
    {
        for (int i = 0; i < 4; i++)
        {
            while (BossWarningText.color.a < 1.0f)
            {
                BossWarningText.color = new Color(BossWarningText.color.r, BossWarningText.color.g, BossWarningText.color.b, BossWarningText.color.a + Time.deltaTime * 4);
                yield return null;
            }
            while (BossWarningText.color.a > 0.0f)
            {
                BossWarningText.color = new Color(BossWarningText.color.r, BossWarningText.color.g, BossWarningText.color.b, BossWarningText.color.a - Time.deltaTime * 4);
                yield return null;
            }
        }
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
    
    private void SpawnEnemyHPSlider(GameObject e)
    {
        GameObject sliderClone = Instantiate(enemyHPSliderPrefab); // 적 체력을 나타내는 sloder UI 생성
        sliderClone.transform.SetParent(canvasTransform); // Slider UI 오브젝트를 parent("Canvas")의 자식으로 설정
        sliderClone.transform.localScale = Vector3.one; // 계층 설정으로 바뀐 크기를 다시 (1, 1, 1)로 설정
        sliderClone.GetComponent<SliderPositionAutoSetter>().Setup(e.transform);
        sliderClone.GetComponent<EnemyHPSliderViewer>().SetUp(e.GetComponent<Enemy>());
    }

    void Start()
    {
        Random_made();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
