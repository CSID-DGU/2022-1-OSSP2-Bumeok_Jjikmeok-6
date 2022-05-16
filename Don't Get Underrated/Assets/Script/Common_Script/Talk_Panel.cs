using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Talk_Panel : MonoBehaviour
{
    // Start is called before the first frame update

    Dictionary<int, string[]> Dic_Name;
    Dictionary<int, string[]> Dic_Context;

    [SerializeField]
    AudioClip Mouse_Sound;

    [SerializeField]
    AudioClip KeyBoard_Sound;

    AudioSource audioSource;

    [SerializeField]
    TextMeshProUGUI Name;

    [SerializeField]
    TextMeshProUGUI Context;

    [SerializeField]
    GameObject Before_Button;

    [SerializeField]
    GameObject After_Button;

    int Index;
    
    [SerializeField]
    int StageNum;

    bool is_Next;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        Index = 0;
        is_Next = true;
        Dic_Name = new Dictionary<int, string[]>();
        Dic_Context = new Dictionary<int, string[]>();
        Before_Button.SetActive(false);
        After_Button.SetActive(false);
        Dic_Context.Add(0, new string[] { "�̻� ����� �װ� �����ϴ� �ٿ� ���� �ٸ��ܴ�", "�������� ���ϰ�.... ���� ������...." });
        Dic_Context.Add(1, new string[] { "������ �����̶�....", "�ѹ� ���� �����غ�!" });
        Dic_Context.Add(2, new string[] { "���� �����⿡��", "������߾�!!" });
        Dic_Context.Add(3, new string[] { "���", "�ٰ��̴� ó������?" });
        Dic_Context.Add(4, new string[] {"�����͸��� �׷���...?", "�һ�� �¼� �ο�� ��" });
    }
    void Start()
    {
        //string path = Application.dataPath;
        //path += "/Data/wa.txt";
        //string[] contents = System.IO.File.ReadAllLines(path);
        Debug.Log(Dic_Context[0].Length);
        Check(StageNum, true);
    }
    public void Next(int StageNum)
    {
        audioSource.clip = Mouse_Sound;
        audioSource.Play();
        Index++;
        Check(StageNum, true);
    }
    public void Before(int StageNum)
    {
        audioSource.clip = Mouse_Sound;
        audioSource.Play();
        Index--;
        Check(StageNum, false);
    }
    void Check(int StageNum, bool is_Next)
    {
        this.is_Next = is_Next;
        After_Button.SetActive(false);
        Before_Button.SetActive(false);
        StartCoroutine(I_Check(StageNum));
    }
    IEnumerator I_Check(int StageNum)
    {
        yield return YieldInstructionCache.WaitForSeconds(0.7f);

        if (this.StageNum != StageNum || Index >= Dic_Context[this.StageNum].Length || Index <= -1)
        {
            Context.text = "�� �Ӹ�!!";
            yield break;
        }

        yield return StartCoroutine(KeyBoard());

        if (Index == 0)
            After_Button.SetActive(true);

        else if (Index == Dic_Context[this.StageNum].Length - 1)
            Before_Button.SetActive(true);

        else
        {
            After_Button.SetActive(true);
            Before_Button.SetActive(true);
        }
        yield return null;
    }
    IEnumerator KeyBoard()
    {
        if (!is_Next)
        {
            Context.text = Dic_Context[StageNum][Index];
            yield return YieldInstructionCache.WaitForSeconds(0.5f);
            yield break;
        }

        Context.text = "";
        audioSource.clip = KeyBoard_Sound;
        audioSource.Play();
        foreach (var u in Dic_Context[StageNum][Index])
        {
            Context.text += u;
            if (u == ' ')
            {
                audioSource.Pause();
                yield return YieldInstructionCache.WaitForSeconds(0.25f);
                audioSource.Play();
            }  
            else
                yield return YieldInstructionCache.WaitForSeconds(0.1f);
               
        }
        audioSource.Stop();
        yield return YieldInstructionCache.WaitForSeconds(0.5f);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
