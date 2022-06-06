using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour // 무기 최상위 클래스
{
    [SerializeField]
    protected GameObject Explosion;

    [SerializeField]
    AudioClip[] BackGroundSource_Collect;

    [SerializeField]
    AudioClip[] EffectSource_Collect;

    private Movement2D movement2D;

    private CameraShake cameraShake;

    private IEnumerator camera_shake;

    private IEnumerator back_ground_color_i;

    protected ImageColor backGroundColor;

    protected SpriteRenderer spriteRenderer;

    protected AudioSource BackGroundSource;

    protected AudioSource EffectSource;

    // 무기 역시 카메라를 흔들 수 있고, 배경 색상을 변경할 수 있으며, 적절한 효과음 및 배경음을 사용할 수 있다.

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
        if (TryGetComponent(out SpriteRenderer SR))
            spriteRenderer = SR;
    }
    public void W_MoveTo(Vector3 Origin) // 무기가 움직이는 방향 (movement2D와 연계)
    {
        if (movement2D != null)
            movement2D.MoveTo(Origin);
    }
    public void W_MoveSpeed(float Origin) // 무기가 움직이는 속도 (movement2D와 연계)
    {
        if (movement2D != null)
            movement2D.MoveSpeed = Origin;
    }
    public virtual void Weak_Weapon() // 무기 파괴. 무기가 사라질 때의 이펙트(Explosion)가 있으면 이를 생성하고 파괴
    {
        if (Explosion != null)
            Instantiate(Explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision) // (스테이지 3 한정) 학생(Student)을 타겟팅으로 무기를 발사했을 때는
                                                                  // 적, 플레이어의 무기 모두 사라져야 하기 때문에 설정한 코드
    {
        if (collision.gameObject != null && collision.CompareTag("Student") && collision.gameObject.TryGetComponent(out Student S))
            Weak_Weapon();
    }
    protected void OnDestroy()
    {
        StopAllCoroutines();
    }

    // 하단의 함수에 대한 설명은 Life.cs 참고
    protected void Camera_Shake(float shake_intensity, float time_persist, bool is_Decline_Camera_Shake, bool is_Continue)
    {
        if (cameraShake != null)
        {
            if (camera_shake != null)
                cameraShake.StopCoroutine(camera_shake);
            camera_shake = cameraShake.Shake_Act(shake_intensity, time_persist, is_Decline_Camera_Shake, is_Continue);
            cameraShake.StartCoroutine(camera_shake);
        }
    }
    protected IEnumerator Camera_Shake_And_Wait(float shake_intensity, float time_persist, bool is_Decline_Camera_Shake, bool is_Continue)
    {
        if (cameraShake != null)
        {
            if (camera_shake != null)
                cameraShake.StopCoroutine(camera_shake);
            camera_shake = cameraShake.Shake_Act(shake_intensity, time_persist, is_Decline_Camera_Shake, is_Continue);
            yield return cameraShake.StartCoroutine(camera_shake);
        }
        else
            yield return null;
    } // 되도록이면 사용 자제

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
    protected void Effect_Sound_Play(int index)
    {
        if (EffectSource_Collect.Length <= index)
            return;
        if (EffectSource != null && EffectSource_Collect[index] != null)
        {
            EffectSource.clip = EffectSource_Collect[index];
            EffectSource.Play();
        }
    }
    protected void Effect_Sound_Stop()
    {
        if (EffectSource != null)
            EffectSource.Stop();
    }
    protected void Effect_Sound_OneShot(int index)
    {
        if (EffectSource_Collect.Length <= index)
            return;
        if (EffectSource != null && EffectSource_Collect[index] != null)
            EffectSource.PlayOneShot(EffectSource_Collect[index]);
    }

    protected void Effect_Sound_Pause()
    {
        if (EffectSource != null)
            EffectSource.Pause();
    }
    protected void BackGround_Sound_Start(int index)
    {
        if (BackGroundSource_Collect.Length <= index)
            return;
        if (BackGroundSource != null && BackGroundSource_Collect[index] != null)
        {
            BackGroundSource.clip = BackGroundSource_Collect[index];
            BackGroundSource.Play();
        }
    }
    protected void BackGround_Sound_Stop()
    {
        if (BackGroundSource != null)
            BackGroundSource.Stop();
    }
    protected void BackGround_Sound_Pause()
    {
        if (BackGroundSource != null)
            BackGroundSource.Pause();
    }
}