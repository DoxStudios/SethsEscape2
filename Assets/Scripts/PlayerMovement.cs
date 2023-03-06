using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float speed = 45f;
	public float jumpForce = 60f;
	public float doubleJumpForce = 45f;
	public float dashDistance = 10f;
	public bool doubleJump = true;
	public LayerMask ground;
	public SpriteRenderer sr;
	public GameObject groundCheck;

	Rigidbody2D rb;
	PlayerStatsManager psm;
	float groundedRadius = 1f;
	bool isGrounded = true;
	bool doubleJumpRemaining = true;
	Vector3 direction = new Vector3(1, 0, 0);
	bool hasDash = true;
	Vector3 dashTarget;
	bool inWall;
	float currentWallDamageCooldown;
	float wallDamageCooldown = 1f;

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
				hasDash = true;
		}
		
		float horizontal = Input.GetAxisRaw("Horizontal");

		if(horizontal < 0 && !psm.stunned)
		{
			sr.flipX = true;
			direction = new Vector3(-1, 0, 0);
		}
		else if(horizontal > 0 && !psm.stunned)
		{
			sr.flipX = false;
			direction = new Vector3(1, 0, 0);
		}

		Vector3 move = new Vector3(horizontal, 0, 0);

		if(!psm.stunned)
		{
			transform.position += move * speed * Time.deltaTime;
		}

		if(Input.GetKeyDown(KeyCode.P))
		{
			psm.Heal(1);
		}

		if((isGrounded || doubleJumpRemaining) && Input.GetButtonDown("Jump") && !psm.stunned)
		{
			if(!isGrounded)
			{
				if(doubleJump)
				{
					rb.velocity = new Vector2(rb.velocity.x, 0);
					doubleJumpRemaining = false;
					rb.AddForce(transform.up * doubleJumpForce, ForceMode2D.Impulse);
				}
			}
			else
			{
				isGrounded = false;
				rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
				
			}
			
		}

		if(hasDash && Input.GetButtonDown("Dash") && !psm.stunned)
		{
			transform.position += direction * dashDistance;
			hasDash = false;
		}

		if(psm.inWall && currentWallDamageCooldown > 0 && !psm.dead)
		{
			currentWallDamageCooldown -= Time.deltaTime;
		}

		if(psm.inWall && currentWallDamageCooldown <= 0 && !psm.dead)
		{
			psm.DamageWithoutKnockback(1);
			currentWallDamageCooldown = wallDamageCooldown;
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

	void OnCollisionStay2D(Collision2D col)
	{
		if(col.gameObject.layer == 3)
		{
			if(col.collider.bounds.Contains(transform.position) && !psm.inWall)
			{
				psm.inWall = true;
				currentWallDamageCooldown = 0;
				psm.stunned = true;
				rb.constraints = RigidbodyConstraints2D.FreezeAll;
			}
		}
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if(col.gameObject.tag == "Enemy")
		{
			Vector2 direction = col.GetContact(0).normal;
			if(direction.y == -1)
			{
				col.gameObject.GetComponent<EnemyStatsManager>().Damage(1);
			}
			else
			{
				EnemyStatsManager esm = col.gameObject.GetComponent<EnemyStatsManager>();
				psm.Damage(esm.damage, col.gameObject.transform, esm.outgoingKnockbackAmount, esm.outgoingKnockbackTime, esm.outgoingStunTime);
			}
		}
	}
}
