using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Student_Random_Move : MonoBehaviour
{
    [SerializeField]
    float pursuitSpeed;

    [SerializeField]
    float wanderSpeed;

    [SerializeField]
    float directionChangeInterval;

    [SerializeField]
    bool followPlayer;

    IEnumerator move;
    IEnumerator wander_routine;

    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    Vector3 endPosition;
    int randomAngle = 0;
    float currentSpeed;

    // Start is called before the first frame update

    private void Awake()
    {
        currentSpeed = wanderSpeed;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        //animator = GetComponent<Animator>(); // TBD
        wander_routine = WanderRoutine();
        StartCoroutine(wander_routine);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(rb.position, endPosition, Color.red); // Showing the direction and distance of an object
        rb.velocity = Vector3.zero;
    }

    IEnumerator WanderRoutine()
    {
        while (true)
        {
            ChooseNewEndpoint();

            if (move != null)
            {
                StopCoroutine(move);
            }
            move = Move(rb, currentSpeed);

            StartCoroutine(move);
            yield return YieldInstructionCache.WaitForSeconds(directionChangeInterval); // �� �ڵ� ����
        }
    }

    void ChooseNewEndpoint() // Setting a destination of an object
    {
        randomAngle = Random.Range(0, 361);
        endPosition = Vector3FromAngle(randomAngle);
    }
    public void Start_Move()
    {
        wander_routine = WanderRoutine();
        StartCoroutine(wander_routine);
    }
    public void Stop_Move()
    {
        StopCoroutine(move);
        StopCoroutine(wander_routine);
    }
    Vector3 Vector3FromAngle(int inputAngleDegrees) // Changing the degree into the radian
    {
        float inputAngleRadians = inputAngleDegrees * Mathf.Deg2Rad;

        int randNum = Random.Range(-13, 14);

        return new Vector3(randNum * Mathf.Cos(inputAngleRadians), transform.position.y, 0);
    }
    public void Be_Attacked()
    {
        spriteRenderer.color = Color.blue;
    }
    public void NotBe_Attacked()
    {
        spriteRenderer.color = Color.white;
    }

    IEnumerator Move(Rigidbody2D rigidBodyToMove, float speed) // Acutual movement of an object according to the value of an endPosition
    {
        float remainingDistance = (transform.position - endPosition).sqrMagnitude;

        while (remainingDistance > float.Epsilon)
        {
            if (rigidBodyToMove != null) // Checking whether an object has rigidbody2D
            {
                //animator.SetBool("isWalking", true);

                Vector3 newPosition = Vector3.MoveTowards(rigidBodyToMove.position, endPosition, speed * Time.deltaTime);

                rb.MovePosition(newPosition);

                remainingDistance = (transform.position - endPosition).sqrMagnitude;
            }

            yield return YieldInstructionCache.WaitForEndOfFrame;
        }

        //animator.SetBool("isWalking",false);
    }
}
