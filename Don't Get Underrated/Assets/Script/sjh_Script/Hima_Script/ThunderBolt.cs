using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderBolt : Weapon_Devil
{
    // Start is called before the first frame update

    BackGroundColor backGroundColor;
    CameraShake cameraShake;
    IEnumerator camera_shake;
    private new void Awake()
    {
        base.Awake();
        backGroundColor = GameObject.Find("Flash").GetComponent<BackGroundColor>();
        cameraShake = GetComponent<CameraShake>();
        cameraShake.mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }
    void Start()
    {
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        //yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(transform.position.x, 0, 0), 0.5f, OriginCurve));
        StartCoroutine(backGroundColor.Flash(new Color(1, 1, 1, 1), 0.1f, 7));
        camera_shake = cameraShake.Shake_Act(.1f, .26f, 0.3f, false);
        cameraShake.StartCoroutine(camera_shake);
        
        yield return null;
    }

    public void DestroyNow()
    {
        if (camera_shake != null)
            cameraShake.StopCoroutine(camera_shake);
        Destroy(gameObject);
    }
}
