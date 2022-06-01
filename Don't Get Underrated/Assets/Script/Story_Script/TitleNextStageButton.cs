using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleNextStageButton : MonoBehaviour
{
    public int sceneNum = 0;


    public void GoingNextScene()
    {
        StartCoroutine(LoadingNextStage());
    }

    IEnumerator LoadingNextStage()
    {
        float fadeTime = GameObject.Find("Fading").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        sceneNum++;
        SceneManager.LoadScene(sceneNum);
    }
}
