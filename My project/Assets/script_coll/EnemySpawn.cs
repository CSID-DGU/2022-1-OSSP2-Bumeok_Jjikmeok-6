using System.Collections;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    GameObject Enemy;

    [SerializeField]
    StageData stageData;

    [SerializeField]
    float dropTime;

    [SerializeField]
    GameObject enemyHPSliderPrefab;

    [SerializeField]
    Transform canvasTransform;

    [SerializeField]
    GameObject textBossWarning;

    [SerializeField]
    GameObject boss;

    [SerializeField]
    int maxEnemyCount = 100;

    private void Awake()
    {
        textBossWarning.SetActive(false);
        boss.SetActive(false);
    }

    void Start()
    {
        Auto_Enemy_Create_Stop(0);
    }
    public void Auto_Enemy_Create_Stop(int param)
    {
        if (param == 0)
            StartCoroutine("Jeokjeok");
        else
            StopCoroutine("Jeokjeok");
    }
    IEnumerator Jeokjeok()
    {
        int currentEnemyCount = 0;
        while (true)
        {
            float positionX = Random.Range(stageData.LimitMin.x, stageData.LimitMax.x);
            GameObject enemyClone = Instantiate(Enemy, new Vector3(positionX, stageData.LimitMax.y + 1.0f, 0.0f), Quaternion.identity);
            SpawnEnemyHPSlider(enemyClone);
            yield return new WaitForSeconds(dropTime);
            currentEnemyCount++;
            if (currentEnemyCount == maxEnemyCount)
            {
                StartCoroutine("SpawnBoss");
                yield break;
            }
            yield return new WaitForSeconds(dropTime);
        }

    }
    IEnumerator SpawnBoss()
    {
        textBossWarning.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        textBossWarning.SetActive(false);
        boss.SetActive(true);
        boss.GetComponent<Boss>().ChangeState(BossState.MoveToAppearPoint);
    }
    private void SpawnEnemyHPSlider(GameObject enemy)
    {
        GameObject sliderClone = Instantiate(enemyHPSliderPrefab); // 적 체력을 나타내는 sloder UI 생성
        sliderClone.transform.SetParent(canvasTransform); // Slider UI 오브젝트를 parent("Canvas")의 자식으로 설정
        sliderClone.transform.localScale = Vector3.one; // 계층 설정으로 바뀐 크기를 다시 (1, 1, 1)로 설정
        sliderClone.GetComponent<SliderPositionAutoSetter>().Setup(enemy.transform);
        sliderClone.GetComponent<EnemyHPViewer>().Setup(enemy.GetComponent<EnemyHP>());
    }
    // Update is called once per frame
    void Update()
    {

    }
}
