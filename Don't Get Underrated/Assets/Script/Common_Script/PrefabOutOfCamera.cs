using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabOutOfCamera : MonoBehaviour
{
    // Start is called before the first frame update

    private Camera selectedCamera;


    private void Start()
    {
        selectedCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }
    // Update is called once per frame
    private void Update()
    {
        Vector3 screenPoint = selectedCamera.WorldToViewportPoint(transform.position);
        bool OnScreen = screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

        if (!OnScreen)
            Destroy(gameObject);
    }
}