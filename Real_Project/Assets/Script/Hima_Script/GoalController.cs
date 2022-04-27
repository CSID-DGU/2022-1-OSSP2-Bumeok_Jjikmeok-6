using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	void OnTriggerEnter2D(Collider2D col) {
		Debug.Log("goal");
		if (col.gameObject.layer == LayerMask.NameToLayer("Player")) {
			LevelManager.instance.Clear();
		}
	}
}
