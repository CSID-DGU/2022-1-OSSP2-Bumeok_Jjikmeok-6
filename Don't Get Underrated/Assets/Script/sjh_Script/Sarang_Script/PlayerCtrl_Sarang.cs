using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerCtrl_Sarang : Player_Info
{
    // 게임의 전체적인 요소는 1. 플레이어 2. 적 3. 학생 4. 학생 게이지 

    //GameObject Fever_Particle_Clone;

    GameObject sliderClone;

    GameObject Student_Clone;

    GameObject Fever_Particle_Copy;

    public Animator animator;

    private bool IsOneClick = false;

    private double Timer = 0;

    private bool Dash_Able = true;

    private bool Move_Able = true;

    private bool FixedSlider = false;

    private bool FixedTarget = false;

    public bool is_Domain = false;

    private bool is_Button = false;

    public bool Is_Fever = false;

    private IEnumerator first_phase;

    private int Floor_Player_Place;

    private int StudentLayerMask;  // Player 레이어만 충돌 체크함

    [SerializeField]
    GameObject Move_Slider;

    [SerializeField]
    Transform canvasTransform;

    [SerializeField]
    GameObject Student_Gaze;

    [SerializeField]
    GameObject Targetting_Object;

    [SerializeField]
    GameObject Heart_Slider;

    [SerializeField]
    TextMeshProUGUI Main3_Score_Text;

    [SerializeField]
    GameObject Fever_Particle;

    [SerializeField]
    GameObject Down_Floor;

    [SerializeField]
    GameObject Up_Floor;

    [SerializeField]
    float IsDoubleClick = 0.25f;

    [SerializeField]
    float PlayerWalkSpeed = 0.03f;

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

        backGroundColor = GameObject.Find("Flash_Interrupt").GetComponent<BackGroundColor>();
        animator = GetComponent<Animator>();

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

        transform.position = new Vector3(0, -2.5f, 0);
        stageData.LimitMin = new Vector2(-3, -6);
        stageData.LimitMax = new Vector2(40, 8);
    }

    // 플레이어의 코루틴 순서 : First_Phase --> Second_Phase --> Third_Phase

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(Start_Game());
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
                if (!FixedTarget)
                {
                    FixedTarget = true;
                    Targetting_Object.SetActive(true);
                    if (Targetting_Object.TryGetComponent(out Targetting_Effect targetting_effect))
                        targetting_effect.Change_Scale_And_Color(hit.transform.gameObject);
                   
                }
                if (Input.GetMouseButtonDown(0))
                {
                    StartCoroutine(Second_Phase());
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
                    float dir_Change = transform.position.x - Student_Clone.transform.position.x;
                    transform.localScale = new Vector3((dir_Change / Mathf.Abs(dir_Change)) * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                    Targetting_Object.SetActive(false);
                    FixedTarget = false;

                    if (!FixedSlider)
                    {
                        FixedSlider = true;
                        Student_Gaze.transform.position = Student_Clone.transform.position + new Vector3(0, 2, 0);
                        Student_Clone.GetComponent<Student>().Stop_Move(); // 공격 받는 중 + 움직임 정지
                    }

                    Heart_Slider.GetComponent<Heart_Gaze_Viewer>().Decrease_HP(Time.deltaTime / (15 * Student_Power));

                    Lazor_In_Second_Phase(Weapon[0], Student_Clone.transform.position, transform.position);

                    int temp = Student_Gaze.GetComponent<Student_Gaze_Info>().Check_Student_HP(Student_Clone.transform.position);

                    if (temp == 0)
                    {
                        StartCoroutine(Third_Phase(Student_Clone));
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
                            This_Scale = new Vector3(1.1f, 1.1f, 0);
                            yield return YieldInstructionCache.WaitForSeconds(2f); // 플레이어가 하트 게이지를 얻거나, 쓰러지는 시간임. 피버 타임일 때는 무시
                            animator.SetBool("Heart_Gain", false);
                            This_Scale = new Vector3(4.9f, 4.9f, 0);
                            All_Start();
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
                    Student_Clone = null;
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
                Student_Clone = null;
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
        Launch_Weapon_For_Move(ref weapon, target - self, Quaternion.identity, 8, transform.position);
    }

    IEnumerator Third_Phase(GameObject targetStudent_t)
    {
        All_Stop();

        IEnumerator lazor_in_second_phase = Lazor_In_Second_Phase(targetStudent_t);

        StartCoroutine(lazor_in_second_phase);

        if (Student_Gaze.TryGetComponent(out Student_Gaze_Info user))
            yield return user.StartCoroutine(user.Competition(targetStudent_t, Student_Power));

        StopCoroutine(lazor_in_second_phase);

        Init_Student();

        if (targetStudent_t != null)
            Destroy(targetStudent_t);

        if (Is_Fever)
            Enter_Fever();
        else
        {
            yield return YieldInstructionCache.WaitForSeconds(2f); // 플레이어가 하트 게이지를 얻거나, 쓰러지는 시간임. 피버 타임일 때는 무시
            This_Scale = new Vector3(4.9f, 4.9f, 0);
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
    }
    IEnumerator Lazor_In_Second_Phase(GameObject targetStudent_t)
    {
        while (true)
        {
            if (targetStudent_t != null)
                Lazor_In_Second_Phase(Weapon[0], targetStudent_t.transform.position, transform.position);
            yield return null;
        }
    }

    public void Enter_Fever()
    {
        if (Fever_Particle_Copy == null)
            Fever_Particle_Copy = Instantiate(Fever_Particle, transform.position + Vector3.down * 1.5f, Quaternion.Euler(-90, 0, 0));
        All_Stop();
        All_Start();
        Dash_Able = false;
        PlayerWalkSpeed = 0.06f;
    }
    public void Out_Fever()
    {
        if (Fever_Particle_Copy != null)
        {
            //Fever_Particle_Copy = null;
            if (Fever_Particle_Copy.TryGetComponent(out ParticleSystem user))
            {
                if (user.IsAlive())
                    Destroy(user.gameObject);
            }
        }
        Is_Fever = false;
        transform.localScale = new Vector3(4.9f, 4.9f, 1);
        animator.SetBool("BulSang", false);
        animator.SetBool("Heart_Gain", false);
        animator.SetBool("IsWalk", false);
        animator.SetBool("IsRun", false);
        animator.SetBool("IsDead", false); // 애니메이션 초기화
        GameObject[] e = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] f = GameObject.FindGameObjectsWithTag("Student");

        foreach (var u in e)
        {
            if (u.TryGetComponent(out Interrupt user1))
                user1.When_Fever_End();
        }
        foreach (var u in f)
        {
            if (u.TryGetComponent(out Student user2))
                user2.When_Fever_End();
        }

        Student_Gaze.GetComponent<Student_Gaze_Info>().When_Fever_End();
        Student_Gaze.SetActive(false);
     
        StopAllCoroutines();
        All_Stop();
        All_Start();
    }

    IEnumerator Move_Delay()
    {
        sliderClone = Instantiate(Move_Slider, transform.position, Quaternion.identity);

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

        Destroy(sliderClone);

        All_Stop();
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
                Timer = Time.time;
                IsOneClick = true;
            }

            else if (IsOneClick && ((Time.time - Timer) < IsDoubleClick))
            {
                Dash_Able = false;
                IsOneClick = false;
                StartCoroutine(Move_Delay());
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
            transform.position = new Vector3(transform.position.x + PlayerWalkSpeed * key, transform.position.y, transform.position.z);
        }
        if (Input.GetKey(KeyCode.A))
        {
            animator.SetBool("IsWalk", true);
            key = -1;
            transform.position = new Vector3(transform.position.x + PlayerWalkSpeed * key, transform.position.y, transform.position.z);
        }

        if (key != 0)
            transform.localScale = new Vector3(-key * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
           
        else
            animator.SetBool("IsWalk", false);
    }

    public void Down_Or_Up(int param)
    {
        if (param == 0) // 아래 층 이동
        {
            Floor_Player_Place--;
            StartCoroutine(I_Move_Floor(38, "down"));
        }
        else if (param == 1) // 위 층 이동
        {
            Floor_Player_Place++;
            StartCoroutine(I_Move_Floor(0, "up"));
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

        GameObject.Find("Main Camera").GetComponent<Camera_Trace>().DoNot_Trace_Player();
        GameObject.FindGameObjectWithTag("LimitTimeText").GetComponent<Limit_Time>().Stop_Time_Persist();

        IEnumerator change_color = backGroundColor.Change_Color(backGroundColor.Get_BGColor(), new Color(0, 0, 0, 1), 2);
        backGroundColor.StartCoroutine(change_color);

        if (CHK == "up")
        {
            stageData.LimitMax = stageData.LimitMax + new Vector2(0, 40);
            stageData.LimitMin = stageData.LimitMin + new Vector2(0, 40);
            yield return StartCoroutine(Position_Lerp(transform.position, transform.position + new Vector3(6, 2, 0), 2, OriginCurve));
        }
        else if (CHK == "down")
        {
            stageData.LimitMax = stageData.LimitMax + new Vector2(0, -40);
            stageData.LimitMin = stageData.LimitMin + new Vector2(0, -40);
            yield return StartCoroutine(Position_Lerp(transform.position, transform.position + new Vector3(-6, -2, 0), 2, OriginCurve));
        }

        backGroundColor.StopCoroutine(change_color);
        backGroundColor.Set_BGColor(Color.black);

        yield return YieldInstructionCache.WaitForSeconds(1f);

        transform.position = new Vector3(Change_X, -2.5f + 40 * (Floor_Player_Place - 1), 0);
        GameObject.Find("Main Camera").GetComponent<Camera_Trace>().Final_Walk_Floor(Floor_Player_Place);
        GameObject.FindGameObjectWithTag("LimitTimeText").GetComponent<Limit_Time>().Final_Walk_Floor();
        yield return backGroundColor.StartCoroutine(backGroundColor.Change_Color(backGroundColor.Get_BGColor(), new Color(1, 1, 1, 0.5f), 0.5f));
        yield return backGroundColor.StartCoroutine(backGroundColor.Change_Color(backGroundColor.Get_BGColor(), new Color(1, 1, 1, 0), 0.2f));

        animator.SetBool("IsWalk", false);

        All_Start();
        is_Domain = true;

        yield return null;
    }
    void All_Stop()
    {
        if (first_phase != null)
            StopCoroutine(first_phase);
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

        first_phase = First_Phase();
        StartCoroutine(first_phase);
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
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, stageData.LimitMin.x, stageData.LimitMax.x),
            Mathf.Clamp(transform.position.y, stageData.LimitMin.y, stageData.LimitMax.y));
        }
        if (is_Button)
        {
            if (transform.position.x <= -3)
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
            if (transform.position.x >= 40)
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
            Fever_Particle_Copy.transform.position = new Vector3(transform.position.x, Fever_Particle_Copy.transform.position.y, Fever_Particle_Copy.transform.position.z);
    }
}