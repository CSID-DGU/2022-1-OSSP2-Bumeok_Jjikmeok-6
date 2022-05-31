using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerCtrl_Sarang : Player_Info
{
    // 게임의 전체적인 요소는 1. 플레이어 2. 적 3. 학생 4. 학생 게이지 

    //GameObject Fever_Particle_Clone;

    private GameObject sliderClone;

    private GameObject Student_Clone;

    private GameObject Fever_Particle_Copy;

    private bool IsOneClick = false;

    private double Timer = 0;

    private bool Dash_Able = true;

    private bool Move_Able = true;

    private bool FixedSlider = false;

    private bool FixedTarget = false;

    private bool is_Button = false;

    private IEnumerator first_phase, lazor_in_second_phase;

    private int Floor_Player_Place;

    public Animator animator;

    public bool Is_Fever = false;

    public bool is_Domain = false; // 계단 올라갈 때만 적용



    private Camera_Trace camera_Trace;

    private Limit_Time limit_Time;

    private int StudentLayerMask;  // Player 레이어만 충돌 체크함

    [SerializeField]
    private GameObject Move_Slider;

    [SerializeField]
    private Transform canvasTransform;

    [SerializeField]
    private GameObject Student_Gaze;

    [SerializeField]
    private GameObject Targetting_Object;

    [SerializeField]
    private GameObject Heart_Slider;

    [SerializeField]
    private TextMeshProUGUI Main3_Score_Text;

    [SerializeField]
    private GameObject Fever_Particle;

    [SerializeField]
    private GameObject Down_Floor;

    [SerializeField]
    private GameObject Up_Floor;

    [SerializeField]
    private float IsDoubleClick = 0.25f;

    [SerializeField]
    private float PlayerWalkSpeed = 0.03f;

    [SerializeField]
    public int Student_Power = 1;

    [SerializeField]
    public int Fever_Power = 2;

    public void Destroy_sliderClone()
    {
        if (sliderClone != null)
            Destroy(sliderClone);
    }
    private new void Awake()
    {
        base.Awake();

        if (GameObject.Find("Flash_Interrupt") && GameObject.Find("Flash_Interrupt").TryGetComponent(out ImageColor IC))
            imageColor = IC;
        if (TryGetComponent(out Animator A))
            animator = A;

        IsOneClick = false;
        Timer = 0;
        Dash_Able = true;
        Move_Able = true;
        FixedSlider = false;
        FixedTarget = false;
        is_Domain = true;
        is_Button = true;
        Is_Fever = false;
        Student_Clone = null;
        first_phase = null;
        Fever_Particle_Copy = null;
        Floor_Player_Place = 1;
        StudentLayerMask = 1 << LayerMask.NameToLayer("Student");

        Targetting_Object.SetActive(false);
        Down_Floor.SetActive(false);
        Up_Floor.SetActive(false);
        Student_Gaze.SetActive(false);

        if (GameObject.Find("Main Camera") && GameObject.Find("Main Camera").TryGetComponent(out Camera_Trace CT))
            camera_Trace = CT;
        if (GameObject.FindGameObjectWithTag("LimitTimeText") && GameObject.FindGameObjectWithTag("LimitTimeText").TryGetComponent(out Limit_Time LT))
            limit_Time = LT;

        My_Position = new Vector3(0, -2.5f, 0);
        stageData.LimitMin = new Vector2(-3, -6);
        stageData.LimitMax = new Vector2(40, 8);
    }

    // 플레이어의 코루틴 순서 : First_Phase --> Second_Phase --> Third_Phase

    // Start is called before the first frame update
    void Start()
    {
        All_Start();
    }
    IEnumerator First_Phase()
    {
        while (true)
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f, StudentLayerMask);

            if (hit.collider != null && hit.transform.gameObject.CompareTag("Student"))
            {
                if (!FixedTarget) // 불안정한 Raycast이므로 타겟팅을 정확히 할 수 있도록
                {
                    FixedTarget = true;
                    Targetting_Object.SetActive(true);
                    if (Targetting_Object.TryGetComponent(out Targetting_Effect TE))
                        TE.Change_Scale_And_Color(hit.transform.gameObject);
                }
                if (Input.GetMouseButtonDown(0))
                {
                    Targetting_Object.SetActive(false);
                    FixedTarget = false;
                    Run_Life_Act(Second_Phase());
                    yield break;
                }
            }
            else
            {
                Targetting_Object.SetActive(false);
                FixedTarget = false;
            }
            yield return null;
        }
    }

    IEnumerator Second_Phase()
    {
        All_Stop();

        if (!Student_Gaze.activeSelf)
            Student_Gaze.SetActive(true);

        while (true)
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f, StudentLayerMask);

            if (hit.collider != null && hit.transform.gameObject.CompareTag("Student"))
            {
                if (Student_Clone == null)
                    Student_Clone = hit.transform.gameObject;
                 
                if (Input.GetMouseButton(0))
                {
                    float dir_Change = My_Position.x - Student_Clone.transform.position.x;
                    My_Scale = new Vector3((dir_Change / Mathf.Abs(dir_Change)) * Mathf.Abs(My_Scale.x), My_Scale.y, My_Scale.z);

                    if (!FixedSlider)
                    {
                        FixedSlider = true;
                        Student_Gaze.transform.position = Student_Clone.transform.position + new Vector3(0, 2, 0);
                        Student_Clone.GetComponent<Student>().Stop_Move(); // 공격 받는 중 + 움직임 정지
                    }

                    Heart_Slider.GetComponent<Heart_Gaze_Viewer>().Decrease_HP(Time.deltaTime / (15 * Student_Power));

                    Lazor_In_Second_Phase(Weapon[0], Student_Clone.transform.position, My_Position);

                    int temp = Student_Gaze.GetComponent<Student_Gaze_Info>().Check_Student_HP(Student_Clone.transform.position);

                    if (temp == 0)
                    {
                        Run_Life_Act(Third_Phase(Student_Clone));
                        yield break;
                    }
                    else if (temp == 1)
                    {
                        Heart_Slider.GetComponent<Heart_Gaze_Viewer>().Ordinary_Case();
                        Student_Gaze.GetComponent<Student_Gaze_Info>().Empty_HP();
                        Student_Gaze.SetActive(false);
                        FixedSlider = false;

                        if (Student_Clone != null)
                            Destroy(Student_Clone);

                        if (!Is_Fever)
                        {
                            Main_3_Score += 100;
                            animator.SetBool("Heart_Gain", true);
                            My_Scale = new Vector3(1.1f, 1.1f, 0);
                            yield return YieldInstructionCache.WaitForSeconds(0.5f); // 플레이어가 하트 게이지를 얻거나, 쓰러지는 시간임. 피버 타임일 때는 무시
                            animator.SetBool("Heart_Gain", false);
                            My_Scale = new Vector3(4.9f, 4.9f, 0);
                            All_Start();
                            yield break;
                        }
                        else
                            Main_3_Score += 100 * 2;

                    }
                    else if (temp == 3)
                    {
                        if (Is_Fever)
                            Main_3_Score += Student_Power * Fever_Power;
                        else
                            Main_3_Score += Student_Power;
                    }
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    Init_Student();
                    //Student_Clone = null;
                    if (Is_Fever)
                        Enter_Fever();
                    else
                        All_Start();

                    yield return YieldInstructionCache.WaitForSeconds(Time.deltaTime);
                    yield break;
                }
            }
            else
            {
                Init_Student();
                //Student_Clone = null;
                if (Is_Fever)
                    Enter_Fever();
                else
                    All_Start();

                yield return YieldInstructionCache.WaitForSeconds(Time.deltaTime);
                yield break;
            }
            yield return null;

        }
    }

    private void Lazor_In_Second_Phase(GameObject weapon, Vector3 target, Vector3 self)
    {
        Launch_Weapon(ref weapon, target - self, Quaternion.identity, 12, My_Position);
    }

    IEnumerator Third_Phase(GameObject targetStudent_t)
    {
        All_Stop();

        Run_Life_Act_And_Continue(ref lazor_in_second_phase, Lazor_In_Second_Phase(targetStudent_t));

        if (Student_Gaze.TryGetComponent(out Student_Gaze_Info S_G_I))
            yield return S_G_I.StartCoroutine(S_G_I.Competition(targetStudent_t, Student_Power));

        Stop_Life_Act(ref lazor_in_second_phase);

        Init_Student();

        if (targetStudent_t != null)
            Destroy(targetStudent_t);

        if (Is_Fever)
        {
            Main_3_Score += 100 * 2;
            Enter_Fever();
        }
        else
        {
            Main_3_Score += 100;
            yield return YieldInstructionCache.WaitForSeconds(0.5f); // 플레이어가 하트 게이지를 얻거나, 쓰러지는 시간임. 피버 타임일 때는 무시
            My_Scale = new Vector3(4.9f, 4.9f, 0);
            animator.SetBool("IsDead", false);
            animator.SetBool("Heart_Gain", false);
            All_Start();
        }
        yield break;
    }
    public void Init_Student()
    {
        Student_Gaze.GetComponent<Student_Gaze_Info>().Empty_HP();
        Student_Gaze.SetActive(false);
        FixedSlider = false;

        if (Student_Clone != null)
            Student_Clone.GetComponent<Student>().Start_Move();
        Student_Clone = null;
    }
    IEnumerator Lazor_In_Second_Phase(GameObject targetStudent_t)
    {
        while (true)
        {
            if (targetStudent_t != null)
                Lazor_In_Second_Phase(Weapon[0], targetStudent_t.transform.position, My_Position);
            yield return null;
        }
    }

    public void Enter_Fever()
    {
        if (Fever_Particle_Copy == null)
            Fever_Particle_Copy = Instantiate(Fever_Particle, My_Position + Vector3.down * 2.3f, Quaternion.Euler(-90, 0, 0));
        All_Stop();
        All_Start();
        Dash_Able = false;
        PlayerWalkSpeed = 0.06f;
    }
    public void Out_Fever()
    {
        if (Fever_Particle_Copy != null)
        {
            if (Fever_Particle_Copy.TryGetComponent(out ParticleSystem PS))
            {
                if (PS.IsAlive())
                    Destroy(PS.gameObject);
            }
        }
        Is_Fever = false;
        My_Scale = new Vector3(4.9f, 4.9f, 1);
        animator.SetBool("BulSang", false);
        animator.SetBool("Heart_Gain", false);
        animator.SetBool("IsWalk", false);
        animator.SetBool("IsRun", false);
        animator.SetBool("IsDead", false); // 애니메이션 초기화
        GameObject[] e = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] f = GameObject.FindGameObjectsWithTag("Student");

        foreach (var u in e)
        {
            if (u.TryGetComponent(out Interrupt I))
                I.When_Fever_End();
        }
        foreach (var u in f)
        {
            if (u.TryGetComponent(out Student S))
                S.When_Fever_End();
        }

        Student_Gaze.GetComponent<Student_Gaze_Info>().When_Fever_End();
        Student_Gaze.SetActive(false);
     
        StopAllCoroutines();
        All_Stop();
        All_Start();
    }

    IEnumerator Move_Delay()
    {
        sliderClone = Instantiate(Move_Slider, My_Position, Quaternion.identity);

        sliderClone.transform.SetParent(canvasTransform);
        sliderClone.transform.position = transform.position + new Vector3(0, 2, 0);
        sliderClone.transform.localScale = Vector3.one;

        Move_Gaze_Info move_Gaze_Info = sliderClone.GetComponent<Move_Gaze_Info>();

        All_Stop();
        Move_Able = true;
        PlayerWalkSpeed = 0.06f;
        animator.SetBool("IsRun", true);

        while (true)
        {
            yield return null;

            move_Gaze_Info.HP_Down();
            if (move_Gaze_Info.Get_HP() >= 1)
                break;
            if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
            {
                animator.SetBool("IsRun", false);
                Destroy(sliderClone);
                All_Start();
                yield break;
            }
        }

        All_Stop();
        Destroy(sliderClone);
        animator.SetTrigger("Tired");
        yield return new WaitForSeconds(2.5f);

        animator.SetBool("IsWalk", false);
        animator.SetBool("IsRun", false);
        All_Start();

        yield return null;
    }

    private void Dash()
    {
        if (IsOneClick && ((Time.time - Timer) > IsDoubleClick))
            IsOneClick = false;

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A))
        {
            if (!IsOneClick)
            {
                IsOneClick = true;
                Timer = Time.time;
            }
            else if (IsOneClick && ((Time.time - Timer) < IsDoubleClick))
            {
                IsOneClick = false;
                Run_Life_Act(Move_Delay());
            }
        }
    }
    private void Move()
    {
        int key = 0;
        if (Input.GetKey(KeyCode.D))
        {
            animator.SetBool("IsWalk", true);
            key = 1;
            My_Position = new Vector3(My_Position.x + PlayerWalkSpeed * key, My_Position.y, My_Position.z);
        }
        if (Input.GetKey(KeyCode.A))
        {
            animator.SetBool("IsWalk", true);
            key = -1;
            My_Position = new Vector3(My_Position.x + PlayerWalkSpeed * key, My_Position.y, My_Position.z);
        }

        if (key != 0)
            My_Scale = new Vector3(-key * Mathf.Abs(My_Scale.x), My_Scale.y, My_Scale.z);
           
        else
            animator.SetBool("IsWalk", false);
    }

    public void Down_Or_Up(int param)
    {
        if (param == 0) // 아래 층 이동
        {
            Floor_Player_Place--;
            Run_Life_Act(I_Move_Floor(38, "down"));
        }
        else if (param == 1) // 위 층 이동
        {
            Floor_Player_Place++;
            Run_Life_Act(I_Move_Floor(0, "up"));
        }
    }
    IEnumerator I_Move_Floor(int Change_X, string CHK)
    {
        All_Stop();
        is_Domain = false;

        if (Down_Floor.activeSelf)
            Down_Floor.SetActive(false);
        
        if (Up_Floor.activeSelf)
            Up_Floor.SetActive(false);

        animator.SetBool("IsWalk", false);
        animator.SetBool("IsRun", false);
        animator.SetBool("IsDead", false);
        animator.SetBool("IsWalk", true);

        if (camera_Trace == null || limit_Time == null)
            yield break;

        camera_Trace.DoNot_Trace_Player();
        limit_Time.Stop_Time_Persist();

        Change_BG(Color.black, 2);

        if (CHK == "up")
        {
            stageData.LimitMax += new Vector2(0, 40);
            stageData.LimitMin += new Vector2(0, 40);
            yield return Move_Straight(My_Position, My_Position + new Vector3(6, 3, 0), 2, OriginCurve);
        }
        else if (CHK == "down")
        {
            stageData.LimitMax += new Vector2(0, -40);
            stageData.LimitMin += new Vector2(0, -40);
            yield return Move_Straight(My_Position, My_Position + new Vector3(-6, -2, 0), 2, OriginCurve);
        }

        My_Position = new Vector3(Change_X, -2.5f + 40 * (Floor_Player_Place - 1), 0);
        camera_Trace.Final_Walk_Floor(Floor_Player_Place);
        limit_Time.Final_Walk_Floor();
        yield return Change_BG_And_Wait(new Color(1, 1, 1, 0.5f), 0.5f);
        yield return Change_BG_And_Wait(Color.clear, 0.2f);

        animator.SetBool("IsWalk", false);

        if (Is_Fever)
            Enter_Fever();
        else
            All_Start();

        is_Domain = true;

        yield return null;
    }
    void All_Stop()
    {
        Stop_Life_Act(ref first_phase);

        Dash_Able = false;
        Move_Able = false;
        is_Button = false;
        FixedSlider = false;
        FixedTarget = false;
        Student_Clone = null;

        PlayerWalkSpeed = 0;
    }
    void All_Start()
    {
        Dash_Able = true;
        Move_Able = true;
        is_Button = true;
        FixedSlider = false;
        FixedTarget = false;
        Student_Clone = null;
        PlayerWalkSpeed = 0.03f;

        Run_Life_Act_And_Continue(ref first_phase, First_Phase());
    }
    void Update()
    {
        if (Dash_Able)
            Dash();
        if (Move_Able)
            Move();
        Main3_Score_Text.text = "점수 : " + Main_3_Score;
    }
    private void LateUpdate()
    {
        if (is_Domain)
        {
            My_Position = new Vector3(Mathf.Clamp(My_Position.x, stageData.LimitMin.x, stageData.LimitMax.x),
            Mathf.Clamp(My_Position.y, stageData.LimitMin.y, stageData.LimitMax.y));
        }
        if (is_Button)
        {
            if (My_Position.x <= -3)
            {
                if (Floor_Player_Place != 1)
                {
                    Down_Floor.SetActive(true);
                    Down_Floor.GetComponent<Floor_Button>().Start_Down_Or_UP(true, 40 * (Floor_Player_Place - 1));
                }
            }
            else
            {
                if (Down_Floor.activeSelf)
                    Down_Floor.SetActive(false);
            }
            if (My_Position.x >= 40)
            {
                if (Floor_Player_Place != 4)
                {
                    Up_Floor.SetActive(true);
                    Up_Floor.GetComponent<Floor_Button>().Start_Down_Or_UP(false, 40 * (Floor_Player_Place - 1));
                }
            }
            else
            {
                if (Up_Floor.activeSelf)
                    Up_Floor.SetActive(false);
            }
        }
        else
        {
            if (Up_Floor.activeSelf)
                Up_Floor.SetActive(false);
            if (Down_Floor.activeSelf)
                Down_Floor.SetActive(false);
        }
        if (Fever_Particle_Copy != null)
            Fever_Particle_Copy.transform.position = new Vector3(My_Position.x, -4.8f + 40 * (Floor_Player_Place - 1), Fever_Particle_Copy.transform.position.z);
    }
}