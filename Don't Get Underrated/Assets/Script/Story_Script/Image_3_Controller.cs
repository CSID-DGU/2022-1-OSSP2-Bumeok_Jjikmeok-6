using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Image_3_Controller : MonoBehaviour
{
    public float interval_time;
    public GameObject Image_2;
    public GameObject StartButton;
    public GameObject Title;
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

        if (Image_2.transform.localPosition.y < 130) 
        {
            if (this.transform.localPosition.x > 0) // Start x = 159, Limitation of x = -154
            {
                transform.Translate(-ImageVelocity, 0, 0);

                Color convertOpacity = this.GetComponent<Image>().color;
                convertOpacity.a += 0.05f;
                this.GetComponent<Image>().color = convertOpacity;
                
                ControllMovementCoroutine(RevealDelayTime());
                StartCoroutine(check_coroutine);
                yield break;
            }
            else
            {
                ControllMovementCoroutine(RevealDelayTime());
                StartCoroutine(DelayAppear());
            }
        }
        else
        {
            ControllMovementCoroutine(RevealDelayTime());
            StartCoroutine(check_coroutine);
        }
    }

    IEnumerator DelayAppear()
    {
        yield return new WaitForSeconds(0.5f);
        StartButton.SetActive(true);
        Title.SetActive(true);
    }

    void ControllMovementCoroutine(IEnumerator NextCoroutine)
    {
        if (check_coroutine != null)
            StopCoroutine(check_coroutine);
        check_coroutine = NextCoroutine;
    }
}