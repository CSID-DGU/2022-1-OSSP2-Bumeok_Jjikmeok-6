using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SolGryn_Peanut : Enemy_Info
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

		cameraShake.mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
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
		if (Count == 1)
		{
			Die();
		}
	}
	public void Die()
	{
		camera_shake = cameraShake.Shake_Act(.03f, .03f, 0.1f, false);
		StartCoroutine(camera_shake);
		Instantiate(deathParticle, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}
}