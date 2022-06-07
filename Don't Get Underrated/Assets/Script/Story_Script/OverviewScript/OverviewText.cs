using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class OverviewText : MonoBehaviour
{
    Dictionary<int, string[]> Dic_Context;

    [SerializeField]
    AudioClip Mouse_Sound;

    [SerializeField]
    AudioClip KeyBoard_Sound;

    AudioSource audioSource;

    [SerializeField]
    Text Context;


    public GameObject Seoul_Morning;
    public GameObject Subway;
    public GameObject School_Going;
    public GameObject School_Morning;
    public GameObject School_InClass;

    public GameObject ImageNow;

    int Index;
    public int PhotoNum;
    int MaxNum = 4;
    int sceneNum;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        Index = 0;
        Dic_Context = new Dictionary<int, string[]>();
        Dic_Context.Add(0, new string[] { "오늘도 서울의 새 아침이 밝았다.", "분명 어제 저녁 먹고 잠깐 한다던 게임이 \n밤을 셀 줄이야", "이런... \n오늘 그러고보니 오전 9시부터 강의구나!" });
        Dic_Context.Add(1, new string[] { "대충 준비한 채 서둘러 지하철을 타고", "충무로 역이라는 도착안내 \n방송과 함께 지하철에서 내린다." });
        Dic_Context.Add(2, new string[] { "익숙한 학교 가는 길...", "오늘은 남은 구간을 꼭 깬 뒤에 자고 만다!" });
        Dic_Context.Add(3, new string[] { "다짐을 하며 좋은 날씨와 함께 학교에 \n도착한 뒤,", "강의실로 발걸음을 옮긴다." });
        Dic_Context.Add(4, new string[] { "강의가 시작되고 20분 지날 무렵,", "역시 밤을 세는 건 무리였나...", "무거워진 눈꺼풀이 스르르 감긴다.", "하이고 졸음이.....?" });
    }
    void Start()
    {
        //string path = Application.dataPath;
        //path += "/Data/wa.txt";
        //string[] contents = System.IO.File.ReadAllLines(path);
        // 여긴 내가 해야할 부분

        //StageNum = PlayerPrefs.GetInt("StageNum", 2);
        Check(); // StageNum은 스테이지 클리어 이후 PlayerPref로 지정해주는 거임. 현재는 전역 변수니 0으로 자동 초기화
        MaxNum = Dic_Context.Count;
        this.sceneNum = GameObject.Find("SkipButton").GetComponent<SkipController>().sceneNum;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DOTween.KillAll();
            Next();
            photoCheck();

            if(PhotoNum == MaxNum)
            {
                //Debug.Log("PhotoNum" + PhotoNum);
                GoingNextScene();
            }
        }
    }

    public void Next()
    {
        //Debug.Log("Mouse Pressed");
        audioSource.clip = Mouse_Sound;
        audioSource.Play();
        Index++;
        Check();
    }
    
    void Check()
    {
        float Time_Persist;

        if (Index >= Dic_Context[this.PhotoNum].Length) // 예외 사항
        {
            PhotoNum++;

            if (Index <= -1)
            {
                //Debug.Log("this.StageNum: " + this.PhotoNum);
                //Debug.Log("StageNum: " + PhotoNum);
                Context.text = "인덱스 초과";
                return;
            }
            else if(PhotoNum == MaxNum)
            {
                return;
            }
            else
            {
                //Debug.Log("PhotoNum Increased");
                Index = 0;
            }
        }

        DOTween.Sequence()
        .Append(Context.DOText("", 0.7f)) // 빈 문자열로 0.7초 대기
        .OnComplete(() =>
        {
            audioSource.Stop();
            audioSource.clip = KeyBoard_Sound;
            audioSource.Play();

            Time_Persist = Dic_Context[PhotoNum][Index].Length * 0.12f; // 읽지 않았을 때. 텍스트 출력 시간은 자유롭게 설정하면 됨

            DOTween.Sequence()
            .Append(Context.DOText(Dic_Context[PhotoNum][Index], Time_Persist)) // 키보드 소리 재생하면서 텍스트를 Time_Persist초 동안 출력
            .OnComplete(() =>
             {
                 audioSource.Stop();
             })
            .OnKill(() =>
            {
                audioSource.Stop();
            });
        });
    }

    void photoCheck()
    {
        switch (PhotoNum)
        {
            case 0:
                ImageNow = Seoul_Morning;
                break;
            case 1:
                ImageNow = Subway;
                break;
            case 2:
                ImageNow = School_Going;
                break;
            case 3:
                ImageNow = School_Morning;
                break;
            case 4:
                ImageNow = School_InClass;
                break;
            default:
                break;
        }
    }

    public void GoingNextScene()
    {
        StartCoroutine(LoadingNextStage());
    }

    IEnumerator LoadingNextStage()
    {
        float fadeTime = GameObject.Find("Fading").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        //sceneNum++;
        //SceneManager.LoadScene(sceneNum);
        singleTone.SceneNumManage++;
        SceneManager.LoadScene(singleTone.SceneNumManage);
    }
}