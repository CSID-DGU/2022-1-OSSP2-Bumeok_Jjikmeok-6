using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { PowerUp = 0, HP}

public class Item : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    ItemType itemtype;
    Movement2D movement2D;

    private void Awake() // Awake �Լ��� ���ǵ��� ���� ������Ʈ�� �ҷ��� �� ���ȴ�.
    {
        movement2D = GetComponent<Movement2D>();

        float x = Random.Range(-1.0f, 1.0f);
        float y = Random.Range(-1.0f, 1.0f);

        movement2D.MoveTo(new Vector3(x, y, 0));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("��!");
            Use(collision.gameObject);
            Destroy(gameObject);
        }
    }
    public void Use(GameObject player)
    {
        switch (itemtype)
        {
            case ItemType.PowerUp:
                player.GetComponent<Weapon>().AttackLevel_I++;
                Debug.Log(player.GetComponent<Weapon>().AttackLevel_I);
                break;
            case ItemType.HP:
                player.GetComponent<PlayerHP>().CurrentHp += 2;
                break;
        }

    }

    void Start()
    {
        
    }

    // Update is called once per frame

}
