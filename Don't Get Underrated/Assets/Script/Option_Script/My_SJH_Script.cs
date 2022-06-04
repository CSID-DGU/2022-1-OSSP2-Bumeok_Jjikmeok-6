using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

public class My_SJH_Script : MonoBehaviour
{
    private static string nextScene;
    public float currentValue;
    public float speed = 25;

    [SerializeField]
    Text Wait_text;

    [SerializeField]
    Text Before_Rank;

    [SerializeField]
    Text After_Rank;

    [System.Serializable]
    class Old_Rank
    {
        public string id;
        public int main_score_1;
        public int main_score_2;
        public int main_score_3;
        public int final_score_1;
        public int final_score_2;
    }

    [System.Serializable]
    class New_Rank
    {
        public string id;
        public int main_score_1;
        public int main_score_2;
        public int main_score_3;
        public int final_score_1;
        public int final_score_2;
    }

    [System.Serializable]
    class Json_From_Rank_DB
    {
        public Old_Rank[] old_rank;
        public New_Rank[] new_rank;
        public string successful_message;
    }

    private void Awake()
    {
        Before_Rank.color = Color.clear;
        After_Rank.color = Color.clear;
    }

    void Start()
    {
        StartCoroutine(Update_Rank());
    }
    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }
    IEnumerator Update_Rank()
    {
        yield return Wait_Text_IEnum(7);

        WWWForm form = new WWWForm();

        form.AddField("main_stage_1_score", 1000);
        form.AddField("main_stage_2_score", 2000);
        form.AddField("main_stage_3_score", 3000);
        form.AddField("final_stage_1_score", 4000);
        form.AddField("final_stage_2_score", 77);

        //form.AddField("main_stage_1_score", singleTone.main_stage_1_score);
        //form.AddField("main_stage_2_score", singleTone.main_stage_2_score);
        //form.AddField("main_stage_3_score", singleTone.main_stage_3_score);
        //form.AddField("final_stage_1_score", singleTone.final_stage_1_score);
        //form.AddField("final_stage_2_score", singleTone.final_stage_2_score);

        singleTone.request = UnityWebRequest.Post("http://localhost:3000/Set_Rank", form);
        yield return singleTone.request.SendWebRequest();
        Debug.Log(singleTone.request.result);
        Debug.Log(singleTone.request.downloadHandler.text);

        if ((singleTone.request.result == UnityWebRequest.Result.ConnectionError) ||
            (singleTone.request.result == UnityWebRequest.Result.ProtocolError) ||
            (singleTone.request.result == UnityWebRequest.Result.DataProcessingError))
        {
            Before_Rank.color = Color.clear;
            After_Rank.color = Color.clear;
            Wait_text.color = Color.red;
            Wait_text.text = singleTone.request.error + '\n' + singleTone.request.downloadHandler.text;
        }
        else
        {
            Before_Rank.color = Color.green;
            After_Rank.color = Color.blue;
            Wait_text.color = Color.clear;
            Json_From_Rank_DB d = JsonUtility.FromJson<Json_From_Rank_DB>(singleTone.request.downloadHandler.text);
            Before_Rank.text = "";
            After_Rank.text = "";
            foreach (var e in d.old_rank)
            {
                Before_Rank.text += e.id + "의 예전 랭킹\n";
                Before_Rank.text += "메인 1 : " + e.main_score_1 + '\n';
                Before_Rank.text += "메인 2 : " + e.main_score_2 + '\n';
                Before_Rank.text += "메인 3 : " + e.main_score_3 + '\n';
                Before_Rank.text += "최종 1 : " + e.final_score_1 + '\n';
                Before_Rank.text += "최종 2 : " + e.final_score_2 + '\n';
            }

            foreach (var e in d.new_rank)
            {
                After_Rank.text += e.id + "의 예전 랭킹\n";
                After_Rank.text += "메인 1 : " + e.main_score_1 + '\n';
                After_Rank.text += "메인 2 : " + e.main_score_2 + '\n';
                After_Rank.text += "메인 3 : " + e.main_score_3 + '\n';
                After_Rank.text += "최종 1 : " + e.final_score_1 + '\n';
                After_Rank.text += "최종 2 : " + e.final_score_2 + '\n';
            }
        }
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
