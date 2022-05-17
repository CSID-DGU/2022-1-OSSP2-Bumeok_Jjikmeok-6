using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emit_Motion : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    AnimationCurve curve;

    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform.localScale = Vector3.zero;

    }
    void Start()
    {
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, .3f);
    }

    public IEnumerator Emit_Change_Size()
    {
        Vector3 temp_scale;
        while (true)
        {
            temp_scale = transform.localScale;
            while (transform.localScale.x - temp_scale.x < .3f)
            {
                transform.localScale = new Vector3(transform.localScale.x + Time.deltaTime * 3, transform.localScale.y + Time.deltaTime * 3, 0);
                yield return YieldInstructionCache.WaitForEndOfFrame;
            }
            temp_scale = transform.localScale;
            while (temp_scale.x - transform.localScale.x < .25f)
            {
                transform.localScale = new Vector3(transform.localScale.x - Time.deltaTime * 2, transform.localScale.y - Time.deltaTime * 2, 0);
                yield return YieldInstructionCache.WaitForEndOfFrame;
            }
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a + Time.deltaTime * 7);
            yield return YieldInstructionCache.WaitForEndOfFrame;
        }
    }
    public IEnumerator Emit_Expand_Circle()
    {
        yield return YieldInstructionCache.WaitForSeconds(0.3f);
        float percent = 0;
        Vector3 temp_scale = transform.localScale;
        while (percent < 1)
        {
            percent += Time.deltaTime;
            transform.localScale = Vector3.Lerp(temp_scale, temp_scale * 21, curve.Evaluate(percent));
            yield return YieldInstructionCache.WaitForEndOfFrame;
           
        }
    }
    public void Set_Color()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0);
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = GameObject.FindGameObjectWithTag("Playerrr").transform.position;
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
