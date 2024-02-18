using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBulletManager : MonoBehaviour
{
    public float damage;
    public float outgoingKnockbackAmount;
    public float outgoingKnockbackTime;
    public float outgoingStunTime;

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            PlayerStatsManager psm = col.gameObject.GetComponent<PlayerStatsManager>();
            psm.Damage(damage, transform, outgoingKnockbackAmount, outgoingKnockbackTime, outgoingStunTime);
            Destroy(gameObject);
        }
        else if(col.gameObject.tag == "Ground" || col.gameObject.tag == "Door")
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            PlayerStatsManager psm = col.gameObject.GetComponent<PlayerStatsManager>();
            psm.Damage(damage, transform, outgoingKnockbackAmount, outgoingKnockbackTime, outgoingStunTime);
            Destroy(gameObject);
        }
        else if(col.gameObject.tag == "Ground")
        {
            Destroy(gameObject);
        }
    }
}
