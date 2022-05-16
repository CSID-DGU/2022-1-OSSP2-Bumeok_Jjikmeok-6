using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Devil_Weapon : MonoBehaviour
{

    [SerializeField]
    GameObject Devil_Explosion;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Playerrr"))
        {
            if (!collision.GetComponent<Player_Info>().Unbeatable)
                Weak_Weapon();
            collision.GetComponent<Player_Info>().TakeDamage(1);
        }
        if (collision.CompareTag("Player"))
        {
            if (!collision.GetComponent<Player_Info>().Unbeatable)
                Weak_Weapon();
            collision.GetComponent<Player_Info>().TakeDamage(1);
        }
    }
    public void Weak_Weapon()
    {
        if (Devil_Explosion != null)
        {
            Instantiate(Devil_Explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
