using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatsManager : MonoBehaviour
{
    public float health = 1;
	float maxHealth;
    public int damage = 1;
    public float outgoingKnockbackAmount;
    public float outgoingKnockbackTime;
    public float outgoingStunTime;
	public SpriteRenderer sr;
	public int state = 0;

	Color defaultColor;
	Rigidbody2D rb;
    bool stunned = false;
	PlayerStatsManager psm;

    public void Heal(float amount)
	{
		health += amount;
	}

	public void Damage(Transform sender, float amount, float knockbackAmount, float knockbackTime, float stunTime)
	{
		health -= amount;
		StopAllCoroutines();
		Vector2 direction = (transform.position - sender.position).normalized;
		rb.AddForce(direction * knockbackAmount * 100, ForceMode2D.Impulse);
		state = 1;
		StartCoroutine(ResetKnockback(knockbackTime));
		StartCoroutine(ResetStun(stunTime));
		StartColorFlash(new Color(1, 0, 0), 0.1f);
	}

    private void Start()
    {
		rb = GetComponent<Rigidbody2D>();
		psm = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatsManager>();
		defaultColor = sr.color;
		maxHealth = health;
    }

    void Update()
	{
		if(health <= 0)
		{
			bool shouldDrop = psm.dropWeapon();
			if(shouldDrop)
			{
				if(GetComponent<EnemyMovement>().ranged)
				{
					GetComponent<WeaponDrop>().DropWeapon();
				}

			}
			psm.Heal(maxHealth * 0.1f);
			Destroy(gameObject);
		}
	}

	void StartColorFlash(Color tempColor, float flashLength)
    {
		StartCoroutine(ResetColor(flashLength));
		sr.color = tempColor;
    }

	private IEnumerator ResetColor(float delay)
    {
		yield return new WaitForSeconds(delay);
		sr.color = defaultColor;
	}

	private IEnumerator ResetKnockback(float delay)
	{
		yield return new WaitForSeconds(delay);
		Vector3 minimizedVelocity = new Vector2(rb.velocity.x / 5, rb.velocity.y / 5);
		rb.velocity = minimizedVelocity;
	}

	private IEnumerator ResetStun(float delay)
	{
		yield return new WaitForSeconds(delay);
		state = 0;
	}
}
