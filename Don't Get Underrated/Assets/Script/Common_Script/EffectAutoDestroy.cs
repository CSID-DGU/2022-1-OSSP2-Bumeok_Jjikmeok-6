using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectAutoDestroy : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    float Destroy_Time;



    void Start()
    {
        StartCoroutine(Auto_Destroy());
    }
    IEnumerator Auto_Destroy()
    {
        yield return YieldInstructionCache.WaitForSeconds(Destroy_Time);
        Destroy(gameObject);
        yield break;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
