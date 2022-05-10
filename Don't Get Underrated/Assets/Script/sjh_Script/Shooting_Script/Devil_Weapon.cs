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
            if (!collision.GetComponent<PlayerControl>().Unbeatable_Player)
                OffLife();
            collision.GetComponent<PlayerControl>().TakeDamage();
        }
    }
    public void OffLife()
    {
        Instantiate(Devil_Explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
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
