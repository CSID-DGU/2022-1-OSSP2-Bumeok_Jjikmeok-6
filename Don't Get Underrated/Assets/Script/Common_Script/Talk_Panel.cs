using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class Talk_Panel : MonoBehaviour
{
    Dictionary<int, string[]> Dic_Context;

    List<int> Already_Read;

    [SerializeField]
    AudioClip Mouse_Sound;

    [SerializeField]
    AudioClip KeyBoard_Sound;

    AudioSource audioSource;

    [SerializeField]
    Text Name;

    [SerializeField]
    Text Context;

    [SerializeField]
    GameObject Before_Button;

    [SerializeField]
    GameObject After_Button;

    int Index;

    public int StageNum;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        Index = 0;
        Dic_Context = new Dictionary<int, string[]>();
        Already_Read = new List<int>();
        Before_Button.SetActive(false);
        After_Button.SetActive(false);
        Dic_Context.Add(0, new string[] { "이상 세계는 네가 생각하는 바와 많이 다르단다", "생각보다 고독하고.... 힘든 법이지...." });
        Dic_Context.Add(1, new string[] { "골프공 게임이라....", "한번 공을 조작해봐!" });
        Dic_Context.Add(2, new string[] { "눈빛 보내기에서", "어레인지했어!!" });
        Dic_Context.Add(3, new string[] { "어서와", "텐가이는 처음이지?" });
        Dic_Context.Add(4, new string[] { "히마와리는 그렇지...?", "불상과 맞서 싸우는 거" });
    }
    void Start()
    {
        //string path = Application.dataPath;
        //path += "/Data/wa.txt";
        //string[] contents = System.IO.File.ReadAllLines(path);
        // 여긴 내가 해야할 부분

        //StageNum = PlayerPrefs.GetInt("StageNum", 2);
        Check(StageNum); // StageNum은 스테이지 클리어 이후 PlayerPref로 지정해주는 거임. 현재는 전역 변수니 0으로 자동 초기화
    }
    public void Next(int StageNum) // 오른쪽 버튼 눌렀을 때 
    {
        audioSource.clip = Mouse_Sound;
        audioSource.Play();
        Index++;
        Check(StageNum);
    }
    public void Before(int StageNum) // 왼쪽 버튼 눌렀을 때 
    {
        audioSource.clip = Mouse_Sound;
        audioSource.Play();
        Index--;
        Check(StageNum);
    }
    void Check(int StageNum)
    {
        float Time_Persist;

        After_Button.SetActive(false);
        Before_Button.SetActive(false);

        if (this.StageNum != StageNum || Index >= Dic_Context[this.StageNum].Length || Index <= -1) // 예외 사항
        {
            Context.text = "인덱스 초과";
            return;
        }

        DOTween.Sequence()
        .Append(Context.DOText("", 0.7f)) // 빈 문자열로 0.7초 대기
        .OnComplete(() =>
        {
            audioSource.Stop();
            audioSource.clip = KeyBoard_Sound;
            audioSource.Play();

            if (Already_Read.Contains(Index)) // 이미 읽었을 때
                Time_Persist = 0;
            else
            {
                Time_Persist = Dic_Context[StageNum][Index].Length * 0.12f; // 읽지 않았을 때. 텍스트 출력 시간은 자유롭게 설정하면 됨
                Already_Read.Add(Index);
            }
                
            DOTween.Sequence()
            .Append(Context.DOText(Dic_Context[StageNum][Index], Time_Persist)) // 키보드 소리 재생하면서 텍스트를 Time_Persist초 동안 출력
            .OnComplete(() =>
            {
                audioSource.Stop();
                if (Index == 0)
                    After_Button.SetActive(true);

                else if (Index == Dic_Context[this.StageNum].Length - 1)
                    Before_Button.SetActive(true);

                else
                {
                    After_Button.SetActive(true);
                    Before_Button.SetActive(true);
                }
            }); // 텍스트 출력 완료 시 해야하는 것들
        });
    }
}
