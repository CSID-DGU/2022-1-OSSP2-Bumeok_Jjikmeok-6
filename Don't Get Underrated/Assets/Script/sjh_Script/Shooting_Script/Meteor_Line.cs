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
        spriteRenderer.color = Color.white;
    }
    public IEnumerator Change_Color(Color Change, int Count, float time_persist)
    {
        float inverse_time_persist = StaticFunc.Reverse_Time(time_persist);
        Color Change_A_1 = Change;
        Color Change_A_0 = new Color(Change.r, Change.g, Change.b, 0);
        for (int i = 0; i < Count; i++)
        {
            float percent = 0;
            while (percent < 1)
            {
                percent += Time.deltaTime * inverse_time_persist;
                spriteRenderer.color = Color.Lerp(Change_A_0, Change_A_1, percent);
                yield return null;
            }
            percent = 0;
            while (percent < 1)
            {
                percent += Time.deltaTime * inverse_time_persist;
                spriteRenderer.color = Color.Lerp(Change_A_1, Change_A_0, percent);
                yield return null;
            }
        }
        yield return null;
    }
    // Update is called once per frame
    private void OnDestroy()
    {
        StopAllCoroutines();
    }

}
