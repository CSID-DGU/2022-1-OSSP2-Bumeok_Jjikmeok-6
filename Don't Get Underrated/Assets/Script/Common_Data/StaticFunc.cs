using System.Collections;
using TMPro;
using UnityEngine;

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
            yield return null;
    }
}
