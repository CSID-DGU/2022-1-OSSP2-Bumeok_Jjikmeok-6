using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge_Beam_Motion : MonoBehaviour
{
    // Start is called before the first frame update

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
    public IEnumerator Change_Size()
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
            while (temp_scale.x - transform.localScale.x < .24f)
            {
                transform.localScale = new Vector3(transform.localScale.x - Time.deltaTime * 2, transform.localScale.y - Time.deltaTime * 2, 0);
                yield return null;
            }
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a + Time.deltaTime * 7);
            yield return null;
        }

    }
    // Update is called once per frame
}
