using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nachi_X : Enemy_Info
{
    private TrailRenderer trailRenderer;

    private float Camera_Shake_At_Once;
    // Start is called before the first frame update
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
    }
    IEnumerator X_Color_Change(Color Origin_C, Color Change_C, float time_persist, int Count)
    {
        float inverse_time_persist = StaticFunc.Reverse_Time(time_persist);
        for (int i = 0; i < Count; i++)
        {
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
        if (TryGetComponent(out TrailCollisions TC))
            TC.Draw_Collision_Line();
        yield return Move_Circle(90, flag * 4, 0, 0.3f, 0.3f, My_Position.x, My_Position.y, 0.3f);

        yield return Move_Straight(My_Position, My_Position + new Vector3(-4 * flag, 2f, 0), 0.4f, declineCurve);
        yield return Move_Straight(My_Position, My_Position + new Vector3(16 * flag, -8f, 0), 0.1f, OriginCurve);

        if (Camera_Shake_At_Once < 0)
            Camera_Shake(0.02f, 2f, true, false);
        yield return X_Color_Change(Color.white, new Color(1, 1, 1, 0), 1, 3);

        trailRenderer.enabled = false;
        Destroy(gameObject);
    }
}
