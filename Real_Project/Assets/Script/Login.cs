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
        Debug.Log(PlayerPrefs.GetInt("userid"));
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
    public class PacketData
    {
        public string url;
        public int userid;
    }
    [System.Serializable]
    public class SaveData
    {
        public int keycode;
        public string id;
        public string pwd;
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
            infoText.text = www.error;
            Popup_X.SetActive(true);
            yield return null;


            yield break;
        }
        else
        {
            SaveData data = JsonUtility.FromJson<SaveData>(www.downloadHandler.text);
            Debug.Log(data);
            PlayerPrefs.SetInt("keycode", data.keycode);
            PlayerPrefs.SetString("id", data.id);
            PlayerPrefs.Save();
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