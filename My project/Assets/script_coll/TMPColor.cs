using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TMPColor : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    float lerpTime = 0.1f;
    TextMeshProUGUI textBossWarning;

    private void Awake()
    {
        textBossWarning = GetComponent<TextMeshProUGUI>();
    }
    private void OnEnable()
    {
        StartCoroutine("ColorLerpLoop");
    }
    IEnumerator ColorLerpLoop()
    {
        while(true)
        {
            yield return StartCoroutine(ColorLerp(Color.white, Color.red));
            yield return StartCoroutine(ColorLerp(Color.red, Color.white));
        }
    }
    IEnumerator ColorLerp (Color startColor, Color endColor)
    {
        float currentTime = 0.0f;
        float percent = 0.0f;

        while(percent < 1)
        { 
            currentTime += Time.deltaTime;
            percent = currentTime / lerpTime;

            textBossWarning.color = Color.Lerp(startColor, endColor, percent);

            yield return null;
        }

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
