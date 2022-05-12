using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targetting_Effect : MonoBehaviour
{
    // Start is called before the first frame update

    SpriteRenderer spriteRenderer;

    IEnumerator change_color_and_scale;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
       
    }
    public void Init()
    {
        transform.localScale = new Vector3(.5f, .5f, 0);
        spriteRenderer.color = new Color(0, 0, 0, 1);
    }
    public void Scale_Color(GameObject e)
    {
       
        if (change_color_and_scale != null)
            StopCoroutine(change_color_and_scale);
        change_color_and_scale = Change_Color_And_Scale(e);
        StartCoroutine(change_color_and_scale);
    }
    IEnumerator Change_Color_And_Scale(GameObject e)
    {
        float percent = 0;
        while(true)
        {
            transform.position = e.transform.position;

            if (transform.localScale.x >= 0.3f)
            {
                percent += Time.deltaTime * 5;
                transform.localScale = new Vector3(transform.localScale.x - Time.deltaTime, transform.localScale.y - Time.deltaTime, transform.localScale.z);
            }
            else
            {
                percent += Time.deltaTime * 2;
                if (percent <= 0.5f)
                    spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a - Time.deltaTime * 4);

                else if (percent >= 0.5f && percent < 1)
                    spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a + Time.deltaTime * 4);

                else
                    percent = 0;
            }
               
            yield return null;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
