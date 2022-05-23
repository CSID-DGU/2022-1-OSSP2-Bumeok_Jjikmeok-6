using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour, Life_Of_Basic
{
    [SerializeField]
    protected GameObject[] Weapon; // 상위

    [SerializeField]
    protected AnimationCurve declineCurve; // 상위

    [SerializeField]
    protected AnimationCurve OriginCurve;

    [SerializeField]
    protected AnimationCurve inclineCurve;

    [SerializeField]
    protected AnimationCurve De_In_Curve;

    [SerializeField]
    protected GameObject When_Dead_Effect;

    [SerializeField]
    protected StageData stageData;

    protected SpriteRenderer spriteRenderer;

    protected Movement2D movement2D;

    protected IEnumerator camera_shake;

    protected CameraShake cameraShake;

    protected BackGroundColor backGroundColor;

    protected float percent;

    protected float Plus_Speed;

    private bool unbeatable;

    public bool Unbeatable
    {
        set { unbeatable = value; }
        get { return unbeatable; }
    }

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        cameraShake = GetComponent<CameraShake>();
        Unbeatable = false;
        Plus_Speed = 0;
        percent = 0;
    }
    public Color SpriteRenderer_Color
    {
        get { return spriteRenderer.color; }
        set { spriteRenderer.color = value; }
    }
    public virtual void TakeDamage(float damage) { }

    public virtual void TakeDamage(int damage) { }
    public virtual void OnDie()
    {
        Destroy(gameObject);
    }
    protected IEnumerator Circle_Move(int Degree, int is_ClockWise_And_Speed, float Start_Degree, float x, float y, float start_x, float start_y, float ratio)
    {
        // 각도, 시계/반시계 방향, x축 원, y축 원

        for (int th = 0; th < Degree; th++)
        {

            float rad = Mathf.Deg2Rad * (is_ClockWise_And_Speed * th + Start_Degree);

            float rad_x = x * Mathf.Sin(rad);
            float rad_y = y * Mathf.Cos(rad);

            transform.position = new Vector3(start_x + rad_x, start_y + rad_y, 0);
            yield return YieldInstructionCache.WaitForSeconds(Time.deltaTime * ratio);
        }
        yield return null;
    }
    protected void Launch_Weapon_For_Move_Blink(GameObject weapon, Vector3 Direction, Quaternion Degree, float speed, bool is_Blink, Vector3 Instantiate_Dir) // 이외의 나머지
    {
        GameObject Copy = Instantiate(weapon, Instantiate_Dir, Degree);
        if (Copy.TryGetComponent(out Weapon_Devil user1))
        {
            user1.W_MoveTo(Direction);
            user1.W_MoveSpeed(speed);
        }
        else if (Copy.TryGetComponent(out Weapon_Player user2))
        {
            user2.W_MoveTo(Direction);
            user2.W_MoveSpeed(speed);
        }
        else
            Destroy(Copy);
    }
    protected IEnumerator Change_Color_Return_To_Origin(Color Origin_C, Color Change_C, float time_persist, bool is_Continue)
    { 
        while (true)
        {
            percent = 0;
            while (percent < 1)
            {
                percent += Time.deltaTime / time_persist;
                spriteRenderer.color = Color.Lerp(Origin_C, Change_C, percent);
                yield return null;
            }
            percent = 0;
            while (percent < 1)
            {
                percent += Time.deltaTime / time_persist;
                spriteRenderer.color = Color.Lerp(Change_C, Origin_C, percent);
                yield return null;
            }
            if (!is_Continue)
                break;
        }
    }
    protected IEnumerator Color_When_UnBeatable()
    {
        bool ee = false;

        while (true)
        {
            if (!ee)
                spriteRenderer.color = new Color32(255, 255, 255, 90);
            else
                spriteRenderer.color = new Color32(255, 255, 255, 180);
            yield return new WaitForSeconds(0.2f);
            ee = !ee;
        }
    }
    public IEnumerator Change_Color_Lerp(Color Origin_C, Color Change_C, float time_persist, float Wait_Second, GameObject Effect)
    {
        if (Effect != null)
            Instantiate(Effect, transform.position, Quaternion.identity);
        percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime / time_persist;
            spriteRenderer.color = Color.Lerp(Origin_C, Change_C, percent);
            yield return null;
        }
        yield return YieldInstructionCache.WaitForSeconds(Wait_Second);
    }
    public IEnumerator Position_Lerp(Vector3 start_location, Vector3 last_location, float time_ratio, AnimationCurve curve)
    {
        float percent = 0;
        while (percent < 1)
        {
            percent += (Time.deltaTime / time_ratio);
            transform.position = Vector3.Lerp(start_location, last_location, curve.Evaluate(percent));
            yield return null;
        }
    }
    protected IEnumerator Position_Slerp(Vector3 start_location, Vector3 last_location, float time_ratio, float decline_ratio, string dir, AnimationCurve curve)
    {
        float percent = 0;
        int dir_int;
        if (dir == "down")
            dir_int = -1;
        else
            dir_int = 1;
        while (percent < 1)
        {
            percent += Time.deltaTime / time_ratio;
            Vector3 center = (start_location + last_location) * 0.5f;
            center -= new Vector3(0, dir_int * decline_ratio, 0);
            Vector3 riseRelCenter = start_location - center;
            Vector3 setRelCenter = last_location - center;

            transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, curve.Evaluate(percent));

            transform.position += center;
            yield return null;
        }
        yield return null;
    }
    protected float Get_Slerp_Distance(Vector3 Start, Vector3 End, Vector3 Center)
    {
        Vector3 Center_to_Start = Start - Center;
        Vector3 Center_to_Last = End - Center;

        float theta = Vector3.Dot(Center_to_Start, Center_to_Last) / (Center_to_Start.magnitude * Center_to_Last.magnitude);
        //Vector3 dirAngle = Vector3.Cross(Center_to_Start, Center_to_Last);
        float angle = Mathf.Acos(theta) * Mathf.Rad2Deg;
        //if (dirAngle.z < 0.0f) angle = 360 - angle;

        //Debug.Log(angle);
        //Debug.Log("dirAngle : " + dirAngle);
        return (2 * Mathf.PI * Vector3.Distance(Center_to_Start, Vector3.zero) * angle) / 360;
    }
    protected Vector3 Get_Center_Vector(Vector3 Start, Vector3 End, float Center_To_Real_Center_Distance, string is_Clock)
    {
        Vector3 Origin_V = Start - End;
        Vector3 Center = (Start + End) / 2;
        Vector3 Dot_Vector = Vector3.zero;

        if (Origin_V.x != 0 && Origin_V.y == 0)
            Dot_Vector = Vector3.up;

        else if (Origin_V.x == 0 && Origin_V.y != 0)
            Dot_Vector = Vector3.right;

        else if (Origin_V.x != 0 && Origin_V.y != 0)
            Dot_Vector = new Vector3(1, -Origin_V.x / Origin_V.y, 0);

        // 기본적으로는 아랫방향이다.
        if (is_Clock == "anti_clock")
            Dot_Vector = -Dot_Vector; // 이거는 윗방향일때 한정.

        Dot_Vector = Dot_Vector.normalized;

        return new Vector3(Center.x - Dot_Vector.x * Center_To_Real_Center_Distance, Center.y - Dot_Vector.y * Center_To_Real_Center_Distance);
    }
    protected IEnumerator Position_Slerp_Temp(Vector3 start_location, Vector3 last_location, Vector3 center, float time_ratio, AnimationCurve curve, bool is_Plus_Speed)
    {
        float percent = 0;
        while (percent < 1)
        {
            if (is_Plus_Speed)
                percent += (Time.deltaTime + Plus_Speed) / time_ratio;
            else
                percent += Time.deltaTime / time_ratio;
            Vector3 riseRelCenter = start_location - center;
            Vector3 setRelCenter = last_location - center;

            transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, curve.Evaluate(percent));

            transform.position += center;
            Plus_Speed += 0.0006f;
            yield return null;
        }
        yield return null;
    }

    protected IEnumerator Size_Change(Vector3 Origin_Size, Vector3 Change_Size, float time_persist, AnimationCurve curve)
    {
        percent = 0;
        while(percent < 1)
        {
            percent += Time.deltaTime / time_persist;
            transform.localScale = Vector3.Lerp(Origin_Size, Change_Size, curve.Evaluate(percent));
            yield return null;
        }
        yield break;
    }

    protected IEnumerator Size_Change_Infinite(float delta_ratio)
    {
        while (true)
        {
            transform.localScale = new Vector3(transform.localScale.x + Time.deltaTime * delta_ratio, transform.localScale.y + Time.deltaTime * delta_ratio, 0);
            yield return null;
        }
    }
    protected IEnumerator Rotate_Dec(Quaternion A, Quaternion B, float speed)
    {
        percent = 0;
        while (percent < 1)
        {
            percent += (Time.deltaTime * speed);
            transform.rotation = Quaternion.Lerp(A, B, percent);
            yield return null;
        }
    }
}
