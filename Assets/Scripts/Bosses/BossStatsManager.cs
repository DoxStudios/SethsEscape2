using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStatsManager : MonoBehaviour
{

    public float defense;
    public float health;
    public float damage;
    public Color defaultColor;
    public float outgoingKnockbackAmount;
	public float outgoingKnockbackTime;
	public float outgoingStunTime;

    Rigidbody2D rb;
    public SpriteRenderer sr;

    public bool isBeetle = false;
    BeetleUtils beetleUtils;
    BeetleStateControl beetleStateControl;
    Transform bossSpawn;
    PlayerStatsManager psm;

    public bool stun = false;

    // Start is called before the first frame update
    void Start()
    {
        psm = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatsManager>();
        bossSpawn = GameObject.FindGameObjectWithTag("BossSpawn").transform;
        transform.position = bossSpawn.position;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        if(sr == null)
        {
            Debug.Log("Sprite renderer not found");
        }

        if(isBeetle)
        {
            beetleUtils = GetComponent<BeetleUtils>();
            beetleStateControl = GetComponent<BeetleStateControl>();
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(psm.health <= 0)
        {
            transform.position = bossSpawn.position;
        }

        if(health <= 0)
        {
            death();
        }
    }

    public void PlayerEnter()
    {
        if(isBeetle)
        {
            beetleUtils.PlayerEnter();
        }
    }

    public void Stun()
    {
        if(isBeetle)
        {
            beetleStateControl.Stun();
        }
    }

    public void Damage(float amount, Transform source, float knockbackAmount)
    {
        health -= amount * defense;
        StartColorFlash(new Color(1, 0, 0), 0.1f);
    }

    void death()
    {
        stun = true;
        Destroy(gameObject);
    }

    public void Reset()
    {
        
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
}
