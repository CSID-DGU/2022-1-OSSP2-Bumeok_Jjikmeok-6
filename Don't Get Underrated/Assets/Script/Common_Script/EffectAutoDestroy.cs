using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectAutoDestroy : MonoBehaviour
{
    // Start is called before the first frame update


    void Start()
    {
        StartCoroutine(Auto_Destroy());
    }
    IEnumerator Auto_Destroy()
    {
        yield return YieldInstructionCache.WaitForSeconds(2f);
        Destroy(gameObject);
        yield break;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
