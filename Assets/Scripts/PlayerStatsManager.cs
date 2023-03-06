using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStatsManager : MonoBehaviour
{
	public GameObject heartPrefab;
	public bool stunned = false;
	public bool dead = false;
	public bool inWall = false;
	public int health = 3;
	int startingHealth = 3;
	GameObject canvas;
	int prevHealth;
	Rigidbody2D rb;
	int heartOffset;
	int heartOffsetIntervals = 40;
	Transform sprite;
	Transform lastCheckpoint;

	public void Heal(int amount)
	{
		health += amount;
	}

	public void Damage(int amount, Transform knockbackPosition, float knockbackAmount, float knockbackTime, float stunTime)
	{
		if(!dead)
		{
			health -= amount;
			DealKnockback(knockbackPosition, knockbackAmount, knockbackTime, stunTime);
		}
	}

	public void DamageWithoutKnockback(int amount)
	{
		if(!dead)
		{
			health -= amount;
		}
	}

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		canvas = GameObject.FindGameObjectsWithTag("Canvas")[0];
		lastCheckpoint = GameObject.FindGameObjectsWithTag("Spawn")[0].transform;
		sprite = transform.Find("sprite");
	}

	void Update()
	{
		if(health != prevHealth)
		{
			GameObject[] hearts = GameObject.FindGameObjectsWithTag("Heart");
			heartOffset = 0;

			for(int i=0; i<hearts.Length; i++)
			{
				Destroy(hearts[i]);
			}

			for(int i=0; i<health; i++)
			{
				GameObject heart = Instantiate(heartPrefab, canvas.transform);
				heart.GetComponent<RectTransform>().anchoredPosition = new Vector3(50, -50, -1);
				heart.GetComponent<RectTransform>().anchoredPosition += new Vector2(heartOffset, 0);
				heartOffset += heartOffsetIntervals;
			}

			if(health <= 0)
			{
				dead = true;
				stunned = true;
				rb.gravityScale = 0f;
			}
		}
		prevHealth = health;

		if(dead)
		{
			sprite.Rotate(Vector3.forward * 60 * Time.deltaTime);
			sprite.localScale += new Vector3(-10, -10, 0) * Time.deltaTime;

			if(sprite.localScale.x < -22)
			{
				transform.position = lastCheckpoint.position;
				sprite.rotation = Quaternion.Euler(0, 0, 0);
				sprite.localScale = new Vector3(1, 1, 1);
				dead = false;
				stunned = false;
				health = startingHealth;
				rb.constraints = RigidbodyConstraints2D.FreezeRotation;
				rb.gravityScale = 10f;
				inWall = false;
			}
		}
	}

	public void DealKnockback(Transform sender, float knockbackAmount, float knockbackTime, float stunTime)
	{
		StopAllCoroutines();
		Vector2 direction = (transform.position-sender.position).normalized;
		rb.AddForce(direction*knockbackAmount*100, ForceMode2D.Impulse);
		stunned = true;
		StartCoroutine(ResetKnockback(knockbackTime));
		StartCoroutine(ResetStun(stunTime));
	}

	private IEnumerator ResetKnockback(float delay)
	{
		yield return new WaitForSeconds(delay);
		Vector3 minimizedVelocity = new Vector2(rb.velocity.x/5, rb.velocity.y/5);
		rb.velocity = minimizedVelocity;
	}

	private IEnumerator ResetStun(float delay)
	{
		yield return new WaitForSeconds(delay);
		if(!dead)
		{
			stunned = false;
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.tag == "Checkpoint" || col.gameObject.tag == "Spawn")
		{
			lastCheckpoint = col.gameObject.transform;
		}
	}
}