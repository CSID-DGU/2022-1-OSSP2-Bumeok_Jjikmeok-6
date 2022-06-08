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
        StopAllCoroutines();
        StartCoroutine(Fade_Out());
    }

    IEnumerator Fade_Out()
    {
        if (GameObject.Find("Network_Confirm") && GameObject.Find("Network_Confirm").TryGetComponent(out Network_On NO))
            NO.StopAllCoroutines();

        if (GameObject.Find("Network_Sprite") && GameObject.Find("Network_Sprite").TryGetComponent(out SpriteColor s1))
            yield return s1.StartCoroutine(s1.Change_Color_Real_Time(Color.black, 2));

        singleTone.request = UnityWebRequest.Get("http://localhost:3000/log_out");
        yield return singleTone.request.SendWebRequest();

        Time.timeScale = 1;
        Application.Quit();

    }
    public void Back_To_Login()
    {
        StopAllCoroutines();
        StartCoroutine(Back_To_Login_Fade_Out());
    }
    IEnumerator Back_To_Login_Fade_Out()
    {
        if (GameObject.Find("Network_Confirm") && GameObject.Find("Network_Confirm").TryGetComponent(out Network_On NO))
            NO.StopAllCoroutines();

        if (GameObject.Find("Network_Sprite") && GameObject.Find("Network_Sprite").TryGetComponent(out SpriteColor s1))
            yield return s1.StartCoroutine(s1.Change_Color_Real_Time(Color.black, 2));

        Time.timeScale = 1;
        singleTone.SceneNumManage = 0;
        SceneManager.LoadScene(singleTone.SceneNumManage);
    }
}