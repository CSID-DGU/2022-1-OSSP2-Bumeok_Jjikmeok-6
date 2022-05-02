using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    // Start is called before the first frame update

    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void Change_color(float r, float g, float b)
    {
        StartCoroutine(I_Change(r, g, b));
    }
    public bool Correct_Color()
    {
        return spriteRenderer.color != new Color(1, 1, 1);
    }
    IEnumerator I_Change(float r, float g, float b)
    {
        spriteRenderer.color = new Color(r/255f, g/255f, b/255f);
        yield return null;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
