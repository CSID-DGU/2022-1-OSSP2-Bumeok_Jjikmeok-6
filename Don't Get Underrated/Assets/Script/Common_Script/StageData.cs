using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StageData : ScriptableObject // 제한 영역을 설정
{
    [SerializeField]
    Vector2 limitMin;

    [SerializeField]
    Vector2 limitMax;

    public Vector2 LimitMin
    {
        get { return limitMin; }
        set { limitMin = value; }
    }
    public Vector2 LimitMax
    {
        get { return limitMax; }
        set { limitMax = value; }
    }
}