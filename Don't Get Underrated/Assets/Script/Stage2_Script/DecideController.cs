using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DecideController : MonoBehaviour
{
    [SerializeField]
    GameObject HOF_Board;

    [SerializeField]
    GameObject Select_Manhae;

    [SerializeField]
    GameObject Select_Bubhak;

    [SerializeField]
    Player_Stage2 Lantern;

    AudioSource HOFAudioSource;

    public int sceneNum = 0;

    float totalPlayTime; // �� �÷��� Ÿ��

    float updatePlayTime; // �÷��� Ÿ�� ������Ʈ

    bool GameDone = false;

    private void Awake()
    {
        if(TryGetComponent(out AudioSource AS_HOF))
        {
            HOFAudioSource = AS_HOF;
            HOFAudioSource.Stop();
        }

        if (GameObject.Find("Lantern") && GameObject.Find("Lantern").TryGetComponent(out Player_Stage2 BC))
            Lantern = BC;
    }

    private void Update()
    {
        updatePlayTime += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && GameDone == false)
        {
            GameDone = true;
            totalPlayTime = updatePlayTime;
            Lantern.totalPlayTime = this.totalPlayTime; // �� �÷��� Ÿ���� �÷��̾�� �Ѱ���.
            //Debug.Log("Playtime: " + totalPlayTime);
            HOFAudioSource.Play();
            HOF_Board.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void ManhaeBubhak()
    {
        HOF_Board.SetActive(false);
        Select_Manhae.SetActive(true);

        Time.timeScale = 1;
        StartCoroutine(NextStage());
    }

    public void BubhakManhae()
    {
        HOF_Board.SetActive(false);
        Select_Bubhak.SetActive(true);

        Time.timeScale = 1;
        StartCoroutine(NextStage());
    }

    IEnumerator NextStage()
    {
        yield return new WaitForSeconds(2f);
        float fadeTime = GameObject.Find("Fading").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        sceneNum++;
        SceneManager.LoadScene(sceneNum);
    }
}
