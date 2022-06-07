using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class StageFS1toFS2Text : MonoBehaviour
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
        Dic_Context.Add(0, new string[] { "해...해냈다! 해치웠나?", "아수라: 네 이놈!!!", "아수라: 널 용서치않겠다!!!" });
        Dic_Context.Add(1, new string[] { "그와 동시에 몸이 점점 작아진다", "아수라: 너의 그 육신에 대한 제한을 두어야겠다!", "아수라: 중력도 생겨 너는 \n자유롭게 피하지도 못할게다!" });
        Dic_Context.Add(2, new string[] { "그렇게 중력의 영향으로 학교로 돌아와 땅을 밟았지만,", "이제 다시 시작이라고?" });
        Dic_Context.Add(3, new string[] { "아니 제가 뭘 그렇게 잘못했다고 이러세요ㅠㅠ", "아수라: 너는 학생들을 번뇌로 \n빠지게 하려는 내 계획을 방해했다.", "아수라: 이렇게 된 이상 이 세상 \n전부 번뇌에 빠지게 만들겠다!!!" });
        Dic_Context.Add(4, new string[] { "그 순간 아수라의 힘이 이 세상을 감싸기 시작했다.", "하... 집에 가고 싶다ㅠㅠ"});
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