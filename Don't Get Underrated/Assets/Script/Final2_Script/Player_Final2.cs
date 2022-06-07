using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Final2 : Player_Info {

	[HideInInspector]
	public bool facingRight = true;

	[HideInInspector]
	public bool jump = false;

	[SerializeField]
	private Text Death_Count;

	[SerializeField]
	private float maxSpeed = 7f;

	[SerializeField]
	private float jumpSpeed = 10f;

	[SerializeField]
	private int maxAirJumps = 2;

	[SerializeField]
	private float distanceToGround = 0;

	[SerializeField]
	int deathCount = 5;

	[SerializeField]
	Text GyoSu;

	private Animator anim;

	private Rigidbody2D rb2d;

	private SolGryn solGryn;

	private bool isMove = false;

	private bool isJump = false;

	float h = 0;

	private bool grounded = false;

	private int jumpCount;

	private int groundLayerMask;

	private IEnumerator color_unbeatable, whilee;

	public int DeathCount => deathCount;

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
		if (TryGetComponent(out Animator A))
			anim = A;
		if (TryGetComponent(out Rigidbody2D RB))
			rb2d = RB;
		if (GameObject.FindGameObjectWithTag("Boss") && GameObject.FindGameObjectWithTag("Boss").TryGetComponent(out SolGryn SG))
			solGryn = SG;
		groundLayerMask = 1 << LayerMask.NameToLayer("Ground");
		deathCount = 0;
		GyoSu.color = Color.clear;
		Death_Count.text = "Death Count : " + deathCount;
		My_Name.text = singleTone.id;
		singleTone.EasterEgg = false;
	}

	private void Start() 
	{
		Load();
	}

	private void Load()
    {
		Unbeatable = true;
		IsMove = true;
		IsJump = true;
		solGryn.WelCome();
	}

	private void Update() 
	{
		if (Input.GetButtonDown("Jump") && jumpCount < maxAirJumps) 
		{
			jump = true;
			jumpCount += 1;
		}
		if (Input.GetKeyDown(KeyCode.Semicolon) && !singleTone.EasterEgg)
        {
			singleTone.EasterEgg = true;
			GyoSu.color = Color.blue;
			solGryn.OnDie();
		}
	}

	private bool isGrounded() 
	{
		Vector2 position = My_Position;
		Vector2 direction = Vector2.down;

		RaycastHit2D hit = Physics2D.Raycast(position, direction, distanceToGround, groundLayerMask);
		return hit.collider != null;
	}

    public override void TakeDamage(int damage)
    {
		if (Unbeatable)
			return;
		Unbeatable = true;

		Instantiate(When_Dead_Effect, transform.position, Quaternion.identity);
		deathCount++;
		Death_Count.text = "Death Count : " + deathCount;

		isMove = false;
		float velo_x = rb2d.velocity.x;
		rb2d.velocity = Vector2.zero;
		Vector2 dir = new Vector2(-Mathf.Sign(velo_x) * 1, 1).normalized;
		rb2d.AddForce(dir * 10, ForceMode2D.Impulse);
		
		Run_Life_Act_And_Continue(ref whilee, Whilee());
    }
	private IEnumerator Whilee()
    {
		Run_Life_Act_And_Continue(ref color_unbeatable, My_Color_When_UnBeatable());

		float percent = 0;
		while(percent < 0.3f)
        {
			percent += Time.deltaTime;
			yield return null;
        } // 정확한 시간 계산을 위해 식을 좀 복잡하게 썼음

		isMove = true;
		yield return YieldInstructionCache.WaitForSeconds(1.5f);
		
		Stop_Life_Act(ref color_unbeatable);
		Return_To_My_Origin_Color();
		Unbeatable = false;
		yield return null;
    }

    private void LateUpdate()
    {
        if (My_Position.y <= -6)
        {
			if (My_Position.x <= -3 || My_Position.x >= 3)
            {
				My_Position = Vector3.zero;
				TakeDamage(1);
            }
        }
    }

    private void FixedUpdate() {
		
		if (isMove)
        {
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

	private bool IsInsideGround() {
		return Physics2D.OverlapPoint(transform.position, groundLayerMask);
	}

	private void Flip() {
		facingRight = !facingRight;
		transform.localRotation = Quaternion.Euler(0f, facingRight ? 0f : 180f, 0f);
	}
    public override void OnDie()
    {
		TakeDamage(1);
    }
}
