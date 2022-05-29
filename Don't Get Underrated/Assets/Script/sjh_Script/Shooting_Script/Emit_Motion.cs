using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emit_Motion : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    AnimationCurve curve;

    SpriteRenderer spriteRenderer;

    PlayerCtrl_Tengai playerCtrl_Tengai;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform.localScale = Vector3.zero;
        if (GameObject.FindGameObjectWithTag("Player") && GameObject.FindGameObjectWithTag("Player").TryGetComponent(out PlayerCtrl_Tengai PC_T))
            playerCtrl_Tengai = PC_T;
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
                yield return null;
            }
            temp_scale = transform.localScale;
            while (temp_scale.x - transform.localScale.x < .25f)
            {
                transform.localScale = new Vector3(transform.localScale.x - Time.deltaTime * 2, transform.localScale.y - Time.deltaTime * 2, 0);
                yield return null;
            }
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a + Time.deltaTime * 7);
            yield return null;
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
            yield return null;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (playerCtrl_Tengai != null)
            transform.position = playerCtrl_Tengai.transform.position;
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
