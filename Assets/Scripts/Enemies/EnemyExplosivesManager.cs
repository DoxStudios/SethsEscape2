using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplosivesManager : MonoBehaviour
{
    public float damage;
    public float knockbackAmount;
    public float knockbackTime;
    public float stunTime;
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
                psm.PlayerExplosive((damage / (explosionSource - psm.gameObject.transform.position).magnitude) * 20, transform, explosivePower * knockbackAmount);
            }
        }

        for(int i = 0; i < moths.Length; i++)
        {
            GameObject moth = moths[i];
            MothMovement mv = moth.GetComponent<MothMovement>();
            mv.targetOverride = false;
        }

        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player" || col.gameObject.tag == "Ground")
        {
            Explode();
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player" || col.gameObject.tag == "Ground")
        {
            Explode();
        }
    }
}
