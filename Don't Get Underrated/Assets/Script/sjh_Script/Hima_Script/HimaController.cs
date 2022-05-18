using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class HimaController : Player_Info {

	[HideInInspector]
	public bool facingRight = true;
	[HideInInspector]
	public bool jump = false;


	public float moveAccel = 30f;
	public float maxSpeed = 7f;
	public float jumpSpeed = 10f;
	public int maxAirJumps = 2;
	public float distanceToGround = 0.78f;
	private bool isMove = false;
	private float h = 0;

	public bool IsMove
    {
        get { return isMove; }
        set { isMove = value; }
    }

	private bool grounded = false;
	private Animator anim;
	private Rigidbody2D rb2d;
	private int jumpCount;
	private int groundLayerMask;

	// Use this for initialization
	private new void Awake() 
	{
		anim = GetComponent<Animator>();
		rb2d = GetComponent<Rigidbody2D>();
		groundLayerMask = 1 << LayerMask.NameToLayer("Ground");
	}

	void Start() 
	{
		StartCoroutine(Load());
	}

	IEnumerator Load()
    {
        h = 0;
        yield return YieldInstructionCache.WaitForSeconds(2f);

        h = 1;
        yield return YieldInstructionCache.WaitForSeconds(.3f);
        h = 0;
        yield return YieldInstructionCache.WaitForSeconds(1f);

        h = -1;
        yield return YieldInstructionCache.WaitForSeconds(.6f);
        h = 0;
        yield return YieldInstructionCache.WaitForSeconds(1f);

        h = 1;
        maxSpeed = 1f;
        yield return YieldInstructionCache.WaitForSeconds(2f);
        h = 0;
        yield return YieldInstructionCache.WaitForSeconds(2f);

        maxSpeed = 2f;
        h = 1;
        yield return YieldInstructionCache.WaitForSeconds(.25f);
        h = 0;
        yield return YieldInstructionCache.WaitForSeconds(.25f);

        h = -1;
        yield return YieldInstructionCache.WaitForSeconds(.25f);
        h = 0;
        yield return YieldInstructionCache.WaitForSeconds(.25f);

        h = 1;
        yield return YieldInstructionCache.WaitForSeconds(.25f);
        h = 0;
        maxSpeed = 7f;
        yield return YieldInstructionCache.WaitForSeconds(.7f);
        GameObject.FindGameObjectWithTag("SolGryn").GetComponent<SolGryn>().WelCome();
	}

	// Update is called once per frame
	void Update() {
		if (Input.GetButtonDown("Jump") && jumpCount < maxAirJumps) {
			jump = true;
			jumpCount += 1;
		}
	}

	bool isGrounded() {
//		return rb2d.IsTouchingLayers(groundLayer);

		Vector2 position = transform.position;
		Vector2 direction = Vector2.down;

		RaycastHit2D hit = Physics2D.Raycast(position, direction, distanceToGround, groundLayerMask);
		return hit.collider != null;
	}

    public override void TakeDamage(int damage)
    {
		OnDie();
    }

    void FixedUpdate() {
		
		if (isMove)
			h = Input.GetAxisRaw("Horizontal");

		if (h == 0)
		{
			rb2d.velocity = new Vector2(0, rb2d.velocity.y);
			anim.SetBool("Walking", false);
		}
		else
		{
			float vx = Mathf.Sign(h) * maxSpeed;
			rb2d.velocity = new Vector2(vx, rb2d.velocity.y);
			anim.SetBool("Walking", true);
		}

		anim.SetFloat("VelocityY", rb2d.velocity.y);

		if (h > 0 && !facingRight)
			Flip();
		else if (h < 0 && facingRight)
			Flip();

		bool nextGrounded = isGrounded();


		if (jump)
		{
			rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
			jump = false;
		}

		if (nextGrounded)
		{
			jumpCount = 0;
		}

		if (nextGrounded && !grounded)
		{

			anim.SetTrigger("Land");
		}

		if (!nextGrounded && grounded)
		{
			anim.SetTrigger("Midair");
		}

		grounded = nextGrounded;

		if (isInsideGround())
		{
			OnDie();
		}
	}

	bool isInsideGround() {
		return Physics2D.OverlapPoint(transform.position, groundLayerMask);
	}

	void Flip() {
		facingRight = !facingRight;
		transform.localRotation = Quaternion.Euler(0f, facingRight ? 0f : 180f, 0f);
	}

	void OnTriggerEnter2D(Collider2D col) {
		Debug.Log(LayerMask.LayerToName(col.gameObject.layer));
	}
    public override void OnDie()
    {
		base.OnDie();
    }
}
