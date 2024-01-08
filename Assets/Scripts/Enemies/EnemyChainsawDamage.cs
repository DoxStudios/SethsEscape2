using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChainsawDamage : MonoBehaviour
{
    public float damage;

    void OnTriggerStay2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            PlayerStatsManager psm = col.gameObject.GetComponent<PlayerStatsManager>();
            psm.DamageWithoutKnockback(damage * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            PlayerStatsManager psm = col.gameObject.GetComponent<PlayerStatsManager>();
            psm.DamageWithoutKnockback(damage);
        }
    }
}
