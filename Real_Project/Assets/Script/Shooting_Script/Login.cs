using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class Login : MonoBehaviour
{
    // Start is called before the first frame update

   
    void Start()
    {
        Debug.Log(PlayerPrefs.GetInt("keycode"));
    }


    [SerializeField]
    TMP_InputField ID_In;

    [SerializeField]
    TMP_InputField PWD_IN;

    [SerializeField]
    Text infoText;

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
        UnityWebRequest www = UnityWebRequest.Get("http://localhost:3000/log_out");
        yield return www.SendWebRequest();

        Debug.Log(www);
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
            infoText.text = www.error + '\n' + www.downloadHandler.text;
            Popup_X.SetActive(true);
            yield return null;

            yield break;
        }
        else
        {
            Login_Success data = JsonUtility.FromJson<Login_Success>(www.downloadHandler.text);
            Debug.Log(data.user_info.keycode);
            Debug.Log(data.user_info.id);
            infoText.text = data.success_message;
            Popup_X.SetActive(true);
            yield return null;
            PlayerPrefs.SetInt("keycode", data.user_info.keycode);
            PlayerPrefs.SetString("id", data.user_info.id);
            PlayerPrefs.Save();
            yield break;
            //RadialProgress.LoadScene("Scene2");
        }
    }

    IEnumerator Wait_Load()
    {
        while(true)
        {
            infoText.text = "로딩 중....";
            yield return new WaitForSeconds(0.3f);

            infoText.text = "로딩 중......";
            yield return new WaitForSeconds(0.3f);

            infoText.text = "로딩 중........";
            yield return new WaitForSeconds(0.3f);
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