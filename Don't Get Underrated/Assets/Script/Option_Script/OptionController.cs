using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionController : MonoBehaviour
{
    public GameObject OptionPage;
    AudioSource bgm;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(OptionPage.activeSelf == false)
            {
                OptionVisible();
            }
            else
            {
                OptionInvisible();
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
}
