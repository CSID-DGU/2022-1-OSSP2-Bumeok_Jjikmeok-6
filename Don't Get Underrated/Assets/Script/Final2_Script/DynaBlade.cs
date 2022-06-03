using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DynaBlade : Enemy_Info
{
    // Start is called before the first frame update

    private Dictionary<int, Vector3> Plus_Start, Minus_Start;

    public float[,] bangmeon = new float[6, 2] { { 6.8f, 4.46f }, { -5.67f, 2.44f }, { -5.67f, 1 }, { 5.81f, -1.29f }, { 5.81f, -3 }, { -13.77f, 3.77f } };

    private IEnumerator size;

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
        Plus_Start = new Dictionary<int, Vector3>();
        Minus_Start = new Dictionary<int, Vector3>();
        for (int i = 0; i < 6; i++)
        {
            Plus_Start.Add(i, new Vector3(bangmeon[i, 0], bangmeon[i, 1], 0));
            Minus_Start.Add(i, new Vector3(-bangmeon[i, 0], bangmeon[i, 1], 0));
        }
    }
    private void Start()
    {
        My_Scale = new Vector3(0.7f, 0.7f, 0);
        if (Mathf.Sign(My_Position.x) < 0)
            Run_Life_Act(Move(Minus_Start));
        else
            Run_Life_Act(Move(Plus_Start));
    }

    private IEnumerator Move(Dictionary<int, Vector3> U) // 루트3 / 2 (0.85)로 끝맺음 짓는게 좋다.
    {
        Run_Life_Act_And_Continue(ref size, Change_My_Size_Infinite(1.6f));

        float A = Get_Curve_Distance(U[0], U[1], Get_Center_Vector(U[0], U[1], Vector3.Distance(U[0], U[1]) * 0.85f, "anti_clock"));

        float B = Get_Curve_Distance(U[1], U[2], Get_Center_Vector(U[1], U[2], Vector3.Distance(U[1], U[2]) * 0.85f, "anti_clock"));

        float C = Get_Curve_Distance(U[2], U[3], Get_Center_Vector(U[2], U[3], Vector3.Distance(U[2], U[3]) * 0.85f, "anti_clock"));

        float Q = Get_Curve_Distance(U[3], U[4], Get_Center_Vector(U[3], U[4], Vector3.Distance(U[3], U[4]) * 0.85f, "clock"));

        float W = Get_Curve_Distance(U[4], U[5], Get_Center_Vector(U[4], U[5], Vector3.Distance(U[4], U[5]) * 0.85f, "anti_clock"));

        yield return Move_Curve(U[0], U[1], Get_Center_Vector(U[0], U[1], Vector3.Distance(U[0], U[1]) * 0.85f, "anti_clock"), 0.5f, OriginCurve);

        yield return My_Rotate_Dec(Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 0, 90), 0.08f, OriginCurve);

        yield return Move_Curve(U[1], U[2], Get_Center_Vector(U[1], U[2], Vector3.Distance(U[1], U[2]) * 0.85f, "anti_clock"), B/A * 0.5f, OriginCurve);

        yield return My_Rotate_Dec(Quaternion.Euler(0, 0, 90), Quaternion.Euler(0, 0, 180), 0.08f, OriginCurve);

        yield return Move_Curve(U[2], U[3], Get_Center_Vector(U[2], U[3], Vector3.Distance(U[2], U[3]) * 0.85f, "anti_clock"), C/A * 0.4f, OriginCurve);

        yield return My_Rotate_Dec(Quaternion.Euler(0, 0, 180), Quaternion.Euler(0, 0, 270), 0.08f, OriginCurve);
       
        yield return Move_Curve(U[3], U[4], Get_Center_Vector(U[3], U[4], Vector3.Distance(U[3], U[4]) * 0.85f, "clock"), Q/A * 0.3f, OriginCurve);

        yield return My_Rotate_Dec(Quaternion.Euler(0, 0, 270), Quaternion.Euler(0, 0, 360), 0.08f, OriginCurve);

        yield return Move_Curve(U[4], U[5], Get_Center_Vector(U[4], U[5], Vector3.Distance(U[4], U[5]) * 0.85f, "anti_clock"), W/A * 0.25f, OriginCurve);

        Stop_Life_Act(ref size);
        GameObject.FindGameObjectWithTag("Boss").GetComponent<SolGryn>().Is_Next_Pattern = true;
        Destroy(gameObject);
    }
}