using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAndBossSpawn : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    GameObject enemy;

    [SerializeField]
    GameObject enemyHPSliderPrefab;

    [SerializeField]
    Transform canvasTransform;

    public void Random_made()
    {
        StartCoroutine("Enemy_Random_made");
    }
    IEnumerator Enemy_Random_made()
    {
        yield return new WaitForSeconds(7f);
        while(true)
        {
            float yy = Random.Range(-4, 2);
            GameObject enemyClone = Instantiate(enemy, new Vector3(7.5f, yy, 1), Quaternion.identity);
            SpawnEnemyHPSlider(enemyClone);
            yield return new WaitForSeconds(0.5f);
        }
    }
    private void SpawnEnemyHPSlider(GameObject e)
    {
        GameObject sliderClone = Instantiate(enemyHPSliderPrefab); // �� ü���� ��Ÿ���� sloder UI ����
        sliderClone.transform.SetParent(canvasTransform); // Slider UI ������Ʈ�� parent("Canvas")�� �ڽ����� ����
        sliderClone.transform.localScale = Vector3.one; // ���� �������� �ٲ� ũ�⸦ �ٽ� (1, 1, 1)�� ����
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
