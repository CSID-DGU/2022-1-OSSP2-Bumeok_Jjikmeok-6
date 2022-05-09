using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController_kch : MonoBehaviour
{
    Rigidbody2D rigid2D;
    Camera Camera;
    Vector2 InitBallPos;
    Vector2 finMousePos;
    Vector2 weightedValue;
    int CheckClick = 0;
    public bool ballHit = false; // Encapsulation needs to be supplemented later on.
    Vector2 lastPos;
    bool checkGumGet = false;
    public float gumItemInterval;
    CircleCollider2D GumBounce;

    // Start is called before the first frame update
    void Start()
    {
        this.rigid2D = GetComponent<Rigidbody2D>();
        Camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        GumBounce = GetComponent<CircleCollider2D>();

        InvokeRepeating("CheckChangePosition", 0, 0.05f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && ballHit == false)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

            if (hit.collider != null && hit.collider.transform == this.transform)
            {
                CheckClick = 1;
                InitBallPos = Input.mousePosition;
                InitBallPos = Camera.ScreenToWorldPoint(InitBallPos);
            }
        }

        if (Input.GetMouseButtonUp(0) && CheckClick == 1)
        {
            finMousePos = Input.mousePosition;
            finMousePos = Camera.ScreenToWorldPoint(finMousePos);

            weightedValue = finMousePos - InitBallPos;
            weightedValue.x *= 75;
            weightedValue.y *= 75;

            CheckClick = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && checkGumGet == true)
        {
            Debug.Log("Gum item is used!");

            StartCoroutine(GumItemRoutine());
            checkGumGet = false;
            Debug.Log("Gum item status: " + checkGumGet);
        }

        this.rigid2D.AddForce(weightedValue);
        weightedValue.x *= 0.5f;
        weightedValue.y *= 0.5f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ballHit = true;
    }

    private void CheckChangePosition()
    {
        if (lastPos.x != this.transform.localPosition.x && lastPos.y != this.transform.localPosition.y)
        {
            lastPos = this.transform.localPosition;
        }
        else
        {
            ballHit = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("gumItem"))
        {
            collision.gameObject.SetActive(false);
            checkGumGet = true;
            Debug.Log("Gum item is acquired!");
        }
    }

    public IEnumerator GumItemRoutine()
    {
        Debug.Log("Routine Ongoing");
        GumBounce.sharedMaterial.bounciness = 0;
        GumBounce.enabled = false;
        GumBounce.enabled = true;
        yield return new WaitForSeconds(gumItemInterval);
        
        GumBounce.sharedMaterial.bounciness = 0.5f;
        GumBounce.enabled = false;
        GumBounce.enabled = true;
        Debug.Log("CoRoutine Terminated");
    }
}