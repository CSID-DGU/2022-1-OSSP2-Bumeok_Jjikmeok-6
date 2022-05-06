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
        StartCoroutine("Show_Ranking_I");
    }
    IEnumerator Show_Ranking_I()
    {

        Popup.SetActive(true);
        StartCoroutine("Wait_Load");
        


        UnityWebRequest www = UnityWebRequest.Get("http://localhost:3000/Get_Rank");
        yield return www.SendWebRequest();

        StopCoroutine("Wait_Load");

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

            infoText.text = d.item[0].id + ", " + d.item[0].score1 + ", " + d.item[0].score2 + ", " + d.item[0].score3 + ", " + '\n' +
                d.item[1].id + ", " + d.item[1].score1 + ", " + d.item[1].score2 + ", " + d.item[1].score3 + ", ";

            infoText.color = Color.blue;
            Popup_X.SetActive(true);
            yield return null;

        }

        yield break;
    }
    IEnumerator Wait_Load()
    {
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
    IEnumerator SignUp_Enum()
    {
        Popup.SetActive(true);
        infoText.text = "로딩 중....";
        yield return null;

        WWWForm form = new WWWForm();

        form.AddField("id", ID_In.text);
        form.AddField("pwd", PWD_IN.text);
        form.AddField("pwd_again", PWD_Again.text);
        UnityWebRequest www = UnityWebRequest.Post("http://localhost:3000/sign_up", form);

        yield return www.SendWebRequest();

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
