using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderBolt : Enemy_Info
{
    // Start is called before the first frame update

    private new void Awake()
    {
        base.Awake();
        flashOn = GameObject.Find("Flash").GetComponent<FlashOn>();
        cameraShake.mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }


    void Start()
    {
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        
        //yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(transform.position.x, 0, 0), 0.5f, OriginCurve));
        StartCoroutine(flashOn.Flash(new Color(1, 1, 1, 1), 0.1f, 7));
        camera_shake = cameraShake.Shake_Act(.1f, .26f, 0.3f, false);
        StartCoroutine(camera_shake);
        
        yield return null;
    }

    public void DestroyNow()
    {

       // StartCoroutine(I_DestroyNow());
        Destroy(gameObject);
    }

    //IEnumerator I_DestroyNow()
    //{
    //    // return YieldInstructionCache.WaitForSeconds(2f);
      
    //}

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
