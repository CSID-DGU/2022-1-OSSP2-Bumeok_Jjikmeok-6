using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Boss_Info : Life
{
    // Start is called before the first frame update

    [SerializeField]
    private float maxHP = 100; // 최대 HP는 보스 본인도 변경할 수 없도록 private

    [SerializeField]
    protected GameObject DisAppear_Effect_1; // 상위

    [SerializeField]
    protected GameObject DisAppear_Effect_2; // 상위

    [SerializeField]
    protected TextMeshProUGUI WarningText; // 상위

    protected TrailRenderer trailRenderer;

    public float speed = 15; // 둘다
    public float rotateSpeed = 200f; // 상위

    private float currentHP;
    public float CurrentHP
    {
        set { currentHP = value; }
        get { return currentHP; }
    }
    public float MaxHP => maxHP;

    public Color Trail_Start_Color
    {
        get{ return trailRenderer.startColor; }
        set {trailRenderer.startColor = value;}
    }
    public Color Trail_End_Color
    {
        get { return trailRenderer.startColor; }
        set { trailRenderer.startColor = value; }
    }
    protected virtual new void Awake()
    {
        base.Awake();
        CurrentHP = MaxHP;
        if (TryGetComponent(out TrailRenderer t1))
        {
            trailRenderer = t1;
            trailRenderer.enabled = false;
        }
           
    }
    public override void TakeDamage(float damage)
    {
        if (Unbeatable)
            return;
        CurrentHP -= damage;
        if (CurrentHP <= 0)
            OnDie();
    }
    public override void OnDie()
    {
        base.OnDie();
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision) // 얘만
    {
        if (collision.CompareTag("Player") && collision.TryGetComponent(out Player_Info PI))
            PI.TakeDamage(1);
    }

    protected virtual void Killed_All_Mine()
    {
        if (trailRenderer != null)
            trailRenderer.enabled = false;
        GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] meteor = GameObject.FindGameObjectsWithTag("Meteor");
        GameObject[] weapon_devil = GameObject.FindGameObjectsWithTag("Weapon_Devil");

        foreach (var e in meteor)
            Destroy(e);

        foreach (var e in enemy)
            Destroy(e);

        foreach (var e in weapon_devil)
            Destroy(e);
        My_Color = Color.white;
        transform.localRotation = Quaternion.identity;
        StopAllCoroutines();
    }
    protected virtual void Init_Back_And_Camera()
    {
        Stop_Camera_Shake();
        Stop_Image_Color_Change();
    }
    protected IEnumerator Warning(Color Text_Color, string warning_message, float time_persist)
    {
        WarningText.color = Text_Color;
        WarningText.text = warning_message;
        float inverse_time_persist = StaticFunc.Reverse_Time(time_persist);
        while (WarningText.color.a < 1.0f)
        {
            WarningText.color = new Color(WarningText.color.r, WarningText.color.g, WarningText.color.b, WarningText.color.a + Time.deltaTime * inverse_time_persist);
            yield return null;
        }
        while (WarningText.color.a > 0.0f)
        {
            WarningText.color = new Color(WarningText.color.r, WarningText.color.g, WarningText.color.b, WarningText.color.a - Time.deltaTime * inverse_time_persist);
            yield return null;
        }
    }
    protected IEnumerator Rotate_Bullet(float rot_Speed, float rot_Radius, float ratio, int Launch_Num, GameObject Bullet)
    {
        float percent = 0;
        int Index = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime * ratio;
            transform.Rotate(Vector3.forward * rot_Speed * rot_Radius * Time.deltaTime);
            Index++;
            if (Index >= Launch_Num)
            {
                Index = 0;
                GameObject T1 = Instantiate(Bullet);
                T1.transform.position = transform.position;
                T1.transform.rotation = transform.rotation;
            }
            yield return null;
        }
        transform.rotation = Quaternion.Euler(0, 0, 0);
        yield return null;
    }
    public IEnumerator Shake_Act(float shake_intensity, float scale_ratio, float time_persist, bool is_Continue)
    {
        Quaternion originRotation;
        originRotation = transform.rotation;
        Vector3 originScale = My_Scale;
        float inverse_time_persist = StaticFunc.Reverse_Time(time_persist);
        while (true)
        {
            float percent = 0;
            while (percent < 1)
            {
                percent += Time.deltaTime * inverse_time_persist;
                transform.localScale = new Vector3(transform.localScale.x + Time.deltaTime * scale_ratio, transform.localScale.y + Time.deltaTime * scale_ratio, 0);
                transform.transform.rotation = new Quaternion(
                                    originRotation.x + Random.Range(-shake_intensity, shake_intensity) * 0.2f,
                                    originRotation.y + Random.Range(-shake_intensity, shake_intensity) * 0.2f,
                                    originRotation.z + Random.Range(-shake_intensity, shake_intensity) * 0.2f,
                                    originRotation.w + Random.Range(-shake_intensity, shake_intensity) * 0.2f);
                yield return null;
            }
            if (!is_Continue)
            {
                transform.rotation = originRotation;
                My_Scale = originScale;
                yield return null;
                yield break;
            }
        }
    }
    protected IEnumerator Trail_Color_Change_And_Back(Color Origin_C, Color Change_C, float time_persist, int Count)
    {
        float percent;
        float inverse_time_persist = StaticFunc.Reverse_Time(time_persist);
        for (int i = 0; i < Count; i++)
        {
            percent = 0;
            while (percent < 1)
            {
                percent += Time.deltaTime * inverse_time_persist;

                trailRenderer.endColor = Color.Lerp(Origin_C, Change_C, percent);
                trailRenderer.startColor = Color.Lerp(Origin_C, Change_C, percent);
                yield return null;
            }
            percent = 0;
            while (percent < 1)
            {
                percent += Time.deltaTime * inverse_time_persist;
                trailRenderer.endColor = Color.Lerp(Change_C, Origin_C, percent);
                trailRenderer.startColor = Color.Lerp(Change_C, Origin_C, percent);
                yield return null;
            }
        }
    }
    protected IEnumerator Trail_Color_Change(Color Origin_C, Color Change_C, float time_persist)
    {
        float percent;
        float inverse_time_persist = StaticFunc.Reverse_Time(time_persist);
        percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime * inverse_time_persist;
            trailRenderer.endColor = Color.Lerp(Origin_C, Change_C, percent);
            trailRenderer.startColor = Color.Lerp(Origin_C, Change_C, percent);
            yield return null;
        }
    }
}
