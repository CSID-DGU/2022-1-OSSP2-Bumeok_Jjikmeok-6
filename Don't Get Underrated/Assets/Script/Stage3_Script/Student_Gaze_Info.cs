using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Student_Gaze_Info : Slider_Viewer
{
    // Start is called before the first frame update

    [SerializeField]
    GameObject YeonTa;

    [SerializeField]
    GameObject Square;

    [SerializeField]
    GameObject Angel;

    List<Interrupt> Interrupt_NonActive; // 맵을 떠도는 인터럽트

    List<Interrupt> Interrupt_Active;

    Camera selectedCamera;

    Heart_Gaze_Viewer heart_Gaze_Viewer;

    Player_Stage3 player_stage3;

    GameObject YeonTa_Copy;

    ImageColor imageColor;

    int StudentLayerMask;  // Player 레이어만 충돌 체크함

    void Init_Start()
    {
        Empty_HP();
        StudentLayerMask = 1 << LayerMask.NameToLayer("Student");
       
        Interrupt_NonActive = new List<Interrupt>();
        Interrupt_Active = new List<Interrupt>();

        selectedCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        if (GameObject.FindGameObjectWithTag("HeartSlider").TryGetComponent(out Heart_Gaze_Viewer H_G_V))
            heart_Gaze_Viewer = H_G_V;
        if (GameObject.FindGameObjectWithTag("Player").TryGetComponent(out Player_Stage3 PC_S))
            player_stage3 = PC_S;
        if (GameObject.Find("Flash_Interrupt") && GameObject.Find("Flash_Interrupt").TryGetComponent(out ImageColor IC))
            imageColor = IC;
    }
    private new void Awake()
    {
        base.Awake();
        Init_Start();
    }
    public void When_Fever_End()
    {
        StopAllCoroutines();
        Init_Start();
        Change_Red_Slider();
    }
    public int Check_Student_HP(Vector3 TempPosition)
    {
        if (!Check_Valid_Slider())
            return 0;

        if (player_stage3.Is_Fever)
            slider.value += Time.deltaTime * 0.1f * player_stage3.Student_Power * player_stage3.Fever_Power;
        else
            slider.value += Time.deltaTime * 0.1f * player_stage3.Student_Power;

        if (slider.value >= 0.5f && Interrupt_NonActive.Count >= 1 && Interrupt_Active.Count == 0)
        {
            Interrupt_Active.Clear();
            foreach (var interrupt in Interrupt_NonActive)
            {
                Interrupt_Active.Add(interrupt);
                interrupt.Fight_With_Player(TempPosition);
            }
            return 0;
        }
        if (slider.value >= 1 && Interrupt_NonActive.Count == 0 && Interrupt_Active.Count == 0)
        {
            GameObject square = Instantiate(Square, TempPosition, Quaternion.identity);
            GameObject w = Instantiate(Angel, square.transform.position, Quaternion.identity);
            w.transform.SetParent(square.transform);
            return 1;
        }
         if (slider.value >= 0.35f && Interrupt_Active.Count == 0)
            return 2;
        return 3;
    }

    public void Empty_HP()
    {
        if (Check_Valid_Slider())
            slider.value = 0;
    }
    public void Change_Pink_Slider()
    {
        YeonTa_Copy = Instantiate(YeonTa, transform.position + 0.5f * Vector3.up, Quaternion.identity);
        if (GameObject.FindGameObjectWithTag("StudentSlider") && GameObject.FindGameObjectWithTag("StudentSlider").TryGetComponent(out Image Im))
            Im.color = new Color(1, 0.2f, 0.6f, 1);
        transform.localScale = new Vector3(2, 1.5f, 1);
        transform.position = transform.position + 0.7f * Vector3.down;
    }
    public void Change_Red_Slider()
    {
        if (YeonTa_Copy != null)
            Destroy(YeonTa_Copy);
        if (GameObject.FindGameObjectWithTag("StudentSlider") && GameObject.FindGameObjectWithTag("StudentSlider").TryGetComponent(out Image Im))
            Im.color = Color.red;
        transform.localScale = new Vector3(1, 1, 1);
    }
    public IEnumerator Competition(GameObject student)
    {
        if (!Check_Valid_Slider())
            yield break;

        slider.value = 0.5f;
        yield return StaticFunc.WaitForRealSeconds(0.3f);
        if (imageColor != null)
        {
            imageColor.StopAllCoroutines();
            imageColor.StartCoroutine(imageColor.Flash(Color.white, 0.3f, 1));
        }
        
        Change_Pink_Slider();

        while (true)
        {
            if (!player_stage3.Is_Fever)
                slider.value -= Time.deltaTime * (0.4f + 0.1f * Interrupt_Active.Count);

            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f, StudentLayerMask);

            if (Input.GetMouseButtonDown(0) && hit.collider != null && hit.transform.gameObject.CompareTag("Student"))
                slider.value += 0.15f;


            if (slider.value <= 0 || slider.value >= 1)
            {
                foreach (var e in Interrupt_Active)
                    e.Stop_Fight_With_Player();

                if (slider.value <= 0)
                {
                    foreach (var e in Interrupt_Active)
                        e.Start_Move();

                    heart_Gaze_Viewer.When_Player_Defeat(Interrupt_Active.Count);
                    player_stage3.animator.SetBool("IsDead", true);

                }
                else if (slider.value >= 1)
                {
                    foreach (var e in Interrupt_Active)
                        e.OnDie();
                   
                    if (student.TryGetComponent(out Student S))
                    {
                        GameObject square = Instantiate(Square, S.transform.position, Quaternion.identity);
                        GameObject angel = Instantiate(Angel, square.transform.position, Quaternion.identity);
                        angel.transform.SetParent(square.transform);
                    }
                    player_stage3.Detect_Interrupt_Active = Interrupt_Active.Count;
                    heart_Gaze_Viewer.When_Interrupt_Defeat(Interrupt_Active.Count);
                }
                Empty_HP();
                Change_Red_Slider();
                Interrupt_Active.Clear();
                Interrupt_NonActive.Clear();
                yield break;
            }
            yield return null;
        }
    }
    void CheckInterrupt()
    {
        GameObject[] e = GameObject.FindGameObjectsWithTag("Enemy");
        if (e != null)
        {
            Interrupt_NonActive.Clear();
            foreach (var u in e)
            {
                Vector3 screenPoint = selectedCamera.WorldToViewportPoint(u.transform.position);
                bool OnScreen = screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

                if (OnScreen && u.TryGetComponent(out Interrupt I))
                    Interrupt_NonActive.Add(I);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Check_Valid_Slider() && slider.value > 0)
            CheckInterrupt();
    } // 학생이 레이저 빔을 과제, 시험같은 방해 오브젝트 및 플레이어에게 맞았을 때 처리한 코드
    private void LateUpdate()
    {
        if (Check_Valid_Slider() && YeonTa_Copy != null)
            YeonTa_Copy.transform.position = new Vector3(transform.position.x - 1.78f + (slider.value * 3.56f), transform.position.y + 1f, 1);
    }
}