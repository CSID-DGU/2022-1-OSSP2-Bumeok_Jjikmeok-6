using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;

public class SignUp : MonoBehaviour
{
    public InputField ID_In;
    public InputField PWD_IN;
    public InputField PWD_Again;
    public Text infoText;

    [System.Serializable]
    public class PacketData
    {
        public string url;
        public int userid;
    }
    [System.Serializable]
    public class LoadData
    {
        public int userid;
        public string name;
        public int score;
        public int state;
        public int level;
    }

    [System.Serializable]
    public class SaveData
    {
        public string url;
        public int userid;
    }
    public void buttonClick()
    {
        StartCoroutine(SignUp_Enum());
    }
    IEnumerator Kuku()
    {
        Debug.Log("제발");
        UnityWebRequest www = UnityWebRequest.Get("http://localhost:5000/logout");
        yield return www.SendWebRequest();

        Debug.Log(www);
    }
    IEnumerator SignUp_Enum()
    {
        WWWForm form = new WWWForm();

        form.AddField("id", ID_In.text);
        form.AddField("pwd", PWD_IN.text);
        form.AddField("pwd_again", PWD_Again.text);
        UnityWebRequest www = UnityWebRequest.Post("http://localhost:3000/sign_up", form);
        
        yield return www.SendWebRequest();
        Debug.Log(www.error);

        if ((www.result == UnityWebRequest.Result.ConnectionError) ||
            (www.result == UnityWebRequest.Result.ProtocolError) ||
            (www.result == UnityWebRequest.Result.DataProcessingError))
        {
            Debug.Log(www.error);
            yield break;
        }

        Debug.Log(www.downloadHandler.text);
        if (www.downloadHandler.text == "null")
        {
            infoText.text = "회원가입 실패...";
        }
        else
        {
            SaveData data = JsonUtility.FromJson<SaveData>(www.downloadHandler.text);
            Debug.Log(data);
            infoText.text = "회원가입 성공!";
        }
        yield return null;
    }
 
    IEnumerator ServerShowUser()
    {
        WWWForm form = new WWWForm();
        form.AddField("userid", PlayerPrefs.GetInt("userid"));
        UnityWebRequest www = UnityWebRequest.Post("http://localhost:3000/show_user", form);
        yield return www.SendWebRequest();
        if (www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            LoadData data = JsonUtility.FromJson<LoadData>(www.downloadHandler.text);
            Debug.Log("name:" + data.name + ",score:" + data.score + ",level:" + data.level);
            infoText.text = "name:" + data.name + "\\nscore:" + data.score + "\\nlevel:" + data.level;
           
        }
    }
    //PlayerPrefs.SetInt("userid", data.userid);
    //PlayerPrefs.SetString("username", data.username);
    //PlayerPrefs.Save();
    //RadialProgress.LoadScene("Scene2");

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(PlayerPrefs.GetInt("userid"));
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        RadialProgress.LoadScene("Scene2");
    //    }
    //}
}
