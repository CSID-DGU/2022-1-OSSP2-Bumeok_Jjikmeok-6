using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Boss_Info : Life
{
    // Start is called before the first frame update

    [SerializeField]
    float maxHP = 100;

    [SerializeField]
    protected GameObject DisAppear_Effect_1; // 상위

    [SerializeField]
    protected GameObject DisAppear_Effect_2; // 상위

    [SerializeField]
    protected TextMeshProUGUI WarningText; // 상위

    public float speed = 15; // 둘다
    public float rotateSpeed = 200f; // 상위

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
    }
    public override void TakeDamage(float damage)
    {
        CurrentHP -= damage;
        if (CurrentHP <= 0)
            OnDie();
    }
    public override void OnDie()
    {
        base.OnDie();
    }
    public void OnTriggerEnter2D(Collider2D collision) // 얘만
    {
        if (collision.CompareTag("Player") && collision.TryGetComponent(out Player_Info PI))
            PI.TakeDamage(1);

    }
    protected virtual void Killed_All_Mine()
    {
        GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] meteor = GameObject.FindGameObjectsWithTag("Meteor");
        GameObject[] weapon_devil = GameObject.FindGameObjectsWithTag("Weapon_Devil");
        GameObject[] item = GameObject.FindGameObjectsWithTag("Item");

        foreach (var e in meteor)
            Destroy(e);

        foreach (var e in enemy)
            Destroy(e);

        foreach (var e in weapon_devil)
            Destroy(e);

        foreach (var e in item)
            Destroy(e);
    }
    protected IEnumerator Warning(string warning_message, float time_persist)
    {
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
            percent += (Time.deltaTime * ratio);
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
        yield return YieldInstructionCache.WaitForEndOfFrame;
    }
    public IEnumerator Shake_Act(float shake_intensity, float scale_ratio, float time_persist, bool is_Continue)
    {
        Vector3 originPosition;
        Quaternion originRotation;
        Vector3 originScale;
        originPosition = transform.position;
        originRotation = transform.rotation;
        originScale = transform.localScale;
        float inverse_time_persist = StaticFunc.Reverse_Time(time_persist);
        while (true)
        {
            float percent = 0;
            while (percent < 1)
            {
                percent += Time.deltaTime * inverse_time_persist;
                transform.position = originPosition + Random.insideUnitSphere * shake_intensity;
                transform.localScale = new Vector3(transform.localScale.x + Time.deltaTime * scale_ratio, transform.localScale.y + Time.deltaTime * scale_ratio, 0);
                transform.transform.rotation = new Quaternion(
                                    originRotation.x + Random.Range(-shake_intensity, shake_intensity) * 0.2f,
                                    originRotation.y + Random.Range(-shake_intensity, shake_intensity) * 0.2f,
                                    originRotation.z + Random.Range(-shake_intensity, shake_intensity) * 0.2f,
                                    originRotation.w + Random.Range(-shake_intensity, shake_intensity) * 0.2f);
                yield return YieldInstructionCache.WaitForEndOfFrame;
            }
            if (!is_Continue)
            {
                transform.position = originPosition;
                transform.rotation = originRotation;
                transform.localScale = originScale;
                yield return null;
                yield break;
            }
        }
    }
}
