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

            if ((singleTone.request.result == UnityWebRequest.Result.ConnectionError) ||
                (singleTone.request.result == UnityWebRequest.Result.ProtocolError) ||
                (singleTone.request.result == UnityWebRequest.Result.DataProcessingError))
            {
                Time.timeScale = 0;
                Network_Off.SetActive(true);
                ErrorMessage.text = singleTone.request.error + " " + singleTone.request.downloadHandler.text;
                yield break;
            }
            else
            {
                Haha d = JsonUtility.FromJson<Haha>(singleTone.request.downloadHandler.text);
                if (singleTone.id != d.user_info)
                {
                    Time.timeScale = 0;
                    Network_Off.SetActive(true);
                    ErrorMessage.text = "계정 정보가 틀리거나 서버가 끊겼습니다. 게임을 종료합니다.";
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
    public void ReTransmmit()
    {
        StartCoroutine(Start_ReTransmmit());
    }
    IEnumerator Start_ReTransmmit()
    {
        IEnumerator i_retransmmit = I_ReTransmmit();

        StartCoroutine(i_retransmmit);
        yield return StartCoroutine(Network_Check_Instant());

        StopCoroutine(i_retransmmit);
        yield return null;
    }
    IEnumerator Network_Check_Instant()
    {
        Button1.SetActive(false);
        Button2.SetActive(false);
        yield return YieldInstructionCache.WaitForEndOfFrame;

        UnityWebRequest www = UnityWebRequest.Get("http://localhost:3000/continue_connect");

        yield return www.SendWebRequest();

        if ((www.result == UnityWebRequest.Result.ConnectionError) ||
            (www.result == UnityWebRequest.Result.ProtocolError) ||
            (www.result == UnityWebRequest.Result.DataProcessingError))
        {
            Time.timeScale = 0;
            Network_Off.SetActive(true);
            ErrorMessage.text = www.error + " " + www.downloadHandler.text;
        }
        else
        {
            Time.timeScale = 1;
            Network_Off.SetActive(false);
            is_network = Network_Check_Infinite();
            StartCoroutine(is_network);
        }
        Button1.SetActive(true);
        Button2.SetActive(true);
        yield return null;
     }
    IEnumerator I_ReTransmmit()
    {
        while (true)
        {
            Debug.Log(1);
            ErrorMessage.text = "로딩 중....";
            yield return StaticFunc.WaitForRealSeconds(.15f);
            Debug.Log(2);
            ErrorMessage.text = "로딩 중......";
            yield return StaticFunc.WaitForRealSeconds(.15f);
            Debug.Log(3);
            ErrorMessage.text = "로딩 중........";
            yield return StaticFunc.WaitForRealSeconds(.15f);
        }
    }
    public void GameExit()
    {
        Application.Quit();
    }
}
