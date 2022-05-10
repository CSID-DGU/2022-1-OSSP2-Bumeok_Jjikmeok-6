using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interrupt_Random_Move : MonoBehaviour
{
    [SerializeField]
    float pursuitSpeed;

    [SerializeField]
    float wanderSpeed;

    [SerializeField]
    float directionChangeInterval;

    [SerializeField]
    bool followPlayer;

    float currentSpeed;

    Rigidbody2D rb;

    Transform targetTransform;

    Vector3 endPosition;

    int randomAngle = 0;

    CircleCollider2D circleColliderObject; // The variable for visualizing gizmos on the screen

    [SerializeField]
    GameObject Lazor;

    IEnumerator move;
    IEnumerator wander_routine;

    public IEnumerator Trigger_Lazor(Vector3 tempPosition) // �� ���� ����, ���� ���� �÷��̾�� �����ϱ� ���� ��� ������ �� �ڵ�
    {
        while (true)
        {
            Lazor.GetComponent<Movement2D_Wow>().MoveTo(new Vector3(tempPosition.x - transform.position.x,
                         tempPosition.y - transform.position.y, 0));
            Instantiate(Lazor, transform.position, Quaternion.identity);
            yield return null;
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        circleColliderObject = GetComponent<CircleCollider2D>();
        targetTransform = null;
        currentSpeed = wanderSpeed;
    }

    // Start is called before the first frame update
    void Start()
    {
        //animator = GetComponent<Animator>();
        wander_routine = WanderRoutine();
        StartCoroutine(wander_routine);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(rb.position, endPosition, Color.red); // Showing the direction and distance of an object
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

        return new Vector3(randNum * Mathf.Cos(inputAngleRadians), this.transform.position.y, 0);
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
}

