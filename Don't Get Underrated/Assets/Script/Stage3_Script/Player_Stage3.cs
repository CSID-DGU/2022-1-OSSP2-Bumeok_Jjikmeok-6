using System.Collections;
using TMPro;
using UnityEngine;

public class Player_Stage3 : Player_Info
{
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
    private int PlayerWalkSpeed = 10;

    [SerializeField]
    public int Student_Power = 7;

    [SerializeField]
    public int Fever_Power = 2;

    private GameObject sliderClone;

    private GameObject Student_Clone;

    private GameObject Fever_Particle_Copy;

    public Animator animator;

    private Camera_Trace camera_Trace;

    private Limit_Time limit_Time;

    private IEnumerator first_phase, lazor_in_second_phase;

    private bool IsOneClick = false;

    private double Timer = 0;

    private bool Dash_Able = true;

    private bool Move_Able = true;

    private bool FixedSlider = false;

    private bool FixedTarget = false;

    private bool is_Button = false;

    private int Floor_Player_Place;

    public bool Is_Fever = false;

    public bool is_Domain = false; // 계단 올라갈 때만 적용

    public float Floor_Interval = 40;

    private int StudentLayerMask;  // Player 레이어만 충돌 체크함

    public int Detect_Interrupt_Active = 1;

    private int Real_Walk_Speed;

    public void Destroy_sliderClone()
    {
        if (sliderClone != null)
            Destroy(sliderClone);
        if (Fever_Particle_Copy != null)
            Destroy(Fever_Particle_Copy);
    }
    private new void Awake()
    {
        base.Awake();

        if (GameObject.Find("Flash_Interrupt") && GameObject.Find("Flash_Interrupt").TryGetComponent(out ImageColor IC))
            imageColor = IC;
        if (TryGetComponent(out Animator A))
            animator = A;

        Unbeatable = true;
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
        Real_Walk_Speed = PlayerWalkSpeed;
        StudentLayerMask = 1 << LayerMask.NameToLayer("Student");

        Targetting_Object.SetActive(false);
        Down_Floor.SetActive(false);
        Up_Floor.SetActive(false);
        Student_Gaze.SetActive(false);

        if (GameObject.Find("Main Camera") && GameObject.Find("Main Camera").TryGetComponent(out Camera_Trace CT))
            camera_Trace = CT;
        if (GameObject.FindGameObjectWithTag("LimitTimeText") && GameObject.FindGameObjectWithTag("LimitTimeText").TryGetComponent(out Limit_Time LT))
            limit_Time = LT;
        if (GameObject.Find("Player_Effect_Sound") && GameObject.Find("Player_Effect_Sound").TryGetComponent(out AudioSource AS1))
            EffectSource = AS1;
        if (GameObject.Find("Player_BackGround_Sound") && GameObject.Find("Player_BackGround_Sound").TryGetComponent(out AudioSource AS2))
            BackGroundSource = AS2;

        My_Position = new Vector3(0, -2.5f, 0);
    }
    void Start()
    {
        BackGround_Sound_Play(0);
        All_Start();
    }
    void Init_Animation()
    {
        animator.SetBool("BulSang", false);
        animator.SetBool("IsWalk", false);
        animator.SetBool("IsRun", false);
        animator.SetBool("IsDead", false); // 애니메이션 초기화
    }
    public void Stop_Walk()
    {
        Is_Fever = false;
        All_Stop();
        Init_Animation();
        Destroy_sliderClone();
        StopAllCoroutines();
        Effect_Sound_Stop();
        Run_Life_Act(Decrease_BackGround_Sound(6));
    }
    IEnumerator First_Phase()
    {
        while (true)
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f, StudentLayerMask);

            if (hit.collider != null && hit.transform.gameObject.CompareTag("Student"))
            {
                if (!FixedTarget) // 불안정한 Raycast2D이므로(대체제 X) 타겟팅이 중복으로 되지 않도록 flag 설정
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

        if (Is_Fever)
            Effect_Sound_Play(2);
        else
            Effect_Sound_Play(0);

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
                        if (Student_Clone.TryGetComponent(out Student S))
                            S.Stop_Move();
                    }

                    Lazor_In_Second_Phase(Weapon[0], Student_Clone.transform.position, My_Position);

