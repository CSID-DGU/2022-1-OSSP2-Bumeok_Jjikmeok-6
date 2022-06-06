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

    IEnumerator wait_load;
    private void Awake()
    {
        singleTone.id = "";
        wait_load = null;
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
        infoText.text = "";
        if (param == 0)
            StartCoroutine(Server_Login());
        else if (param == 1)
            StartCoroutine(Server_Logout());
    }
    public void GameStart()
    {
        infoText.text = "";
        StartCoroutine(GameStart_Behave());
    }
    IEnumerator Server_Logout()
    {
        Popup.SetActive(true);
        wait_load = Wait_Load();
        StartCoroutine(wait_load);

        singleTone.request = UnityWebRequest.Get("http://localhost:3000/log_out");
        yield return singleTone.request.SendWebRequest();

        StopCoroutine(wait_load);

        if ((singleTone.request.result == UnityWebRequest.Result.ConnectionError) ||
          (singleTone.request.result == UnityWebRequest.Result.ProtocolError) ||
          (singleTone.request.result == UnityWebRequest.Result.DataProcessingError))
        {
            infoText.text = singleTone.request.error + '\n' + singleTone.request.downloadHandler.text;
        }
        else
        {
            infoText.text = singleTone.request.downloadHandler.text + '\n' + "로그아웃 성공!";
        }
        yield break;
    }
    IEnumerator Server_Login()
    {
        Popup.SetActive(true);

        wait_load = Wait_Load();
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
            infoText.color = Color.red;
            infoText.text = singleTone.request.error + '\n' + singleTone.request.downloadHandler.text;
        }
        else
        {
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
        singleTone.request = UnityWebRequest.Get("http://localhost:3000/continue_connect");
        yield return singleTone.request.SendWebRequest();

        if ((singleTone.request.result == UnityWebRequest.Result.ConnectionError) ||
            (singleTone.request.result == UnityWebRequest.Result.ProtocolError) ||
            (singleTone.request.result == UnityWebRequest.Result.DataProcessingError))
        {
            infoText.color = Color.red;
            infoText.text = singleTone.request.error + '\n' + singleTone.request.downloadHandler.text;
        }

        else
        {
            Haha d = JsonUtility.FromJson<Haha>(singleTone.request.downloadHandler.text);
            if (singleTone.id != d.user_info)
            {
                infoText.color = Color.red;
                infoText.text = "계정 정보가 틀리거나 서버가 끊겼습니다.";
            }
            else
            {
                infoText.color = Color.blue;
                infoText.text = "게임을 시작합니다!";
                X_Button.SetActive(false);
                yield return YieldInstructionCache.WaitForSeconds(1);
                LoadingProgress.LoadScene("Stage3");
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