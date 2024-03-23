using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainsawDamage : MonoBehaviour
{
    public float initDmg;
    public float contDmg;
    public PlayerStatsManager psm;

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Enemy")
        {
            EnemyStatsManager esm = col.gameObject.GetComponent<EnemyStatsManager>();
            esm.Damage(transform, initDmg, psm.outgoingKnockbackAmount, psm.outgoingKnockbackTime, psm.outgoingStunTime);
        }

        if(col.gameObject.tag == "Boss")
        {
            col.gameObject.GetComponent<BossStatsManager>().Damage(initDmg, transform, psm.outgoingKnockbackAmount);
        }

        if(col.gameObject.tag == "EnemyChainsaw")
        {
            col.gameObject.GetComponent<EnemyChainsawHealth>().Damage(0.5f);
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if(col.gameObject.tag == "Enemy")
        {
            EnemyStatsManager esm = col.gameObject.GetComponent<EnemyStatsManager>();
            esm.Damage(transform, contDmg * Time.deltaTime, psm.outgoingKnockbackAmount, psm.outgoingKnockbackTime, psm.outgoingStunTime);
        }

        if(col.gameObject.tag == "Boss")
        {
            col.gameObject.GetComponent<BossStatsManager>().Damage(contDmg * Time.deltaTime, transform, psm.outgoingKnockbackAmount);
        }
    }
}
