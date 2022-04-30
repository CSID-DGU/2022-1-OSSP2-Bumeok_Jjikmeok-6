using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolGryn : MonoBehaviour
{
    // Start is called before the first frame update

    // Update is called once per frame

    [SerializeField]
    GameObject W1;
    void Update()
    {
        
    }
    CameraShake cameraShake;


    private void Awake()
    {
        cameraShake = GetComponent<CameraShake>();
    }

    void Start()
    {
        transform.position = new Vector3(7, 3, 0);
        StartCoroutine("Move");
    }
    IEnumerator Move()
    {
        float temp = 0.4f;
        while (transform.position.y >= -3)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - (Mathf.Pow(5, temp) * Time.deltaTime), transform.position.z);
            temp += 0.02f;
            yield return null;
        }
        temp = 0.4f;
        StartCoroutine(W1_Go());
        cameraShake.Shake();
        while (transform.position.x >= -7 && transform.position.y <= 3)
        {
            transform.position = new Vector3(transform.position.x - (Mathf.Pow(5, temp) * Time.deltaTime), transform.position.y + (0.4286f * Mathf.Pow(5, temp) * Time.deltaTime), transform.position.z);
            temp += 0.01f;
            yield return null;
        }
        temp = 0.4f;
        StartCoroutine(W1_Go());
        cameraShake.Shake();
        while (transform.position.y >= -3)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - (Mathf.Pow(5, temp) * Time.deltaTime), transform.position.z);
            temp += 0.02f;
            yield return null;
        }
        temp = .4f;
        StartCoroutine(W1_Go());
        cameraShake.Shake();
        while (transform.position.x <= 7 && transform.position.y <= 3)
        {
            transform.position = new Vector3(transform.position.x + (Mathf.Pow(5, temp) * Time.deltaTime), transform.position.y + (0.4286f * Mathf.Pow(5, temp) * Time.deltaTime), transform.position.z);
            temp += 0.01f;
            yield return null;
        }
        StartCoroutine(W1_Go());
        cameraShake.Shake();
    }
    IEnumerator W1_Go()
    {
        int count = 20;
        float intervalAngle = 360 / count;
        for (int i = 1; i <= count; i++)
        {
            GameObject clone = Instantiate(W1, transform.position, Quaternion.identity);
            float angle = intervalAngle * i;
            float x = Mathf.Cos(angle * Mathf.PI / 180.0f);
            float y = Mathf.Sin(angle * Mathf.PI / 180.0f);
            clone.GetComponent<Movement2D>().MoveSpeed = 8;
            clone.GetComponent<Movement2D>().MoveTo(new Vector3(x, y));
            yield return null;
        }
    }
}
