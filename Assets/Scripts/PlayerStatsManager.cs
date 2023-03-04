using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStatsManager : MonoBehaviour
{
	public GameObject heartPrefab;
	public bool stunned = false;
	public bool dead;
	public int health = 3;
	GameObject canvas;
	int prevHealth;
	Rigidbody2D rb;
	int heartOffset;
	int heartOffsetIntervals = 40;

	public void Heal(int amount)
	{
		health += amount;
	}

	public void Damage(int amount, Transform knockbackPosition, float knockbackAmount, float knockbackTime, float stunTime)
	{
		health -= amount;
		DealKnockback(knockbackPosition, knockbackAmount, knockbackTime, stunTime);   
	}

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		canvas = GameObject.FindGameObjectsWithTag("Canvas")[0];
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
				heart.GetComponent<RectTransform>().anchoredPosition = new Vector3(-500, 200, 0);
				heart.GetComponent<RectTransform>().anchoredPosition += new Vector2(heartOffset, 0);
				heartOffset += heartOffsetIntervals;
			}

			if(health <= 0)
			{
				dead = true;
			}
		}

		prevHealth = health;
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
		stunned = false;
	}
}
