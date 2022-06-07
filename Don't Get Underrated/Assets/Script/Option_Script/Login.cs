using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class Login : MonoBehaviour
{
    [SerializeField]
    TMP_InputField ID_In;

    [SerializeField]
    TMP_InputField PWD_IN;

    [SerializeField]
    TextMeshProUGUI infoText;

    [SerializeField]
    GameObject Popup;

    [SerializeField]
    GameObject X_Button;

    private void Awake()
    {
        singleTone.request = null;
        singleTone.main_stage_1_score = 0;
        singleTone.main_stage_2_score = 0;
        singleTone.main_stage_3_score = 0;
        singleTone.final_stage_1_score = 0;
        singleTone.final_stage_2_score = 0;

        singleTone.id = "";
        singleTone.ESC_On = true;
        singleTone.EasterEgg = false;
        singleTone.Music_Volume = 1;
        singleTone.Music_Decrease = true;

    }
    [System.Serializable]
    public class UserInfo
    {
        public int keycode;
        public string id;
        public string pwd;
    }

    [System.Serializable]
    public class Login_Success
    {
        public UserInfo user_info;
        public string success_message;
    }
    [System.Serializable]
    public class Haha
    {
        public string user_info;
        public string message;
    }
    public void ButtonClick(int param)
    {
        StopAllCoroutines();
        infoText.text = "";
        if (param == 0)
            StartCoroutine(Server_Login());
        else if (param == 1)
            StartCoroutine(Server_Logout());
    }
    public void GameStart()
    {
        StopAllCoroutines();
        infoText.text = "";
        StartCoroutine(GameStart_Behave());
    }
    IEnumerator Server_Logout()
    {
        Popup.SetActive(true);
        X_Button.SetActive(false);

        IEnumerator wait_load = Wait_Load();
        StartCoroutine(wait_load);

        singleTone.request = UnityWebRequest.Get("http://localhost:3000/log_out");
        yield return singleTone.request.SendWebRequest();

        StopCoroutine(wait_load);

        if ((singleTone.request.result == UnityWebRequest.Result.ConnectionError) ||
          (singleTone.request.result == UnityWebRequest.Result.ProtocolError) ||
          (singleTone.request.result == UnityWebRequest.Result.DataProcessingError))
        {
            X_Button.SetActive(true);
            infoText.text = singleTone.request.error + '\n' + singleTone.request.downloadHandler.text;
        }
        else
        {
            X_Button.SetActive(true);
            infoText.text = singleTone.request.downloadHandler.text + '\n' + "로그아웃 성공!";
        }
        yield break;
    }
    IEnumerator Server_Login()
    {
        Popup.SetActive(true);
        X_Button.SetActive(false);

        IEnumerator wait_load = Wait_Load();
        StartCoroutine(wait_load);

        WWWForm form = new WWWForm();

        form.AddField("id", ID_In.text);
        form.AddField("pwd", PWD_IN.text);

        singleTone.request = UnityWebRequest.Post("http://localhost:3000/log_in", form);
        yield return singleTone.request.SendWebRequest();

        StopCoroutine(wait_load);

        if ((singleTone.request.result == UnityWebRequest.Result.ConnectionError) ||
           (singleTone.request.result == UnityWebRequest.Result.ProtocolError) ||
           (singleTone.request.result == UnityWebRequest.Result.DataProcessingError))
        {
            X_Button.SetActive(true);
            infoText.color = Color.red;
            infoText.text = singleTone.request.error + '\n' + singleTone.request.downloadHandler.text;
        }
        else
        {
            X_Button.SetActive(true);
            infoText.color = Color.black;
            Login_Success data = JsonUtility.FromJson<Login_Success>(singleTone.request.downloadHandler.text);
            singleTone.id = data.user_info.id;
            infoText.text = data.success_message;
        }
        yield break;
    }

    IEnumerator GameStart_Behave()
    {
        Popup.SetActive(true);
        X_Button.SetActive(false);

        IEnumerator wait_load = Wait_Load();
        StartCoroutine(wait_load);

        singleTone.request = UnityWebRequest.Get("http://localhost:3000/continue_connect");
        yield return singleTone.request.SendWebRequest();

        StopCoroutine(wait_load);

        if ((singleTone.request.result == UnityWebRequest.Result.ConnectionError) ||
            (singleTone.request.result == UnityWebRequest.Result.ProtocolError) ||
            (singleTone.request.result == UnityWebRequest.Result.DataProcessingError))
        {
            X_Button.SetActive(true);
            infoText.color = Color.red;
            infoText.text = singleTone.request.error + '\n' + singleTone.request.downloadHandler.text;
        }

        else
        {
            Haha d = JsonUtility.FromJson<Haha>(singleTone.request.downloadHandler.text);
            if (singleTone.id != d.user_info)
            {
                X_Button.SetActive(true);
                infoText.color = Color.red;
                infoText.text = "로그 아웃 후 다시 로그인을 시도해주세요!";
            }
            else
            {
                X_Button.SetActive(false);
                infoText.color = Color.blue;
                infoText.text = "게임을 시작합니다!";
                yield return YieldInstructionCache.WaitForSeconds(1);
                LoadingProgress.LoadScene(3);
                yield break;
            }

        }
        yield break;
    }

    IEnumerator Wait_Load()
    {
        infoText.color = Color.black;
        while (true)
        {
            infoText.text = "로딩 중....";
            yield return YieldInstructionCache.WaitForSeconds(0.15f);

            infoText.text = "로딩 중......";
            yield return YieldInstructionCache.WaitForSeconds(0.15f);

            infoText.text = "로딩 중........";
            yield return YieldInstructionCache.WaitForSeconds(0.15f);
        }
    }
}