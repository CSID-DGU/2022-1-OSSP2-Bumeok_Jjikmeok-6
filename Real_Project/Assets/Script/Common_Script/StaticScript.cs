using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal static class StaticStript
{
    public static T[] ShuffleList<T>(T[] list)
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
}
