using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HimaController : Player_Info {

	[HideInInspector]
	public bool facingRight = true;
	[HideInInspector]
	public bool jump = false;

	[SerializeField]
	Text Death_Count;

	[SerializeField]
	float maxSpeed = 7f;

	[SerializeField]
	float jumpSpeed = 10f;

	[SerializeField]
	int maxAirJumps = 2;

	[SerializeField]
	float distanceToGround = 0;

	Animator anim;

	Rigidbody2D rb2d;

	bool isMove = false;

	bool isJump = false;

	float h = 0;

	bool grounded = false;

	int jumpCount;

	int groundLayerMask;

	IEnumerator color_unbeatable;

	public bool IsMove
    {
        get { return isMove; }
        set { isMove = value; }
    }
	public bool IsJump
	{
		get { return isJump; }
		set { isJump = value; }
	}

	// Use this for initialization
	private new void Awake() 
	{
		base.Awake();
		anim = GetComponent<Animator>();
		rb2d = GetComponent<Rigidbody2D>();
		groundLayerMask = 1 << LayerMask.NameToLayer("Ground");
		LifeTime = 0;
		Death_Count.text = "Death Count : " + LifeTime;
	}

	void Start() 
	{
		Run_Life_Act(Load());
	}

	IEnumerator Load()
    {
		Unbeatable = true;
		IsMove = true;
		IsJump = true;
        GameObject.FindGameObjectWithTag("Boss").GetComponent<SolGryn>().WelCome();
		yield return null;
	}

    // Update is called once per frame
    void Update() 
	{
		if (Input.GetButtonDown("Jump") && jumpCount < maxAirJumps) 
		{
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
        if (!Unbeatable)
        {
			LifeTime++;
			Death_Count.text = "Death Count : " + LifeTime;
			Unbeatable = true;
			isMove = false;
			float velo_x = rb2d.velocity.x;
			rb2d.velocity = Vector2.zero;
			Vector2 dir = new Vector2(-Mathf.Sign(velo_x) * 1, 1).normalized;
			rb2d.AddForce(dir * 14, ForceMode2D.Impulse);

			Run_Life_Act(Whilee());
		}

    }
	IEnumerator Whilee()
    {
		Run_Life_Act_And_Continue(ref color_unbeatable, My_Color_When_UnBeatable());

		float percent = 0;
		while(percent < 0.3f)
        {
			percent += Time.deltaTime;
			yield return null;
        } // 정확한 시간 계산을 위해 식을 좀 복잡하게 썼음
		isMove = true;
		yield return YieldInstructionCache.WaitForSeconds(2f);
		Stop_Life_Act(ref color_unbeatable);
		Unbeatable = false;
		yield return null;
    }

    void FixedUpdate() {
		
		if (isMove)
        {
			Debug.Log("야");
			h = Input.GetAxisRaw("Horizontal");
			float vh = Mathf.Sign(h);

			if (h == 0)
			{
				rb2d.velocity = new Vector2(0, rb2d.velocity.y);
				anim.SetBool("Walking", false);
			}
			else
			{
				float vx = vh * maxSpeed;
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

			if (IsInsideGround())
			{
				OnDie();
			}
		}		
    }

	bool IsInsideGround() {
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
		TakeDamage(1);
    }
}
