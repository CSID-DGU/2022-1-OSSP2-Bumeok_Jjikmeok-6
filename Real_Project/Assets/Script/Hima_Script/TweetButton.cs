using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class TweetButton : MonoBehaviour {

	private string baseURL = "https://twitter.com/intent/tweet?url=https://himajin-no-tameiki.github.io/v2/&text=";
	private string textParamFormat = "I Wanna Be Himawari(v2)を{0}でクリアしました。";

	[DllImport("__Internal")]
	private static extern void OpenURL(string str);

	public void OnClick() {
		float time = LevelManager.instance.playTime;
		float seconds = time % 60f;
		float minutes = Mathf.FloorToInt(time / 60f);

		string score = System.String.Format("{0}m {1:F3}s", minutes, seconds);

		string textParam = WWW.EscapeURL(System.String.Format(textParamFormat, score));
		string url = baseURL + textParam;
		Debug.Log(url);

		#if UNITY_EDITOR
		Application.OpenURL(url);
		#elif UNITY_WEBGL
		OpenURL(url);
		#else
		Application.OpenURL(url);
		#endif

	}
}
