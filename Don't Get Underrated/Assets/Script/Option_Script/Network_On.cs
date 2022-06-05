using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class Network_On : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    GameObject Network_Off;

    [SerializeField]
    TextMeshProUGUI ErrorMessage;

    [SerializeField]
    GameObject Button1;

    [SerializeField]
    GameObject Button2;

    protected IEnumerator is_network;
    private void Awake()
    {
        Network_Off.SetActive(false);
        is_network = Network_Check_Infinite();
    }
    private void Start()
    {
        if (is_network != null)
            StopCoroutine(is_network);
        is_network = Network_Check_Infinite();
        StartCoroutine(is_network);
    }

    [System.Serializable]
    public class Haha
    {
        public string user_info;
        public string message;
    }

    protected IEnumerator Network_Check_Infinite()
    {
        while (true)
        {
            singleTone.request = UnityWebRequest.Get("http://localhost:3000/continue_connect");

            yield return singleTone.request.SendWebRequest();

            Button1.SetActive(true);
            Button2.SetActive(false);

            if ((singleTone.request.result == UnityWebRequest.Result.ConnectionError) ||
                (singleTone.request.result == UnityWebRequest.Result.ProtocolError) ||
                (singleTone.request.result == UnityWebRequest.Result.DataProcessingError))
            {
                Time.timeScale = 0;
                Network_Off.SetActive(true);
                ErrorMessage.text = singleTone.request.error + '\n' + singleTone.request.downloadHandler.text;
                yield break;
            }
            else
            {
                Haha d = JsonUtility.FromJson<Haha>(singleTone.request.downloadHandler.text);
                if (singleTone.id != d.user_info)
                {
                    Time.timeScale = 0;
                    Network_Off.SetActive(true);
                    ErrorMessage.text = "계정 정보가 틀리거나 서버가 끊겼습니다.";
                    yield break;
                }
                else
                {
                    Time.timeScale = 1;
                    Network_Off.SetActive(false);
                }
            }
            yield return YieldInstructionCache.WaitForEndOfFrame;
        }
    }
    public void Real_End()
    {
        Button1.SetActive(false);
        Button2.SetActive(true);
        ErrorMessage.text = "게임을 종료합니다.";
    }

    public void Enter_End()
    {
        Debug.Log("흠");
        StartCoroutine(Fade_Out());
    }

    IEnumerator Fade_Out()
    {
        if (GameObject.Find("Jebal") && GameObject.Find("Jebal").TryGetComponent(out SpriteColor s1))
        {
            yield return s1.StartCoroutine(s1.Change_Color_Real_Time(Color.black, 2));
            // 씬 이동
        }
        else
            yield return null;
    }
}
