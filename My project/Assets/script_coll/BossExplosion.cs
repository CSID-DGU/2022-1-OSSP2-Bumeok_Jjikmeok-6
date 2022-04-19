using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossExplosion : MonoBehaviour
{
    // Start is called before the first frame update

    PlayerController playerController;
    string sceneName;

    public void Setup(PlayerController playerController, string sceneName)
    {
        this.playerController = playerController;
        this.sceneName = sceneName;
    }
    private void OnDestroy()
    {
        playerController.Score += 10000;
        PlayerPrefs.SetInt("Score", playerController.Score);
        SceneManager.LoadScene(sceneName);
       
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
