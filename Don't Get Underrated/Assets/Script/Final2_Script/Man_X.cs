using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Man_X : Enemy_Info
{
    private TrailRenderer trailRenderer;

    private float Camera_Shake_At_Once;
    private new void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Player") && collision.TryGetComponent(out Player_Info HC))
        {
            if (!HC.Unbeatable)
                HC.TakeDamage(1);
        }
    }
    private new void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject != null && collision.gameObject.CompareTag("Player") && collision.gameObject.TryGetComponent(out Player_Info HC))
        {
            if (!HC.Unbeatable)
                HC.TakeDamage(1);
        }
    }
    private new void Awake()
    {
        base.Awake();
        if (TryGetComponent(out TrailRenderer TR))
            trailRenderer = TR;
        Camera_Shake_At_Once = My_Position.x;
        if (GameObject.Find("Flash") && GameObject.Find("Flash").TryGetComponent(out ImageColor IC))
            imageColor = IC;
        if (GameObject.Find("Enemy_Effect_Sound") && GameObject.Find("Enemy_Effect_Sound").TryGetComponent(out AudioSource AS1))
            EffectSource = AS1;
    }
    IEnumerator X_Color_Change(Color Origin_C, Color Change_C, float time_persist, int Count)
    {
        float inverse_time_persist = StaticFunc.Reverse_Time(time_persist);
        for (int i = 0; i < Count; i++)
        {
            Effect_Sound_OneShot(2);
            float percent = 0;
            while (percent < 1)
            {
                percent += Time.deltaTime * inverse_time_persist;
                trailRenderer.endColor = Color.Lerp(Origin_C, Change_C, percent);
                trailRenderer.startColor = Color.Lerp(Origin_C, Change_C, percent);
                yield return null;
            }
        }
    }
    public IEnumerator Move(int flag)
    {
        trailRenderer.enabled = true;
        Effect_Sound_OneShot(0);
        yield return Move_Circle(360.0f, flag * 13, 0.3f, 0.3f, My_Position.x, My_Position.y);

        Effect_Sound_OneShot(1);
        Run_Life_Act(My_Rotate_Dec(Quaternion.identity, Quaternion.Euler(new Vector3(0, 0, 120 * -flag)), 0.8f, declineCurve));
        Run_Life_Act(Change_My_Color(My_Color, Color.red, 0.8f, 0, null));
        Change_BG(new Color(1, 1, 1, 0.7f), 0.8f);
        yield return Move_Straight(My_Position, My_Position + new Vector3(-4 * flag, 2f, 0), 0.8f, declineCurve);

        Change_BG(Color.black, 0.2f);
        yield return Move_Straight(My_Position, My_Position + new Vector3(16 * flag, -8f, 0), 0.1f, OriginCurve);

        if (Camera_Shake_At_Once < 0)
            Camera_Shake(0.02f, 2f, true, false);
        yield return X_Color_Change(Color.white, Color.clear, 1, 3);

        Run_Life_Act(Change_My_Color(My_Color, Color.clear, 2, 0, null));
        yield return Change_BG_And_Wait(Color.clear, 1);

        trailRenderer.enabled = false;
        OnDie();
    }
}