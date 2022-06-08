using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Linq;
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

    [SerializeField]
    GameObject Scroll_View;

    [SerializeField]
    Text Rank_Text;

    [SerializeField]
    Text ID;

    [SerializeField]
    Text Score_1;

    [SerializeField]
    Text Score_2;

    [SerializeField]
    Text Score_3;

    [SerializeField]
    Text Score_4;

    [SerializeField]
    Text Score_5;

    private void Awake()
    {
        Popup.SetActive(false);
        Scroll_View.SetActive(false);
    }

    [System.Serializable]
    class Ranking_Total_Up
    {
        public Ranking_Total_Down[] rank_mine = null;
        public Ranking_Total_Down[] rank_total = null;
    }
    [System.Serializable]
    class Ranking_Total_Down
    {
        public string id = "";
        public int main_score_1 = 0;
        public int main_score_2 = 0;
        public int main_score_3 = 0;
        public int final_score_1 = 0;
        public int final_score_2 = 0;
    }

    [System.Serializable]
    class Ranking_Main_1_Up
    {
        public Ranking_Main_1_Down[] rank_mine = null;
        public Ranking_Main_1_Down[] rank_total = null;
    }
    [System.Serializable]
    class Ranking_Main_2_Up
    {
        public Ranking_Main_2_Down[] rank_mine = null;
        public Ranking_Main_2_Down[] rank_total = null;
    }
    [System.Serializable]
    class Ranking_Main_3_Up
    {
        public Ranking_Main_3_Down[] rank_mine = null;
        public Ranking_Main_3_Down[] rank_total = null;
    }
    [System.Serializable]
    class Ranking_Final_1_Up
    {
        public Ranking_Final_1_Down[] rank_mine = null;
        public Ranking_Final_1_Down[] rank_total = null;
    }
    [System.Serializable]
    class Ranking_Final_2_Up
    {
        public Ranking_Final_2_Down[] rank_mine = null;
        public Ranking_Final_2_Down[] rank_total = null;
    }

    [System.Serializable]
    class Ranking_Main_1_Down
    {
        public string id = "";
        public int main_score_1 = 0;
    }
    [System.Serializable]
    class Ranking_Main_2_Down
    {
        public string id = "";
        public int main_score_2 = 0;
    }
    [System.Serializable]
    class Ranking_Main_3_Down
    {
        public string id = "";
        public int main_score_3 = 0;
    }
    [System.Serializable]
    class Ranking_Final_1_Down
    {
        public string id = "";
        public int final_score_1 = 0;
    }
    [System.Serializable]
    class Ranking_Final_2_Down
    {
        public string id = "";
        public int final_score_2 = 0;
    }

    public void buttonClick()
    {
        StopAllCoroutines();
        StartCoroutine(SignUp_Behave());
    }
    public void Show_Ranking_Detail(int Params)
    {
        StopAllCoroutines();
        Popup.SetActive(true);
        
        StartCoroutine(Show_Detail_Ranking_For_Login(Params)); // 상세 랭킹 (로그인 O --> 본인 것까지)
    }
    public void Show_Ranking_Total()
    {
        StopAllCoroutines();
        Popup.SetActive(true);

        StartCoroutine(Show_Total_Ranking_For_Login()); // 전체 랭킹 (로그인 O --> 본인 것까지)
    }
    public class Number
    {
        public int identifier;
    }
    IEnumerator Show_Total_Ranking_For_Login()
    {
        Popup_X.SetActive(false);
        IEnumerator wait_load = Wait_Load();
        StartCoroutine(wait_load);

        singleTone.request = UnityWebRequest.Get("http://localhost:3000/get_rank_total_login");
        yield return singleTone.request.SendWebRequest();

        StopCoroutine(wait_load);

        if ((singleTone.request.result == UnityWebRequest.Result.ConnectionError) ||
            (singleTone.request.result == UnityWebRequest.Result.ProtocolError) ||
            (singleTone.request.result == UnityWebRequest.Result.DataProcessingError))
        {
            Popup_X.SetActive(true);
            infoText.color = Color.red;
            infoText.text = singleTone.request.error + '\n' + singleTone.request.downloadHandler.text;
        }
        else
        {
            Scroll_View.SetActive(true);
            Popup_X.SetActive(true);
            infoText.text = "";
            Rank_Text.text = "";
            ID.text = "";
            Score_1.text = "";
            Score_2.text = "";
            Score_3.text = "";
            Score_4.text = "";
            Score_5.text = "";
            Ranking_Total_Up d = JsonUtility.FromJson<Ranking_Total_Up>(singleTone.request.downloadHandler.text);

            int Index = 0;
            int My_Rank = 0;



            foreach (var e in d.rank_total)
            {
                Index++;
                foreach (var u in d.rank_mine)
                {
                    if (e.id == u.id)
                        My_Rank = Index;
                }
            }
            Rank_Text.text += "내 랭킹\n";
            Rank_Text.text += "\n아이디\n";
            Score_1.text += "\n\nMAIN 1\n";
            Score_2.text += "\n\nMAIN 2\n";
            Score_3.text += "\n\nMAIN 3\n";
            Score_4.text += "\n\nFINAL 1\n";
            Score_5.text += "\n\nFINAL 2\n";
            foreach (var e in d.rank_mine)
            {
                Rank_Text.text += e.id + '\n' + '\n';
                Score_1.text += e.main_score_1.ToString() + '\n' + '\n';
                Score_2.text += e.main_score_2.ToString() + '\n' + '\n';
                Score_3.text += e.main_score_3.ToString() + '\n' + '\n';
                Score_4.text += e.final_score_1.ToString() + '\n' + '\n';
                Score_5.text += e.final_score_2.ToString() + '\n' + '\n';
            }
            

            Rank_Text.text += "전체 랭킹\n";
            Rank_Text.text += "\n아이디\n";
            Score_1.text += "\n\nMAIN 1\n";
            Score_2.text += "\n\nMAIN 2\n";
            Score_3.text += "\n\nMAIN 3\n";
            Score_4.text += "\n\nFINAL 1\n";
            Score_5.text += "\n\nFINAL 2\n";


            foreach (var e in d.rank_total)
            {
                Rank_Text.text += e.id + '\n';
                Score_1.text += e.main_score_1.ToString() + '\n';
                Score_2.text += e.main_score_2.ToString() + '\n';
                Score_3.text += e.main_score_3.ToString() + '\n';
                Score_4.text += e.final_score_1.ToString() + '\n';
                Score_5.text += e.final_score_2.ToString() + '\n';
            }
        }
        yield break;
    }
   
    IEnumerator Show_Detail_Ranking_For_Login(int Params)
    {
        Popup_X.SetActive(false);
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
            Popup_X.SetActive(true);
            infoText.color = Color.red;
            infoText.text = singleTone.request.error + '\n' + singleTone.request.downloadHandler.text;
        }
        else
        {
            Scroll_View.SetActive(true);
            Popup_X.SetActive(true);
            infoText.text = "";
            Rank_Text.text = "";
            ID.text = "";
            Score_1.text = "";
            Score_2.text = "";
            Score_3.text = "";
            Score_4.text = "";
            Score_5.text = "";
            Dictionary<string, int> dic = new Dictionary<string, int>();
            int My_Rank = 0;
            if (Params == 0)
            {
                Ranking_Main_1_Up d = JsonUtility.FromJson<Ranking_Main_1_Up>(singleTone.request.downloadHandler.text);

                foreach (var u in d.rank_total)
                    dic.Add(u.id, u.main_score_1);

                var myList_Ollim = dic.ToList();

                myList_Ollim.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));

                int Index = 0;

                foreach (var e in myList_Ollim)
                {
                    Index++;
                    foreach (var u in d.rank_mine)
                    {
                        if (e.Key == u.id)
                            My_Rank = Index;
                    }
                }

                Rank_Text.text += "내 랭킹\n";
                
                Rank_Text.text += "\n등수\n";
                ID.text += "\n\n아이디\n";
                Score_1.text += "\n\nMAIN 1 (제한 시간)\n";

                foreach (var e in d.rank_mine)
                {
                    Rank_Text.text += My_Rank.ToString() + '\n' + '\n';
                    ID.text += e.id + '\n' + '\n';
                    Score_1.text += e.main_score_1.ToString() + '\n' + '\n';
                }


                Rank_Text.text += "전체 랭킹\n";
                Rank_Text.text += "\n등수\n";
                ID.text += "\n\n아이디\n";
                Score_1.text += "\n\nMAIN 1 (제한 시간)\n";

                Index = 0;

                foreach (var e in d.rank_total)
                {
                    Index++;
                    Rank_Text.text += Index.ToString() + '\n';
                    ID.text += e.id + '\n';
                    Score_1.text += e.main_score_1.ToString() + '\n';
                }
            }
            if (Params == 1)
            {
                Ranking_Main_2_Up d = JsonUtility.FromJson<Ranking_Main_2_Up>(singleTone.request.downloadHandler.text);

                foreach (var u in d.rank_total)
                    dic.Add(u.id, u.main_score_2);

                var myList_Ollim = dic.ToList();

                myList_Ollim.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));
                int Index = 0;

                foreach (var e in myList_Ollim)
                {
                    Index++;
                    foreach (var u in d.rank_mine)
                    {
                        if (e.Key == u.id)
                            My_Rank = Index;
                    }
                }

                Rank_Text.text += "내 랭킹\n";

                Rank_Text.text += "\n등수\n";
                ID.text += "\n\n아이디\n";
                Score_1.text += "\n\nMAIN 2 (제한 시간)\n";
                foreach (var e in d.rank_mine)
                {
                    Rank_Text.text += My_Rank.ToString() + '\n' + '\n';
                    ID.text += e.id + '\n' + '\n';
                    Score_1.text += e.main_score_2.ToString() + '\n' + '\n';
                }



                Rank_Text.text += "전체 랭킹\n";
                Rank_Text.text += "\n등수\n";
                ID.text += "\n\n아이디\n";
                Score_1.text += "\n\nMAIN 2 (제한 시간)\n";

                Index = 0;

                foreach (var e in d.rank_total)
                {
                    Index++;
                    Rank_Text.text += Index.ToString() + '\n';
                    ID.text += e.id + '\n';
                    Score_1.text += e.main_score_2.ToString() + '\n';
                }

            }
            if (Params == 2)
            {
                Ranking_Main_3_Up d = JsonUtility.FromJson<Ranking_Main_3_Up>(singleTone.request.downloadHandler.text);

                foreach (var u in d.rank_total)
                    dic.Add(u.id, u.main_score_3);

                var myList_Ollim = dic.ToList();

                int Index = 0;

                foreach (var e in myList_Ollim)
                {
                    Index++;
                    foreach (var u in d.rank_mine)
                    {
                        if (e.Key == u.id)
                            My_Rank = Index;
                    }
                }

                Rank_Text.text += "내 랭킹\n";

                Rank_Text.text += "\n등수\n";
                ID.text += "\n\n아이디\n";
                Score_1.text += "\n\nMAIN 3 (점수)\n";
                foreach (var e in d.rank_mine)
                {
                    Rank_Text.text += My_Rank.ToString() + '\n' + '\n';
                    ID.text += e.id + '\n' + '\n';
                    Score_1.text += e.main_score_3.ToString() + '\n' + '\n';
                }

                Rank_Text.text += "전체 랭킹\n";
                Rank_Text.text += "\n등수\n";
                ID.text += "\n\n아이디\n";
                Score_1.text += "\n\nMAIN 3 (점수)\n";

                Index = 0;

                foreach (var e in d.rank_total)
                {
                    Index++;
                    Rank_Text.text += Index.ToString() + '\n';
                    ID.text += e.id + '\n';
                    Score_1.text += e.main_score_3.ToString() + '\n';
                }
            }
            if (Params == 3)
            {
                Ranking_Final_1_Up d = JsonUtility.FromJson<Ranking_Final_1_Up>(singleTone.request.downloadHandler.text);

                foreach (var u in d.rank_total)
                    dic.Add(u.id, u.final_score_1);

                var myList_Ollim = dic.ToList();

                myList_Ollim.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));

                int Index = 0;

                foreach (var e in myList_Ollim)
                {
                    Index++;
                    foreach (var u in d.rank_mine)
                    {
                        if (e.Key == u.id)
                            My_Rank = Index;
                    }
                }

                Rank_Text.text += "내 랭킹\n";

                Rank_Text.text += "\n등수\n";
                ID.text += "\n\n아이디\n";
                Score_1.text += "\n\nFINAL 1 (데스 카운트)\n";
                foreach (var e in d.rank_mine)
                {
                    Rank_Text.text += My_Rank.ToString() + '\n' + '\n';
                    ID.text += e.id + '\n' + '\n';
                    Score_1.text += e.final_score_1.ToString() + '\n' + '\n';
                }

                Rank_Text.text += "전체 랭킹\n";
                Rank_Text.text += "\n등수\n";
                ID.text += "\n\n아이디\n";
                Score_1.text += "\n\nFINAL 1 (데스 카운트)\n";

                Index = 0;

                foreach (var e in d.rank_total)
                {
                    Index++;
                    Rank_Text.text += Index.ToString() + '\n';
                    ID.text += e.id + '\n';
                    Score_1.text += e.final_score_1.ToString() + '\n';
                }
            }
            if (Params == 4)
            {
                Ranking_Final_2_Up d = JsonUtility.FromJson<Ranking_Final_2_Up>(singleTone.request.downloadHandler.text);

                foreach (var u in d.rank_total)
                    dic.Add(u.id, u.final_score_2);

                var myList_Ollim = dic.ToList();

                myList_Ollim.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));

                int Index = 0;

                foreach (var e in myList_Ollim)
                {
                    Index++;
                    foreach (var u in d.rank_mine)
                    {
                        if (e.Key == u.id)
                            My_Rank = Index;
                    }
                }

                Rank_Text.text += "내 랭킹\n";

                Rank_Text.text += "\n등수\n";
                ID.text += "\n\n아이디\n";
                Score_1.text += "\n\nFINAL 2 (데스 카운트)\n";
                foreach (var e in d.rank_mine)
                {
                    Rank_Text.text += My_Rank.ToString() + '\n' + '\n';
                    ID.text += e.id + '\n' + '\n';
                    Score_1.text += e.final_score_2.ToString() + '\n' + '\n';
                }

                Rank_Text.text += "전체 랭킹\n";
                Rank_Text.text += "\n등수\n";
                ID.text += "\n\n아이디\n";
                Score_1.text += "\n\nFINAL 2 (데스 카운트)\n";

                Index = 0;

                foreach (var e in d.rank_total)
                {
                    Index++;
                    Rank_Text.text += Index.ToString() + '\n';
                    ID.text += e.id + '\n';
                    Score_1.text += e.final_score_2.ToString() + '\n';
                }

            }
        }
        yield break;
    }

 
    IEnumerator SignUp_Behave()
    {
        Popup.SetActive(true);
        Popup_X.SetActive(false);

        IEnumerator wait_load = Wait_Load();
        StartCoroutine(wait_load);

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
            Popup_X.SetActive(true);
            infoText.color = Color.red;
            infoText.text = www.error + '\n' + www.downloadHandler.text;
        }
        else
        {
            Popup_X.SetActive(true);
            if (www.downloadHandler.text == "null")
            {
                infoText.color = Color.red;
                infoText.text = "회원가입 실패";
            }
            else
            {
                infoText.color = Color.blue;
                infoText.text = www.downloadHandler.text;
                Popup_X.SetActive(true);
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
            yield return StaticFunc.WaitForRealSeconds(0.15f);

            infoText.text = "로딩 중......";
            yield return StaticFunc.WaitForRealSeconds(0.15f);

            infoText.text = "로딩 중........";
            yield return StaticFunc.WaitForRealSeconds(0.15f);
        }
    }
    public void Enter_X()
    {
        if (Scroll_View.activeSelf)
            Scroll_View.SetActive(false);
    }
}