using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SolGryn_Peanut : MonoBehaviour
{

	[SerializeField]
	float distanceToGround = 0.2f;

	[SerializeField]
	GameObject deathParticle;

	private int groundLayerMask;

	private int Count = 0;

	// Use this for initialization
	void Awake()
	{
		groundLayerMask = 1 << LayerMask.NameToLayer("Ground");
		Debug.Log(groundLayerMask);
	}
	bool isGrounded()
	{
		Vector2 position = transform.position;
		Vector2 direction = Vector2.down;

		RaycastHit2D hit = Physics2D.Raycast(position, direction, distanceToGround, groundLayerMask);
		return hit.collider != null;
	}

	void FixedUpdate()
	{
		bool nextGrounded = isGrounded();

		if (nextGrounded)
		{
			Debug.Log("¿Ö");
			Count++;
		}
		if (Count == 2)
		{
			Die();
		}
	}
	public void Die()
	{
		Instantiate(deathParticle, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}
}