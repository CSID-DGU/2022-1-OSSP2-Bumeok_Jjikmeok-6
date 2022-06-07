using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class Network_On : MonoBehaviour
{
    [SerializeField]
    GameObject Network_Off;

    [SerializeField]
    TextMeshProUGUI ErrorMessage;

    [SerializeField]
    GameObject Game_End;

    [SerializeField]
    GameObject Yes;

    // 네트워크 감지를 위한 쪽

    [SerializeField]
    TextMeshProUGUI infoText;

    [SerializeField]
    GameObject Popup_Rank;

    [SerializeField]
    GameObject Popup_X;

    [SerializeField]
    GameObject Scroll_View;

    [SerializeField]
    Text Rank_Text;

    // 랭킹 표시를 위한 쪽

    SpriteColor spriteColor;

    private void Awake()
    {
        Network_Off.SetActive(false);
        Scroll_View.SetActive(false);

        if (GameObject.Find("Network_Sprite") && GameObject.Find("Network_Sprite").TryGetComponent(out SpriteColor s1))
        {
            spriteColor = s1;
            spriteColor.Set_BG_Clear();
            //spriteColor.Set_BGColor(Color.clear);
        }
    }
    private void Start()
    {
        StartCoroutine(Network_Check_Infinite());
    }

    [System.Serializable]
    public class Log_Message
    {
        public string user_info;
        public string message;
    }
    protected IEnumerator Network_Check_Infinite()
    {
        Player_Info player_info = null;
        Boss_Info boss_info = null;

        if (GameObject.Find("Player") && GameObject.Find("Player").TryGetComponent(out Player_Info PS_3))
            player_info = PS_3;
        if (GameObject.Find("Boss") && GameObject.Find("Boss").TryGetComponent(out Boss_Info BI))
            boss_info  = BI;

        while (true)
        {
            singleTone.request = UnityWebRequest.Get("http://localhost:3000/continue_connect");

            yield return singleTone.request.SendWebRequest();

            Game_End.SetActive(true);

            Yes.SetActive(false);

            if ((singleTone.request.result == UnityWebRequest.Result.ConnectionError) ||
                (singleTone.request.result == UnityWebRequest.Result.ProtocolError) ||
                (singleTone.request.result == UnityWebRequest.Result.DataProcessingError))
            {
                if (player_info != null)
                    player_info.Stop_When_Network_Stop();
                if (boss_info != null)
                    boss_info.Stop_When_Network_Stop();

                singleTone.ESC_On = false;
                Time.timeScale = 0;
                Network_Off.SetActive(true);
                ErrorMessage.text = singleTone.request.error + '\n' + singleTone.request.downloadHandler.text + '\n';
                ErrorMessage.text += "서버가 끊겼거나 로그인 상태가 아닙니다.\n 게임을 종료합니다.";
                yield break;
            }
            else
            {
                Log_Message d = JsonUtility.FromJson<Log_Message>(singleTone.request.downloadHandler.text);
                if (singleTone.id != d.user_info)
                {
                    if (player_info != null)
                        player_info.Stop_When_Network_Stop();
                    if (boss_info != null)
                        boss_info.Stop_When_Network_Stop();

                    singleTone.ESC_On = false;
                    Time.timeScale = 0;
                    Network_Off.SetActive(true);
                    ErrorMessage.text = "로그인 없이 서버 접속을 시도했습니다.\n 게임을 종료합니다.";
                    yield break;
                }

            }
            yield return YieldInstructionCache.WaitForEndOfFrame;
        }
    }
    public void Enter_End()
    {
        Game_End.SetActive(false);
        Yes.SetActive(true);
        ErrorMessage.text = "게임을 종료합니다.";
    }

    public void Real_End()
    {
        StartCoroutine(Fade_Out());
    }

    IEnumerator Fade_Out()
    {
        if (spriteColor != null)
        {
            yield return spriteColor.StartCoroutine(spriteColor.Change_Color_Real_Time(Color.black, 2));
        }
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
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
    public void Show_Ranking_Detail(int Params)
    {
        StopAllCoroutines();
        Popup_Rank.SetActive(true);
        StartCoroutine(Show_Detail_Ranking_For_Login(Params)); // 상세 랭킹 (로그인 O --> 본인 것까지)
    }
    public void Show_Ranking_Total()
    {
        StopAllCoroutines();
        Popup_Rank.SetActive(true);
        StartCoroutine(Show_Total_Ranking_For_Login()); // 전체 랭킹 (로그인 O --> 본인 것까지)
    }
    public void Enter_X()
    {
        StopAllCoroutines();
        Scroll_View.SetActive(false);
        StartCoroutine(Network_Check_Infinite());
    }
    public class Number
    {
        public int identifier;
    }
    IEnumerator Show_Total_Ranking_For_Login()
    {
        Popup_X.SetActive(false);
        Scroll_View.SetActive(false);
        Rank_Text.text = "";

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

            Rank_Text.text += "내 랭킹 \n";
            Rank_Text.text += "아이디\t MAIN 1 \t MAIN 2 \t MAIN 3 \t FINAL 1 \t FINAL 2\n";

            foreach (var e in d.rank_mine)
                Rank_Text.text += e.id + '\t' + e.main_score_1 + '\t' + e.main_score_2 + '\t' + e.main_score_3 + '\t' + e.final_score_1 + '\t' + e.final_score_2 + '\n';

            Index = 0;

            Rank_Text.text += "\n전체 랭킹 \n";
            Rank_Text.text += "아이디\t MAIN 1 \t MAIN 2 \t MAIN 3 \t FINAL 1 \t FINAL 2\n";

            foreach (var e in d.rank_total)
            {
                Index++;
                Rank_Text.text += e.id + '\t' + e.main_score_1 + '\t' + e.main_score_2 + '\t' + e.main_score_3 + '\t' + e.final_score_1 + '\t' + e.final_score_2 + '\n';
            }
        }
        yield break;
    }
    IEnumerator Show_Detail_Ranking_For_Login(int Params)
    {
        Popup_X.SetActive(false);
        Scroll_View.SetActive(false);
        Rank_Text.text = "";

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

                Rank_Text.text += "내 랭킹 \n";
                Rank_Text.text += "등수 \t 아이디\t MAIN 1 (제한시간)\n";
                foreach (var u in d.rank_mine)
                    Rank_Text.text += My_Rank + " " + '\t' + u.id + '\t' + u.main_score_1 + '\n';

                Index = 0;


                Rank_Text.text += "\n전체 랭킹 \n";
                Rank_Text.text += "등수 \t 아이디\t MAIN 1 (제한시간)\n";

                foreach (var e in myList_Ollim)
                {
                    Index++;
                    //Debug.Log(e.Key + " " + e.Value);
                    Rank_Text.text += Index + " " + '\t' + e.Key + '\t' + e.Value + '\n';
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
                Rank_Text.text += "내 랭킹 \n";
                Rank_Text.text += "등수 \t 아이디\t MAIN 2 (제한시간)\n";
                foreach (var u in d.rank_mine)
                    Rank_Text.text += My_Rank + " " + '\t' + u.id + '\t' + u.main_score_2 + '\n';

                Index = 0;


                Rank_Text.text += "\n전체 랭킹 \n";
                Rank_Text.text += "등수 \t 아이디\t MAIN 2 (제한시간)\n";

                foreach (var e in myList_Ollim)
                {
                    Index++;
                    //Debug.Log(e.Key + " " + e.Value);
                    Rank_Text.text += Index + " " + '\t' + e.Key + '\t' + e.Value + '\n';
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
                Rank_Text.text += "내 랭킹 \n";
                Rank_Text.text += "등수 \t 아이디\t MAIN 3 (점수)\n";
                foreach (var u in d.rank_mine)
                    Rank_Text.text += My_Rank + " " + '\t' + u.id + '\t' + u.main_score_3 + '\n';

                Index = 0;


                Rank_Text.text += "\n전체 랭킹 \n";
                Rank_Text.text += "등수 \t 아이디\t MAIN 3 (점수)\n";

                foreach (var e in myList_Ollim)
                {
                    Index++;
                    //Debug.Log(e.Key + " " + e.Value);
                    Rank_Text.text += Index + " " + '\t' + e.Key + '\t' + e.Value + '\n';
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
                Rank_Text.text += "내 랭킹 \n";
                Rank_Text.text += "등수 \t 아이디\t FINAL 1 (데스 카운트)\n";
                foreach (var u in d.rank_mine)
                    Rank_Text.text += My_Rank + " " + '\t' + u.id + '\t' + u.final_score_1 + '\n';

                Index = 0;


                Rank_Text.text += "\n전체 랭킹 \n";
                Rank_Text.text += "등수 \t 아이디\t FINAL 1 (데스 카운트)\n";

                foreach (var e in myList_Ollim)
                {
                    Index++;
                    //Debug.Log(e.Key + " " + e.Value);
                    Rank_Text.text += Index + " " + '\t' + e.Key + '\t' + e.Value + '\n';
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
                Rank_Text.text += "내 랭킹 \n";
                Rank_Text.text += "등수 \t 아이디\t FINAL 2 (데스 카운트)\n";
                foreach (var u in d.rank_mine)
                    Rank_Text.text += My_Rank + " " + '\t' + u.id + '\t' + u.final_score_2 + '\n';

                Index = 0;


                Rank_Text.text += "\n전체 랭킹 \n";
                Rank_Text.text += "등수 \t 아이디\t FINAL 2 (데스 카운트)\n";

                foreach (var e in myList_Ollim)
                {
                    Index++;
                    //Debug.Log(e.Key + " " + e.Value);
                    Rank_Text.text += Index + " " + '\t' + e.Key + '\t' + e.Value + '\n';
                }
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
}