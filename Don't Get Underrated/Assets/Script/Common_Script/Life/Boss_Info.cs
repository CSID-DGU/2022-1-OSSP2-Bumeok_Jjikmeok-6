using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Boss_Info : Life
{
    [SerializeField]
    private float maxHP = 100; // 최대 HP는 보스 본인도 변경할 수 없도록 private

    [SerializeField]
    protected GameObject DisAppear_Effect_1; // 보스의 순간이동 시의 이펙트 1

    [SerializeField]
    protected GameObject DisAppear_Effect_2; // 보스의 순간이동 시의 이펙트 2

    [SerializeField]
    protected TextMeshProUGUI WarningText; // 경고 문구

    protected TrailRenderer trailRenderer;

    private float currentHP;
    public float CurrentHP
    {
        set { currentHP = value; }
        get { return currentHP; }
    }
    public float MaxHP => maxHP;
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
    public override void TakeDamage(float damage) // 보스가 데미지를 입을 때
    {
        if (Unbeatable) // 무적 상태일 때는 무시
            return;
        CurrentHP -= damage;
        if (CurrentHP <= 0) // 현재 HP가 0이면 죽도록
            OnDie();
    }
    public override void OnDie()
    {
        base.OnDie();
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision) // 플레이어에 직접 부딧혔으면 플레이어에게 1의 데미지를 줌
    {
        if (collision != null && collision.CompareTag("Player") && collision.TryGetComponent(out Player_Info PI))
            PI.TakeDamage(1);
    }

    protected virtual void Killed_All_Mine() // 보스가 죽었을 때 본인이 소환하는 모든 것들을 파괴한 후, 본인의 행동을 멈추는 함수
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

        Return_To_My_Origin_Color();
        transform.localRotation = Quaternion.identity;
        StopAllCoroutines();
    }
    protected virtual void Init_Back_And_Camera() // 배경색상 변경과 카메라 흔들림에 대한 행동 중지
    {
        Stop_Camera_Shake();
        Stop_Image_Color_Change();
    }
    protected IEnumerator Warning(Color Text_Color, string warning_message, float time_persist) // 경고 문구 출력
        //(경고 문구 색깔(Text_Color), 경고 문구(warning_message), 경고 문구가 나오는 지속 시간 (time_persist)
    {
        WarningText.color = Text_Color;
        WarningText.color = new Color(WarningText.color.r, WarningText.color.g, WarningText.color.b, 0);
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
    protected IEnumerator Rotate_Bullet(float rot_Speed, float rot_Radius, float time_persist, int Launch_Num, GameObject Bullet)
        // 회전하면서 총알을 발사하는 로직 (회전 속도(rot_Speed), 회전 반지름(rot_Radius), 지속 시간(time_persist(초)), 발사 횟수, 총알 종류(Bullet)으로 구성)
    {
        float percent = 0;
        int Index = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime * time_persist;
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
        // 본인을 떨 수 있도록 하는 함수 (떠는 강도(shake_intensity. 0.3이 일반적인 수치), 떨면서 커지는 크기(scale_ratio. 0.3이 일반적인 수치),
        // 떠는 지속시간(time_persist), is_Continue를 true로 하면 무한 반복
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
        // 트레일 렌더러로 그린 선의 색상을 바꿨다 원래대로 돌아오는 로직(본래 색상(Origin_C) --> 바꿀 색상(Change_C)),
        // time_persist(초) 동안 색상 변경. Count 수만큼 변경
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
    // 트레일 렌더러로 그린 선의 색상을 바꾸는 로직(본래 색상(Origin_C) --> 바꿀 색상(Change_C)),
    // time_persist(초) 동안 색상 변경
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