using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynaBlade : Enemy_Info
{
    // Start is called before the first frame update

    Dictionary<int, Vector3> Plus_Start, Minus_Start;

    float[,] bangmeon = new float[6, 2] { { 6.8f, 4.46f }, { -5.67f, 2.44f }, { -5.67f, 1 }, { 5.81f, -1.29f }, { 5.81f, -3 }, { -13.77f, 3.77f } };


    private new void Awake()
    {
        base.Awake();
        cameraShake.mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        Plus_Start = new Dictionary<int, Vector3>();
        Minus_Start = new Dictionary<int, Vector3>();
        for (int i = 0; i < 6; i++)
        {
            Plus_Start.Add(0, new Vector3(bangmeon[i, 0], bangmeon[i, 1], 0));
            Minus_Start.Add(0, new Vector3(-bangmeon[i, 0], bangmeon[i, 1], 0));
        }
    }
    private void Start()
    {
        transform.localScale = new Vector3(0.1f, 0.1f, 0);
        if (Mathf.Sign(transform.position.x) == -1)
            StartCoroutine(Move(Minus_Start));
        else
            StartCoroutine(Move(Plus_Start));
    }
   
    void Camera_Origin()
    {
        cameraShake.mainCamera.transform.position = new Vector3(0, 0, -10);
        cameraShake.mainCamera.transform.rotation = Quaternion.identity;
        cameraShake.mainCamera.transform.localScale = new Vector3(1, 1, 1);
    }

    IEnumerator Move(Dictionary<int, Vector3> U) // 루트3 / 2 (0.85)로 끝맺음 짓는게 좋다.
    {
        Camera_Origin();
        yield return null;

        IEnumerator size = Size_Change_Infinite(1.6f);
        StartCoroutine(size);
        Plus_Speed = 0;
        
        float A = Get_Slerp_Distance(U[0], U[1], Get_Center_Vector(U[0], U[1], Vector3.Distance(U[0], U[1]) * 0.85f, "anti_clock"));

        float B = Get_Slerp_Distance(U[1], U[2], Get_Center_Vector(U[1], U[2], Vector3.Distance(U[1], U[2]) * 0.85f, "anti_clock"));

        float C = Get_Slerp_Distance(U[2], U[3], Get_Center_Vector(U[2], U[3], Vector3.Distance(U[2], U[3]) * 0.85f, "anti_clock"));

        float Q = Get_Slerp_Distance(U[3], U[4], Get_Center_Vector(U[3], U[4], Vector3.Distance(U[3], U[4]) * 0.85f, "clock"));

        float W = Get_Slerp_Distance(U[4], U[5], Get_Center_Vector(U[4], U[5], Vector3.Distance(U[4], U[5]) * 0.85f, "anti_clock"));

        camera_shake = cameraShake.Shake_Act(.05f, .24f, 1, true);
        StartCoroutine(camera_shake);

        yield return StartCoroutine(Position_Slerp_Temp(U[0], U[1], Get_Center_Vector(U[0], U[1], Vector3.Distance(U[0], U[1]) * 0.85f, "anti_clock"), 15f, OriginCurve, true));

        yield return StartCoroutine(Rotate_Dec(Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 0, 90), 13));

        yield return StartCoroutine(Position_Slerp_Temp(U[1], U[2], Get_Center_Vector(U[1], U[2], Vector3.Distance(U[1], U[2]) * 0.85f, "anti_clock"), B/A * 15f, OriginCurve, true));

        yield return StartCoroutine(Rotate_Dec(Quaternion.Euler(0, 0, 90), Quaternion.Euler(0, 0, 180), 13));

        yield return StartCoroutine(Position_Slerp_Temp(U[2], U[3], Get_Center_Vector(U[2], U[3], Vector3.Distance(U[2], U[3]) * 0.85f, "anti_clock"), C/A * 15f, OriginCurve, true));

        yield return StartCoroutine(Rotate_Dec(Quaternion.Euler(0, 0, 180), Quaternion.Euler(0, 0, 270), 13));
       
        yield return StartCoroutine(Position_Slerp_Temp(U[3], U[4], Get_Center_Vector(U[3], U[4], Vector3.Distance(U[3], U[4]) * 0.85f, "clock"), Q/A * 15f, OriginCurve, true));

        yield return StartCoroutine(Rotate_Dec(Quaternion.Euler(0, 0, 270), Quaternion.Euler(0, 0, 360), 13));

        yield return StartCoroutine(Position_Slerp_Temp(U[4], U[5], Get_Center_Vector(U[4], U[5], Vector3.Distance(U[4], U[5]) * 0.85f, "anti_clock"), W/A * 20f, OriginCurve, true));
       
        StopCoroutine(camera_shake);
        StopCoroutine(size);

        Camera_Origin();
        GameObject.FindGameObjectWithTag("Boss").GetComponent<SolGryn>().Is_Next_Pattern = true;
        yield return null;
        Destroy(gameObject);
    }
}
