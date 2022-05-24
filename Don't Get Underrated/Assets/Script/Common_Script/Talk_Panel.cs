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
        Dic_Context.Add(0, new string[] { "�̻� ����� �װ� �����ϴ� �ٿ� ���� �ٸ��ܴ�", "�������� ���ϰ�.... ���� ������...." });
        Dic_Context.Add(1, new string[] { "������ �����̶�....", "�ѹ� ���� �����غ�!" });
        Dic_Context.Add(2, new string[] { "���� �����⿡��", "������߾�!!" });
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

        if (this.StageNum != StageNum || Index >= Dic_Context[this.StageNum].Length || Index <= -1) // ���� ����
        {
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
                Already_Read.Add(Index);
            }
                
            DOTween.Sequence()
            .Append(Context.DOText(Dic_Context[StageNum][Index], Time_Persist)) // Ű���� �Ҹ� ����ϸ鼭 �ؽ�Ʈ�� Time_Persist�� ���� ���
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
            }); // �ؽ�Ʈ ��� �Ϸ� �� �ؾ��ϴ� �͵�
        });
    }
}
