using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Stage2to3Text : MonoBehaviour
{
    Dictionary<int, string[]> Dic_Context;

    [SerializeField]
    AudioClip Mouse_Sound;

    [SerializeField]
    AudioClip KeyBoard_Sound;

    AudioSource audioSource;

    [SerializeField]
    Text Context;


    public GameObject First;
    public GameObject Second;
    public GameObject Third;
    public GameObject Fourth;
    public GameObject Fifth;

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
        Dic_Context.Add(0, new string[] { "으어어어 뭐 이렇게 게임이 다 어려워", "그리고 내 결정으로 학교 건물\n 이름이 결정되는 거... 이거 맞아...?", "이 생각으로 밖을 나서는데 많은 학생들이 \n축구장으로 이동하고 있다." });
        Dic_Context.Add(1, new string[] { "그렇다! 이 세계에선 오늘이 학교 축제날이었다.", "그래서 그런지 축구장에 사람이 점점 늘어나고 있었다.", "오호, 학교 축제날이라니..." });
        Dic_Context.Add(2, new string[] { "학교 축제날이 되면 캠퍼스에 즐거운 분위기가 맴돈다.", "그리고 저녁에 신나는 축제 공연도 빠질 수 없다.", "다들 잠시 학업으로부터 벗어나 \n청춘의 열정을 쏟는 기간,", "나도 이건 못 참지. \n바로 축구장으로 달려간다!" });
        Dic_Context.Add(3, new string[] { "그런데 스치듯이 보이는 명진관 내부 광경.", "학생들이 다들 이상하게 복도를 거닐기만 한다.", "그리고 그 위에 날라다니며 감시하는 듯한 \n과제 뭉치와 레포트 용지들.", "분명 뭔가 이상하다...\n 도움을 구하는 듯한 눈빛인데" });
        Dic_Context.Add(4, new string[] { "도움을 보고도 지날칠 수는 없지! \n 당장 명진관으로 달려간다!"});
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

            if (PhotoNum == MaxNum)
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
            else if (PhotoNum == MaxNum)
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
                ImageNow = First;
                break;
            case 1:
                ImageNow = Second;
                break;
            case 2:
                ImageNow = Third;
                break;
            case 3:
                ImageNow = Fourth;
                break;
            case 4:
                ImageNow = Fifth;
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