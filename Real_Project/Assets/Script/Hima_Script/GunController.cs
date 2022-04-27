using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {
	public float bulletSpeed = 10f;
	public GameObject bulletPrefab;
	public Transform firePosition;
	public AudioSource fireSound;


	private HimaController hima;


	// Use this for initialization
	void Start () {
		hima = gameObject.GetComponent<HimaController>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Fire1")) {
			GameObject bullet = GameObject.Instantiate(bulletPrefab, firePosition.position, Quaternion.identity);
			float vx = hima.facingRight ? bulletSpeed : -bulletSpeed;
			bullet.GetComponent<BulletController>().velocity = new Vector3(vx, 0, 0);

			if (fireSound) {
				fireSound.PlayOneShot(fireSound.clip);
			}
		}
	}
}
