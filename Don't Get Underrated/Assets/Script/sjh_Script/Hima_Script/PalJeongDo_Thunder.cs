using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalJeongDo_Thunder : MonoBehaviour
{
    // Start is called before the first frame update

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void M1()
    {
        Debug.Log("��");
        transform.position = transform.position + new Vector3(0.32f, 0, 0);
    }
    public void M2()
    {
        Debug.Log("��");
        transform.position = transform.position + new Vector3(0.24f, 0, 0);
    }
    public void M3()
    {
        Debug.Log("��");
        transform.position = transform.position + new Vector3(0.26f, 0, 0);
    }
    public void Destroy_It()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(transform.position);
    }
}
