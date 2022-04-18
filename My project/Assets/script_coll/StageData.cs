using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StageData : ScriptableObject
{
    [SerializeField]
    Vector2 limitMin;

    [SerializeField]
    Vector2 limitMax;

    public Vector2 LimitMin => limitMin;

    public Vector2 LimitMax => limitMax;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
