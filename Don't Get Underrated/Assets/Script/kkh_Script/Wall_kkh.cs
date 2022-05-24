using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall_kkh : MonoBehaviour
{
    public PhysicsMaterial2D bounceMat, normalMat;

    [SerializeField]
    public GameObject Player;

    private Rigidbody2D rigid2D;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        rigid2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Player)
        {
            rigid2D.sharedMaterial = bounceMat;
        }
        else
        {
            
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject == Player)
        {
            rigid2D.sharedMaterial = bounceMat;
        }
        else
        {

        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
       
    }
}
