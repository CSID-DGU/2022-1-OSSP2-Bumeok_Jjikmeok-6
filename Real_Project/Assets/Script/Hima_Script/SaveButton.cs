using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SaveButton : MonoBehaviour, IDamageable {

//	public UnityEvent saveEvent;
	public Color defaultColor;
	public Color activeColor;

	private SpriteRenderer sr;
	private Coroutine turnOffCoroutine;

	// Use this for initialization
	void Start () {
//		if (saveEvent == null) 
//			saveEvent = new UnityEvent();

		sr = GetComponent<SpriteRenderer>();
		sr.color = defaultColor;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Damage(float damage) {
		Save();
		TurnOn();
	}

	void Save() {
//		saveEvent.Invoke();
		LevelManager.instance.SaveGame(transform.position + Vector3.up * 2.5f);

		if (turnOffCoroutine != null)
			StopCoroutine(turnOffCoroutine);

		TurnOn();
		turnOffCoroutine = StartCoroutine(TurnOffLater());
	}

	void TurnOn() {
		sr.color = activeColor;

	}

	void TurnOff() {
		sr.color = defaultColor;
	}

	IEnumerator TurnOffLater() {
		yield return new WaitForSeconds(0.5f);
		TurnOff();
	}
}
