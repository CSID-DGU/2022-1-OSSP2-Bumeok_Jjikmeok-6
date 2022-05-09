using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabOutOfArea : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    StageData stageData;

    float destroyWeight = 2.0f;

    void Start()
    {
        
    }
    private void LateUpdate()
    {
        if ((transform.position.y < stageData.LimitMin.y - destroyWeight) ||
            (transform.position.y > stageData.LimitMax.y + destroyWeight) ||
            (transform.position.x < stageData.LimitMin.x - destroyWeight) ||
            (transform.position.x > stageData.LimitMax.x + destroyWeight))
        {
            Destroy(gameObject);
        }
           
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}