using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Login : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
//    using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine.UI;
//using UnityEngine.Networking;

//public class SignUp : MonoBehaviour
//{
//    public InputField ID_In;
//    public InputField PWD_IN;
//    public InputField PWD_Again;
//    public Text infoText;

//    [System.Serializable]
//    public class PacketData
//    {
//        public string url;
//        public int userid;
//    }
//    [System.Serializable]
//    public class LoadData
//    {
//        public int userid;
//        public string name;
//        public int score;
//        public int state;
//        public int level;
//    }

//    [System.Serializable]
//    public class SaveData
//    {
//        public string url;
//        public int userid;
//        public string username;
//    }
//    public void buttonClick()
//    {
//        StartCoroutine(ServerMakeUser());
//    }
//    IEnumerator ServerMakeUser()
//    {
//        WWWForm form = new WWWForm();

//        form.AddField("id", ID_In.text);
//        form.AddField("password", PWD_IN.text);
//        form.AddField("password_again", PWD_Again.text);
//        UnityWebRequest www = UnityWebRequest.Post("http://localhost:3000/make_user", form);
//        yield return www.SendWebRequest();

//        //if (www.isNetworkError)
//        //{
//        //    Debug.Log("야 이 병신아");
//        //    Debug.Log(www.error);
//        //}
//        //else
//        //{
//        Debug.Log(www.downloadHandler.text);
//        if (www.downloadHandler.text == "null")
//        {
//            infoText.text = "병시나 로그인 잘못 했잖아";
//            yield return null;
//        }
//        else
//        {
//            SaveData data = JsonUtility.FromJson<SaveData>(www.downloadHandler.text);
//            Debug.Log(data);
//            PlayerPrefs.SetInt("userid", data.userid);
//            PlayerPrefs.SetString("username", data.username);
//            PlayerPrefs.Save();
//            RadialProgress.LoadScene("Scene2");
//        }
//        // }
//    }

//    IEnumerator ServerShowUser()
//    {
//        WWWForm form = new WWWForm();
//        form.AddField("userid", PlayerPrefs.GetInt("userid"));
//        UnityWebRequest www = UnityWebRequest.Post("http://localhost:3000/show_user", form);
//        yield return www.SendWebRequest();
//        if (www.isNetworkError)
//        {
//            Debug.Log(www.error);
//        }
//        else
//        {
//            Debug.Log(www.downloadHandler.text);
//            LoadData data = JsonUtility.FromJson<LoadData>(www.downloadHandler.text);
//            Debug.Log("name:" + data.name + ",score:" + data.score + ",level:" + data.level);
//            infoText.text = "name:" + data.name + "\\nscore:" + data.score + "\\nlevel:" + data.level;

//        }
//    }


//    // Start is called before the first frame update
//    void Start()
//    {
//        Debug.Log(PlayerPrefs.GetInt("userid"));
//    }

//    // Update is called once per frame
//    //void Update()
//    //{
//    //    if (Input.GetKeyDown(KeyCode.Space))
//    //    {
//    //        RadialProgress.LoadScene("Scene2");
//    //    }
//    //}
//}

}
