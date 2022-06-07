using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Outro : MonoBehaviour
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
    public GameObject Sixth;

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
        Dic_Context.Add(0, new string[] { "아수라의 약속대로 2분의 번뇌의 \n시간을 버티니 다시 현실로 돌아왔다.", "픽셀 세상이 아닌 내가 살고 있는 세상으로", "어느새 세상은 어두컴컴 밤이 되어있었다." });
        Dic_Context.Add(1, new string[] { "아 드디어 현실로 돌아왔구나", "너무 힘든 하루였어...", "나도 이제 슬슬 집으로 돌아가야겠다" });
        Dic_Context.Add(2, new string[] { "그렇게 발걸음을 옮기다가 문득 다시 뒤를 돌아봤다.", "그러고보니 축제를 못 즐겼네... 아쉽다", "안 그래도 이 세상에선 벌써 2년간 축제도 못 즐긴 것 같다." });
        Dic_Context.Add(3, new string[] { "사실 아수라의 번뇌가 이미 이 세상을 뒤덮은거 아닐까", "서울 도심이 오늘 따라 어두워보인다" });
        Dic_Context.Add(4, new string[] { "오늘은 일찍 자고 내일은 \n더욱 힘차게 학교 생활을 해야겠다.", "더군다나 오늘 말 그대로 \n게임을 직접 체험을 해봤으니까..." });
        Dic_Context.Add(5, new string[] { "다시 즐거운 내일의 학교 생활을 기대하며 충무로역 지하철에 몸을 실었다.", "==THE END==" });
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
            case 5:
                ImageNow = Sixth;
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