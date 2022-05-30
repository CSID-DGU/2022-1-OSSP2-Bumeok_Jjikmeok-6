using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    protected GameObject Explosion;

    private Movement2D movement2D;

    private CameraShake cameraShake;

    protected ImageColor backGroundColor;

    private IEnumerator camera_shake;

    private IEnumerator back_ground_color_i;

    protected virtual void Awake()
    {
        if (TryGetComponent(out Movement2D M_2D))
            movement2D = M_2D;
        if (TryGetComponent(out CameraShake CS) && GameObject.Find("Main Camera"))
        {
            cameraShake = CS;
            if (GameObject.Find("Main Camera") && GameObject.Find("Main Camera").TryGetComponent(out Camera C))
                 cameraShake.mainCamera = C;
        }
    }
    public void W_MoveTo(Vector3 Origin)
    {
        if (movement2D != null)
            movement2D.MoveTo(Origin);
    }
    public void W_MoveSpeed(float Origin)
    {
        if (movement2D != null)
            movement2D.MoveSpeed = Origin;
    }
    public virtual void Weak_Weapon()
    {
        if (Explosion != null)
            Instantiate(Explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    protected void Start_Camera_Shake(float shake_intensity, float time_persist, bool is_Decline_Camera_Shake, bool is_Continue)
    {
        if (camera_shake != null)
            cameraShake.StopCoroutine(camera_shake);
        camera_shake = cameraShake.Shake_Act(shake_intensity, time_persist, is_Decline_Camera_Shake, is_Continue);
        cameraShake.StartCoroutine(camera_shake);
    }
    protected IEnumerator Start_Camera_Shake_For_Wait(float shake_intensity, float time_persist, bool is_Decline_Camera_Shake, bool is_Continue)
    {
        if (camera_shake != null)
            cameraShake.StopCoroutine(camera_shake);
        camera_shake = cameraShake.Shake_Act(shake_intensity, time_persist, is_Decline_Camera_Shake, is_Continue);
        yield return cameraShake.StartCoroutine(camera_shake);
    } // 되도록이면 사용 자제

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != null && collision.CompareTag("Student") && collision.gameObject.TryGetComponent(out Student S))
            Weak_Weapon();
    }
    protected void OnDestroy()
    {
        StopAllCoroutines();
    }
    protected void Flash(Color FlashColor, float wait_second, float time_persist)
    {
        if (backGroundColor != null)
        {
            if (back_ground_color_i != null)
                backGroundColor.StopCoroutine(back_ground_color_i);
            back_ground_color_i = backGroundColor.Flash(FlashColor, wait_second, time_persist);
            backGroundColor.StartCoroutine(back_ground_color_i);
        }
    }
}