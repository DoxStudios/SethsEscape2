using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public float damage;
    public PlayerStatsManager psm;
    public int pierceLevel;


    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Enemy")
        {
            EnemyStatsManager esm = col.gameObject.GetComponent<EnemyStatsManager>();
            esm.Damage(transform, damage, psm.outgoingKnockbackAmount, psm.outgoingKnockbackTime, psm.outgoingStunTime);
        }

        pierceLevel -= 1;
        if(pierceLevel == 0)
        {
            Destroy(gameObject);
        }
    }
}
