using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectAutoDestroy : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private float Destroy_Time;

    private void Start()
    {
        StartCoroutine(Auto_Destroy());
    }
    private IEnumerator Auto_Destroy()
    {
        yield return YieldInstructionCache.WaitForSeconds(Destroy_Time);
        Destroy(gameObject);
        yield break;
    }
}
