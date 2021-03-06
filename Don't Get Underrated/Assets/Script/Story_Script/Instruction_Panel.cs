using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Instruction_Panel : MonoBehaviour
{
    Dictionary<int, string[]> Dic_Context;

    List<int> Already_Read;

    [SerializeField]
    AudioClip Mouse_Sound;

    [SerializeField]
    AudioClip KeyBoard_Sound;

    AudioSource audioSource;

    [SerializeField]
    Text Context;

    [SerializeField]
    GameObject Before_Button;

    [SerializeField]
    GameObject After_Button;

    [SerializeField]
    GameObject Right_Bar;

    [SerializeField]
    GameObject Left_Bar;

    [SerializeField]
    GameObject Game_Start;

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
        Right_Bar.SetActive(false);
        Left_Bar.SetActive(false);
        Game_Start.SetActive(false);
        Dic_Context.Add(0, new string[] { "방향키로 좌, 우 이동", "스페이스 바를 적절히 눌러 점프 세기 조절", "발판에서 미끄러지지 말고 무사히 정상까지 오르자!", "굿 럭!" });
        Dic_Context.Add(1, new string[] { "마우스를 연등을 날릴 방향에 반대 방향으로 끌어 당기자.", "끌어당긴 길이가 곧 세기", "공중에서 여러번 조작은 가능하지만 세기가 감소",
                                            "중간중간 나오는 탄성을 없애주는 껌 아이템을 먹고 1번을 누르면 10초간 효과 발동", "아이템을 먹었는지 마우스 오른쪽을 눌러 확인", 
                                            "물체에서 떨어지지 않게 조심하여 꼭대기까지 가보자!", "굿 럭!" });
        Dic_Context.Add(2, new string[] { "A와 D로 플레이어의 좌, 우 이동", "학생을 클릭하여 게이지를 채우자.", "과제와 레포트의 방해 레이저가 온다면 마우스 클릭을 연타하여 이겨내자.", 
                                            "학생을 구출하면 점수와 연등 피버게이지 획득", "피버 타임이 되면 공격 및 이동속도 증가", "제한 시간내에 최대한 많은 학생들을 구출하자!",
                                            "굿 럭!"});
        Dic_Context.Add(3, new string[] { "방향키를 눌러 플레이어 조작", "스페이스바를 눌러 공격", "Z키를 눌러 큰 데미지를 주는 폭탄 사용", "보스의 공격을 피해 최대한 적은 death count로 보스의 체력을 0으로 만들자!",
                                            "굿 럭!" });
        Dic_Context.Add(4, new string[] { "좌, 우 방향키를 눌러 플레이어 이동", "스페이스를 눌러 점프. 스페이스를 연속으로 눌러 이단점프까지 가능", "보스의 공격을 피해 최대한 적은 death count로 2분 동안 버티자!", "굿 럭!" });
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
        Right_Bar.SetActive(false);
        Left_Bar.SetActive(false);
        Game_Start.SetActive(false);

        if (this.StageNum != StageNum || Index >= Dic_Context[this.StageNum].Length || Index <= -1) // 예외 사항
        {
            //Debug.Log("this.StageNum: " + this.StageNum);
            //Debug.Log("StageNum: " + StageNum);
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
                // 우선은 문장 길이 25 당 3초 출력으로 정해둠 (3 / 25) = 0.12
                Already_Read.Add(Index);
            }

            DOTween.Sequence()
            .Append(Context.DOText(Dic_Context[StageNum][Index], Time_Persist)) // 키보드 소리 재생하면서 텍스트를 Time_Persist초 동안 출력

            .OnComplete(() =>
            {
                audioSource.Stop();
                if (Index == 0)
                {
                    After_Button.SetActive(true);
                    Right_Bar.SetActive(true);
                }


                else if (Index == Dic_Context[this.StageNum].Length - 1)
                {
                    Before_Button.SetActive(true);
                    Left_Bar.SetActive(true);
                    Game_Start.SetActive(true);
                }

                else
                {
                    After_Button.SetActive(true);
                    Before_Button.SetActive(true);
                    Right_Bar.SetActive(true);
                    Left_Bar.SetActive(true);
                }
            }); // 텍스트 출력 완료 시 해야하는 것들
        });
    }
}
