using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	[HideInInspector]
	public static LevelManager instance = null;
	[HideInInspector]
	public GameObject playerObject = null;
	[HideInInspector]
	public float playTime = 0f;
	[HideInInspector]
	public Vector3 savedPosition;

	public Vector3 initialPosition = new Vector3(-26f, 14f, 0f);
	public GameoverUI gameoverUI;
	public AudioSource deathSound;

	private bool playing = true;

	void Awake () {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);
	}

	void Start () {
		savedPosition = initialPosition;
	}

	void Update () {
		if (playing) {
			playTime += Time.deltaTime;
		}

		if (Input.GetButtonDown("Reset")) {
			Scene scene = SceneManager.GetActiveScene();
			SceneManager.LoadScene(scene.name);
			playing = true;
		}
	}

	public void SaveGame(Vector3 position) {
		Debug.Log("Save!!");
		if (playerObject)
			savedPosition = position;
	}

	public void OnDeath() {
		if (gameoverUI)
			gameoverUI.Trigger();

		if (deathSound)
			deathSound.Play();

		playing = false;
	}

	public void Clear() {
		savedPosition = initialPosition;
		SceneManager.LoadScene("Score");
		playing = false;
	}

	public void GameRestart() {
		playing = true;
		playTime = 0f;
		SceneManager.LoadScene("Main");
	}
}
