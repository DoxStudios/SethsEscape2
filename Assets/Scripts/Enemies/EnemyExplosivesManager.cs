using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplosivesManager : MonoBehaviour
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
        if(col.gameObject.tag == "Enemy")
        {
            EnemyStatsManager esm = col.gameObject.GetComponent<EnemyStatsManager>();
            esm.Damage(transform, damage, psm.outgoingKnockbackAmount * knockbackMultiplier, psm.outgoingKnockbackTime * knockbackTimeMultiplier, psm.outgoingStunTime * stunTimeMultiplier);
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
}
