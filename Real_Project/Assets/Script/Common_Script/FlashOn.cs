using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FlashOn : MonoBehaviour
{
    Image image;
    float flashSpeed = 5f;
    Color flashColor = new Color(1, 1, 1, 1);
    float[,] RGB_Color = { { 1, 0, 0 }, { 0, 1, 0 }, { 0, 0, 1 }, { 1, 1, 0 }, { 1, 0, 1 }, { 0, 1, 1 }, { 1, 1, 1 } };
    // Start is called before the first frame update
    
    private void Awake()
    {
        image = GetComponent<Image>();
    }
    public void Origin()
    {
        image.color = new Color(1, 1, 1, 0);
    }
    void Start()
    {
        
    }

    public IEnumerator White_Flash()
    {
        image.color = flashColor;
        yield return new WaitForSeconds(.05f);

        while(true)
        {
            yield return YieldInstructionCache.WaitForEndOfFrame;
            if (image.color.a <= 0)
                yield break;
            image.color = Color.Lerp(image.color, Color.clear, flashSpeed * Time.deltaTime);
        }

    }
    public IEnumerator Thunder()
    {
        while(true)
        {
            int Random_Color = Random.Range(0, 7);
            image.color = new Color(RGB_Color[Random_Color, 0], RGB_Color[Random_Color, 1], RGB_Color[Random_Color, 2], 1);

            float percent = 0;
            while (percent < 1)
            {
                percent += Time.deltaTime * 2.5f;
                image.color = Color.Lerp(image.color, Color.clear, flashSpeed * Time.deltaTime);
                yield return YieldInstructionCache.WaitForEndOfFrame;
            }
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
