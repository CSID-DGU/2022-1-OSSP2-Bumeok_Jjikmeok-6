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
public class Life : MonoBehaviour, Life_Of_Basic // ���� ���õ� Ŭ����
{
    [SerializeField]
    protected GameObject[] Weapon; // ��� ������ ���⸦ ����� �� �ִ�.

    [SerializeField]
    protected AnimationCurve declineCurve;

    [SerializeField]
    protected AnimationCurve OriginCurve;

    [SerializeField]
    protected AnimationCurve inclineCurve;

    [SerializeField]
    protected AnimationCurve De_In_Curve;

    // ��� ������ 4���� �ִϸ��̼� Ŀ�꿡 ���� ��ä�ο� �ൿ�� �� �� �ִ�.

    [SerializeField]
    protected GameObject When_Dead_Effect; // ������ �׾��� ���� ����Ʈ

    [SerializeField]
    protected StageData stageData; // ������ Ư�� �������� ����� �ʵ��� ���� ����

    [SerializeField]
    AudioClip[] BackGroundSource_Collect = null;

    [SerializeField]
    AudioClip[] EffectSource_Collect = null;

    // �� ������ ����ϴ� �����, ȿ������(�迭)

    [SerializeField]
    Vector2 Stage3_Min_Limit;

    [SerializeField]
    Vector2 Stage3_Max_Limit;

    // �������� ����� �ʴ� ������ ������ ����

    protected SpriteRenderer spriteRenderer;

    protected Movement2D movement2D; // �������� �����ϴ� ��ũ��Ʈ (������ٵ� �̿��Ͽ� ������ ���� �ش� ��ũ��Ʈ ��� X)

    protected ImageColor imageColor; // ��� ������ ������ ������ �� �ִ�.

    private CameraShake cameraShake; // ��� ������ ī�޶� ���� ȿ���� �� �� �ִ�.

    private IEnumerator camera_shake_i;

    private IEnumerator back_ground_color_i;

    protected AudioSource BackGroundSource;

    protected AudioSource EffectSource;

    private bool unbeatable;

    public bool Unbeatable
    {
        get { return unbeatable; }
        set { unbeatable = value; }
    } // ���� ����
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
    public Color My_Color // ������ ����
    {
        get { return spriteRenderer.color; }
        set { spriteRenderer.color = value; }
    }
    public Vector3 My_Scale // ������ ũ��
    {
        get { return transform.localScale; }
        set { transform.localScale = value; }
    }
    public Vector3 My_Position // ������ ��ġ
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    protected void Return_To_My_Origin_Color()
    {
        My_Color = Color.white;
    } // ������ ���� �������� ���� (����� ������ �׾��� ���� �����Ͽ���)

    protected void Effect_Sound_Play(int index) // ȿ���� ��� �Լ� (���� �ݺ�)
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
    protected void Effect_Sound_Stop() // ȿ���� ���� �Լ�
    {
        if (EffectSource != null)
            EffectSource.Stop();
    }
    protected void Effect_Sound_Pause() // ȿ���� �Ͻ� ���� �Լ�
    {
        if (EffectSource != null)
            EffectSource.Pause();
    }
    protected void Effect_Sound_OneShot(int index) // ȿ���� ��� �Լ� (�ѹ���)
    {
        if (EffectSource_Collect.Length <= index)
            return;
        if (EffectSource != null && EffectSource_Collect[index] != null)
            EffectSource.PlayOneShot(EffectSource_Collect[index]);
    }
    protected void BackGround_Sound_Play(int index) // ����� ��� �Լ� (���� �ݺ�)
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
    protected void BackGround_Sound_Stop() // ����� ���� �Լ�
    {
        if (BackGroundSource != null)
            BackGroundSource.Stop();
    }
    protected void BackGround_Sound_Pause() // ����� �Ͻ� ���� �Լ�
    {
        if (BackGroundSource != null)
            BackGroundSource.Pause();
    }

    protected IEnumerator Decrease_BackGround_Sound(float time_persist) // ������� time_persist(��)��ŭ ���ҽ�Ű�� �Լ�
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

