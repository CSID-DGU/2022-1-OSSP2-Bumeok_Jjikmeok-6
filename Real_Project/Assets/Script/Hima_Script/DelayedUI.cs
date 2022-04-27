using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DelayedUI : MonoBehaviour {

	public float delay;

	private Text text;

	void Start () {
//		text = GetComponent<Text>();
//		text.enabled = false;
		transform.localScale = Vector3.zero;
		StartCoroutine(ShowDelayed());
	}

	IEnumerator ShowDelayed() {
		yield return new WaitForSeconds(delay);
//		text.enabled = true;
		transform.localScale = Vector3.one;
	}
}
