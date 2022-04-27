using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeText : MonoBehaviour {

	Text text;

	void Awake() {
		text = GetComponent<Text>();
	}

	void Start () {
		float time = LevelManager.instance.playTime;
		float seconds = time % 60f;
		float minutes = Mathf.FloorToInt(time / 60f);

		string score = System.String.Format("{0}m {1:F3}s", minutes, seconds);
		text.text= score;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
