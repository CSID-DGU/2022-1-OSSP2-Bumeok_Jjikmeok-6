using System.Collections;
using TMPro;
using UnityEngine;

internal static class StaticFunc
{
    public static T[] ShuffleList<T>(T[] list) // �迭 ����
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
    public static IEnumerator WaitForRealSeconds(float seconds) // ������ ���絵 ���� ���� �ð��� ������
    {
        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - startTime < seconds)
        {
            yield return null;
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

}
