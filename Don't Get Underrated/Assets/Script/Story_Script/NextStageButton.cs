using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class NextStageButton : MonoBehaviour
{

    public int sceneNum = 0;

    public void LoadingInstructionStage()
    {
        sceneNum++;
        SceneManager.LoadScene(sceneNum);
        DOTween.KillAll();
    }
}
