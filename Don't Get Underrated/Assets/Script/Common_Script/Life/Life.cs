using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Set_Slerp_Move
{
    private Vector3 next_Position;
    private string dir;
    public Set_Slerp_Move(Vector3 _next_Position, string _dir)
    {
        next_Position = _next_Position;
        dir = _dir;
    }
    public Vector3 Next_Position => next_Position;
    public string Dir => dir;
}
public class Life : MonoBehaviour, Life_Of_Basic // 생명에 관련된 클래스
{
    [SerializeField]
    protected GameObject[] Weapon; // 모든 생명은 무기를 사용할 수 있다.

    [SerializeField]
    protected AnimationCurve declineCurve;

    [SerializeField]
    protected AnimationCurve OriginCurve;

    [SerializeField]
    protected AnimationCurve inclineCurve;

    [SerializeField]
    protected AnimationCurve De_In_Curve;

    // 모든 생명은 4개의 애니메이션 커브에 따라 다채로운 행동을 할 수 있다.

    [SerializeField]
    protected GameObject When_Dead_Effect; // 생명이 죽었을 때의 이펙트

    [SerializeField]
    protected StageData stageData; // 생명이 특정 영역에서 벗어나지 않도록 영역 설정

    [SerializeField]
    AudioClip[] BackGroundSource_Collect = null;

    [SerializeField]
    AudioClip[] EffectSource_Collect = null;

    // 각 생명이 사용하는 배경음, 효과음들(배열)

    [SerializeField]
    Vector2 Stage3_Min_Limit;

    [SerializeField]
    Vector2 Stage3_Max_Limit;

    // 영역에서 벗어나지 않는 영역을 값으로 설정

    protected SpriteRenderer spriteRenderer;

    protected Movement2D movement2D; // 움직임을 관장하는 스크립트 (리지드바디를 이용하여 움직일 때는 해당 스크립트 사용 X)

    protected ImageColor imageColor; // 모든 생명은 배경색을 조절할 수 있다.

    private CameraShake cameraShake; // 모든 생명은 카메라를 흔드는 효과를 쓸 수 있다.

    private IEnumerator camera_shake_i;

    private IEnumerator back_ground_color_i;

    protected AudioSource BackGroundSource;

    protected AudioSource EffectSource;

    private bool unbeatable;

    public bool Unbeatable
    {
        get { return unbeatable; }
        set { unbeatable = value; }
    } // 무적 상태
    protected virtual void Awake()
    {
        if (TryGetComponent(out SpriteRenderer SR))
            spriteRenderer = SR;
        if (TryGetComponent(out CameraShake CS))
            cameraShake = CS;
        if (cameraShake != null && GameObject.Find("Main Camera") && GameObject.Find("Main Camera").TryGetComponent(out Camera camera))
            cameraShake.mainCamera = camera;

        Unbeatable = false;

        if (stageData != null)
        {
            stageData.LimitMin = Stage3_Min_Limit;
            stageData.LimitMax = Stage3_Max_Limit;
        }
    }
    public Color My_Color // 생명의 색깔
    {
        get { return spriteRenderer.color; }
        set { spriteRenderer.color = value; }
    }
    public Vector3 My_Scale // 생명의 크기
    {
        get { return transform.localScale; }
        set { transform.localScale = value; }
    }
    public Vector3 My_Position // 생명의 위치
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    protected void Return_To_My_Origin_Color()
    {
        My_Color = Color.white;
    } // 색상을 본래 색상으로 변경 (현재는 보스가 죽었을 때만 적용하였음)

    protected void Effect_Sound_Play(int index) // 효과음 재생 함수 (무한 반복)
    {
        if (EffectSource_Collect.Length <= index)
            return;
        if (EffectSource != null && EffectSource_Collect[index] != null)
        {
            EffectSource.Stop();
            EffectSource.clip = EffectSource_Collect[index];
            EffectSource.Play();
        }
    }
    protected void Effect_Sound_Stop() // 효과음 중지 함수
    {
        if (EffectSource != null)
            EffectSource.Stop();
    }
    protected void Effect_Sound_Pause() // 효과음 일시 중지 함수
    {
        if (EffectSource != null)
            EffectSource.Pause();
    }
    protected void Effect_Sound_OneShot(int index) // 효과음 재생 함수 (한번만)
    {
        if (EffectSource_Collect.Length <= index)
            return;
        if (EffectSource != null && EffectSource_Collect[index] != null)
            EffectSource.PlayOneShot(EffectSource_Collect[index]);
    }
    protected void BackGround_Sound_Play(int index) // 배경음 재생 함수 (무한 반복)
    {
        if (BackGroundSource_Collect.Length <= index)
            return;
        if (BackGroundSource != null && BackGroundSource_Collect[index] != null)
        {
            BackGroundSource.Stop();
            BackGroundSource.clip = BackGroundSource_Collect[index];
            BackGroundSource.Play();
        }
    }
    protected void BackGround_Sound_Stop() // 배경음 중지 함수
    {
        if (BackGroundSource != null)
            BackGroundSource.Stop();
    }
    protected void BackGround_Sound_Pause() // 배경음 일시 중지 함수
    {
        if (BackGroundSource != null)
            BackGroundSource.Pause();
    }

