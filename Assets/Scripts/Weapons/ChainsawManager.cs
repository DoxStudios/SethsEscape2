using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainsawManager : MonoBehaviour
{

    public PlayerStatsManager psm;
    public float damage;
    public float speed;
    public direction currentDirection = direction.right;
    Rigidbody2D rb;

    public enum direction {
        up,
        down,
        left,
        right
    };

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentDirection == direction.up)
        {
            rb.velocity = new Vector2(0, speed);
        }
        else if(currentDirection == direction.down)
        {
            rb.velocity = new Vector2(0, -speed);
        }
        else if(currentDirection == direction.left)
        {
            rb.velocity = new Vector2(-speed, 0);
        }
        else if(currentDirection == direction.right)
        {
            rb.velocity = new Vector2(speed, 0);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Enemy")
        {
            EnemyStatsManager esm = col.gameObject.GetComponent<EnemyStatsManager>();
            esm.Damage(transform, damage, psm.outgoingKnockbackAmount, psm.outgoingKnockbackTime, psm.outgoingStunTime);
        }
        else if(col.gameObject.tag == "Ground")
        {
            rb.gravityScale = 0;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Enemy")
        {
            EnemyStatsManager esm = col.gameObject.GetComponent<EnemyStatsManager>();
            esm.Damage(transform, damage, psm.outgoingKnockbackAmount, psm.outgoingKnockbackTime, psm.outgoingStunTime);
        }
        else if(col.gameObject.tag == "Ground")
        {
            rb.gravityScale = 0;
        }
    }
}
