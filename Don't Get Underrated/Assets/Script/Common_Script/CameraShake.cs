using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Camera mainCamera;

    private Vector3 originPosition;
    private Quaternion originRotation;


    public void Init_Camera()
    {
        mainCamera.transform.position = new Vector3(0, 0, -10);
        mainCamera.transform.rotation = Quaternion.identity;
        mainCamera.transform.localScale = new Vector3(1, 1, 1);
    }
    public IEnumerator Shake_Act(float shake_intensity, float time_persist, bool is_Decline_Camera_Shake, bool is_Continue)
    {
        float inverse_time_persist = StaticFunc.Reverse_Time(time_persist);
        Init_Camera();
        yield return YieldInstructionCache.WaitForSeconds(Time.deltaTime);
        originPosition = mainCamera.transform.position;
        originRotation = mainCamera.transform.rotation;
        float use_shake_intensity = shake_intensity;
        int decline_camera_Shake = 0;
        if (is_Decline_Camera_Shake)
            decline_camera_Shake = 1;
        while (true)
        {
            float percent = 0;
            while (percent < 1)
            {
                percent += Time.deltaTime * inverse_time_persist;
                use_shake_intensity -= shake_intensity * decline_camera_Shake * (Time.deltaTime * inverse_time_persist);
                mainCamera.transform.position = originPosition + Random.insideUnitSphere * use_shake_intensity;
                mainCamera.transform.localScale = new Vector3(transform.localScale.x + Time.deltaTime * use_shake_intensity, transform.localScale.y + Time.deltaTime * use_shake_intensity, 0);
                mainCamera.transform.transform.rotation = new Quaternion(
                                    originRotation.x + Random.Range(-use_shake_intensity, use_shake_intensity),
                                    originRotation.y + Random.Range(-use_shake_intensity, use_shake_intensity),
                                    originRotation.z + Random.Range(-use_shake_intensity, use_shake_intensity),
                                    originRotation.w + Random.Range(-use_shake_intensity, use_shake_intensity));
                yield return null;
            }
            if (!is_Continue)
            {
                Init_Camera();
                yield break;
            }
        }
    }
}

