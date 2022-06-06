using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Linq;
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
    class Ranking_Total_Up
    {
        public Ranking_Total_Down[] rank_mine;
        public Ranking_Total_Down[] rank_total;
    }
    [System.Serializable]
    class Ranking_Total_Down
    {
        public string id;
        public int main_score_1;
        public int main_score_2;
        public int main_score_3;
        public int final_score_1;
        public int final_score_2;
    }

    [System.Serializable]
    class Ranking_Main_1_Up
    {
        public Ranking_Main_1_Down[] rank_mine;
        public Ranking_Main_1_Down[] rank_total;
    }
    [System.Serializable]
    class Ranking_Main_2_Up
    {
        public Ranking_Main_2_Down[] rank_mine;
        public Ranking_Main_2_Down[] rank_total;
    }
    [System.Serializable]
    class Ranking_Main_3_Up
    {
        public Ranking_Main_3_Down[] rank_mine;
        public Ranking_Main_3_Down[] rank_total;
    }
    [System.Serializable]
    class Ranking_Final_1_Up
    {
        public Ranking_Final_1_Down[] rank_mine;
        public Ranking_Final_1_Down[] rank_total;
    }
    [System.Serializable]
    class Ranking_Final_2_Up
    {
        public Ranking_Final_2_Down[] rank_mine;
        public Ranking_Final_2_Down[] rank_total;
    }

    [System.Serializable]
    class Ranking_Main_1_Down
    {
        public string id;
        public int main_score_1;
    }
    [System.Serializable]
    class Ranking_Main_2_Down
    {
        public string id;
        public int main_score_2;
    }
    [System.Serializable]
    class Ranking_Main_3_Down
    {
        public string id;
        public int main_score_3;
    }
    [System.Serializable]
    class Ranking_Final_1_Down
    {
        public string id;
        public int final_score_1;
    }
    [System.Serializable]
    class Ranking_Final_2_Down
    {
        public string id;
        public int final_score_2;
    }

    public void buttonClick()
    {
        StartCoroutine(SignUp_Enum());
    }
    public void Show_Ranking(int Params)
    {
        StartCoroutine(Show_Total_Ranking_For_Login()); // 상세 랭킹 (로그인 O --> 본인 것까지(등수는 없음))
        StartCoroutine(Show_Total_Ranking_For_Not_Login()); // 전체 랭킹 (로그인 X)
        StartCoroutine(Show_Detail_Ranking_For_Login(Params)); // 상세 랭킹 (로그인 O --> 본인 것까지)
        StartCoroutine(Show_Detail_Ranking_For_Not_Login(Params)); // 상세 랭킹 (로그인 X)
    }
    public class Number
    {
        public int identifier;
    }
    IEnumerator Show_Total_Ranking_For_Login()
    {
        Popup.SetActive(true);
        IEnumerator wait_load = Wait_Load();
        StartCoroutine(wait_load);

        singleTone.request = UnityWebRequest.Get("http://localhost:3000/get_rank_total_login");
        yield return singleTone.request.SendWebRequest();

        StopCoroutine(wait_load);

        if ((singleTone.request.result == UnityWebRequest.Result.ConnectionError) ||
            (singleTone.request.result == UnityWebRequest.Result.ProtocolError) ||
            (singleTone.request.result == UnityWebRequest.Result.DataProcessingError))
        {
            infoText.text = singleTone.request.error + '\n' + singleTone.request.downloadHandler.text;
            infoText.color = Color.red;
            Popup_X.SetActive(true);
        }
        else
        {
            Ranking_Total_Up d = JsonUtility.FromJson<Ranking_Total_Up>(singleTone.request.downloadHandler.text);

            int Index = 0;
            int My_Rand = 0;

            foreach (var e in d.rank_total)
            {
                Index++;
                foreach (var u in d.rank_mine)
                {
                    if (e.id == u.id)
                        My_Rand = Index;
                }
            }

            Index = 0;

            Debug.Log("내 랭킹");
            foreach (var e in d.rank_mine)
                Debug.Log(My_Rand + " " + e.id + " " + e.main_score_1 + " " + e.main_score_2 + " " + e.main_score_3 + " " + e.final_score_1 + " " + e.final_score_2);

            Debug.Log("전체 랭킹 : ");
            foreach (var e in d.rank_total)
            {
                Index++;
                Debug.Log(Index + " " + e.id + " " + e.main_score_1 + " " + e.main_score_2 + " " + e.main_score_3 + " " + e.final_score_1 + " " + e.final_score_2);
            }
            Popup_X.SetActive(true);
        }
        yield return null;
    }
    IEnumerator Show_Total_Ranking_For_Not_Login()
    {
        Popup.SetActive(true);
        IEnumerator wait_load = Wait_Load();
        StartCoroutine(wait_load);

        singleTone.request = UnityWebRequest.Get("http://localhost:3000/get_rank_total_not_login");
        yield return singleTone.request.SendWebRequest();

        StopCoroutine(wait_load);

        if ((singleTone.request.result == UnityWebRequest.Result.ConnectionError) ||
            (singleTone.request.result == UnityWebRequest.Result.ProtocolError) ||
            (singleTone.request.result == UnityWebRequest.Result.DataProcessingError))
        {
            infoText.text = singleTone.request.error + '\n' + singleTone.request.downloadHandler.text;
            infoText.color = Color.red;
            Popup_X.SetActive(true);
        }
        else
        {
            Ranking_Total_Up d = JsonUtility.FromJson<Ranking_Total_Up>(singleTone.request.downloadHandler.text);

            Debug.Log("전체 랭킹 : ");
            int Index = 0;
            foreach (var e in d.rank_total)
            {
                Index++;
                Debug.Log(Index + " " + e.id + " " + e.main_score_1 + " " + e.main_score_2 + " " + e.main_score_3 + " " + e.final_score_1 + " " + e.final_score_2);
            }

            Popup_X.SetActive(true);

        }
        yield return null;
    }
    IEnumerator Show_Detail_Ranking_For_Not_Login(int Params)
    {
        Popup.SetActive(true);
        IEnumerator wait_load = Wait_Load();
        StartCoroutine(wait_load);

        WWWForm form = new WWWForm();
        form.AddField("identifier", Params);

        singleTone.request = UnityWebRequest.Post("http://localhost:3000/get_rank_detail_not_login", form);
        yield return singleTone.request.SendWebRequest();

        StopCoroutine(wait_load);

        if ((singleTone.request.result == UnityWebRequest.Result.ConnectionError) ||
            (singleTone.request.result == UnityWebRequest.Result.ProtocolError) ||
            (singleTone.request.result == UnityWebRequest.Result.DataProcessingError))
        {
            infoText.text = singleTone.request.error + '\n' + singleTone.request.downloadHandler.text;
            infoText.color = Color.red;
            Popup_X.SetActive(true);
        }
        else
        {
            infoText.text = "";
            Dictionary<string, int> dic = new Dictionary<string, int>();
            if (Params == 0)
            {
                Ranking_Main_1_Up d = JsonUtility.FromJson<Ranking_Main_1_Up>(singleTone.request.downloadHandler.text);

                foreach (var u in d.rank_total)
                    dic.Add(u.id, u.main_score_1);

                var myList_Ollim = dic.ToList();
                var myList_Narim = dic.ToList();

                myList_Ollim.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));

                int Index = 0;

                Debug.Log("상대방 랭킹 : ");
                foreach (var e in myList_Ollim)
                {
                    Index++;
                    Debug.Log(Index + " " + e.Key + " " + e.Value);
                }

                Popup_X.SetActive(true);
            }
            if (Params == 1)
            {
                Ranking_Main_2_Up d = JsonUtility.FromJson<Ranking_Main_2_Up>(singleTone.request.downloadHandler.text);

                foreach (var u in d.rank_total)
                    dic.Add(u.id, u.main_score_2);

                var myList_Ollim = dic.ToList();
                var myList_Narim = dic.ToList();

                myList_Ollim.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));

                int Index = 0;

                Debug.Log("상대방 랭킹 : ");
                foreach (var e in myList_Ollim)
                {
                    Index++;
                    Debug.Log(Index + " " + e.Key + " " + e.Value);
                }

                Popup_X.SetActive(true);
            }
            if (Params == 2)
            {
                Ranking_Main_3_Up d = JsonUtility.FromJson<Ranking_Main_3_Up>(singleTone.request.downloadHandler.text);

                foreach (var u in d.rank_total)
                    dic.Add(u.id, u.main_score_3);

                var myList_Ollim = dic.ToList();
                var myList_Narim = dic.ToList();

                myList_Ollim.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));

                int Index = 0;

                Debug.Log("상대방 랭킹 : ");
                foreach (var e in myList_Ollim)
                {
                    Index++;
                    Debug.Log(Index + " " + e.Key + " " + e.Value);
                }

                Popup_X.SetActive(true);
            }
            if (Params == 3)
            {
                Ranking_Final_1_Up d = JsonUtility.FromJson<Ranking_Final_1_Up>(singleTone.request.downloadHandler.text);

                foreach (var u in d.rank_total)
                    dic.Add(u.id, u.final_score_1);

                var myList_Ollim = dic.ToList();
                var myList_Narim = dic.ToList();

                myList_Ollim.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));

                int Index = 0;

                Debug.Log("상대방 랭킹 : ");
                foreach (var e in myList_Ollim)
                {
                    Index++;
                    Debug.Log(Index + " " + e.Key + " " + e.Value);
                }

                Popup_X.SetActive(true);

            }
            if (Params == 4)
            {
                Ranking_Final_2_Up d = JsonUtility.FromJson<Ranking_Final_2_Up>(singleTone.request.downloadHandler.text);

                foreach (var u in d.rank_total)
                    dic.Add(u.id, u.final_score_2);

                var myList_Ollim = dic.ToList();
                var myList_Narim = dic.ToList();

                myList_Ollim.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));

                int Index = 0;

                Debug.Log("상대방 랭킹 : ");
                foreach (var e in myList_Ollim)
                {
                    Index++;
                    Debug.Log(Index + " " + e.Key + " " + e.Value);
                }

                Popup_X.SetActive(true);
            }
        }
        yield return null;
    }
    IEnumerator Show_Detail_Ranking_For_Login(int Params)
    {
        Popup.SetActive(true);
        IEnumerator wait_load = Wait_Load();
        StartCoroutine(wait_load);


        WWWForm form = new WWWForm();
        form.AddField("identifier", Params);


        singleTone.request = UnityWebRequest.Post("http://localhost:3000/get_rank_detail_login", form);
        yield return singleTone.request.SendWebRequest();

        StopCoroutine(wait_load);

        if ((singleTone.request.result == UnityWebRequest.Result.ConnectionError) ||
            (singleTone.request.result == UnityWebRequest.Result.ProtocolError) ||
            (singleTone.request.result == UnityWebRequest.Result.DataProcessingError))
        {
            infoText.text = singleTone.request.error + '\n' + singleTone.request.downloadHandler.text;
            infoText.color = Color.red;
            Popup_X.SetActive(true);

        }
        else
        {
            infoText.text = "";
            Dictionary<string, int> dic = new Dictionary<string, int>();
            int My_Rand = 0;
            if (Params == 0)
            {
                Ranking_Main_1_Up d = JsonUtility.FromJson<Ranking_Main_1_Up>(singleTone.request.downloadHandler.text);

                foreach (var u in d.rank_total)
                    dic.Add(u.id, u.main_score_1);

                var myList_Ollim = dic.ToList();
                var myList_Narim = dic.ToList();

                myList_Ollim.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));

                int Index = 0;

                foreach (var e in myList_Ollim)
                {
                    Index++;
                    foreach (var u in d.rank_mine)
                    {
                        if (e.Key == u.id)
                            My_Rand = Index;
                    }
                }
                Index = 0;
                foreach (var u in d.rank_mine)
                    Debug.Log("나의 랭킹 : " + My_Rand + " " + u.id + " " + u.main_score_1);

                Debug.Log("상대방 랭킹 : ");
                foreach (var e in myList_Ollim)
                {
                    Index++;
                    Debug.Log(Index + " " + e.Key + " " + e.Value);
                }

                Popup_X.SetActive(true);
            }
            if (Params == 1)
            {
                Ranking_Main_2_Up d = JsonUtility.FromJson<Ranking_Main_2_Up>(singleTone.request.downloadHandler.text);

                foreach (var u in d.rank_total)
                    dic.Add(u.id, u.main_score_2);

                var myList_Ollim = dic.ToList();
                var myList_Narim = dic.ToList();

                myList_Ollim.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));
                int Index = 0;

                foreach (var e in myList_Ollim)
                {
                    Index++;
                    foreach (var u in d.rank_mine)
                    {
                        if (e.Key == u.id)
                            My_Rand = Index;
                    }
                }
                foreach (var u in d.rank_mine)
                    Debug.Log("나의 랭킹 : " + My_Rand + " " + u.id + " " + u.main_score_2);

                Index = 0;

                Debug.Log("상대방 랭킹 : ");
                foreach (var e in myList_Ollim)
                {
                    Index++;
                    Debug.Log(Index + " " + e.Key + " " + e.Value);
                }
                Popup_X.SetActive(true);
            }
            if (Params == 2)
            {
                Ranking_Main_3_Up d = JsonUtility.FromJson<Ranking_Main_3_Up>(singleTone.request.downloadHandler.text);

                foreach (var u in d.rank_total)
                    dic.Add(u.id, u.main_score_3);

                var myList_Ollim = dic.ToList();
                var myList_Narim = dic.ToList();

                myList_Ollim.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));

                int Index = 0;

                foreach (var e in myList_Ollim)
                {
                    Index++;
                    foreach (var u in d.rank_mine)
                    {
                        if (e.Key == u.id)
                            My_Rand = Index;
                    }
                }
                foreach (var u in d.rank_mine)
                    Debug.Log("나의 랭킹 : " + My_Rand + " " + u.id + " " + u.main_score_3);

                Index = 0;

                Debug.Log("상대방 랭킹 : ");
                foreach (var e in myList_Ollim)
                {
                    Index++;
                    Debug.Log(Index + " " + e.Key + " " + e.Value);
                }
                Popup_X.SetActive(true);
            }
            if (Params == 3)
            {
                Ranking_Final_1_Up d = JsonUtility.FromJson<Ranking_Final_1_Up>(singleTone.request.downloadHandler.text);

                foreach (var u in d.rank_total)
                    dic.Add(u.id, u.final_score_1);

                var myList_Ollim = dic.ToList();
                var myList_Narim = dic.ToList();

                myList_Ollim.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));

                int Index = 0;

                foreach (var e in myList_Ollim)
                {
                    Index++;
                    foreach (var u in d.rank_mine)
                    {
                        if (e.Key == u.id)
                            My_Rand = Index;
                    }
                }
                foreach (var u in d.rank_mine)
                    Debug.Log("나의 랭킹 : " + My_Rand + " " + u.id + " " + u.final_score_1);

                Index = 0;

                Debug.Log("상대방 랭킹 : ");
                foreach (var e in myList_Ollim)
                {
                    Index++;
                    Debug.Log(Index + " " + e.Key + " " + e.Value);
                }
                Popup_X.SetActive(true);

            }
            if (Params == 4)
            {
                Ranking_Final_2_Up d = JsonUtility.FromJson<Ranking_Final_2_Up>(singleTone.request.downloadHandler.text);

                foreach (var u in d.rank_total)
                    dic.Add(u.id, u.final_score_2);

                var myList_Ollim = dic.ToList();
                var myList_Narim = dic.ToList();

                myList_Ollim.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));

                int Index = 0;

                foreach (var e in myList_Ollim)
                {
                    Index++;
                    foreach (var u in d.rank_mine)
                    {
                        if (e.Key == u.id)
                            My_Rand = Index;
                    }
                }
                foreach (var u in d.rank_mine)
                    Debug.Log("나의 랭킹 : " + My_Rand + " " + u.id + " " + u.final_score_2);

                Index = 0;

                Debug.Log("상대방 랭킹 : ");
                foreach (var e in myList_Ollim)
                {
                    Index++;
                    Debug.Log(Index + " " + e.Key + " " + e.Value);
                }
                Popup_X.SetActive(true);
            }
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