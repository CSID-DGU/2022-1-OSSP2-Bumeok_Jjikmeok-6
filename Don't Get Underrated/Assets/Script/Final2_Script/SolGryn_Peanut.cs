using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolGryn_Peanut : Weapon_Devil
{
	[SerializeField]
	float distanceToGround = 0.2f;

	[SerializeField]
	GameObject deathParticle;

	private int groundLayerMask;

	private int Count = 0;
	private new void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision != null && collision.CompareTag("Player") && collision.TryGetComponent(out Player_Info HC))
		{
			if (!HC.Unbeatable)
				HC.TakeDamage(1);
		}
	}
	private new void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject != null && collision.gameObject.CompareTag("Player") && collision.gameObject.TryGetComponent(out Player_Info HC))
		{
			if (!HC.Unbeatable)
				HC.TakeDamage(1);
		}
	}

	private new void Awake()
	{
		base.Awake();
		groundLayerMask = 1 << LayerMask.NameToLayer("Ground");
	}
    private void Start()
    {
		Camera_Shake(0.009f, 0.1f, false, false);
	}
    bool IsGrounded()
	{
		Vector2 position = transform.position;
		Vector2 direction = Vector2.down;

		RaycastHit2D hit = Physics2D.Raycast(position, direction, distanceToGround, groundLayerMask);
		return hit.collider != null;
	}

	void FixedUpdate()
	{
		bool nextGrounded = IsGrounded();

		if (nextGrounded)
			Count++;

		if (Count == 1)
			Die();
	}
	public void Die()
	{
		Camera_Shake(0.00001f, 0.05f, false, false);
		Instantiate(deathParticle, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}
}