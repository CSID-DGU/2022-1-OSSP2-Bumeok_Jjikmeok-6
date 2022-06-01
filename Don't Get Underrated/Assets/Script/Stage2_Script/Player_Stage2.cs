using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Stage2 : Player_Info
{
    Rigidbody2D rigid2D;

    Camera Camera;

    CircleCollider2D GumBounce;

    Vector2 InitMousePos;

    Vector2 finMousePos;

    Vector2 weighedVector;

    Vector2 lastPos;

    int clickNum = 0;

    bool CheckClick = false;

    float totalCharge = 0f;

    float totalChargeNeeded = 0.1f;

    public bool ballHit = false; // Encapsulation needs to be supplemented later on.

    public bool checkGumGet = false;

    public float gumItemInterval;

    public float MaxBallSpeedVector;

    public float weighedSpeed;
    
    // Start is called before the first frame update

    private new void Awake()
    {
        base.Awake();
        if (TryGetComponent(out Rigidbody2D RB2D))
            rigid2D = RB2D;
        if (GameObject.Find("Main Camera") && GameObject.Find("Main Camera").TryGetComponent(out Camera C))
            Camera = C;
        if (TryGetComponent(out CircleCollider2D CC2D))
            GumBounce = CC2D;
    }
    void Start()
    {
        GumBounce.sharedMaterial.bounciness = 0.5f;
        InvokeRepeating("CheckChangePosition", 0, 0.05f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !ballHit) // When clicking the left button 
        {
            InitMousePos = Input.mousePosition;
            InitMousePos = Camera.ScreenToWorldPoint(InitMousePos);
            CheckClick = true; // This variable also contains info of ballHit;
        }

        if(Input.GetMouseButton(0) && !ballHit)
        {
            totalCharge += Time.deltaTime;
        }

        if (Input.GetMouseButtonUp(0) && CheckClick) // After releasing the left button 
        {
            clickNum++;

            finMousePos = Input.mousePosition;
            finMousePos = Camera.ScreenToWorldPoint(finMousePos);

            weighedVector = InitMousePos - finMousePos;

            if (weighedVector.magnitude > MaxBallSpeedVector) // When the speed exceeds the defined maximum
            {
                weighedVector.x *= (MaxBallSpeedVector / weighedVector.magnitude);
                weighedVector.y *= (MaxBallSpeedVector / weighedVector.magnitude);
            }

            //Increase the speeed mutiplying with weighed value
            AttemptShooting();

            CheckClick = false;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && checkGumGet == true)
        {
            Run_Life_Act(GumItemRoutine());
            checkGumGet = false;
        }
        
        // For making the speed of the ball decrease
        weighedVector.x *= 0.5f;
        weighedVector.y *= 0.5f;
    }

    private void OnCollisionEnter2D(Collision2D collision) // Check if the ball hit the object
    {
        ballHit = true;
        clickNum = 0;
    }

    private void CheckChangePosition() // Check if the ball halts after the collision occured
    {
        if (lastPos.x != transform.localPosition.x && lastPos.y != transform.localPosition.y)
        {
            lastPos = transform.localPosition;
        }
        else
        {
            ballHit = false;
        }
    }

    private void AttemptShooting()
    {
        if (clickNum == 1)
        {
            weighedVector.x *= weighedSpeed;
            weighedVector.y *= weighedSpeed;
        }
        else // clickNum >= 2
        {
            if (totalCharge < totalChargeNeeded)
            {
                weighedVector.x *= (weighedSpeed + 30 * clickNum) * Mathf.Pow((totalCharge / totalChargeNeeded), clickNum);
                weighedVector.y *= (weighedSpeed + 30 * clickNum) * Mathf.Pow((totalCharge / totalChargeNeeded), clickNum);
            }
            else
            {
                weighedVector.x *= (weighedSpeed + 30 * clickNum) * Mathf.Pow(0.83f, (clickNum));
                weighedVector.y *= (weighedSpeed + 30 * clickNum) * Mathf.Pow(0.83f, (clickNum));
            }
        }

        rigid2D.AddForce(weighedVector);
        totalCharge = 0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("gumItem"))
        {
            collision.gameObject.SetActive(false);
            checkGumGet = true;
        }
    }

    public IEnumerator GumItemRoutine()
    {
        GumBounce.sharedMaterial.bounciness = 0;
        GumBounce.enabled = false;
        GumBounce.enabled = true;
        yield return YieldInstructionCache.WaitForSeconds(gumItemInterval);
        
        GumBounce.sharedMaterial.bounciness = 0.5f;
        GumBounce.enabled = false;
        GumBounce.enabled = true;
    }
}