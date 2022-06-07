using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Stage1to2Text : MonoBehaviour
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
        Dic_Context.Add(0, new string[] { "모든게 픽셀로만 되어있는 세상", "분명 처음이지만 낯설지는 않은 것 같네.", "게임을 너무 많이 했나...", "근데 게임 속에서도 108계단을 오르며 등교라니ㅠㅠ" });
        Dic_Context.Add(1, new string[] { "그래도 등교하며 높아지는 고도에 바뀌는 도심 풍경은", "픽셀 세상이어도 여전히 멋졌다." });
        Dic_Context.Add(2, new string[] { "드디어 정상을 올랐을 때 보이는 학교의 풍경은", "여전히 그대로이다.", "서울 도심이 훤히 보이는 그 곳, \n 정상까지는 힘들었지만 보람차다!" });
        Dic_Context.Add(3, new string[] { "팔정도나 한 번 둘러볼까나?","정상에 있는 팔정도를 지날 때 보이는 불상이", "오늘 만큼은 픽셀로 보여 익숙하지만 오묘하다..." });
        Dic_Context.Add(4, new string[] { "법학관을 지나고 있을 무렵,", "법학관에서 싸우는 소리가 들리는 것 같다.", "???1:올해는 분명 법학/만해관이야!", "???2: 어림도 없지! 만해/법학관임.", "점점 싸움이 격해지는 것 같아 말리러 \n가보니 대충 사연은 이러했다." });
        Dic_Context.Add(5, new string[] { "우리 학교에는 전설이 하나 있는데,", "법학과와 불교학과의 축구 대결로 \n법학/만해관의 이름 순서가 결정된다는 것이다.", "근데 올해는 축구 경기가 취소되어 \n순서가 어떻게 될지 모른다는 것이다.", "???1: 그럼 이렇게 하자. 이 친구가 게임을 깨서 정하는걸로!",
                                           "???2: 그래 제삼자가 정하는게 낫겠네!", "엥 뭐라고요???? 제가요????"});
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