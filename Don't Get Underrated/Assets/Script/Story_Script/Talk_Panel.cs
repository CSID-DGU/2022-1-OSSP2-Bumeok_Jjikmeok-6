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

    [SerializeField]
    GameObject Right_Bar;

    [SerializeField]
    GameObject Left_Bar;

    [SerializeField]
    GameObject Game_Instruction;

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
        Game_Instruction.SetActive(false);
        Dic_Context.Add(0, new string[] { "으어어 여긴 어디지...", "뭐야 왜 세상이 다 픽셀인거야?", "그리고 이건... 내가 어제 깨고 있던 게임인데?", "그럼 정상까지 올라야 여기서 벗어날 수 있는건가?", "일단 정상까지 올라가보자!" });
        Dic_Context.Add(1, new string[] { "??? 2: 이 연등을 꼭대기까지 가져가면 \n너가 법학/만해 순서를 결정할 수 있어", "??? 1: 물론 거기까지 가는게 힘들지만, \n부탁이야 너가 정해줘!",
                                            "난감하네... \n그래도 이렇게 부탁하니 한 번 해볼까?"});
        Dic_Context.Add(2, new string[] { "역시 학생들이 과제와 레포트로 감시받고 있는 것 같네", "근데 이 떨어져 있는 연등은 뭐지? \n팔정도에서 떨어졌나?", "연등을 조작하자 레이저가 나가고, \n레포트와 과제로부터 학생이 벗어난다..",
                                            "???: 해방이다~! 축제야 기다려라~", "그래 이걸로 학생들을 구출하면 되는건가?"});
        Dic_Context.Add(3, new string[] { "아수라: 나를 쓰러트리기만 한다면 널 보내준다고 약조하지. 단,... ", "그러자 갑자기 손에 꼬끼리 모양의 물총이 생겼다.", "아수라: 그 물총으로 날 쓰러트린다면 말이지! 으하하하하",
                                            "아수라: 참고로 지금까지 내 공격을 \n피했던 사람치고 무사한 이들은 없었지. ", "아수라: 각오해라!!!!"});
        Dic_Context.Add(4, new string[] { "아수라: 다만, 내가 약조한 것도 있으니 2분만 내 공격을 참으면 이 모든걸 철회하도록 하지.", "아수라: 물론 너도 집에 갈 수 있게 되고", "ㅠㅠ 그냥 집에 가고 싶지만... \n그래 그럼 한 번만 더 견뎌보자.", 
                                            "아수라의 공격을 버텨내고 꼭 집에 돌아가고 말겠어!" });
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
        Game_Instruction.SetActive(false);

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
                    Game_Instruction.SetActive(true);
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
