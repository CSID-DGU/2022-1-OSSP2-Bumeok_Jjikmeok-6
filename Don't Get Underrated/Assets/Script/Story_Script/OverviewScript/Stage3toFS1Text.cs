using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Stage3toFS1Text : MonoBehaviour
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
        Dic_Context.Add(0, new string[] { "�������� �л����� ������ �� ������ ������ ���Դ�.", "�л����� �����ϸ� �ٵ� �౸������ ���ߴ�.", "���� �׷� ���� ���� �౸������ �̵��غ���?"});
        Dic_Context.Add(1, new string[] { "�ٷ� �׶�!", "�Ƽ���: ��� �����°ų�!!!!", "�Ƽ���: ���� �л����� ������ ������ \n�����ξ��µ�, �װ� �����ϴٴ�!!!", "�̰� ���� ���� �����̷Ǵ�!!!" });
        Dic_Context.Add(2, new string[] { "�׿� ���ÿ� ���� �ϴ÷� �������.", "���ƾƾ� ����ּ���Ф�", "�Ƽ���: �� ������ ��� �̰ܳ��� \n�� ����Ʈ���ٸ�", "�� ����ִ� �� �����غ���.", "�Ƽ���: ������ ���� ������?" });
        Dic_Context.Add(3, new string[] { "���߿��� ���̴� �츮 �б��� �þ߿� ���´�.", "���� ���� ���� ���� ������ ���������̶��Ф�"});
    }
    void Start()
    {
        //string path = Application.dataPath;
        //path += "/Data/wa.txt";
        //string[] contents = System.IO.File.ReadAllLines(path);
        // ���� ���� �ؾ��� �κ�

        //StageNum = PlayerPrefs.GetInt("StageNum", 2);
        Check(); // StageNum�� �������� Ŭ���� ���� PlayerPref�� �������ִ� ����. ����� ���� ������ 0���� �ڵ� �ʱ�ȭ
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

        if (Index >= Dic_Context[this.PhotoNum].Length) // ���� ����
        {
            PhotoNum++;

            if (Index <= -1)
            {
                //Debug.Log("this.StageNum: " + this.PhotoNum);
                //Debug.Log("StageNum: " + PhotoNum);
                Context.text = "�ε��� �ʰ�";
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
        .Append(Context.DOText("", 0.7f)) // �� ���ڿ��� 0.7�� ���
        .OnComplete(() =>
        {
            audioSource.Stop();
            audioSource.clip = KeyBoard_Sound;
            audioSource.Play();

            Time_Persist = Dic_Context[PhotoNum][Index].Length * 0.12f; // ���� �ʾ��� ��. �ؽ�Ʈ ��� �ð��� �����Ӱ� �����ϸ� ��

            DOTween.Sequence()
            .Append(Context.DOText(Dic_Context[PhotoNum][Index], Time_Persist)) // Ű���� �Ҹ� ����ϸ鼭 �ؽ�Ʈ�� Time_Persist�� ���� ���
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