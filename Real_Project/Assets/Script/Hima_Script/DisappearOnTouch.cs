using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearOnTouch : MonoBehaviour {

	public float delaySeconds = 0f;

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.layer == LayerMask.NameToLayer("Player")) {
			Destroy(gameObject, delaySeconds);
		}
	}
}
