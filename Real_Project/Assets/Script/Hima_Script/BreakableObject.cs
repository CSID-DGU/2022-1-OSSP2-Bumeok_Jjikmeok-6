using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour, IDamageable {

	public float health = 2f;


	public void Damage(float damage) {
		health = Mathf.Max(0, health - damage);

		if (health == 0) {
			Destroy(gameObject);
		}
	}
}
