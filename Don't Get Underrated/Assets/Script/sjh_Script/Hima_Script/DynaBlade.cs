using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynaBlade : Enemy_Info
{
    // Start is called before the first frame update

    Dictionary<int, Vector3> D, E;


    private new void Awake()
    {
        base.Awake();
        cameraShake.mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        D = new Dictionary<int, Vector3>();
        D.Add(0, new Vector3(6.8f, 4.46f, 0));
        D.Add(1, new Vector3(-5.67f, 2.44f, 0));
        D.Add(2, new Vector3(-5.67f, 0.99f, 0));
        D.Add(3, new Vector3(5.81f, -1.29f, 0));
        D.Add(4, new Vector3(5.81f, -2.99f, 0));
        D.Add(5, new Vector3(-13.77f, 3.77f, 0));

        E = new Dictionary<int, Vector3>();
        E.Add(0, new Vector3(-6.8f, 4.46f, 0));
        E.Add(1, new Vector3(5.67f, 2.44f, 0));
        E.Add(2, new Vector3(5.67f, 0.99f, 0));
        E.Add(3, new Vector3(-5.81f, -1.29f, 0));
        E.Add(4, new Vector3(-5.81f, -2.99f, 0));
        E.Add(5, new Vector3(13.77f, 3.77f, 0));
    }
    void Start()
    {
       
    }
    public void Dyna_Start(bool Opposite)
    {
        if (Opposite)
        {
            transform.position = new Vector3(-6.8f, 4.46f, 0);
            transform.localScale = new Vector3(0.1f, 0.1f, 0);
            StartCoroutine(Move(E));
        }
        else
        {
            transform.position = new Vector3(6.8f, 4.46f, 0);
            transform.localScale = new Vector3(0.1f, 0.1f, 0);
            StartCoroutine(Move(D));
        }
    }
   

    IEnumerator Move(Dictionary<int, Vector3> U) // 루트3 / 2 (0.85)로 끝맺음 짓는게 좋다.
    {
        cameraShake.mainCamera.transform.position = new Vector3(0, 0, -10);
        cameraShake.mainCamera.transform.rotation = Quaternion.identity;
        cameraShake.mainCamera.transform.localScale = new Vector3(1, 1, 1);
        yield return null;

        IEnumerator size = Size_Change_Infinite(1.6f);
        StartCoroutine(size);
        Plus_Speed = 0;
        
        float A = Get_Slerp_Distance(U[0], U[1], Get_Center_Vector(U[0], U[1], Vector3.Distance(U[0], U[1]) * 0.85f, "anti_clock"));

        float B = Get_Slerp_Distance(U[1], U[2], Get_Center_Vector(U[1], U[2], Vector3.Distance(U[1], U[2]) * 0.85f, "anti_clock"));

        float C = Get_Slerp_Distance(U[2], U[3], Get_Center_Vector(U[2], U[3], Vector3.Distance(U[2], U[3]) * 0.85f, "anti_clock"));

        float Q = Get_Slerp_Distance(U[3], U[4], Get_Center_Vector(U[3], U[4], Vector3.Distance(U[3], U[4]) * 0.85f, "clock"));

        float W = Get_Slerp_Distance(U[4], U[5], Get_Center_Vector(U[4], U[5], Vector3.Distance(U[4], U[5]) * 0.85f, "anti_clock"));

        camera_shake = cameraShake.Shake_Act(.02f, .15f, 1, true);
        StartCoroutine(camera_shake);
        yield return StartCoroutine(Position_Slerp_Temp(U[0], U[1], Get_Center_Vector(U[0], U[1], Vector3.Distance(U[0], U[1]) * 0.85f, "anti_clock"), 15f, OriginCurve, true));
        StopCoroutine(camera_shake);

        yield return StartCoroutine(Rotate_Dec(Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 0, 90), 13));

        camera_shake = cameraShake.Shake_Act(.03f, .18f, 1, true);
        StartCoroutine(camera_shake);
        yield return StartCoroutine(Position_Slerp_Temp(U[1], U[2], Get_Center_Vector(U[1], U[2], Vector3.Distance(U[1], U[2]) * 0.85f, "anti_clock"), B/A * 15f, OriginCurve, true));
        StopCoroutine(camera_shake);

        yield return StartCoroutine(Rotate_Dec(Quaternion.Euler(0, 0, 90), Quaternion.Euler(0, 0, 180), 13));

        camera_shake = cameraShake.Shake_Act(.04f, .21f, 1, true);
        StartCoroutine(camera_shake);
        yield return StartCoroutine(Position_Slerp_Temp(U[2], U[3], Get_Center_Vector(U[2], U[3], Vector3.Distance(U[2], U[3]) * 0.85f, "anti_clock"), C/A * 15f, OriginCurve, true));
        StopCoroutine(camera_shake);

        yield return StartCoroutine(Rotate_Dec(Quaternion.Euler(0, 0, 180), Quaternion.Euler(0, 0, 270), 13));

        camera_shake = cameraShake.Shake_Act(.05f, .24f, 1, true);
        StartCoroutine(camera_shake);
        yield return StartCoroutine(Position_Slerp_Temp(U[3], U[4], Get_Center_Vector(U[3], U[4], Vector3.Distance(U[3], U[4]) * 0.85f, "clock"), Q/A * 15f, OriginCurve, true));
        StopCoroutine(camera_shake);

        yield return StartCoroutine(Rotate_Dec(Quaternion.Euler(0, 0, 270), Quaternion.Euler(0, 0, 360), 13));

        camera_shake = cameraShake.Shake_Act(.06f, .27f, 1, true);
        StartCoroutine(camera_shake);
        yield return StartCoroutine(Position_Slerp_Temp(U[4], U[5], Get_Center_Vector(U[4], U[5], Vector3.Distance(U[4], U[5]) * 0.85f, "anti_clock"), W/A * 20f, OriginCurve, true));
        StopCoroutine(camera_shake);
        StopCoroutine(size);

        cameraShake.mainCamera.transform.position = new Vector3(0, 0, -10);
        cameraShake.mainCamera.transform.rotation = Quaternion.identity;
        cameraShake.mainCamera.transform.localScale = new Vector3(1, 1, 1);
        yield return null;

        Destroy(gameObject);
    }
 
    void Update()
    {
        
    }
}
