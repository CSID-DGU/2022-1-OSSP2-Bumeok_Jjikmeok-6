using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor_Line : MonoBehaviour
{
    // Start is called before the first frame update

    SpriteRenderer spriteRenderer;

   

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(1, 1, 1, 0);
    }

    void Start()
    { 
    }
    public IEnumerator Change_Color(float R1, float R2, float R3)
    {
        while(true)
        {
            float percent = 0;
            Color temp = spriteRenderer.color;
            while (percent < 1)
            {
                percent += Time.deltaTime * 4;
                spriteRenderer.color = Color.Lerp(temp, new Color(R1, R2, R3, 1), percent);
                yield return null;
            }

            temp = spriteRenderer.color;
            percent = 0;
            while (percent < 1)
            {
                percent += Time.deltaTime * 4;
                spriteRenderer.color = Color.Lerp(temp, new Color(R1, R2, R3, 0), percent);
                yield return null;
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
