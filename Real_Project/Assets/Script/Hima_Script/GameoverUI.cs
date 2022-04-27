using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameoverUI : MonoBehaviour {

	private Image imageComp;

	void Awake() {
		LevelManager.instance.gameoverUI = this;
	}

	void Start () {
		imageComp = GetComponent<Image>();
		imageComp.enabled = false;
	}

	void Update () {
		
	}

	public void Trigger() {
		StartCoroutine(Show());
	}

	IEnumerator Show() {
		yield return new WaitForSeconds(1);
		imageComp.enabled = true;
	}
}
