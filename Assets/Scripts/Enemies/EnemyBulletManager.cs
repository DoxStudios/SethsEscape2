using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletManager : MonoBehaviour
{
    public int damage;
    public float knockbackMultiplier;
    public float knockbackTimeMultiplier;
    public float stunTimeMultiplier;
    public EnemyStatsManager esm;
    public int pierceLevel;
    public float survivalTime;

    void Update()
    {
        survivalTime -= Time.deltaTime;

        if(survivalTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            PlayerStatsManager psm = col.gameObject.GetComponent<PlayerStatsManager>();
            psm.Damage(damage, transform, esm.outgoingKnockbackAmount * knockbackMultiplier, esm.outgoingKnockbackTime * knockbackTimeMultiplier, esm.outgoingStunTime * stunTimeMultiplier);
        }
        else if(col.gameObject.tag == "Ground")
        {
            Destroy(gameObject);
        }

        pierceLevel -= 1;
        if(pierceLevel == 0)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            PlayerStatsManager psm = col.gameObject.GetComponent<PlayerStatsManager>();
            psm.Damage(damage, transform, esm.outgoingKnockbackAmount * knockbackMultiplier, esm.outgoingKnockbackTime * knockbackTimeMultiplier, esm.outgoingStunTime * stunTimeMultiplier);
        }
        else if(col.gameObject.tag == "Ground")
        {
            Destroy(gameObject);
        }

        pierceLevel -= 1;
        if(pierceLevel == 0)
        {
            Destroy(gameObject);
        }
    }
}
