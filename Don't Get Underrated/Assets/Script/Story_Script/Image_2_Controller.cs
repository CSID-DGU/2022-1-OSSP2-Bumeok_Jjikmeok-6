using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Image_2_Controller : MonoBehaviour
{
    public float interval_time;
    public GameObject Image_2;
    public GameObject Image_1;
    public float ImageVelocity = 5f;

    IEnumerator check_coroutine = null;

    void Start()
    {
        ControllMovementCoroutine(RevealDelayTime());
        StartCoroutine(check_coroutine);
    }

    IEnumerator RevealDelayTime()
    {
        yield return new WaitForSeconds(interval_time);

        if (Image_1.transform.localPosition.y > 80)
        {
            if (this.transform.localPosition.x < 0 && this.transform.localPosition.y > 194) // Start x = -98, Start y = 254, Limitation of x = 100, Limitation of y = -255
            {
                transform.Translate(ImageVelocity, -ImageVelocity, 0);

                Color convertOpacity = this.GetComponent<Image>().color;
                convertOpacity.a += 0.04f;
                this.GetComponent<Image>().color = convertOpacity;

                ControllMovementCoroutine(RevealDelayTime());
                StartCoroutine(check_coroutine);
            }
            else
            {
                ControllMovementCoroutine(PerishDelayTime());
                StartCoroutine(check_coroutine);
            }
        }
        else
        {
            ControllMovementCoroutine(RevealDelayTime());
            StartCoroutine(check_coroutine);
        }
    }

    IEnumerator PerishDelayTime()
    {
        yield return new WaitForSeconds(interval_time);

        if (this.transform.localPosition.x < 70 && this.transform.localPosition.y > 100)
        {
            transform.Translate(ImageVelocity, -ImageVelocity, 0);

            Color convertOpacity = this.GetComponent<Image>().color;
            convertOpacity.a *= 0.8f;
            this.GetComponent<Image>().color = convertOpacity;

            ControllMovementCoroutine(PerishDelayTime());
            StartCoroutine(check_coroutine);
        }
        else
        {
            ControllMovementCoroutine(PerishDelayTime());
            Image_2.SetActive(false);
        }
    }

    void ControllMovementCoroutine(IEnumerator NextCoroutine)
    {
        if (check_coroutine != null)
            StopCoroutine(check_coroutine);
        check_coroutine = NextCoroutine;
    }
}