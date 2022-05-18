using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Razor_Launch : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Color_Change");
    }
    IEnumerator Color_Change()
    {
        while(true)
        {
            spriteRenderer.color = new Color(214/255f, 102/255f, 102/255f);
            yield return new WaitForSeconds(0.2f);

            spriteRenderer.color = new Color(107/255f, 51/255f, 51/255f);
            yield return new WaitForSeconds(0.2f);
        }
    }

    // Update is called once per frame
}
