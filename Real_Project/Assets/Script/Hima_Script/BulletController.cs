using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {
	[HideInInspector]
	public Vector3 velocity = Vector3.zero;

	void Start () {
	}

	void FixedUpdate () {
		transform.position += velocity * Time.deltaTime;
		transform.rotation = Quaternion.FromToRotation(Vector3.left, velocity);
	}

	void OnBecameInvisible() {
		Destroy(gameObject, 0.5f);
	}

	void OnTriggerEnter2D(Collider2D col) {
		IDamageable damagable = col.gameObject.GetComponent<IDamageable>();

		if (damagable != null) {
			damagable.Damage(1f);
		}

		Destroy(gameObject);
	}

	void checkCollider(Collider2D col) {
		int layer = col.gameObject.layer;

		if (layer == LayerMask.NameToLayer("Ground")) {
			Destroy(gameObject);
		}

		if (layer == LayerMask.NameToLayer("Enemies")) {
			Destroy(gameObject);
		}
	}
}
