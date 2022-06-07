using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class OptionController : MonoBehaviour
{
    public GameObject OptionPage;

    void Update()
    {
        if (singleTone.ESC_On)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (OptionPage.activeSelf == false)
                {
                    OptionVisible();
                }
                else
                {
                    OptionInvisible();
                }
            }
        }
    }

    void OptionVisible()
    {
        Time.timeScale = 0;
        OptionPage.SetActive(true);
    }

    public void OptionInvisible()
    {
        Time.timeScale = 1;
        OptionPage.SetActive(false);
    }
    public void Real_End()
    {
        StartCoroutine(Fade_Out());
    }

    IEnumerator Fade_Out()
    {
        if (GameObject.Find("Network_Sprite") && GameObject.Find("Network_Sprite").TryGetComponent(out SpriteColor s1))
            yield return s1.StartCoroutine(s1.Change_Color_Real_Time(Color.black, 2));
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
