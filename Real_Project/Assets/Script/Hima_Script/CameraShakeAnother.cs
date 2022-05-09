using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeAnother : MonoBehaviour
{

    Vector3 Origin = new Vector3(0, 0, -10);

    [SerializeField]
    [Range(0.01f, 1f)] float shakeRange = 0.5f;

    [SerializeField]
    [Range(0.1f, 1f)] float duration = 0.5f;

    private Vector3 originPosition;
    private Quaternion originRotation;

    //private float shake_decay = 0.002f;

    public void Shake()
    {
        InvokeRepeating("StartShake", 0f, 0.005f);
        Invoke("StopShake", duration);
    }
    void StartShake()
    {
        float cameraPosX = Random.value * shakeRange * 2 - shakeRange;
        float cameraPosY = Random.value * shakeRange * 2 - shakeRange;
        Vector3 cameraPos =transform.position;
        cameraPos.x += cameraPosX;
        cameraPos.y += cameraPosY;
        transform.position = cameraPos;
    }
    void StopShake()
    {
        CancelInvoke("StartShake");
        transform.position = Origin;
    }
    public void Use_Shake(float shake_intensity, float ratio)
    {
        StartCoroutine(Shake_Act(shake_intensity, ratio));
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    public IEnumerator Shake_Act(float shake_intensity, float ratio)
    {
        originPosition = transform.position;
        originRotation = transform.rotation;
        while (true)
        {
            float percent = 0;
            while (percent < 1)
            {
                percent += Time.deltaTime;
                transform.position = originPosition + Random.insideUnitSphere * shake_intensity;
                transform.localScale = new Vector3(transform.localScale.x + Time.deltaTime * ratio, transform.localScale.y + Time.deltaTime * ratio, 0);
                transform.transform.rotation = new Quaternion(
                                    originRotation.x + Random.Range(-shake_intensity, shake_intensity) * 0.2f,
                                    originRotation.y + Random.Range(-shake_intensity, shake_intensity) * 0.2f,
                                    originRotation.z + Random.Range(-shake_intensity, shake_intensity) * 0.2f,
                                    originRotation.w + Random.Range(-shake_intensity, shake_intensity) * 0.2f);
                yield return YieldInstructionCache.WaitForEndOfFrame;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

