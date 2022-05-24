using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Time_Over_Text : MonoBehaviour
{
    // Start is called before the first frame update
    float delta_ratio;

    float percent;

    private void Awake()
    {
        transform.localScale = new Vector3(1f, 1f, 0);
        delta_ratio = 1;
    }
    private void Start()
    {
        StartCoroutine(Change());
    }
    IEnumerator Change()
    {
        yield return StartCoroutine(Size_Change(transform.localScale, new Vector3(0.05f, 4, 0), true));

        yield return StartCoroutine(Size_Change(transform.localScale, new Vector3(0.65f, 0.65f, 0), false));

        yield return StartCoroutine(Size_Change(transform.localScale, new Vector3(0.1f, 3, 0), false));

        yield return StartCoroutine(Size_Change(transform.localScale, new Vector3(0.5f, 0.5f, 0), false));
    }
    IEnumerator Size_Change(Vector3 Origin, Vector3 Change, bool is_Plus_Speed)
    {
        percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime * delta_ratio;
            transform.localScale = Vector3.Lerp(Origin, Change, percent);
            if (is_Plus_Speed)
                delta_ratio += 0.03f;
            yield return null;
        }
    }
}
