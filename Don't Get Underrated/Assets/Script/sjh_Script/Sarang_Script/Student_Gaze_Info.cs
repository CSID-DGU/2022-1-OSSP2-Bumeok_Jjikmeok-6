using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Student_Gaze_Info : Slider_Viewer
{
    // Start is called before the first frame update

    Camera selectedCamera;

    GameObject Player_Heart_Slider;

    ArrayList Interrupt_Num_On_NonActive; // 맵을 떠도는 인터럽트

    Dictionary<GameObject, IEnumerator> Wow;

    [SerializeField]
    GameObject YeonTa;

    GameObject YeonTa_Copy;

    int StudentLayerMask;  // Player 레이어만 충돌 체크함
    private new void Awake()
    {
        base.Awake();
        StudentLayerMask = 1 << LayerMask.NameToLayer("Student");
        Player_Heart_Slider = GameObject.FindGameObjectWithTag("HeartSlider");
        Interrupt_Num_On_NonActive = new ArrayList();
        Wow = new Dictionary<GameObject, IEnumerator>();
        selectedCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }
    public int Check_Student_HP(Vector3 TempPosition)
    {
        slider.value += Time.deltaTime / 2;

        if (slider.value >= 0.5f && Interrupt_Num_On_NonActive.Count >= 1 && Wow.Count == 0)
        {
            Wow.Clear();
            foreach (var interrupt in Interrupt_Num_On_NonActive)
            {
                GameObject copy_interrupt = (GameObject)interrupt;

                if (copy_interrupt.TryGetComponent(out Interrupt user))
                {
                    IEnumerator temp_s = user.Trigger_Lazor(TempPosition);
                    user.StartCoroutine(temp_s);
                    Wow.Add(copy_interrupt, temp_s);
                }
            }
            return 0;
        }
        if (slider.value >= 1 && Interrupt_Num_On_NonActive.Count == 0 && Wow.Count == 0)
            return 1;
         if (slider.value >= 0.25f && Wow.Count == 0)
            return 3;
        return 2;
    }

    public void Empty_HP()
    {
        slider.value = 0;
    }
    public IEnumerator Competition(GameObject student, float student_power)
    {
        yield return YieldInstructionCache.WaitForSeconds(0.3f);
        StartCoroutine(GameObject.Find("Flash_Interrupt").GetComponent<BackGroundColor>().Flash(new Color(1, 1, 1, 1), 0.2f, 5));
        YeonTa_Copy = Instantiate(YeonTa, transform.position + 0.5f * Vector3.up, Quaternion.identity);
        GameObject.FindGameObjectWithTag("StudentSlider").GetComponent<Image>().color = new Color(1, 0.2f, 0.6f, 1);
        transform.localScale = new Vector3(2, 1.5f, 1);
        transform.position = transform.position + 1.5f * Vector3.down;
        while (true)
        {
            if (4 - Wow.Count > 0)
                slider.value -= Time.deltaTime / (4 - Wow.Count);
            else
                slider.value -= Time.deltaTime;

            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f, StudentLayerMask);

            if (Input.GetMouseButtonDown(0) && hit.collider != null && hit.transform.gameObject.CompareTag("Student"))
                slider.value += 0.15f * student_power;

            if (slider.value <= 0 || slider.value >= 1)
            {
                GameObject.FindGameObjectWithTag("StudentSlider").GetComponent<Image>().color = new Color(1, 0, 0, 1);
                transform.localScale = new Vector3(1, 1, 1);
                Destroy(YeonTa_Copy);
                foreach (var e in Wow)
                {
                   if (e.Key.TryGetComponent(out Interrupt user))
                        user.StopCoroutine(e.Value);
                }
                if (slider.value <= 0)
                {
                    foreach (var e in Wow)
                    {
                        if (e.Key.TryGetComponent(out Interrupt user))
                            user.Start_Move();
                    }
                    Player_Heart_Slider.GetComponent<Heart_Gaze_Viewer>().When_Player_Defeat();
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl_Sarang>().animator.SetBool("IsDead", true);
                }
                else if (slider.value >= 1)
                {
                    foreach (var e in Wow)
                        Destroy(e.Key);

                    Player_Heart_Slider.GetComponent<Heart_Gaze_Viewer>().When_Interrupt_Defeat();
                    slider.value = 0;

                    Destroy(student);
                }
                Wow.Clear();
                Interrupt_Num_On_NonActive.Clear();
                yield return null;
                yield break;
            }
            yield return null;
        }
    }
    void CheckInterrupt()
    {
        GameObject[] e = GameObject.FindGameObjectsWithTag("Interrupt");
        if (e != null)
        {
            Interrupt_Num_On_NonActive.Clear();
            foreach (var u in e)
            {
                Vector3 screenPoint = selectedCamera.WorldToViewportPoint(u.transform.position);
                bool OnScreen = screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

                if (OnScreen)
                    Interrupt_Num_On_NonActive.Add(u);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        CheckInterrupt();
    } // 학생이 레이저 빔을 과제, 시험같은 방해 오브젝트 및 플레이어에게 맞았을 때 처리한 코드
    private void LateUpdate()
    {
        if (YeonTa_Copy != null)
            YeonTa_Copy.transform.position = new Vector3(transform.position.x - 1.78f + (slider.value * 3.56f), transform.position.y + 1f, 1);
    }
}