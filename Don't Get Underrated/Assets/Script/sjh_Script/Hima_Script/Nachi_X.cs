using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nachi_X : Enemy_Info
{
    TrailRenderer trailRenderer;
    // Start is called before the first frame update

    private new void Awake()
    {
        base.Awake();
        trailRenderer = GetComponent<TrailRenderer>();
        cameraShake.mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }
    IEnumerator X_Color_Change(Color Origin_C, Color Change_C, float time_persist)
    {
        for (int i = 0; i < 3; i++)
        {
            percent = 0;
            while (percent < 1)
            {
                percent += Time.deltaTime / time_persist;
                trailRenderer.endColor = Color.Lerp(Origin_C, Change_C, percent);
                trailRenderer.startColor = Color.Lerp(Origin_C, Change_C, percent);
                yield return null;
            }
        }
    }
    public IEnumerator Move(int flag)
    {
        yield return StartCoroutine(Circle_Move(90, flag * 4, 0, 0.3f, 0.3f, transform.position.x, transform.position.y, 0.5f));
        yield return StartCoroutine(Position_Lerp(transform.position, transform.position + new Vector3(-4 * flag, 2f, 0), 0.8f, declineCurve));
        yield return StartCoroutine(Position_Lerp(transform.position, transform.position + new Vector3(16 * flag, -8f, 0), 0.2f, declineCurve));
        StartCoroutine(cameraShake.Shake_Act(0.2f, 0.2f, 0.4f, false));
        yield return StartCoroutine(X_Color_Change(Color.white, new Color(1, 1, 1, 0), 1));

        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
