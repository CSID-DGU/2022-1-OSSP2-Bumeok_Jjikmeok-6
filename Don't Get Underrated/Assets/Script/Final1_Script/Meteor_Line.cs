using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor_Line : MonoBehaviour
{
    // Start is called before the first frame update

    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        if (TryGetComponent(out SpriteRenderer SR))
        {
            spriteRenderer = SR;
            spriteRenderer.color = Color.white;
        }
    }
    public IEnumerator Change_Color(Color Change, int Count, float time_persist)
    {
        if (spriteRenderer == null)
            yield break;
        float inverse_time_persist = StaticFunc.Reverse_Time(time_persist);
        Color Alpha_1 = Change;
        Color Alpha_0 = new Color(Change.r, Change.g, Change.b, 0);
        for (int i = 0; i < Count; i++)
        {
            float percent = 0;
            while (percent < 1)
            {
                percent += Time.deltaTime * inverse_time_persist;
                spriteRenderer.color = Color.Lerp(Alpha_0, Alpha_1, percent);
                yield return null;
            }
            percent = 0;
            while (percent < 1)
            {
                percent += Time.deltaTime * inverse_time_persist;
                spriteRenderer.color = Color.Lerp(Alpha_1, Alpha_0, percent);
                yield return null;
            }
        }
        yield return null;
    }
    public void Return_Origin_Color(Color Origin)
    {
        spriteRenderer.color = Origin;
    }
    // Update is called once per frame
    private void OnDestroy()
    {
        StopAllCoroutines();
    }

}
