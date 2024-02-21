using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossExplosivesManager : MonoBehaviour
{
    public float damage;
    public float knockbackAmount;
    public float knockbackTime;
    public float stunTime;
    public float explosivePower;
    public float explosiveRange;
    public GameObject explosion;
    public bool bowling;

    void Update()
    {
    
    }

    public void Explode()
    {
        GameObject explosionObject = Instantiate(explosion, transform.position, Quaternion.identity);
        explosionObject.transform.localScale = new Vector3(explosiveRange*2, explosiveRange*2, 1);

        Vector3 explosionSource = transform.position;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionSource, explosiveRange);

        foreach(Collider2D hit in colliders)
        {
            if(hit.gameObject.tag == "Player")
            {
                PlayerStatsManager psm = hit.gameObject.GetComponent<PlayerStatsManager>();
                psm.PlayerExplosive((damage / (explosionSource - psm.gameObject.transform.position).magnitude) * 2, transform, explosivePower * knockbackAmount);
            }
        }
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(!bowling)
        {
            if(col.gameObject.tag == "Player" || col.gameObject.tag == "Ground" || col.gameObject.tag == "BowlingTag")
            {
                Explode();
            }
        }
        else
        {
            if(col.gameObject.tag == "Player" || col.gameObject.tag == "BowlingTag")
            {
                Explode();
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(!bowling)
        {
            if(col.gameObject.tag == "Player" || col.gameObject.tag == "Ground" || col.gameObject.tag == "BowlingTag")
            {
                Explode();
            }
        }
        else
        {
            if(col.gameObject.tag == "Player" || col.gameObject.tag == "BowlingTag")
            {
                Explode();
            }
        }
    }
}
