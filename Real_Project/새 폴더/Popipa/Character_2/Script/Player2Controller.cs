using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Player2Controller : MonoBehaviour
{
	
	public Animator anim;           

	// Controls facing direction
	public bool facingRight;

	// Use this for initialization
	void Start()
	{
		

	}

	public void Escape()
	{
		anim.SetBool("Escape", true);
	}

	public void EscapeOff()
	{
		anim.SetBool("Escape", false);
	}

	
	public void Walk()
	{
		anim.SetBool("Walk", true);
	}

	public void WalkOff()
	{
		anim.SetBool("Walk", false);
	}
	public void Doll()
	{
		anim.SetBool("Doll", true);
	}
	public void DollOff()
	{
		anim.SetBool("Doll", false);
	}
	public void Attack()
	{
		anim.SetBool("Attack", true);
	}
	public void AttackOff()
	{
		anim.SetBool("Attack", false);
	}





	// Update is called once per frame
	void Update()
	{



	} // Closes Update()

	// Function to flip Character in direction it's moving
	

}



