using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

//1.JOIN문->select id, main_score_1, main_score_2, main_score_3, final_score_1, final_score_2 from auth left join ranking on auth.keycode = ranking.Auth_id;
//2.랭킹은 SignUp.cs에 기술
//3. 랭킹 삽입 insert into Ranking (Auth_id, main_score_1, main_score_2, main_score_3, final_score_1, final_score_2) values(1, 16, 60, 18, 19, 20);


//insert into Ranking (Auth_id, main_score_1, main_score_2, main_score_3, final_score_1, final_score_2) values(2, 999, 999, 999, 999, 999);


//*메인 스테이지 1의 랭킹 순서를 보고 싶을 때 - ASC는 오름차순 정렬, DESC는 내림차순 정렬
//select id, final_score_1 from auth left join ranking on auth.keycode = ranking.Auth_id order by final_score_1 ASC;

//SELECT* FROM 테이블 ORDER BY 컬럼1 ASC;

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
    class Item_Total
    {
        public string id;
        public int main_score_1;
        public int main_score_2;
        public int main_score_3;
        public int final_score_1;
        public int final_score_2;
    }
    [System.Serializable]
    class Item_Main_1
    {
        public string id;
        public int main_score_1;
    }
    [System.Serializable]
    class Item_Main_2
    {
        public string id;
        public int main_score_2;
    }
    [System.Serializable]
    class Item_Main_3
    {
        public string id;
        public int main_score_3;
    }
    [System.Serializable]
    class Item_Final_1
    {
        public string id;
        public int final_score_1;

    }
    [System.Serializable]
    class Item_Final_2
    {
        public string id;
        public int final_score_2;
    }

    [System.Serializable]
    class Data_Total
    {
        public Item_Total[] item_total;
    }

    [System.Serializable]
    class Data_Main_1
    {
        public Item_Main_1[] item_main_1;
    }

    [System.Serializable]
    class Data_Main_2
    {
        public Item_Main_2[] item_main_2;
    }

    [System.Serializable]
    class Data_Main_3
    {
        public Item_Main_3[] item_main_3;
    }

    [System.Serializable]
    class Data_Final_1
    {
        public Item_Final_1[] item_final_1;
    }

    [System.Serializable]
    class Data_Final_2
    {
        public Item_Final_2[] item_final_2;
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

        UnityWebRequest www = UnityWebRequest.Get("http://localhost:3000/Get_Rank_For_Main_1");
        yield return www.SendWebRequest();

        StopCoroutine(wait_load);

        if ((www.result == UnityWebRequest.Result.ConnectionError) ||
            (www.result == UnityWebRequest.Result.ProtocolError) ||
            (www.result == UnityWebRequest.Result.DataProcessingError))
        {
            infoText.text = www.error + '\n' + www.downloadHandler.text;
            infoText.color = Color.red;
            Popup_X.SetActive(true);

        }
        else
        {
            Data_Main_1 d = JsonUtility.FromJson<Data_Main_1>(www.downloadHandler.text);
            // Data_Main_2, Data_Main_3, Data_Final_1, Data_Final_2도 있으니 잘 보셈
            infoText.text = "";
            List<int> Rank_Ordered = new List<int>();

            foreach (var u in d.item_main_1)
            {
                Rank_Ordered.Add(u.main_score_1);
                infoText.text += u.id + ", " + u.main_score_1 + ", " + '\n';
            }
            Rank_Ordered.Sort(); // 오름차순 정렬. 내림차순으로 출력하고 싶으면 for문을 반대로

            infoText.color = Color.blue;
            Popup_X.SetActive(true);
        }
        yield return null;
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