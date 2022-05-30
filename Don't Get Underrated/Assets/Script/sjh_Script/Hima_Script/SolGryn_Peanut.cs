using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SolGryn_Peanut : Weapon_Devil
{

	[SerializeField]
	float distanceToGround = 0.2f;

	[SerializeField]
	GameObject deathParticle;

	private int groundLayerMask;

	private int Count = 0;

	// Use this for initialization
	private new void Awake()
	{
		base.Awake();
		groundLayerMask = 1 << LayerMask.NameToLayer("Ground");
		Debug.Log(groundLayerMask);
	}
    private void Start()
    {
		Start_Camera_Shake(0.015f, 0.1f, false, false);
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
		{
			Debug.Log("¿Ö");
			Count++;
		}
		if (Count == 1)
			Die();
	}
	public void Die()
	{
		Start_Camera_Shake(0.0001f, 0.05f, false, false);
		Instantiate(deathParticle, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}
}