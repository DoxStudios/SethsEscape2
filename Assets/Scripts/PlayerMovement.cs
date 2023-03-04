using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

	public Transform tempObject;

	public float speed = 45f;
	public float jumpForce = 60f;
	public float doubleJumpForce = 45f;
	public bool doubleJump = true;
	public LayerMask ground;
	public SpriteRenderer sr;
	public GameObject groundCheck;

	Rigidbody2D rb;
	PlayerStatsManager psm;
	float groundedRadius = 1f;
	bool isGrounded = true;
	bool doubleJumpRemaining = true;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		psm = GetComponent<PlayerStatsManager>();
	}

	void Update()
	{

	   isGrounded = checkGrounded();
	   if(isGrounded)
	   {
			doubleJumpRemaining = true;
	   }

		float horizontal = Input.GetAxisRaw("Horizontal");
		Vector3 move = new Vector3(horizontal, 0, 0);
		if(horizontal < 0)
		{
			sr.flipX = true;
		}
		else if(horizontal > 0)
		{
			sr.flipX = false;
		}

		if(!psm.stunned)
		{
			transform.position += move * speed * Time.deltaTime;
		}

		if(Input.GetKeyDown(KeyCode.P))
		{
			psm.Heal(1);
		}

		if(Input.GetKeyDown(KeyCode.O))
		{
			psm.Damage(1, tempObject, 1, 0.15f, 0.2f);
		}


		if((isGrounded || doubleJumpRemaining) && Input.GetButtonDown("Jump") && !psm.stunned)
		{
			if(!isGrounded)
			{
				rb.velocity = new Vector2(rb.velocity.x, 0);
				doubleJumpRemaining = false;
				rb.AddForce(transform.up * doubleJumpForce, ForceMode2D.Impulse);
			}
			else
			{
				isGrounded = false;
				rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
			}
			
		}
	}

	bool checkGrounded()
	{
		Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.transform.position, groundedRadius, ground);
		for (int i = 0; i < colliders.Length; i++) {
			if (colliders[i].gameObject != gameObject) {
				return true;
			}
		}
		return false;
	}
}
