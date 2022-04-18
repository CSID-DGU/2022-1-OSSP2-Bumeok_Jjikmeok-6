using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    float damage = 5;
    [SerializeField]
    private int scorePoint = 100;
    PlayerController playerController;

    [SerializeField]
    GameObject explosionPrefab;

    [SerializeField]
    GameObject[] itemPrefabs;

    private void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        // �� ���� �׷��� �̰�.... �׷� �ǰ�?
        // 1. Player '������Ʈ' �� Player �±�(�ν����Ϳ��� ����)�� ����
        // 2. Player���� PlayerController�� ��ũ��Ʈ�� ����ִ�. �װ� �����ϴ°�
        // �������������������̹� �Ҹ� ���Ҵ�
        // ���� ������Ʈ �� Player �±׸� ���� �༮�� ã�� ��, PlayerController �Ӽ��� �ο�?
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerHP>().TakeDamage(damage);
            // ���� �׷��� �̰Ŵ� 
            // 1. �浹�� �Ϳ��� ��ȿ�ϴ�. (collision)
            // 2. �� �浹�� ���� '��ü'�� �����ϰ�, ���� �Լ��� ȣ��
            // 3. �׷��� �� �������� ���� ������ ����
            // ���� �ٵ� �̰� �����ϳİ�;;;
            // Ŭ���� ��ü�� �ٸ���(Enemy <-> PlayerHP)
            // ��� �� Ŭ�������� �ٸ� Ŭ������
            // ������ �ִ°� ���� �ǳİ�;;
            OnDie();
        }
    }
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnDie()
    {
        playerController.Score += scorePoint;
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        SpawnItem();
        Destroy(gameObject);
    }
    private void SpawnItem()
    {
        int spawnItem = Random.Range(0, 100);
        //if (spawnItem < 90)
        //{
        //    Instantiate(itemPrefabs[0], transform.position, Quaternion.identity);
        //}
        if (spawnItem < 70)
        {
            Instantiate(itemPrefabs[1].transform, transform.position, Quaternion.identity);
        }
    }
}
