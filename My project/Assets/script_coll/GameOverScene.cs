using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScene : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    Text text;
    void Start()
    {
        text.text = "SCORE : " + PlayerPrefs.GetInt("Score");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
