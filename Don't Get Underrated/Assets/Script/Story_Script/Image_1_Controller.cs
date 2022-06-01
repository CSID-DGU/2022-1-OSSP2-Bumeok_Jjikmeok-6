using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Image_1_Controller : MonoBehaviour
{
    public float interval_time;
    public GameObject Image_1;
    public float ImageVelocity = 5f;

    IEnumerator check_coroutine = null;

    void Start()
    {
        ControllMovementCoroutine(PerishDelayTime());
        StartCoroutine(PerishDelayTime());
    }

    IEnumerator PerishDelayTime()
    {
        yield return new WaitForSeconds(interval_time);

        if (this.transform.localPosition.y < 100) // Start y = 4.49, Limitation of y = 179
        {
            Debug.Log("this.transform.localPosition.y: " + this.transform.localPosition.y);
            transform.Translate(0, ImageVelocity, 0);

            Color convertOpacity = this.GetComponent<Image>().color;
            convertOpacity.a *= 0.85f;
            this.GetComponent<Image>().color = convertOpacity;

            ControllMovementCoroutine(PerishDelayTime());
            StartCoroutine(check_coroutine); // Recurrence occurred
        }
        else
        {
            ControllMovementCoroutine(PerishDelayTime());
            Image_1.SetActive(false);
        }

        Debug.Log("After this.transform.localPosition.y: " + this.transform.localPosition.y);
    }

    void ControllMovementCoroutine(IEnumerator NextCoroutine)
    {
        if (check_coroutine != null)
            StopCoroutine(check_coroutine);
        check_coroutine = NextCoroutine;
    }
}