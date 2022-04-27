using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTrapController : MonoBehaviour {

	public Vector3 targetPositionRelative;
	public float rotateAngle;
	public float duration = 1f;
	public bool alternateRepeat = false;
	public bool startAwake = false;
	public bool changeTagToTrap = true;

	private bool triggered = false;
	private float startTime;
	private Vector3 startPosition;
	private Quaternion startRotation;

	void Start () {
		startPosition = transform.position;
		startRotation = transform.rotation;

		if (startAwake) {
			Trigger();
		}
	}

	void FixedUpdate () {
		if (triggered) {
			float currentTime;

			if (alternateRepeat) {
				float modTime = (Time.time - startTime) % (duration * 2);
				currentTime = duration - Mathf.Abs(modTime - duration);
			} else {
				currentTime = Mathf.Min(Time.time - startTime, duration);
			}
				
			transform.position = startPosition + targetPositionRelative * currentTime / duration;

			float angle = rotateAngle * currentTime / duration;
			transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle)) * startRotation;

		}
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (!triggered && col.gameObject.layer == LayerMask.NameToLayer("Player")) {
			Trigger();
		}
	}

	void OnBecameInvisible() {
		Destroy(gameObject, 0.5f);
	}

	void Trigger() {
		triggered = true;
		startTime = Time.time;

		if (changeTagToTrap && !gameObject.CompareTag("Trap")) {
			gameObject.tag = "Trap";
		}
	}
}
