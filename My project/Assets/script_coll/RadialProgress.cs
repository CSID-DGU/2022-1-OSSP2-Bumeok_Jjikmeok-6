using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RadialProgress : MonoBehaviour
{
    public static string nextScene;
    public Text ProgressIndicator;
    public Image LoadingBar;
    public float currentValue;
    public float speed = 25;
    // Start is called before the first frame update
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
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;
        currentValue = 0;
        while (!op.isDone)
        {
            yield return null;
            if (currentValue < 100)
            {
                currentValue += speed * Time.deltaTime;
                ProgressIndicator.text = ((int)currentValue).ToString() + "%";
            }
            else
            {
                ProgressIndicator.text = "Done";
                yield return new WaitForSeconds(1);
                op.allowSceneActivation = true;
                yield break;
            }
            LoadingBar.fillAmount = currentValue / 100;
        }

    }
    // Update is called once per frame
    //public static string nextScene;

    //[SerializeField]
    //Image progressBar;

    //private void Start()
    //{
    //    StartCoroutine(LoadScene());
    //}

    //public static void LoadScene(string sceneName)
    //{
    //    nextScene = sceneName;
    //    SceneManager.LoadScene("LoadingScene");
    //}
    //IEnumerator LoadScene()
    //{
    //    yield return null;
    //    AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
    //    op.allowSceneActivation = false;
    //    float timer = 0.0f;
    //    while (!op.isDone)
    //    {
    //        yield return null;
    //        timer += Time.deltaTime;
    //        if (op.progress < 0.9f)
    //        {
    //            progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer);
    //            if (progressBar.fillAmount >= op.progress)
    //            {
    //                timer = 0f;
    //            }
    //        }
    //        else
    //        {
    //            progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);
    //            if (progressBar.fillAmount == 1.0f)
    //            {
    //                op.allowSceneActivation = true;
    //                yield break;
    //            }
    //        }
    //    }

    //}
}
