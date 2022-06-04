using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

public class LoadingProgress : MonoBehaviour
{
    private static string nextScene;
    public float currentValue;
    public float speed = 25;

    [SerializeField]
    TextMeshProUGUI Wait_text;

    [SerializeField]
    TextMeshProUGUI Percent;

    [SerializeField]
    Slider slider;

    void Start()
    {
        StartCoroutine(LoadScene());
    }
    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }
    IEnumerator LoadScene()
    {
        yield return YieldInstructionCache.WaitForSeconds(1f);
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;
        currentValue = 0;
        yield return null;

        IEnumerator wait_text_num = Wait_Text_IEnum();
        StartCoroutine(wait_text_num);

        while (!op.isDone)
        {
            yield return null;
            if (currentValue < 100)
            {
                currentValue += speed * Time.deltaTime;
                Percent.text = ((int)currentValue).ToString() + "%";
            }
            else
            {
                StopCoroutine(wait_text_num);
                Wait_text.text = "Done";
                yield return YieldInstructionCache.WaitForSeconds(1f);
                op.allowSceneActivation = true;
                yield break;
            }
            slider.value = currentValue * 0.01f;
        }
    }
    IEnumerator Wait_Text_IEnum()
    {
        while(true)
        {
            Wait_text.text = "잠시만 기다려주세요..";
            yield return YieldInstructionCache.WaitForSeconds(0.2f);

            Wait_text.text = "잠시만 기다려주세요....";
            yield return YieldInstructionCache.WaitForSeconds(0.2f);

            Wait_text.text = "잠시만 기다려주세요......";
            yield return YieldInstructionCache.WaitForSeconds(0.2f);
        }
      
    }
}
