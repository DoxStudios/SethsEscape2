using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    float damage;
    PlayerStatsManager psm;

    int pierceLevel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
            Destroy(this);
        }
    }
}
