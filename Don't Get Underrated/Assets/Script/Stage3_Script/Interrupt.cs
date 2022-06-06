using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interrupt : Enemy_Info
{
    [SerializeField]
    float pursuitSpeed;

    [SerializeField]
    float wanderSpeed;

    [SerializeField]
    float directionChangeInterval;

    [SerializeField]
    GameObject Exclamation;

    [SerializeField]
    bool followPlayer;

    float currentSpeed;

    Rigidbody2D rb;

    Transform targetTransform;

    Vector3 endPosition;

    int randomAngle = 0;

    CircleCollider2D circleColliderObject; // The variable for visualizing gizmos on the screen

    IEnumerator move, wander_routine, trigger_lazor;

    GameObject Exclamation_Copy;

    void Init_Start()
    {
        if (TryGetComponent(out Rigidbody2D RB2D))
            rb = RB2D;
        if (TryGetComponent(out CircleCollider2D CC2D))
            circleColliderObject = CC2D;
        targetTransform = null;
        currentSpeed = wanderSpeed;
    }
    public override void OnDie()
    {
        Effect_Sound_Stop();
        base.OnDie();
    }
    private new void Awake()
    {
        base.Awake();
        Init_Start();
        if (GameObject.Find("Enemy_Effect_Sound") && GameObject.Find("Enemy_Effect_Sound").TryGetComponent(out AudioSource AS1))
            EffectSource = AS1;
    }
    IEnumerator Trigger_Lazor(Vector3 TargetPos) // 과제, 시험 등이 플레이어랑 경쟁하기 위해 쏘는 레이저 빔 코드
    {
        Stop_Move();
        Exclamation_Copy = Instantiate(Exclamation, transform.position, Quaternion.identity);
        yield return StaticFunc.WaitForRealSeconds(0.3f); // 컴퓨터 사양에 따라 레이저가 나오는 시간이 달라지지 않도록 실제 시간 적용

        Destroy(Exclamation_Copy);

        Effect_Sound_Play(0); // 레이저 발사 소리
        while (true)
        {
            Launch_Weapon(ref Weapon[0], new Vector3(TargetPos.x - My_Position.x,
                         TargetPos.y - My_Position.y, 0), Quaternion.identity, 90, My_Position);
            yield return null;
        } // 실제 레이저 발사 코드
    }
    public void Fight_With_Player(Vector3 TargetPos) // 레이저 발사 시작
    {
        Run_Life_Act_And_Continue(ref trigger_lazor, Trigger_Lazor(TargetPos));
    }
    public void Stop_Fight_With_Player()  // 레이저 발사 중지
    {
        Stop_Life_Act(ref trigger_lazor);
    }
    public void Disappear()  // 타임 아웃 됐을 때 동적인 연출을 위해 인터럽트의 색깔을 서서히 감소
    {
        Run_Life_Act(Change_My_Color(Color.white, Color.clear, 1.5f, 0.1f, null));
    }
    public void Stop_Lazor()  // 인터럽트의 레이저 발사 행동 중지
    {
        if (Exclamation_Copy != null)
            Destroy(Exclamation_Copy);
        Effect_Sound_Stop();
        StopAllCoroutines();
    }

    public void When_Fever_End() // 인터럽트의 모든 행동 중지 후 다시 이동
    {
        Stop_Move();
        Stop_Lazor();
        Init_Start();
        Start_Move();
    }
    void Start()
    {
        Run_Life_Act_And_Continue(ref wander_routine, WanderRoutine());
    }
    public IEnumerator WanderRoutine() // 인터럽트가 좌, 우 랜덤으로 돌아다니도록 하는 함수
    {
        while (true)
        {
            ChooseNewEndpoint();

            Run_Life_Act_And_Continue(ref move, Move(rb, currentSpeed));

            yield return YieldInstructionCache.WaitForSeconds(directionChangeInterval);
        }
    }
    public void Start_Move()
    {
        Effect_Sound_Stop();
        Run_Life_Act_And_Continue(ref wander_routine, WanderRoutine());
    }
    public void Stop_Move()
    {
        Stop_Life_Act(ref move);
        Stop_Life_Act(ref wander_routine);
    }

    void ChooseNewEndpoint() // Setting a destination of an object
    {
        randomAngle = Random.Range(0, 361);

        endPosition = Vector3FromAngle(randomAngle);
    }

    Vector3 Vector3FromAngle(int inputAngleDegrees) // Changing the degree into the radian
    {
        float inputAngleRadians = inputAngleDegrees * Mathf.Deg2Rad;

        int randNum = Random.Range(-13, 14);

        return new Vector3(randNum * Mathf.Cos(inputAngleRadians) + transform.position.x, this.transform.position.y, 0);
    }

    public IEnumerator Move(Rigidbody2D rigidBodyToMove, float speed) // 인터럽트의 실질적인 이동
    {
        float remainingDistance = (My_Position - endPosition).sqrMagnitude;

        while (remainingDistance > float.Epsilon)
        {
            if (targetTransform != null) // 플레이어 추적 안할 시 --> 랜덤으로 위치 조정 
            {
                endPosition = new Vector3(targetTransform.position.x, My_Position.y, 0); // an object moves towards to taregeted player
            }


            if (rigidBodyToMove != null) // 플레이어 추적 시 --> 플레이어를 따라다니도록 거리 조정 (170번째 줄의 OnTriggerEnter2D 함수와 연결)
            {
                Vector3 newPosition = Vector3.MoveTowards(rigidBodyToMove.position, endPosition, speed * Time.deltaTime);

                rb.MovePosition(newPosition);

                remainingDistance = (My_Position - endPosition).sqrMagnitude;
            }

            yield return new WaitForFixedUpdate();
        }
    }


    private new void OnTriggerEnter2D(Collider2D collision) // 플레이어 추적을 위한 함수
    {
        if (collision.gameObject.CompareTag("Player") && followPlayer) // 플레이어를 발견했으면 그에 따른 위치를 조정한 후, 재이동
        {
            currentSpeed = pursuitSpeed;

            targetTransform = collision.gameObject.transform;

            Run_Life_Act_And_Continue(ref move, Move(rb, currentSpeed));
        }
    }

    void OnTriggerExit2D(Collider2D collision) // 플레이어와 거리가 멀어졌을 때 처리해주는 함수
    {
        if (collision.gameObject.CompareTag("Player")) // 인터럽트의 위치를 다시 조정
        {
            currentSpeed = wanderSpeed;

            Stop_Life_Act(ref move);

            targetTransform = null;
        }
    }

    private void OnDrawGizmos() //This method for visualizing gizmos
    {
        if (circleColliderObject != null)
        {
            Gizmos.DrawWireSphere(transform.position, circleColliderObject.radius);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(rb.position, endPosition, Color.red); // Showing the direction and distance of an object
        My_Position = new Vector3(Mathf.Clamp(My_Position.x, stageData.LimitMin.x, stageData.LimitMax.x),
         Mathf.Clamp(My_Position.y, stageData.LimitMin.y, stageData.LimitMax.y));
    }
}

