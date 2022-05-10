using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class SignUp : MonoBehaviour
{
    [SerializeField]
    TMP_InputField ID_In;

    [SerializeField]
    TMP_InputField PWD_IN;

    [SerializeField]
    TMP_InputField PWD_Again;

    [SerializeField]
    TextMeshProUGUI infoText;

    [SerializeField]
    GameObject Popup;

    [SerializeField]
    GameObject Popup_X;

    private void Awake()
    {
        Popup.SetActive(false);
        Popup_X.SetActive(false);
    }

    [System.Serializable]
    class Item
    {
        public string id;
        public int score1;
        public int score2;
        public int score3;
    }

    [System.Serializable]
    class Data
    {
        public Item[] item;
    }

    public void buttonClick()
    {
        StartCoroutine(SignUp_Enum());
    }
    public void Show_Ranking()
    {
        StartCoroutine(Show_Ranking_I());
    }
    IEnumerator Show_Ranking_I()
    {

        Popup.SetActive(true);
        IEnumerator wait_load = Wait_Load();
        StartCoroutine(wait_load);
        


        UnityWebRequest www = UnityWebRequest.Get("http://localhost:3000/Get_Rank");
        yield return www.SendWebRequest();

        

        StopCoroutine(wait_load);

        if ((www.result == UnityWebRequest.Result.ConnectionError) ||
            (www.result == UnityWebRequest.Result.ProtocolError) ||
            (www.result == UnityWebRequest.Result.DataProcessingError))
        {
            infoText.text = www.error + '\n' + www.downloadHandler.text;
            infoText.color = Color.red;
            Popup_X.SetActive(true);
            yield return null;
        }
        else
        {
            Data d = JsonUtility.FromJson<Data>(www.downloadHandler.text);
            infoText.text = "";
            foreach (var u in d.item)
            {
                infoText.text += u.id + ", " + u.score1 + ", " + u.score2 + ", " + u.score3 + '\n';
            }

            infoText.color = Color.blue;
            Popup_X.SetActive(true);
            yield return YieldInstructionCache.WaitForEndOfFrame;

        }

        yield break;
    }
 
    IEnumerator SignUp_Enum()
    {
        Popup.SetActive(true);
        IEnumerator wait_load = Wait_Load();
        StartCoroutine(wait_load);
        yield return null;

        WWWForm form = new WWWForm();

        form.AddField("id", ID_In.text);
        form.AddField("pwd", PWD_IN.text);
        form.AddField("pwd_again", PWD_Again.text);
        UnityWebRequest www = UnityWebRequest.Post("http://localhost:3000/sign_up", form);

        yield return www.SendWebRequest();
        StopCoroutine(wait_load);

        if ((www.result == UnityWebRequest.Result.ConnectionError) ||
            (www.result == UnityWebRequest.Result.ProtocolError) ||
            (www.result == UnityWebRequest.Result.DataProcessingError))
        {

            infoText.text = www.error + '\n' + www.downloadHandler.text;
            infoText.color = Color.red;
            Popup_X.SetActive(true);
            yield return null;
        }
        else
        {
            if (www.downloadHandler.text == "null")
            {
                infoText.text = "회원가입 실패...";
                infoText.color = Color.red;
                Popup_X.SetActive(true);
                yield return null;
            }
            else
            {
                infoText.text = www.downloadHandler.text;
                infoText.color = Color.blue;
                Popup_X.SetActive(true);
                yield return null;
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
            yield return new WaitForSeconds(0.15f);

            infoText.text = "로딩 중......";
            yield return new WaitForSeconds(0.15f);

            infoText.text = "로딩 중........";
            yield return new WaitForSeconds(0.15f);
        }
    }

    void Start()
    {
        Debug.Log(PlayerPrefs.GetInt("userid"));
    }
}
