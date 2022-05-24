using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class Login : MonoBehaviour
{
    void Start()
    {
        Debug.Log(PlayerPrefs.GetInt("keycode"));
    }


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
    private void Awake()
    {
        Popup.SetActive(false);
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
        StartCoroutine("Wait_Load");

        UnityWebRequest www = UnityWebRequest.Get("http://localhost:3000/log_out");
        yield return www.SendWebRequest();

        StopCoroutine("Wait_Load");

        if ((www.result == UnityWebRequest.Result.ConnectionError) ||
          (www.result == UnityWebRequest.Result.ProtocolError) ||
          (www.result == UnityWebRequest.Result.DataProcessingError))
        {
            infoText_2.text = www.error + '\n' + www.downloadHandler.text;
            Popup_X.SetActive(true);
            yield return null;

            yield break;
        }
        else
        {
            infoText_2.text = www.downloadHandler.text;
            Popup_X.SetActive(true);
            yield return null;
        }
        yield break;
    }
    IEnumerator ServerLogin()
    {
        Popup.SetActive(true);
        StartCoroutine("Wait_Load");

        WWWForm form = new WWWForm();

        form.AddField("id", ID_In.text);
        form.AddField("pwd", PWD_IN.text);

        UnityWebRequest www = UnityWebRequest.Post("http://localhost:3000/log_in", form);
        yield return www.SendWebRequest();

        StopCoroutine("Wait_Load");

        if ((www.result == UnityWebRequest.Result.ConnectionError) ||
           (www.result == UnityWebRequest.Result.ProtocolError) ||
           (www.result == UnityWebRequest.Result.DataProcessingError))
        {
            infoText_2.color = Color.red;
            infoText_2.text = www.error + '\n' + www.downloadHandler.text;
            Popup_X.SetActive(true);
            yield return YieldInstructionCache.WaitForEndOfFrame;

            yield break;
        }
        else
        {
            infoText_2.color = Color.black;
            Login_Success data = JsonUtility.FromJson<Login_Success>(www.downloadHandler.text);

            infoText_2.text = data.success_message;
            Popup_X.SetActive(true);
            yield return YieldInstructionCache.WaitForEndOfFrame;
            PlayerPrefs.SetInt("keycode", data.user_info.keycode);
            PlayerPrefs.SetString("id", data.user_info.id);
            PlayerPrefs.Save();
            LoadingProgress.LoadScene("IWannaScene");
        }
    }

    IEnumerator Wait_Load()
    {
        infoText_2.color = Color.black;
        while (true)
        {
            infoText_2.text = "로딩 중....";
            yield return new WaitForSeconds(0.15f);

            infoText_2.text = "로딩 중......";
            yield return new WaitForSeconds(0.15f);

            infoText_2.text = "로딩 중........";
            yield return new WaitForSeconds(0.15f);
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