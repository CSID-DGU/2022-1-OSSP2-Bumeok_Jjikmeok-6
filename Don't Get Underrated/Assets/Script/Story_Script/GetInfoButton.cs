using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class GetInfoButton : MonoBehaviour
{
    [SerializeField]
    GameObject TalkPanel;

    [SerializeField]
    GameObject InfoPanel;

    public void LoadingInfoButton()
    {
        DOTween.KillAll();

        TalkPanel.SetActive(false);
        InfoPanel.SetActive(true);
    }
}