    public virtual void TakeDamage(float damage)  // ��� ������ �������� �Դ´�. (������ �Ǽ�ġ�� ����, ����ġ�� ���� ������, �̿� ���� �Լ��� ���� Ŭ��������
        // ������ �ؾ��ϱ� ������ ���� Ŭ���������� �������� �Ծ��� �� �ٷ� �״� ������ ����)
    {
        if (When_Dead_Effect != null)
            Instantiate(When_Dead_Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    public virtual void TakeDamage(int damage) // ��� ������ �������� �Դ´�. (������ �Ǽ�ġ�� ����, ����ġ�� ���� ������, �̿� ���� �Լ��� ���� Ŭ��������
        // ������ �ؾ��ϱ� ������ ���� Ŭ���������� �������� �Ծ��� �� �ٷ� �״� ������ ����)
    {
        if (When_Dead_Effect != null)
            Instantiate(When_Dead_Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    public virtual void OnDie() // ��� ������ �״´�.
    {
        if (When_Dead_Effect != null)
            Instantiate(When_Dead_Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    protected IEnumerator Move_Circle(int Degree, int is_ClockWise_And_Speed, float Start_Degree, float x, float y, float start_x, float start_y, float time_persist)
    {
        // ����, �ð�/�ݽð� ����, x�� ��, y�� ��

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
    // ������ (x, y)���� �����Ǵ� Ÿ�� �� ���� ���� ��ġ (start_x, start_y), ���� ����(Start_Degree)���� ���ϴ� ����(Degree) ��ŭ �ݽð�/�ð�(is_ClockWise_And_Speed) ��ϸ�,
    // time_persist(��) ���� �̵��ϴ� ���
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
    // ������ ����(Direction), ������ ȸ�� ����(Degree), ������ �̵� �ӵ�(speed), ���Ⱑ �����Ǵ� ��ġ (Instantiate_Dir)�� ������ �� ���� �߻�
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
    // ������ ������ ũ�⸦ ������ �� �ִ�. (���� ũ��(Origin) --> �ٲ� ũ��(Change)), time_persist(��) ���� ũ�� ����. �ִϸ��̼� Ŀ�긦 ���� ũ�⸦ ��ä�Ӱ� ����
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
    // ������ ������ �ٲ�� ������� ���ƿ��� ���� (���� ����(Origin_C) --> �ٲ� ����(Change_C)), time_persist(��) ���� ũ�� ����. is_Continue�� true�� �ϸ� ���� �ݺ�
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
    // ���� ������ �� �̸� ǥ���ϵ��� ���ִ� ����
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
    // ������ ������ �ٲٴ� ���� (���� ����(Origin_C) --> �ٲ� ����(Change_C)), time_persist(��) ���� ũ�� ����.
    // ������ �����ϰ� ���� �Ⱓ ����� �� �ֵ��� Wait_Second �߰� ����. ���� ���� �� ����Ʈ�� �߰��� �� �ֵ��� Effect_When_Change_My_Color �߰� ����
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
    // ������ ���� �̵� (���� ��ġ(start_location) --> �̵� ��ġ(last_location)), time_persist(��) ���� ��ġ ����. �ִϸ��̼� Ŀ�긦 ���� �̵��� ��ä�Ӱ� ����
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
    // ������ � �̵� (���� ��ġ(start_location) --> �̵� ��ġ(last_location)), � �̵� �� ������ �Ǵ� ��ġ(center) �߰� ����.
    // time_persist(��) ���� ��ġ ����. �ִϸ��̼� Ŀ�긦 ���� �̵��� ��ä�Ӱ� ����
    protected float Get_Curve_Distance(Vector3 Start, Vector3 End, Vector3 Center)
    {
        Vector3 Center_to_Start = Start - Center;
        Vector3 Center_to_Last = End - Center;

        float theta = Vector3.Dot(Center_to_Start, Center_to_Last) / (Center_to_Start.magnitude * Center_to_Last.magnitude);
        float angle = Mathf.Acos(theta) * Mathf.Rad2Deg;
        return (2 * Mathf.PI * Vector3.Distance(Center_to_Start, Vector3.zero) * angle) / 360;
    }
    // ������ � �̵��� ���� �Ÿ� ���ϱ�
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

        // �⺻�����δ� �Ʒ������̴�.
        if (is_Clock == "anti_clock")
            Dot_Vector = -Dot_Vector; // �������϶� ����.

        Dot_Vector = Dot_Vector.normalized;

        return new Vector3(Center.x - Dot_Vector.x * Center_To_Real_Center_Distance, Center.y - Dot_Vector.y * Center_To_Real_Center_Distance);
    }
    // ������ � �̵��� ���� ������ ���ϱ� (���� ��ġ(Start), �̵� ��ġ(End)�� �߰� �������� ������ ������ ������ �Ÿ� (Center_To_Real_Center_Distance)����,
    // �ݽð�/�ð� �̵� ����(is_Clock)�� ���� +, -�� ������ ���ϱ�
    protected IEnumerator Change_My_Size_Infinite(float delta_ratio)
    {
        while (true)
        {
            My_Scale = new Vector3(My_Scale.x + Time.deltaTime * delta_ratio, My_Scale.y + Time.deltaTime * delta_ratio, 0);
            yield return null;
        }
    }
    // ũ�⸦ ���Ѵ�� ���� (delta_ratio ��ŭ)
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
    // ������ ���ڸ� ȸ�� (���� ����(Origin) --> ȸ���� ����(Change),
    // time_persist(��) ���� ȸ��. �ִϸ��̼� Ŀ�긦 ���� ȸ���� ��ä�Ӱ� ����
    protected void Stop_Image_Color_Change()
    {
        if (imageColor != null)
        {
            imageColor.Init();
            imageColor.StopAllCoroutines();
        }
    }
    // ��� ���� ������ ������ ���ߴ� �Լ� (����� ������ �׾��� ���� ����)
    protected void Stop_Camera_Shake()
    {
        if (cameraShake != null)
        {
            cameraShake.Init_Camera();
            cameraShake.StopAllCoroutines();
        } 
    }
    // ī�޶��� ��鸲�� ������ ���ߴ� �Լ� (����� ������ �׾��� ���� ����)
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
    // ī�޶� ���� �Լ� (shake_intensity�� ������ŭ, time_persist(��) ���� ����. is_Decline_Camera_Shake�� true�� �ϸ�
    // ī�޶��� ��鸲�� ������ ���ߴ� ȿ���� �� �� ����. is_Continue�� true�� �ϸ� ī�޶� ��鸲 ���� �ݺ�.
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
    // ī�޶� �������, ī�޶� ���°� ��� ��ٷȴٰ� �ൿ�� �簳�� �� �ִ� ���� (shake_intensity�� ������ŭ, time_persist(��) ���� ����. is_Decline_Camera_Shake�� true�� �ϸ�
    // ī�޶��� ��鸲�� ������ ���ߴ� ȿ���� �� �� ����. is_Continue�� true�� �ϸ� ī�޶� ��鸲 ���� �ݺ�.
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
    // �÷��ø� �ִ� �Լ� (FlashColor�� �÷��� ���� ����. �÷��� ���� ��� ����ϵ��� wait_second(��) ����.
    // �÷����� ���� �ð�(time_persist(��))
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
    // �÷��ø� ������, �÷��ð� ��� �����°� ��ٷȴٰ� �ൿ�� �簳�� �� �ִ� ���� (FlashColor�� �÷��� ���� ����. �÷��� ���� ��� ����ϵ��� wait_second(��) ����.
    // �÷����� ���� �ð�(time_persist(��))
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
    // �������� ������ �ٲٴ� �Լ� (Change�� ��� ���� ����. ������ ���� ���� �ð�(time_persist(��))
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
    // �������� ������ �ٲ�����, ������ ������ ��� �����°� ��ٷȴٰ� �ൿ�� �簳�� �� �ִ� ����
    // (Change�� ��� ���� ����. ������ ���� ���� �ð�(time_persist(��))
    protected void Stop_Life_Act(ref IEnumerator Recv)
    {
        if (Recv != null)
            StopCoroutine(Recv);
    }
    // ������ �ൿ ����
    protected void Run_Life_Act_And_Continue(ref IEnumerator Recv, IEnumerator Send)
    {
        Stop_Life_Act(ref Recv);
        Recv = Send;
        StartCoroutine(Recv);
    }
    // ������ �ൿ ����
    protected void Run_Life_Act(IEnumerator Send)
    {
        StartCoroutine(Send);
    }
    // ������ �ൿ ����
    protected void OnDestroy()
    {
        StopAllCoroutines();
    }
}