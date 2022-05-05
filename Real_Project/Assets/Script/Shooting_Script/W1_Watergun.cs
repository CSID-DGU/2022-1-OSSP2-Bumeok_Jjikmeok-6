using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W1_Watergun : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    GameObject watergun;

    [SerializeField]
    float gun_damage = 3;

    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().OnDie();
            Destroy(gameObject);
        }
        if (collision.CompareTag("Boss"))
        {
            collision.GetComponent<Boss>().TakeDamage(gun_damage);
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
