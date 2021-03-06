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

    float totalPlayTime; // 총 플레이 타임

    float updatePlayTime; // 플레이 타임 업데이트

    public bool GameDone = false;

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
            if (GameObject.Find("Network_Confirm") && GameObject.Find("Network_Confirm").TryGetComponent(out Network_On NO))
            {
                NO.StopAllCoroutines();
            }
            GameDone = true;
            totalPlayTime = updatePlayTime;
            Lantern.totalPlayTime = this.totalPlayTime; // 총 플레이 타임을 플레이어에게 넘겨줌.
            //Debug.Log("Playtime: " + totalPlayTime);
            singleTone.main_stage_2_score = (int)this.totalPlayTime;
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

        singleTone.SceneNumManage++;
        SceneManager.LoadScene(singleTone.SceneNumManage);
    }
}
