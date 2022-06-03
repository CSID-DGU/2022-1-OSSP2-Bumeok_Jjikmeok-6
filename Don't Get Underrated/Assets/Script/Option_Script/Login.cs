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
    TextMeshProUGUI infoText_2;

    [SerializeField]
    GameObject Popup;

    [SerializeField]
    GameObject Popup_X;

    IEnumerator wait_load;
    private void Awake()
    {
        Popup.SetActive(false);
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
    public void buttonClick(int param)
    {
        if (param == 0)
            StartCoroutine(ServerLogin());
        else if (param == 1)
            StartCoroutine(ServerLogout());
    }
    IEnumerator ServerLogout()
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
            infoText_2.text = singleTone.request.error + '\n' + singleTone.request.downloadHandler.text;
            Popup_X.SetActive(true);
            yield return null;

            yield break;
        }
        else
        {
            infoText_2.text = singleTone.request.downloadHandler.text;
            Popup_X.SetActive(true);
            yield return null;
        }
        yield break;
    }
    IEnumerator ServerLogin()
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
            infoText_2.color = Color.red;
            infoText_2.text = singleTone.request.error + '\n' + singleTone.request.downloadHandler.text;
            Popup_X.SetActive(true);
            yield return null;

            yield break;
        }
        else
        {
            infoText_2.color = Color.black;
            Login_Success data = JsonUtility.FromJson<Login_Success>(singleTone.request.downloadHandler.text);

            infoText_2.text = data.success_message;
            Popup_X.SetActive(true);
            yield return null;
            LoadingProgress.LoadScene("My_SJH_Scene");
        }
    }

    IEnumerator Wait_Load()
    {
        infoText_2.color = Color.black;
        while (true)
        {
            infoText_2.text = "로딩 중....";
            yield return YieldInstructionCache.WaitForSeconds(0.15f);

            infoText_2.text = "로딩 중......";
            yield return YieldInstructionCache.WaitForSeconds(0.15f);

            infoText_2.text = "로딩 중........";
            yield return YieldInstructionCache.WaitForSeconds(0.15f);
        }
    }




    // Start is called before the first frame update

    // Update is called once per frame
    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        RadialProgress.LoadScene("Scene2");
    //    }
    //}
}