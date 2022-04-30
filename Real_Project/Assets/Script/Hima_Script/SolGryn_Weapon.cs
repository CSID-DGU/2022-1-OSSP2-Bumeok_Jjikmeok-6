using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolGryn_Weapon : MonoBehaviour
{
    // Start is called before the first frame update

    public void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<HimaController>().Die();
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
