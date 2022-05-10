using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_And_Boss : MonoBehaviour
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

    protected IEnumerator camera_shake;

    protected CameraShake cameraShake;

    protected FlashOn flashOn;

    private float percent;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        cameraShake = GetComponent<CameraShake>();
    }
    public virtual void OnDie()
    {
        Destroy(gameObject);
    }
    void Start()
    {
        
    }

    protected void Launch_Weapon_For_Move(GameObject weapon, Vector3 target, Vector3 self)
    {
        weapon.GetComponent<Movement2D_Wow>().MoveTo(new Vector3(target.x - self.x,
                        target.y - self.y, 0));
        Instantiate(weapon, self, Quaternion.identity);
    }
   // GameObject L5 = Instantiate(Boss_Weapon[2], new Vector3(0, 3, 0), Quaternion.Euler(new Vector3(0, 0, -9)));
    protected void Launch_Weapon_For_Move(GameObject weapon, Vector3 target, Quaternion Degree, float Destroy_Time)
    {
        weapon.GetComponent<Movement2D>().MoveTo(target);
        GameObject e = Instantiate(weapon, transform.position, Degree);
        Destroy(e, Destroy_Time);
    }

    protected void Launch_Weapon_For_Still(GameObject weapon, Vector3 instantiate_position, Quaternion Degree, float Destroy_Time)
    {
        GameObject e = Instantiate(weapon, instantiate_position, Degree);
        Destroy(e, Destroy_Time);
    }
    protected IEnumerator Change_Color_Return_To_Origin(Color Origin_C, Color Change_C, float time_persist, bool is_Continue)
    { 
        while (true)
        {
            percent = 0;
            while (percent < 1)
            {
                percent += Time.deltaTime * time_persist;
                spriteRenderer.color = Color.Lerp(Origin_C, Change_C, percent);
                yield return null;
            }
            percent = 0;
            while (percent < 1)
            {
                percent += Time.deltaTime * time_persist;
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
    protected IEnumerator Change_Color_Temporary(Color Origin_C, Color Change_C, float time_persist, float Wait_Second, GameObject Effect)
    {
        if (Effect != null)
            Instantiate(Effect, transform.position, Quaternion.identity);
        percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime * time_persist;
            spriteRenderer.color = Color.Lerp(Origin_C, Change_C, percent);
            yield return YieldInstructionCache.WaitForEndOfFrame;
        }
        yield return YieldInstructionCache.WaitForSeconds(Wait_Second);
    }
    public IEnumerator Position_Lerp(Vector3 start_location, Vector3 last_location, float time_ratio, AnimationCurve curve)
    {
        float percent = 0;
        while (percent < 1)
        {
            percent += (Time.deltaTime * time_ratio);
            transform.position = Vector3.Lerp(start_location, last_location, curve.Evaluate(percent));
            yield return null;
        }
    }
    protected IEnumerator Position_Curve(Vector3 start_location, Vector3 last_location, float time_ratio, float decline_ratio, string dir, AnimationCurve curve)
    {
        float percent = 0;
        int dir_int;
        if (dir == "down")
            dir_int = -1;
        else
            dir_int = 1;
        while (percent < 1)
        {
            percent += Time.deltaTime * time_ratio;
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
}
