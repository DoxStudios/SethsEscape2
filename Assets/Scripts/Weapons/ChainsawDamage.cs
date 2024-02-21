using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainsawDamage : MonoBehaviour
{
    public float damage;
    public PlayerStatsManager psm;

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Enemy")
        {
            EnemyStatsManager esm = col.gameObject.GetComponent<EnemyStatsManager>();
            esm.Damage(transform, damage, psm.outgoingKnockbackAmount, psm.outgoingKnockbackTime, psm.outgoingStunTime);
        }

        if(col.gameObject.tag == "Boss")
        {
            col.gameObject.GetComponent<BossStatsManager>().Damage(damage, transform, psm.outgoingKnockbackAmount);
        }

        if(col.gameObject.tag == "EnemyChainsaw")
        {
            col.gameObject.GetComponent<EnemyChainsawHealth>().Damage(0.5f);
        }
    }
}
