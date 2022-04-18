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
        // 와 씨발 그러면 이게.... 그런 건가?
        // 1. Player '오브젝트' 중 Player 태그(인스펙터에서 수정)를 선택
        // 2. Player에는 PlayerController의 스크립트가 들어있다. 그걸 선택하는거
        // 씨이이이이이이이이이발 소름 돋았다
        // 게임 오브젝트 중 Player 태그를 가진 녀석을 찾은 후, PlayerController 속성을 부여?
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerHP>().TakeDamage(damage);
            // 씨발 그러면 이거는 
            // 1. 충돌한 것에만 유효하다. (collision)
            // 2. 그 충돌한 것의 '객체'를 생성하고, 그의 함수를 호출
            // 3. 그러면 그 곳에서도 값에 영향이 갈터
            // 씨발 근데 이게 가능하냐고;;;
            // 클래스 자체가 다른데(Enemy <-> PlayerHP)
            // 어느 한 클래스에서 다른 클래스에
            // 영향을 주는게 말이 되냐고;;
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
