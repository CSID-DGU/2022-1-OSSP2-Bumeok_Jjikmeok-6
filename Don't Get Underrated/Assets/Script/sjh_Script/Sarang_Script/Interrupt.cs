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

    IEnumerator move;
    IEnumerator wander_routine;

    GameObject Exclamation_Copy;

    private new void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        circleColliderObject = GetComponent<CircleCollider2D>();
        targetTransform = null;
        currentSpeed = wanderSpeed;
    }
    public IEnumerator Trigger_Lazor(Vector3 tempPosition) // 이 쪽은 과제, 시험 등이 플레이어랑 경쟁하기 위해 쏘는 레이저 빔 코드
    {
        Stop_Move();
        Exclamation_Copy = Instantiate(Exclamation, transform.position, Quaternion.identity);
        yield return YieldInstructionCache.WaitForSeconds(0.3f);
        Destroy(Exclamation_Copy);

        while (true)
        {
            Launch_Weapon_For_Move_Blink(Weapon[0], new Vector3(tempPosition.x - transform.position.x,
                         tempPosition.y - transform.position.y, 0), Quaternion.identity, 9, false, transform.position);
            yield return null;
        }
    }
    public void Disappear()
    {
        StartCoroutine(Change_Color_Lerp(new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), 1.5f, 0.1f, null));
    }

    public void Stop_Coroutine()
    {
        if (Exclamation_Copy != null)
            Destroy(Exclamation_Copy);
        StopAllCoroutines();
    }

    // Start is called before the first frame update
    void Start()
    {
        //animator = GetComponent<Animator>();
        wander_routine = WanderRoutine();
        StartCoroutine(wander_routine);
    }


    public IEnumerator WanderRoutine()
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
            yield return new WaitForSeconds(directionChangeInterval);
        }
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

    public IEnumerator Move(Rigidbody2D rigidBodyToMove, float speed) // Acutual movement of an object according to the value of an endPosition
    {
        float remainingDistance = (transform.position - endPosition).sqrMagnitude;

        while (remainingDistance > float.Epsilon)
        {
            if (targetTransform != null) // If chaisng a player
            {
                Debug.Log("Caught!!");
                endPosition = new Vector3(targetTransform.position.x, this.transform.position.y, 0); // an object moves towards to taregeted player
            }


            if (rigidBodyToMove != null) // Checking whether an object has rigidbody2D
            {
                //animator.SetBool("isWalking", true);

                Vector3 newPosition = Vector3.MoveTowards(rigidBodyToMove.position, endPosition, speed * Time.deltaTime);

                rb.MovePosition(newPosition);

                remainingDistance = (transform.position - endPosition).sqrMagnitude;
            }

            yield return new WaitForFixedUpdate();
        }

        //animator.SetBool("isWalking",false);
    }


    void OnTriggerEnter2D(Collider2D collision) // This method for chasing a player
    {
        if (collision.gameObject.CompareTag("Player") && followPlayer) // If the collision has occured between the player and the object
        {
            currentSpeed = pursuitSpeed;

            targetTransform = collision.gameObject.transform;
            
            if (move != null)
            {
                StopCoroutine(move);
            }

            move = Move(rb, currentSpeed);
            StartCoroutine(move);
        }
    }

    void OnTriggerExit2D(Collider2D collision) // This method for a player getting away from the sight of a chaser
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //animator.SetBool("isWalking", false);

            currentSpeed = wanderSpeed;

            if (move != null)
            {
                StopCoroutine(move);
            }

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
    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(rb.position, endPosition, Color.red); // Showing the direction and distance of an object
    }
}

