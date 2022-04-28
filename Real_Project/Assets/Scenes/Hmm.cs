using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hmm : MonoBehaviour
{
    // Start is called before the first frame update

    Movement2D movement2D;
    void Start()
    {
        
    }

    private void Awake()
    {
        movement2D = GetComponent<Movement2D>();
    }
    

    // Update is called once per frame
    void Update()
    {
        Debug.Log(transform.position);
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        movement2D.MoveTo(new Vector3(x, y, 0));
    }
}
