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
    IEnumerator Trigger_Lazor(Vector3 TargetPos) // ����, ���� ���� �÷��̾�� �����ϱ� ���� ��� ������ �� �ڵ�
    {
        Stop_Move();
        Exclamation_Copy = Instantiate(Exclamation, transform.position, Quaternion.identity);
        yield return StaticFunc.WaitForRealSeconds(0.3f); // ��ǻ�� ��翡 ���� �������� ������ �ð��� �޶����� �ʵ��� ���� �ð� ����

        Destroy(Exclamation_Copy);

        Effect_Sound_Play(0); // ������ �߻� �Ҹ�
        while (true)
        {
            Launch_Weapon(ref Weapon[0], new Vector3(TargetPos.x - My_Position.x,
                         TargetPos.y - My_Position.y, 0), Quaternion.identity, 90, My_Position);
            yield return null;
        } // ���� ������ �߻� �ڵ�
    }
    public void Fight_With_Player(Vector3 TargetPos) // ������ �߻� ����
    {
        Run_Life_Act_And_Continue(ref trigger_lazor, Trigger_Lazor(TargetPos));
    }
    public void Stop_Fight_With_Player()  // ������ �߻� ����
    {
        Stop_Life_Act(ref trigger_lazor);
    }
    public void Disappear()  // Ÿ�� �ƿ� ���� �� ������ ������ ���� ���ͷ�Ʈ�� ������ ������ ����
    {
        Run_Life_Act(Change_My_Color(Color.white, Color.clear, 1.5f, 0.1f, null));
    }
    public void Stop_Lazor()  // ���ͷ�Ʈ�� ������ �߻� �ൿ ����
    {
        if (Exclamation_Copy != null)
            Destroy(Exclamation_Copy);
        Effect_Sound_Stop();
        StopAllCoroutines();
    }

    public void When_Fever_End() // ���ͷ�Ʈ�� ��� �ൿ ���� �� �ٽ� �̵�
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
    public IEnumerator WanderRoutine() // ���ͷ�Ʈ�� ��, �� �������� ���ƴٴϵ��� �ϴ� �Լ�
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

    public IEnumerator Move(Rigidbody2D rigidBodyToMove, float speed) // ���ͷ�Ʈ�� �������� �̵�
    {
        float remainingDistance = (My_Position - endPosition).sqrMagnitude;

        while (remainingDistance > float.Epsilon)
        {
            if (targetTransform != null) // �÷��̾� ���� ���� �� --> �������� ��ġ ���� 
            {
                endPosition = new Vector3(targetTransform.position.x, My_Position.y, 0); // an object moves towards to taregeted player
            }


            if (rigidBodyToMove != null) // �÷��̾� ���� �� --> �÷��̾ ����ٴϵ��� �Ÿ� ���� (170��° ���� OnTriggerEnter2D �Լ��� ����)
            {
                Vector3 newPosition = Vector3.MoveTowards(rigidBodyToMove.position, endPosition, speed * Time.deltaTime);

                rb.MovePosition(newPosition);

                remainingDistance = (My_Position - endPosition).sqrMagnitude;
            }

            yield return new WaitForFixedUpdate();
        }
    }


    private new void OnTriggerEnter2D(Collider2D collision) // �÷��̾� ������ ���� �Լ�
    {
        if (collision.gameObject.CompareTag("Player") && followPlayer) // �÷��̾ �߰������� �׿� ���� ��ġ�� ������ ��, ���̵�
        {
            currentSpeed = pursuitSpeed;

            targetTransform = collision.gameObject.transform;

            Run_Life_Act_And_Continue(ref move, Move(rb, currentSpeed));
        }
    }

    void OnTriggerExit2D(Collider2D collision) // �÷��̾�� �Ÿ��� �־����� �� ó�����ִ� �Լ�
    {
        if (collision.gameObject.CompareTag("Player")) // ���ͷ�Ʈ�� ��ġ�� �ٽ� ����
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

