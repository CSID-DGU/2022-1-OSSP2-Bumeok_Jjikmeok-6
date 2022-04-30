using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hmm : MonoBehaviour
{
    // Start is called before the first frame update

    CameraShake cameraShake;


    private void Awake()
    {
        cameraShake = GetComponent<CameraShake>();
    }

    void Start()
    {
        transform.position = new Vector3(7, 4, 0);
        StartCoroutine("Move");
    }
    IEnumerator Move()
    {
        float temp = 0.4f;
        while (transform.position.y >= -4)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - (Mathf.Pow(5, temp) * Time.deltaTime), transform.position.z);
            temp += 0.02f;
            yield return null;
        }
        temp = 0.4f;
        cameraShake.Shake();
        while(transform.position.x >= -7 && transform.position.y <= 4)
        {
            transform.position = new Vector3(transform.position.x - (Mathf.Pow(5, temp) * Time.deltaTime), transform.position.y + (0.5714f * Mathf.Pow(5, temp) * Time.deltaTime), transform.position.z);
            temp += 0.01f;
            yield return null;
        }
        temp = 0.4f;
        cameraShake.Shake();
        while (transform.position.y >= -4)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - (Mathf.Pow(5, temp) * Time.deltaTime), transform.position.z);
            temp += 0.02f;
            yield return null;
        }
        temp = .4f;
        cameraShake.Shake();
        while (transform.position.x <= 7 && transform.position.y <= 4)
        {
            transform.position = new Vector3(transform.position.x + (Mathf.Pow(5, temp) * Time.deltaTime), transform.position.y + (0.5714f * Mathf.Pow(5, temp) * Time.deltaTime), transform.position.z);
            temp += 0.01f;
            yield return null;
        }
        cameraShake.Shake();
    }
    // Update is called once per frame
    void Update()
    {
        //Debug.Log(transform.position);
        //float x = Input.GetAxisRaw("Horizontal");
        //float y = Input.GetAxisRaw("Vertical");
        //movement2D.MoveTo(new Vector3(x, y, 0));
    }
}
