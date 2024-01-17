using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public float damage;
    public float knockbackMultiplier;
    public float knockbackTimeMultiplier;
    public float stunTimeMultiplier;
    public PlayerStatsManager psm;
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
        if(col.gameObject.tag == "Boss")
        {
            col.gameObject.GetComponent<BossStatsManager>().Damage(damage, transform, psm.outgoingKnockbackAmount * knockbackMultiplier);
        }
        if(col.gameObject.tag == "Enemy")
        {
            EnemyStatsManager esm = col.gameObject.GetComponent<EnemyStatsManager>();
            esm.Damage(transform, damage, psm.outgoingKnockbackAmount * knockbackMultiplier, psm.outgoingKnockbackTime * knockbackTimeMultiplier, psm.outgoingStunTime * stunTimeMultiplier);
        }
        else if(col.gameObject.tag == "Ground")
        {
            Destroy(gameObject);
        }

        if(col.gameObject.tag == "EnemyChainsaw")
        {
            col.gameObject.GetComponent<EnemyChainsawHealth>().Damage(damage);
        }

        pierceLevel -= 1;
        if(pierceLevel == 0)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Boss")
        {
            col.gameObject.GetComponent<BossStatsManager>().Damage(damage, transform, psm.outgoingKnockbackAmount * knockbackMultiplier);
        }
        if(col.gameObject.tag == "Enemy")
        {
            EnemyStatsManager esm = col.gameObject.GetComponent<EnemyStatsManager>();
            esm.Damage(transform, damage, psm.outgoingKnockbackAmount * knockbackMultiplier, psm.outgoingKnockbackTime * knockbackTimeMultiplier, psm.outgoingStunTime * stunTimeMultiplier);
        }
        else if(col.gameObject.tag == "Ground")
        {
            Destroy(gameObject);
        }

        if(col.gameObject.tag == "EnemyChainsaw")
        {
            col.gameObject.GetComponent<EnemyChainsawHealth>().Damage(damage);
        }

        pierceLevel -= 1;
        if(pierceLevel == 0)
        {
            Destroy(gameObject);
        }

    }
}
