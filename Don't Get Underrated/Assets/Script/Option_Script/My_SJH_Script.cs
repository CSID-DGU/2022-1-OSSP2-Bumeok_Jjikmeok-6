using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

public class My_SJH_Script : MonoBehaviour
{
    public float currentValue;
    public float speed = 25;

    [SerializeField]
    Text Wait_text;

    [SerializeField]
    Text Before_Rank;

    [SerializeField]
    Text After_Rank;

    [SerializeField]
    GameObject Button1;

    [SerializeField]
    GameObject Button2;

    SpriteColor spriteColor;

    [System.Serializable]
    class Old_Rank
    {
        public string id = "";
        public int main_score_1 = 0;
        public int main_score_2 = 0;
        public int main_score_3 = 0;
        public int final_score_1 = 0;
        public int final_score_2 = 0;
    }

    [System.Serializable]
    class New_Rank
    {
        public string id = "";
        public int main_score_1 = 0;
        public int main_score_2 = 0;
        public int main_score_3 = 0;
        public int final_score_1 = 0;
        public int final_score_2 = 0;
    }

    [System.Serializable]
    class Json_From_Rank_DB
    {
        public Old_Rank[] old_rank = null;
        public New_Rank[] new_rank = null;
        public string successful_message = "";
    }

    private void Awake()
    {
        Before_Rank.color = Color.clear;
        After_Rank.color = Color.clear;
        Button1.SetActive(false);
        Button2.SetActive(false);
        if (GameObject.Find("Network_Sprite") && GameObject.Find("Network_Sprite").TryGetComponent(out SpriteColor s1))
        {
            spriteColor = s1;
            spriteColor.Set_BG_Clear();
        }
    }

    void Start()
    {
        StartCoroutine(Update_Rank());
    }
    IEnumerator Update_Rank()
    {
        yield return Wait_Text_IEnum(7);

        WWWForm form = new WWWForm();

        form.AddField("main_stage_1_score", singleTone.main_stage_1_score);
        form.AddField("main_stage_2_score", singleTone.main_stage_2_score);
        form.AddField("main_stage_3_score", singleTone.main_stage_3_score);
        form.AddField("final_stage_1_score", singleTone.final_stage_1_score);
        form.AddField("final_stage_2_score", singleTone.final_stage_2_score);

        singleTone.request = UnityWebRequest.Post("http://localhost:3000/set_rank", form);
        yield return singleTone.request.SendWebRequest();

        if ((singleTone.request.result == UnityWebRequest.Result.ConnectionError) ||
            (singleTone.request.result == UnityWebRequest.Result.ProtocolError) ||
            (singleTone.request.result == UnityWebRequest.Result.DataProcessingError))
        {
            Before_Rank.color = Color.clear;
            After_Rank.color = Color.clear;
            Wait_text.color = Color.red;
            Wait_text.text = singleTone.request.error + '\n' + singleTone.request.downloadHandler.text + '\n';
            Wait_text.text += "혹은 서버와의 접속이 끊겨 랭킹 반영이 안됐을 수 있습니다." + '\n';
        }

        else
        {
            if (singleTone.EasterEgg)
            {
                Before_Rank.color = Color.clear;
                After_Rank.color = Color.clear;
                Wait_text.color = Color.red;
                Wait_text.text = "이스터에그를 적용한 사람은 랭킹 반영이 안됩니다. 죄송합니다.";
                singleTone.EasterEgg = false;
            }
            else
            {
                Json_From_Rank_DB d = JsonUtility.FromJson<Json_From_Rank_DB>(singleTone.request.downloadHandler.text);
               
                foreach (var e in d.old_rank)
                {
                    if (singleTone.id != e.id)
                    {
                        Before_Rank.color = Color.clear;
                        After_Rank.color = Color.clear;
                        Wait_text.color = Color.red;
                        Wait_text.text = "게임 내에서 로그인을 안 했군요!";
                        Button1.SetActive(true);
                        yield break;
                    }
                }

                Before_Rank.color = Color.green;
                After_Rank.color = Color.blue;
                Wait_text.color = Color.clear;
                Before_Rank.text = "";
                After_Rank.text = "";
                foreach (var e in d.old_rank)
                {
                    Before_Rank.text += e.id + "의 예전 랭킹\n";
                    Before_Rank.text += "메인 1 : " + e.main_score_1 + " / ";
                    Before_Rank.text += "메인 2 : " + e.main_score_2 + " / ";
                    Before_Rank.text += "메인 3 : " + e.main_score_3 + '\n';
                    Before_Rank.text += "최종 1 : " + e.final_score_1 + " / ";
                    Before_Rank.text += "최종 2 : " + e.final_score_2 + " / ";
                }

                foreach (var e in d.new_rank)
                {
                    After_Rank.text += e.id + "의 최신 랭킹\n";
                    After_Rank.text += "메인 1 : " + e.main_score_1 + " / ";
                    After_Rank.text += "메인 2 : " + e.main_score_2 + " / ";
                    After_Rank.text += "메인 3 : " + e.main_score_3 + '\n';
                    After_Rank.text += "최종 1 : " + e.final_score_1 + " / ";
                    After_Rank.text += "최종 2 : " + e.final_score_2 + " / ";
                }
            }
        }
        Button1.SetActive(true);
        yield break;
    }

    public void Enter_End()
    {
        Button1.SetActive(false);
        Button2.SetActive(true);
        Before_Rank.color = Color.clear;
        After_Rank.color = Color.clear;
        Wait_text.color = Color.white;
        Wait_text.text = "게임을 종료합니다.";
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
            singleTone.SceneNumManage = 0;
            SceneManager.LoadScene(singleTone.SceneNumManage);
        }
        else
            yield return null;
    }
    IEnumerator Wait_Text_IEnum(int Count)
    {
        for (int i = 0; i < Count; i++)
        {
            Wait_text.text = "랭킹 반영 중입니다.\n잠시만 기다려주세요..";
            yield return YieldInstructionCache.WaitForSeconds(0.2f);

            Wait_text.text = "랭킹 반영 중입니다.\n잠시만 기다려주세요....";
            yield return YieldInstructionCache.WaitForSeconds(0.2f);

            Wait_text.text = "랭킹 반영 중입니다.\n잠시만 기다려주세요......";
            yield return YieldInstructionCache.WaitForSeconds(0.2f);
        }
    }
}