using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emit_Motion : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    AnimationCurve curve;

    SpriteRenderer spriteRenderer;

    Player_Final1 player_final1;

    IEnumerator Actions;
    private void Awake()
    {
        if (TryGetComponent(out SpriteRenderer SR))
            spriteRenderer = SR;
        transform.localScale = Vector3.zero;
        if (GameObject.FindGameObjectWithTag("Player") && GameObject.FindGameObjectWithTag("Player").TryGetComponent(out Player_Final1 PC_T))
            player_final1 = PC_T;
    }
    void Start()
    {
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.3f);
    }

    public bool Check_Valid_Emit()
    {
        if (spriteRenderer != null && player_final1 != null)
            return true;
        return false;
    }

    public IEnumerator Emit_Change_Size()
    {
        transform.localScale = Vector3.zero;
        Vector3 temp_scale;
        while (true)
        {
            temp_scale = transform.localScale;
            while (transform.localScale.x - temp_scale.x < 0.4f)
            {
                transform.localScale = new Vector3(transform.localScale.x + Time.deltaTime * 4, transform.localScale.y + Time.deltaTime * 4, 0);
                yield return null;
            }
            temp_scale = transform.localScale;
            while (temp_scale.x - transform.localScale.x < 0.36f)
            {
                transform.localScale = new Vector3(transform.localScale.x - Time.deltaTime * 3, transform.localScale.y - Time.deltaTime * 3, 0);
                yield return null;
            }
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a + Time.deltaTime * 7);
            yield return null;
        }
    }
    public IEnumerator Expand()
    {
        Stop_Action();
        Actions = Emit_Expand_Circle();
        yield return Actions;
    }
    public void Ready_To_Expand()
    {
        Stop_Action();
        Actions = Emit_Change_Size();
        StartCoroutine(Actions);
    }
    public void Stop_Action()
    {
        if (Actions != null)
            StopCoroutine(Actions);
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
        if (player_final1 != null)
            transform.position = player_final1.transform.position;
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
