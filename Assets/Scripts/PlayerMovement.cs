using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float speed = 45f;
	public float jumpForce = 60f;
	public float doubleJumpForce = 45f;
	public float dashDistance = 10f;
	public bool doubleJump = true;
	public bool dashUnlocked = true;
	public LayerMask ground;
	public SpriteRenderer sr;
	public GameObject groundCheck;
	public GameObject firstParticleGO;
	public GameObject secondParticleGO;
	public float coyoteTime = 0.1f;
	public float bashThreshold;
	public float standardDashTime;
	public float standardDashMultiplier;
	public string dashType;

	Rigidbody2D rb;
	PlayerStatsManager psm;
	float groundedRadius = 1f;
	bool isGrounded = true;
	bool doubleJumpRemaining = true;
	Vector3 direction = new Vector3(1, 0, 0);
	bool hasDash = true;
	Vector3 dashTarget;
	float currentWallDamageCooldown;
	float wallDamageCooldown = 0.5f;
	ParticleSystem firstParticleSYS;
	ParticleSystem secondParticleSYS;
	float horizontal;
	bool inCoyotetime = false;
	float currentCoyotetime;
	bool inDash = false;
	bool dashRemoveControl = false;
	float dashTimer;
	bool meetsBashReqs = false;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		psm = GetComponent<PlayerStatsManager>();
		firstParticleSYS = firstParticleGO.GetComponent<ParticleSystem>();
		secondParticleSYS = secondParticleGO.GetComponent<ParticleSystem>();
	}

	void Update()
	{
		isGrounded = checkGrounded();
		if(isGrounded)
		{
			doubleJumpRemaining = true;
			hasDash = true;
		}

		if(!dashRemoveControl)
		{
			horizontal = Input.GetAxisRaw("Horizontal");
		}

		if(horizontal < 0 && !psm.stunned)
		{
			if(!psm.movingLeft)
			{
				psm.setLeft();
			}
			sr.flipX = true;
			direction = new Vector3(-1, 0, 0);
		}
		else if(horizontal > 0 && !psm.stunned)
		{
			if(psm.movingLeft)
			{
				psm.setRight();
			}
			sr.flipX = false;
			direction = new Vector3(1, 0, 0);
		}

		if(!psm.stunned && !dashRemoveControl)
		{
			rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
		}

		if(dashUnlocked && hasDash && Input.GetButtonDown("Dash") && !psm.stunned)
		{
			if(dashType == "teleport")
			{
				firstParticleGO.transform.position = transform.position;
				firstParticleSYS.Play();
				transform.position += direction * dashDistance;
				secondParticleGO.transform.position = transform.position;
				secondParticleSYS.Play();
				hasDash = false;
			}
			else if(dashType == "standard")
			{
				inDash = true;
				dashRemoveControl = true;
				dashTimer = standardDashTime;
				rb.gravityScale = 0;
				hasDash = false;
			}
		}

		if(inDash)
		{
			if(dashType == "standard")
			{
				if(psm.movingLeft)
				{
					rb.velocity = new Vector2(-1 * (speed * standardDashMultiplier), rb.velocity.y);
				}
				else
				{
					rb.velocity = new Vector2(1 * (speed * standardDashMultiplier), rb.velocity.y);
				}

				dashTimer -= Time.deltaTime;

				if(dashTimer <= 0)
				{
					inDash = false;
					dashRemoveControl = false;
					rb.gravityScale = 10;
				}
			}
		}

		if(Input.GetKeyDown(KeyCode.P))
		{
			psm.Heal(1);
		}

		if(((isGrounded || inCoyotetime) || doubleJumpRemaining) && Input.GetButtonDown("Jump") && !psm.stunned)
		{
			if(!isGrounded && !inCoyotetime)
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

		if(psm.inWall && currentWallDamageCooldown > 0 && !psm.dead)
		{
			currentWallDamageCooldown -= Time.deltaTime;
		}

		if(psm.inWall && currentWallDamageCooldown <= 0 && !psm.dead)
		{
			psm.DamageWithoutKnockback(1);
			currentWallDamageCooldown = wallDamageCooldown;
		}

		if(inCoyotetime)
		{
			currentCoyotetime -= Time.deltaTime;
			if(currentCoyotetime <= 0)
			{
				inCoyotetime = false;
			}
		}

		meetsBashReqs = rb.velocity.magnitude > bashThreshold;
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

	void OnCollisionExit2D(Collision2D col)
	{
		if(col.gameObject.layer == LayerMask.NameToLayer("Ground"))
		{
			inCoyotetime = true;
			currentCoyotetime = coyoteTime;
		}
	}

	void OnCollisionStay2D(Collision2D col)
	{
		if(col.gameObject.layer == LayerMask.NameToLayer("Ground"))
		{
			Tilemap tilemap = col.gameObject.GetComponent<Tilemap>();
			if(tilemap.HasTile(tilemap.WorldToCell(transform.position)) && !psm.inWall)
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
				if(meetsBashReqs)
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
}
