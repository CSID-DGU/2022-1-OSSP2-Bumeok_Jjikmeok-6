using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

internal static class StaticFunc
{
    public static T[] ShuffleList<T>(T[] list) // 배열 섞기
    {
        int random1, random2;
        T temp;

        for (int i = 0; i < list.Length; ++i)
        {
            random1 = Random.Range(0, list.Length);
            random2 = Random.Range(0, list.Length);

            temp = list[random1];
            list[random1] = list[random2];
            list[random2] = temp;
        }
        return list;
    }
    public static IEnumerator WaitForRealSeconds(float seconds) // 게임이 멈춰도 실제 현실 시간을 따른다
    {
        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - startTime < seconds)
        {
            yield return null;
        }
    }
    public static IEnumerator Position_Lerp(GameObject copy, Vector3 start_location, Vector3 last_location, float time_ratio, AnimationCurve curve)
    {
        float percent = 0;
        while (percent < 1)
        {
            percent += (Time.deltaTime * time_ratio);
            copy.transform.position = Vector3.Lerp(start_location, last_location, curve.Evaluate(percent));
            yield return YieldInstructionCache.WaitForEndOfFrame;
        }
    }
    public static IEnumerator Warning(TextMeshProUGUI warning_UI, string warning_message, float time_ratio)
    {
        warning_UI.text = warning_message;
        while (warning_UI.color.a < 1.0f)
        {
            warning_UI.color = new Color(warning_UI.color.r, warning_UI.color.g, warning_UI.color.b, warning_UI.color.a + Time.deltaTime * time_ratio);
            yield return null;
        }
        while (warning_UI.color.a > 0.0f)
        {
            warning_UI.color = new Color(warning_UI.color.r, warning_UI.color.g, warning_UI.color.b, warning_UI.color.a - Time.deltaTime * time_ratio);
            yield return null;
        }
    }
    //public static IEnumerator Rotation_Lerp(GameObject copy, float rot_Speed, float rot_radius, float time_ratio, GameObject Bullet)
    //{
    //    //gameObject, 7, 250, .5f, Boss_Weapon[4]

    //    float percent = 0;
    //    while (percent < 1)
    //    {
    //        percent += (Time.deltaTime * time_ratio);
    //        copy.transform.Rotate(Vector3.forward * rot_Speed * rot_radius * Time.deltaTime);
    //        for (int i = 0; i < 4; i++)
    //        {
    //            if (i == 3)
    //            {

    //                GameObject T1 = Instantiate(Bullet);
    //                T1.transform.position = transform.position;
    //                T1.transform.rotation = transform.rotation;
    //                yield return YieldInstructionCache.WaitForEndOfFrame;
    //            }
    //        }
    //    }
    //}
    public static IEnumerator Position_Slerp()
    {
        yield return null;
    }

    public static IEnumerator Color_Lerp()
    {
        yield return null;
    }
}