                    int temp = 3;

                    if (Student_Gaze.TryGetComponent(out Student_Gaze_Info SGI))
                        temp = SGI.Check_Student_HP(Student_Clone.transform.position);

                    if (temp == 0)
                    {
                        Run_Life_Act(Third_Phase(Student_Clone));
                        yield break;
                    }
                    else if (temp == 1)
                    {
                        yield return YieldInstructionCache.WaitForEndOfFrame;
                        Heart_Slider.GetComponent<Heart_Gaze_Viewer>().Ordinary_Case();
                        Student_Gaze.GetComponent<Student_Gaze_Info>().Empty_HP();
                        Student_Gaze.SetActive(false);
                        FixedSlider = false;

                        if (Student_Clone != null)
                            Destroy(Student_Clone);

                        if (!Is_Fever)
                        {
                            Main_Stage_3_Score += 150;
                            All_Start();
                            Effect_Sound_OneShot(4);
                        }
                        else
                        {
                            Main_Stage_3_Score += 150 * 2;
                            Enter_Fever();
                            Effect_Sound_OneShot(1);
                        }
                        yield break;
                    }
                    else if (temp == 2)
                    {
                        if (Is_Fever)
                            Main_Stage_3_Score += 1 * Fever_Power;
                        else
                            Main_Stage_3_Score += 1;
                    }
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    Init_Student();
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
        Launch_Weapon(ref weapon, target - self, Quaternion.identity, 90, My_Position);
    }

    IEnumerator Third_Phase(GameObject targetStudent_t)
    {
        All_Stop();

        Run_Life_Act_And_Continue(ref lazor_in_second_phase, Lazor_In_Second_Phase(targetStudent_t));

        if (Student_Gaze.TryGetComponent(out Student_Gaze_Info S_G_I))
            yield return S_G_I.StartCoroutine(S_G_I.Competition(targetStudent_t));

        Stop_Life_Act(ref lazor_in_second_phase);

        Init_Student();

        if (targetStudent_t != null)
            Destroy(targetStudent_t);

        yield return YieldInstructionCache.WaitForEndOfFrame;

        Effect_Sound_Stop();

        if (animator.GetBool("IsDead"))
        {
            Effect_Sound_OneShot(5);
            yield return YieldInstructionCache.WaitForSeconds(1.5f);
            animator.SetBool("IsDead", false);
            All_Start();
        }
        else
        {
            if (Is_Fever)
            {
                Main_Stage_3_Score += 300 * 2 * Detect_Interrupt_Active;
                Enter_Fever();
                Effect_Sound_OneShot(1);
            }
            else
            {
                Main_Stage_3_Score += 300 * Detect_Interrupt_Active;
                All_Start();
                Effect_Sound_OneShot(4);
            }
            Detect_Interrupt_Active = 1;
        }
        yield break;
    }
    public void Init_Student()
    {
        if (Student_Gaze.TryGetComponent(out Student_Gaze_Info SGI))
            SGI.Empty_HP();

        Student_Gaze.SetActive(false);
        FixedSlider = false;

        if (Student_Clone != null && Student_Clone.TryGetComponent(out Student S))
            S.Start_Move();
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
        Real_Walk_Speed = PlayerWalkSpeed * 2;
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
        Init_Animation();

        GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] student = GameObject.FindGameObjectsWithTag("Student");

        foreach (var e in enemy)
        {
            if (e.TryGetComponent(out Interrupt I))
                I.When_Fever_End();
        }
        foreach (var e in student)
        {
            if (e.TryGetComponent(out Student S))
                S.When_Fever_End();
        }

        if (Student_Gaze.TryGetComponent(out Student_Gaze_Info SGI))
            SGI.When_Fever_End();

        Student_Gaze.SetActive(false);
     
        StopAllCoroutines();
        All_Stop();
        All_Start();
    }

    IEnumerator Dash_Behave()
    {
        sliderClone = Instantiate(Move_Slider, My_Position, Quaternion.identity);

        sliderClone.transform.SetParent(canvasTransform);
        sliderClone.transform.position = transform.position + new Vector3(0, 2, 0);
        sliderClone.transform.localScale = Vector3.one;

        if (Targetting_Object.activeSelf)
            Targetting_Object.SetActive(false);

        Move_Gaze_Info move_Gaze_Info = null;
        if (sliderClone.TryGetComponent(out Move_Gaze_Info MGI))
            move_Gaze_Info = MGI;
        if (move_Gaze_Info == null)
            yield break;

        All_Stop();
        Move_Able = true;
        Real_Walk_Speed = PlayerWalkSpeed * 2;
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
        yield return YieldInstructionCache.WaitForSeconds(2.5f);

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
                Run_Life_Act(Dash_Behave());
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
            My_Position = new Vector3(My_Position.x + Real_Walk_Speed * Time.deltaTime * key, My_Position.y, My_Position.z);
        }
        if (Input.GetKey(KeyCode.A))
        {
            animator.SetBool("IsWalk", true);
            key = -1;
            My_Position = new Vector3(My_Position.x + Real_Walk_Speed * Time.deltaTime * key, My_Position.y, My_Position.z);
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
            Run_Life_Act(I_Move_Floor(stageData.LimitMax.x - 2, "down"));
        }
        else if (param == 1) // 위 층 이동
        {
            Floor_Player_Place++;
            Run_Life_Act(I_Move_Floor(0, "up"));
        }
    }
    IEnumerator I_Move_Floor(float Change_X, string CHK)
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
        limit_Time.Stop_Time_Decrease();

        Change_BG(Color.black, 2);

        Effect_Sound_OneShot(3);

        if (CHK == "up")
        {
            if (My_Scale.x > 0)
                My_Scale = new Vector3(-My_Scale.x, My_Scale.y, My_Scale.z);
            stageData.LimitMax += new Vector2(0, Floor_Interval);
            stageData.LimitMin += new Vector2(0, Floor_Interval);
            yield return Move_Straight(My_Position, My_Position + new Vector3(6, 3, 0), 2, OriginCurve);
        }
        else if (CHK == "down")
        {
            if (My_Scale.x < 0)
                My_Scale = new Vector3(-My_Scale.x, My_Scale.y, My_Scale.z);
            stageData.LimitMax += new Vector2(0, -Floor_Interval);
            stageData.LimitMin += new Vector2(0, -Floor_Interval);
            yield return Move_Straight(My_Position, My_Position + new Vector3(-6, -2, 0), 2, OriginCurve);
        }

        My_Position = new Vector3(Change_X, -2.5f + Floor_Interval * (Floor_Player_Place - 1), 0);
        camera_Trace.Trace_Player(Floor_Player_Place);
        limit_Time.Start_Time_Decrease();
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

        Real_Walk_Speed = 0;
    }
    void All_Start()
    {
        Dash_Able = true;
        Move_Able = true;
        is_Button = true;
        FixedSlider = false;
        FixedTarget = false;
        Student_Clone = null;
        Real_Walk_Speed = PlayerWalkSpeed;
        Effect_Sound_Stop();

        Run_Life_Act_And_Continue(ref first_phase, First_Phase());
    }
    void Update()
    {
        if (Dash_Able)
            Dash();
        if (Move_Able)
            Move();
        Main3_Score_Text.text = "점수 : " + Main_Stage_3_Score;
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
            if (My_Position.x <= stageData.LimitMin.x)
            {
                if (Floor_Player_Place != 1)
                {
                    Down_Floor.SetActive(true);
                    if (Down_Floor.TryGetComponent(out Floor_Button FB))
                        FB.Start_Down_Or_UP(true, stageData.LimitMin.x, Floor_Interval * (Floor_Player_Place - 1));
                }
            }
            else
            {
                if (Down_Floor.activeSelf)
                    Down_Floor.SetActive(false);
            }
            if (My_Position.x >= stageData.LimitMax.x)
            {
                if (Floor_Player_Place != 4)
                {
                    Up_Floor.SetActive(true);
                    if (Up_Floor.TryGetComponent(out Floor_Button FB))
                        FB.Start_Down_Or_UP(false, stageData.LimitMax.x, Floor_Interval * (Floor_Player_Place - 1));
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
            Fever_Particle_Copy.transform.position = new Vector3(My_Position.x, -4.8f + Floor_Interval * (Floor_Player_Place - 1), Fever_Particle_Copy.transform.position.z);
    }
}