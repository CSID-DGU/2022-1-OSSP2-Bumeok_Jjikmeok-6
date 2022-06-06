using System.Collections;
using UnityEngine;

internal static class StaticFunc
{
    public static T[] ShuffleList<T>(T[] list) // 배열 섞기 (최종 1 - 운석 발사 시 사용)
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
    public static IEnumerator WaitForRealSeconds(float seconds) // 게임이 멈춰도 실제 현실 시간을 따른다. 게임 일시 정지 시 사용
    {
        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - startTime < seconds)
            yield return null;
    }
    
    public static float Reverse_Time(float persist_time) // 나눗셈은 속도가 느리기 때문에 이를 미리 캐싱하여 사용
    {
        if (persist_time <= 0)
            return 1;
        else
            return 1 / persist_time;
    }
}