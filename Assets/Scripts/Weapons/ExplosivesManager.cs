using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosivesManager : MonoBehaviour
{
    public float damage;
    public float knockbackMultiplier;
    public float knockbackTimeMultiplier;
    public float stunTimeMultiplier;
    public PlayerStatsManager psm;
    public int pierceLevel;
    public float survivalTime;
    public float explosivePower;
    public float explosiveRange;
    public GameObject explosion;
    GameObject[] moths;

    void Awake()
    {
        moths = GameObject.FindGameObjectsWithTag("Moth");
        for(int i = 0; i <moths.Length; i++)
        {
            moths[i] = moths[i].transform.parent.gameObject;
        }

        for(int i = 0; i < moths.Length; i++)
        {
            GameObject moth = moths[i];
            if((moth.transform.position - transform.position).magnitude < 100)
            {
                MothMovement mv = moth.GetComponent<MothMovement>();
                mv.targetOverride = true;
                mv.targetOverrideTransform = transform;
            }
        }
    }

    void Update()
    {
        survivalTime -= Time.deltaTime;

        if(survivalTime <= 0)
        {
            for(int i = 0; i < moths.Length; i++)
            {
                GameObject moth = moths[i];
                MothMovement mv = moth.GetComponent<MothMovement>();
                mv.targetOverride = false;
            }
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {

        if(col.gameObject.tag == "BulletDetect")
        {
            return;
        }

        for(int i = 0; i < moths.Length; i++)
        {
            GameObject moth = moths[i];
            MothMovement mv = moth.GetComponent<MothMovement>();
            mv.targetOverride = false;
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

        GameObject explosionObject = Instantiate(explosion, transform.position, Quaternion.identity);
        explosionObject.transform.localScale = new Vector3(explosiveRange*2, explosiveRange*2, 1);

        Vector3 explosionSource = transform.position;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionSource, explosiveRange);

        foreach(Collider2D hit in colliders)
        {
            if(hit.gameObject.tag == "Enemy")
            {
                EnemyStatsManager esm = hit.gameObject.GetComponent<EnemyStatsManager>();
                esm.Damage(transform, (damage / (explosionSource - esm.gameObject.transform.position).magnitude) * 20, explosivePower * knockbackMultiplier / (explosionSource - esm.gameObject.transform.position).magnitude, psm.outgoingKnockbackTime * knockbackTimeMultiplier, psm.outgoingStunTime * stunTimeMultiplier);
            }

            if(hit.gameObject.tag == "Player")
            {
                PlayerStatsManager psm = hit.gameObject.GetComponent<PlayerStatsManager>();
                psm.PlayerExplosive((damage / (explosionSource - psm.gameObject.transform.position).magnitude), transform, explosivePower * knockbackMultiplier);
            }
        }

        pierceLevel -= 1;
        if(pierceLevel == 0)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {

         if(col.gameObject.tag == "BulletDetect")
        {
            return;
        }
        
        for(int i = 0; i < moths.Length; i++)
        {
            GameObject moth = moths[i];
            MothMovement mv = moth.GetComponent<MothMovement>();
            mv.targetOverride = false;
        }

        if(col.gameObject.tag == "Enemy")
        {
            EnemyStatsManager esm = col.gameObject.GetComponent<EnemyStatsManager>();
            esm.Damage(transform, damage, psm.outgoingKnockbackAmount * knockbackMultiplier, psm.outgoingKnockbackTime * knockbackTimeMultiplier, psm.outgoingStunTime * stunTimeMultiplier);
        }
        else if(col.gameObject.tag == "Boss")
        {
            BossStatsManager bsm = col.gameObject.GetComponent<BossStatsManager>();
            bsm.Damage(damage, transform, psm.outgoingKnockbackAmount * knockbackMultiplier);
        }
        else if(col.gameObject.tag == "Ground")
        {
            Destroy(gameObject);
        }

        GameObject explosionObject = Instantiate(explosion, transform.position, Quaternion.identity);
        explosionObject.transform.localScale = new Vector3(explosiveRange*2, explosiveRange*2, 1);

        Vector3 explosionSource = transform.position;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionSource, explosiveRange);

        foreach(Collider2D hit in colliders)
        {
            if(hit.gameObject.tag == "Enemy")
            {
                EnemyStatsManager esm = hit.gameObject.GetComponent<EnemyStatsManager>();
                esm.Damage(transform, (damage / (explosionSource - esm.gameObject.transform.position).magnitude) * 20, explosivePower * knockbackMultiplier / (explosionSource - esm.gameObject.transform.position).magnitude, psm.outgoingKnockbackTime * knockbackTimeMultiplier, psm.outgoingStunTime * stunTimeMultiplier);
            }

            if(col.gameObject.tag == "Boss")
            {
                BossStatsManager bsm = col.gameObject.GetComponent<BossStatsManager>();
                bsm.Damage((damage / (explosionSource - bsm.gameObject.transform.position).magnitude) * 20, transform, psm.outgoingKnockbackAmount * knockbackMultiplier);
            }

            if(hit.gameObject.tag == "Player")
            {
                PlayerStatsManager psm = hit.gameObject.GetComponent<PlayerStatsManager>();
                psm.PlayerExplosive((damage / (explosionSource - psm.gameObject.transform.position).magnitude), transform, explosivePower * knockbackMultiplier);
            }
        }

        pierceLevel -= 1;
        if(pierceLevel == 0)
        {
            Destroy(gameObject);
        }
    }
}
