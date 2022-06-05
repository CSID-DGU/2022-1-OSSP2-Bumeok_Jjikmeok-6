using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Student : Life
{
    [SerializeField]
    float pursuitSpeed;

    [SerializeField]
    float wanderSpeed;

    [SerializeField]
    float directionChangeInterval;

    [SerializeField]
    bool followPlayer;

    IEnumerator move, wander_routine;

    Animator animator;

    Rigidbody2D rb;

    Vector3 endPosition;

    int randomAngle = 0;

    float currentSpeed;
    // Start is called before the first frame update

    private new void Awake()
    {
        base.Awake();
        if (TryGetComponent(out Rigidbody2D RB))
            rb = RB;
        Init_Start();
    }

    void Init_Start()
    {
        currentSpeed = wanderSpeed;
        if (TryGetComponent(out Animator A))
            animator = A;
    }
    public Color Get_Color()
    {
        return spriteRenderer.color;
    }
    void Start()
    {
        Run_Life_Act_And_Continue(ref wander_routine, WanderRoutine());
    }                
    public void Disappear()
    {
        Run_Life_Act(Change_My_Color(Color.white, Color.clear, 1.5f, 0.1f, null));
    }
    void Update()
    {
        rb.velocity = Vector3.zero;
        My_Position = new Vector3(Mathf.Clamp(My_Position.x, stageData.LimitMin.x, stageData.LimitMax.x),
           Mathf.Clamp(My_Position.y, stageData.LimitMin.y, stageData.LimitMax.y));
    }

    IEnumerator WanderRoutine()
    {
        while (true)
        {
            ChooseNewEndpoint();

            Run_Life_Act_And_Continue(ref move, Move(rb, currentSpeed));

            yield return YieldInstructionCache.WaitForSeconds(directionChangeInterval); // ÀÌ ÄÚµå ¹¹Áö
        }
    }

    void ChooseNewEndpoint() // Setting a destination of an object
    {
        randomAngle = Random.Range(0, 361);
        endPosition = Vector3FromAngle(randomAngle);
    }
    public void Start_Move()
    {
        spriteRenderer.color = Color.white;
        Run_Life_Act_And_Continue(ref wander_routine, WanderRoutine());
        animator.SetBool("Electric", false);
    }
    public void Stop_Move()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0.95f);
        Stop_Life_Act(ref move);
        Stop_Life_Act(ref wander_routine);
        animator.SetBool("Electric", true);
    }
    public void When_Fever_End()
    {
        Stop_Move();
        StopAllCoroutines();
        Init_Start();
        Start_Move();
    }

    Vector3 Vector3FromAngle(int inputAngleDegrees) 
    {
        float inputAngleRadians = inputAngleDegrees * Mathf.Deg2Rad;

        int randNum = Random.Range(-13, 14);

        return new Vector3(randNum * Mathf.Cos(inputAngleRadians) + transform.position.x, transform.position.y, 0);
    }
    public void Stop_Coroutine()
    {
        StopAllCoroutines();
    }

    IEnumerator Move(Rigidbody2D rigidBodyToMove, float speed)
    {
        float remainingDistance = (My_Position - endPosition).sqrMagnitude;
        int key;
        while (remainingDistance > float.Epsilon)
        {
            if (rigidBodyToMove != null)
            {
                if (endPosition.x - rigidBodyToMove.position.x > 0)
                    key = -1;
                else
                    key = 1;
                My_Scale = new Vector3(-key * Mathf.Abs(My_Scale.x), My_Scale.y, My_Scale.z);

                Vector3 newPosition = Vector3.MoveTowards(rigidBodyToMove.position, endPosition, speed * Time.deltaTime);

                rb.MovePosition(newPosition);

                remainingDistance = (My_Position - endPosition).sqrMagnitude;
            }
            yield return null;
        }
    }
}