    protected IEnumerator Decrease_BackGround_Sound(float time_persist) // 배경음을 time_persist(초)만큼 감소시키는 함수
    {
        if (BackGroundSource != null)
        {
            if (BackGroundSource.isPlaying)
            {
                float inverse_time_persist = StaticFunc.Reverse_Time(time_persist);
                while (BackGroundSource.volume > 0)
                {
                    BackGroundSource.volume -= Time.deltaTime * inverse_time_persist;
                    yield return null;
                }
                yield return null;
            }
            else
                yield return null;
        }
        else
            yield return null;
    }

    public virtual void TakeDamage(float damage)  // 모든 생명은 데미지를 입는다. (생명이 실수치일 수도, 정수치일 수도 있으며, 이에 따른 함수를 하위 클래스에서
        // 재정의 해야하기 때문에 상위 클래스에서는 데미지를 입었을 때 바로 죽는 것으로 설정)
    {
        if (When_Dead_Effect != null)
            Instantiate(When_Dead_Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    public virtual void TakeDamage(int damage) // 모든 생명은 데미지를 입는다. (생명이 실수치일 수도, 정수치일 수도 있으며, 이에 따른 함수를 하위 클래스에서
        // 재정의 해야하기 때문에 상위 클래스에서는 데미지를 입었을 때 바로 죽는 것으로 설정)
    {
        if (When_Dead_Effect != null)
            Instantiate(When_Dead_Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    public virtual void OnDie() // 모든 생명은 죽는다.
    {
        if (When_Dead_Effect != null)
            Instantiate(When_Dead_Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    protected IEnumerator Move_Circle(int Degree, int is_ClockWise_And_Speed, float Start_Degree, float x, float y, float start_x, float start_y, float time_persist)
    {
        // 각도, 시계/반시계 방향, x축 원, y축 원

        for (int th = 0; th < Degree; th++)
        {
            float rad = Mathf.Deg2Rad * (is_ClockWise_And_Speed * th + Start_Degree);

            float rad_x = x * Mathf.Sin(rad);
            float rad_y = y * Mathf.Cos(rad);

            transform.position = new Vector3(start_x + rad_x, start_y + rad_y, 0);
            yield return YieldInstructionCache.WaitForSeconds(Time.deltaTime * time_persist);
        }
        yield return null;
    }
    // 반지름 (x, y)으로 형성되는 타원 및 원을 시작 위치 (start_x, start_y), 시작 각도(Start_Degree)에서 원하는 각도(Degree) 만큼 반시계/시계(is_ClockWise_And_Speed) 운동하며,
    // time_persist(초) 동안 이동하는 원운동
    protected void Launch_Weapon(ref GameObject weapon, Vector3 Direction, Quaternion Degree, float speed, Vector3 Instantiate_Dir) 
    {
        GameObject Copy = Instantiate(weapon, Instantiate_Dir, Degree);
        if (Copy.TryGetComponent(out Weapon_Devil WD))
        {
            WD.W_MoveTo(Direction.normalized);
            WD.W_MoveSpeed(speed);
        }
        else if (Copy.TryGetComponent(out Weapon_Player WP))
        {
            WP.W_MoveTo(Direction.normalized);
            WP.W_MoveSpeed(speed);
        }
        else
            Destroy(Copy);
    }
    // 무기의 방향(Direction), 무기의 회전 각도(Degree), 무기의 이동 속도(speed), 무기가 생성되는 위치 (Instantiate_Dir)을 설정한 후 무기 발사
    protected IEnumerator Change_My_Size(Vector3 Origin, Vector3 Change, float time_persist, AnimationCurve curve)
    {
        float percent = 0;
        float inverse_time_persist = StaticFunc.Reverse_Time(time_persist);
        while (percent < 1)
        {
            percent += Time.deltaTime * inverse_time_persist;
            transform.localScale = Vector3.Lerp(Origin, Change, curve.Evaluate(percent));
            yield return null;
        }
    }
    // 생명은 본인의 크기를 조절할 수 있다. (본래 크기(Origin) --> 바꿀 크기(Change)), time_persist(초) 동안 크기 변경. 애니메이션 커브를 통해 크기를 다채롭게 조정
    protected IEnumerator Change_My_Color_And_Back(Color Origin_C, Color Change_C, float time_persist, bool is_Continue)
    {
        float percent;
        float inverse_time_persist = StaticFunc.Reverse_Time(time_persist);
        while (true)
        {
            percent = 0;
            while (percent < 1)
            {
                percent += Time.deltaTime * inverse_time_persist;
                My_Color = Color.Lerp(Origin_C, Change_C, percent);
                yield return null;
            }
            percent = 0;
            while (percent < 1)
            {
                percent += Time.deltaTime * inverse_time_persist;
                My_Color = Color.Lerp(Change_C, Origin_C, percent);
                yield return null;
            }
            if (!is_Continue)
                break;
        }
    }
    // 생명의 색깔을 바꿨다 원래대로 돌아오는 로직 (본래 색상(Origin_C) --> 바꿀 색상(Change_C)), time_persist(초) 동안 크기 변경. is_Continue를 true로 하면 무한 반복
    protected IEnumerator My_Color_When_UnBeatable()
    {
        bool flag = false;

        while (true)
        {
            if (!flag)
                My_Color = new Color32(255, 255, 255, 90);
            else
                My_Color = new Color32(255, 255, 255, 180);
            yield return new WaitForSeconds(0.2f);
            flag = !flag;
        }
    }
    // 무적 상태일 때 이를 표시하도록 해주는 로직
    protected IEnumerator Change_My_Color(Color Origin_C, Color Change_C, float time_persist, float Wait_Second, GameObject Effect_When_Change_My_Color)
    {
        float percent = 0;
        float inverse_time_persist = StaticFunc.Reverse_Time(time_persist);

        if (Effect_When_Change_My_Color != null)
            Instantiate(Effect_When_Change_My_Color, transform.position, Effect_When_Change_My_Color.transform.localRotation);

        while (percent < 1)
        {
            percent += Time.deltaTime * inverse_time_persist;
            My_Color = Color.Lerp(Origin_C, Change_C, percent);
            yield return null;
        }
        yield return YieldInstructionCache.WaitForSeconds(Wait_Second);
    }
    // 생명의 색깔을 바꾸는 로직 (본래 색상(Origin_C) --> 바꿀 색상(Change_C)), time_persist(초) 동안 크기 변경.
    // 색깔을 변경하고 일정 기간 대기할 수 있도록 Wait_Second 추가 설정. 색깔 변경 시 이펙트를 추가할 수 있도록 Effect_When_Change_My_Color 추가 설정
    protected IEnumerator Move_Straight(Vector3 start_location, Vector3 last_location, float time_persist, AnimationCurve curve)
    {
        float percent = 0;
        float inverse_time_persist = StaticFunc.Reverse_Time(time_persist);
        while (percent < 1)
        {
            percent += Time.deltaTime * inverse_time_persist;
            My_Position = Vector3.Lerp(start_location, last_location, curve.Evaluate(percent));
            yield return null;
        }
    }
    // 생명의 직선 이동 (본래 위치(start_location) --> 이동 위치(last_location)), time_persist(초) 동안 위치 변경. 애니메이션 커브를 통해 이동을 다채롭게 조정
    protected IEnumerator Move_Curve(Vector3 start_location, Vector3 last_location, Vector3 center, float time_persist, AnimationCurve curve)
    {
        float percent = 0;
        float inverse_time_persist = StaticFunc.Reverse_Time(time_persist);
        while (percent < 1)
        {
            percent += Time.deltaTime * inverse_time_persist;
            Vector3 riseRelCenter = start_location - center;
            Vector3 setRelCenter = last_location - center;

            My_Position = Vector3.Slerp(riseRelCenter, setRelCenter, curve.Evaluate(percent));

            My_Position += center;
            yield return null;
        }
        yield return null;
    }
    // 생명의 곡선 이동 (본래 위치(start_location) --> 이동 위치(last_location)), 곡선 이동 시 기준이 되는 위치(center) 추가 설정.
    // time_persist(초) 동안 위치 변경. 애니메이션 커브를 통해 이동을 다채롭게 조정
    protected float Get_Curve_Distance(Vector3 Start, Vector3 End, Vector3 Center)
    {
        Vector3 Center_to_Start = Start - Center;
        Vector3 Center_to_Last = End - Center;

        float theta = Vector3.Dot(Center_to_Start, Center_to_Last) / (Center_to_Start.magnitude * Center_to_Last.magnitude);
        float angle = Mathf.Acos(theta) * Mathf.Rad2Deg;
        return (2 * Mathf.PI * Vector3.Distance(Center_to_Start, Vector3.zero) * angle) / 360;
    }
    // 생명의 곡선 이동에 따른 거리 구하기
    protected Vector3 Get_Center_Vector_For_Curve_Move(Vector3 Start, Vector3 End, float Center_To_Real_Center_Distance, string is_Clock)
    {
        Vector3 Origin_V = Start - End;
        Vector3 Center = (Start + End) * 0.5f;
        Vector3 Dot_Vector = Vector3.zero;

        if (Origin_V.x != 0 && Origin_V.y == 0)
            Dot_Vector = Vector3.up;

        else if (Origin_V.x == 0 && Origin_V.y != 0)
            Dot_Vector = Vector3.right;

        else if (Origin_V.x != 0 && Origin_V.y != 0)
            Dot_Vector = new Vector3(1, -Origin_V.x / Origin_V.y, 0);

        // 기본적으로는 아랫방향이다.
        if (is_Clock == "anti_clock")
            Dot_Vector = -Dot_Vector; // 윗방향일때 한정.

        Dot_Vector = Dot_Vector.normalized;

        return new Vector3(Center.x - Dot_Vector.x * Center_To_Real_Center_Distance, Center.y - Dot_Vector.y * Center_To_Real_Center_Distance);
    }
    // 생명의 곡선 이동을 위한 기준점 구하기 (본래 위치(Start), 이동 위치(End)의 중간 지점에서 직교로 떨어진 임의의 거리 (Center_To_Real_Center_Distance)에서,
    // 반시계/시계 이동 여부(is_Clock)에 따라 +, -로 기준점 구하기
    protected IEnumerator Change_My_Size_Infinite(float delta_ratio)
    {
        while (true)
        {
            My_Scale = new Vector3(My_Scale.x + Time.deltaTime * delta_ratio, My_Scale.y + Time.deltaTime * delta_ratio, 0);
            yield return null;
        }
    }
    // 크기를 무한대로 조정 (delta_ratio 만큼)
    protected IEnumerator My_Rotate_Dec(Quaternion Origin, Quaternion Change, float time_persist, AnimationCurve curve)
    {
        float inverse_time_persist = StaticFunc.Reverse_Time(time_persist);
        float percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime * inverse_time_persist;
            transform.rotation = Quaternion.Lerp(Origin, Change, curve.Evaluate(percent));
            yield return null;
        }
    }
    // 생명의 제자리 회전 (본래 각도(Origin) --> 회전할 각도(Change),
    // time_persist(초) 동안 회전. 애니메이션 커브를 통해 회전을 다채롭게 조정
    protected void Stop_Image_Color_Change()
    {
        if (imageColor != null)
        {
            imageColor.Init();
            imageColor.StopAllCoroutines();
        }
    }
    // 배경 색깔 변경을 강제로 멈추는 함수 (현재는 보스가 죽었을 때만 적용)
    protected void Stop_Camera_Shake()
    {
        if (cameraShake != null)
        {
            cameraShake.Init_Camera();
            cameraShake.StopAllCoroutines();
        } 
    }
    // 카메라의 흔들림을 강제로 멈추는 함수 (현재는 보스가 죽었을 때만 적용)
    protected void Camera_Shake(float shake_intensity, float time_persist, bool is_Decline_Camera_Shake, bool is_Continue)
    {
        if (cameraShake != null)
        {
            if (camera_shake_i != null)
                cameraShake.StopCoroutine(camera_shake_i);
            camera_shake_i = cameraShake.Shake_Act(shake_intensity, time_persist, is_Decline_Camera_Shake, is_Continue);
            cameraShake.StartCoroutine(camera_shake_i);
        }
    }
    // 카메라를 흔드는 함수 (shake_intensity의 강도만큼, time_persist(초) 동안 지속. is_Decline_Camera_Shake를 true로 하면
    // 카메라의 흔들림이 서서히 멈추는 효과를 줄 수 있음. is_Continue를 true로 하면 카메라 흔들림 무한 반복.
    protected IEnumerator Camera_Shake_And_Wait(float shake_intensity, float time_persist, bool is_Decline_Camera_Shake, bool is_Continue)
    {
        if (cameraShake != null)
        {
            if (camera_shake_i != null)
                cameraShake.StopCoroutine(camera_shake_i);
            camera_shake_i = cameraShake.Shake_Act(shake_intensity, time_persist, is_Decline_Camera_Shake, is_Continue);
            yield return cameraShake.StartCoroutine(camera_shake_i);
        }
        else
            yield return null;
    }
    // 카메라를 흔들지만, 카메라가 흔드는걸 모두 기다렸다가 행동을 재개할 수 있는 로직 (shake_intensity의 강도만큼, time_persist(초) 동안 지속. is_Decline_Camera_Shake를 true로 하면
    // 카메라의 흔들림이 서서히 멈추는 효과를 줄 수 있음. is_Continue를 true로 하면 카메라 흔들림 무한 반복.
    protected void Flash(Color FlashColor, float wait_second, float time_persist)
    {
        if (imageColor != null)
        {
            if (back_ground_color_i != null)
                imageColor.StopCoroutine(back_ground_color_i);
            back_ground_color_i = imageColor.Flash(FlashColor, wait_second, time_persist);
            imageColor.StartCoroutine(back_ground_color_i);
        }
    }
    // 플래시를 주는 함수 (FlashColor로 플래시 색깔 설정. 플래시 이후 잠깐 대기하도록 wait_second(초) 적용.
    // 플래시의 지속 시간(time_persist(초))
    protected IEnumerator Flash_And_Wait(Color FlashColor, float wait_second, float time_persist)
    {
        if (imageColor != null)
        {
            if (back_ground_color_i != null)
                imageColor.StopCoroutine(back_ground_color_i);
            back_ground_color_i = imageColor.Flash(FlashColor, wait_second, time_persist);
            yield return imageColor.StartCoroutine(back_ground_color_i);
        }
        else
            yield return null;
    }
    // 플래시를 주지만, 플래시가 모두 끝나는걸 기다렸다가 행동을 재개할 수 있는 로직 (FlashColor로 플래시 색깔 설정. 플래시 이후 잠깐 대기하도록 wait_second(초) 적용.
    // 플래시의 지속 시간(time_persist(초))
    protected void Change_BG(Color Change, float time_persist)
    {
        if (imageColor != null)
        {
            if (back_ground_color_i != null)
                imageColor.StopCoroutine(back_ground_color_i);
            back_ground_color_i = imageColor.Change_BG(Change, time_persist);
            imageColor.StartCoroutine(back_ground_color_i);
        }
    }
    // 배경색상을 서서히 바꾸는 함수 (Change로 배경 색깔 설정. 배경색상 변경 지속 시간(time_persist(초))
    protected IEnumerator Change_BG_And_Wait(Color Change, float time_persist)
    {
        if (imageColor != null)
        {
            if (back_ground_color_i != null)
                imageColor.StopCoroutine(back_ground_color_i);
            back_ground_color_i = imageColor.Change_BG(Change, time_persist);
            yield return imageColor.StartCoroutine(back_ground_color_i);
        }
        else
            yield return null;
    }
    // 배경색상을 서서히 바꾸지만, 배경색상 변경이 모두 끝나는걸 기다렸다가 행동을 재개할 수 있는 로직
    // (Change로 배경 색깔 설정. 배경색상 변경 지속 시간(time_persist(초))
    protected void Stop_Life_Act(ref IEnumerator Recv)
    {
        if (Recv != null)
            StopCoroutine(Recv);
    }
    // 생명의 행동 중지
    protected void Run_Life_Act_And_Continue(ref IEnumerator Recv, IEnumerator Send)
    {
        Stop_Life_Act(ref Recv);
        Recv = Send;
        StartCoroutine(Recv);
    }
    // 생명의 행동 시작
    protected void Run_Life_Act(IEnumerator Send)
    {
        StartCoroutine(Send);
    }
    // 생명의 행동 시작
    protected void OnDestroy()
    {
        StopAllCoroutines();
    }
}