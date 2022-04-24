using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Weapon : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    float damage = 9;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Playerrr"))
        {
            if (!collision.GetComponent<PlayerControl>().Unbeatable_Player)
            {
                collision.GetComponent<PlayerControl>().TakeDamage(damage);
            }
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
