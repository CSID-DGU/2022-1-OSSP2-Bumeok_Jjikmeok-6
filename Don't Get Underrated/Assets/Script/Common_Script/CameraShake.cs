using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Camera mainCamera;

    //[SerializeField]
    //[Range(0.01f, 1f)] float shakeRange = 0.5f;

    //[SerializeField]
    //[Range(0.1f, 1f)] float duration = 0.5f;

    private Vector3 originPosition;
    private Quaternion originRotation;

    public void Origin_Camera()
    {
        mainCamera.transform.position = new Vector3(0, 0, -10);
        mainCamera.transform.rotation = Quaternion.identity;
        mainCamera.transform.localScale = new Vector3(1, 1, 1);
    }

    public IEnumerator Shake_Act(float shake_intensity, float ratio, float time_persist, bool is_Continue)
    {
        originPosition = mainCamera.transform.position;
        originRotation = mainCamera.transform.rotation;
        while(true)
        {
            float percent = 0;
            while (percent < 1)
            {
                percent += Time.deltaTime / time_persist;
                mainCamera.transform.position = originPosition + Random.insideUnitSphere * shake_intensity;
                mainCamera.transform.localScale = new Vector3(transform.localScale.x + Time.deltaTime * ratio, transform.localScale.y + Time.deltaTime * ratio, 0);
                mainCamera.transform.transform.rotation = new Quaternion(
                                    originRotation.x + Random.Range(-shake_intensity, shake_intensity) * 0.2f,
                                    originRotation.y + Random.Range(-shake_intensity, shake_intensity) * 0.2f,
                                    originRotation.z + Random.Range(-shake_intensity, shake_intensity) * 0.2f,
                                    originRotation.w + Random.Range(-shake_intensity, shake_intensity) * 0.2f);
                yield return YieldInstructionCache.WaitForEndOfFrame;
            }
            if (!is_Continue)
            {
                mainCamera.transform.position = new Vector3(0, 0, -10);
                mainCamera.transform.rotation = Quaternion.identity;
                mainCamera.transform.localScale = new Vector3(1, 1, 1);
                yield return null;
                yield break;
            }
        }
    }
    // Update is called once per frame
    public void Trigger_Coroutine()
    {
        StopAllCoroutines();
    }
}

