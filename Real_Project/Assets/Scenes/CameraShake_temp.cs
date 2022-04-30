using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake_temp : MonoBehaviour
{
    public Camera mainCamera;

    Vector3 Origin = new Vector3(0, 0, -10);

    [SerializeField]
    [Range(0.01f, 1f)] float shakeRange = 0.5f;

    [SerializeField]
    [Range(0.1f, 1f)] float duration = 0.5f;

    public void Shake()
    {
        InvokeRepeating("StartShake", 0f, 0.005f);
        Invoke("StopShake", duration);
    }
    void StartShake()
    {
        float cameraPosX = Random.value * shakeRange * 2 - shakeRange;
        float cameraPosY = Random.value * shakeRange * 2 - shakeRange;
        Vector3 cameraPos = mainCamera.transform.position;
        cameraPos.x += cameraPosX;
        cameraPos.y += cameraPosY;
        mainCamera.transform.position = cameraPos;
    }
    void StopShake()
    {
        CancelInvoke("StartShake");
        mainCamera.transform.position = Origin;
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
