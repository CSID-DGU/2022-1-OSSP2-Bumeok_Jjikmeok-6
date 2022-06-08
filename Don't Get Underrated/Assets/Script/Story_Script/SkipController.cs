using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class SkipController : MonoBehaviour
{
    public float SkipTimeInterval;
    public int sceneNum = 0;

    void Update()
    {
        StartCoroutine(SkipTime());
    }
    
    public void GoingNextScene()
    {
        StartCoroutine(LoadingNextStage());
    }

    IEnumerator LoadingNextStage()
    {
        float fadeTime = GameObject.Find("Fading").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        //sceneNum++;
        //SceneManager.LoadScene(sceneNum);
        singleTone.SceneNumManage++;
        SceneManager.LoadScene(singleTone.SceneNumManage);
        DOTween.KillAll();
    }

    IEnumerator SkipTime()
    {
        yield return new WaitForSeconds(SkipTimeInterval);
    }

}