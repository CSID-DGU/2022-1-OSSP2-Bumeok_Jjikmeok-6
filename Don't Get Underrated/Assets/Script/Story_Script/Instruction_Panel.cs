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
        Dic_Context.Add(0, new string[] { "����Ű�� ��, �� �̵�", "�����̽� �ٸ� ������ ���� ���� ���� ����", "���ǿ��� �̲������� ���� ������ ������� ������!", "�� ��!" });
        Dic_Context.Add(1, new string[] { "���콺�� ���ϴ� ��ŭ ���� ��� ����� ���� ����", "�ѹ� ���� �����غ�!" });
        Dic_Context.Add(2, new string[] { "W�� D�� �÷��̾��� ��, �� �̵�", "������߾�!!" });
        Dic_Context.Add(3, new string[] { "���", "�ٰ��̴� ó������?" });
        Dic_Context.Add(4, new string[] { "�����͸��� �׷���...?", "�һ�� �¼� �ο�� ��" });
    }
    void Start()
    {
        //string path = Application.dataPath;
        //path += "/Data/wa.txt";
        //string[] contents = System.IO.File.ReadAllLines(path);
        // ���� ���� �ؾ��� �κ�

        //StageNum = PlayerPrefs.GetInt("StageNum", 2);
        Check(StageNum); // StageNum�� �������� Ŭ���� ���� PlayerPref�� �������ִ� ����. ����� ���� ������ 0���� �ڵ� �ʱ�ȭ
    }
    public void Next(int StageNum) // ������ ��ư ������ �� 
    {
        audioSource.clip = Mouse_Sound;
        audioSource.Play();
        Index++;
        Check(StageNum);
    }
    public void Before(int StageNum) // ���� ��ư ������ �� 
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

        if (this.StageNum != StageNum || Index >= Dic_Context[this.StageNum].Length || Index <= -1) // ���� ����
        {
            Debug.Log("this.StageNum: " + this.StageNum);
            Debug.Log("StageNum: " + StageNum);
            Context.text = "�ε��� �ʰ�";
            return;
        }

        DOTween.Sequence()
        .Append(Context.DOText("", 0.7f)) // �� ���ڿ��� 0.7�� ���
        .OnComplete(() =>
        {
            audioSource.Stop();
            audioSource.clip = KeyBoard_Sound;
            audioSource.Play();

            if (Already_Read.Contains(Index)) // �̹� �о��� ��
                Time_Persist = 0;
            else
            {
                Time_Persist = Dic_Context[StageNum][Index].Length * 0.12f; // ���� �ʾ��� ��. �ؽ�Ʈ ��� �ð��� �����Ӱ� �����ϸ� ��
                // �켱�� ���� ���� 25 �� 3�� ������� ���ص� (3 / 25) = 0.12
                Already_Read.Add(Index);
            }

            DOTween.Sequence()
            .Append(Context.DOText(Dic_Context[StageNum][Index], Time_Persist)) // Ű���� �Ҹ� ����ϸ鼭 �ؽ�Ʈ�� Time_Persist�� ���� ���

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
            }); // �ؽ�Ʈ ��� �Ϸ� �� �ؾ��ϴ� �͵�
        });
    }
}
