using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{

    public float damage;
    public float speed;
    public int pierceLevel;
    public GameObject bullet;
    public GameObject firePosition;

    PlayerStatsManager psm;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectsWithTag("Player")[0];
        psm = player.GetComponent<PlayerStatsManager>();
    }

    public void Fire(Vector2 direction, float damageMultiplier, int addedPierce)
    {
        float finalDamage = damage * damageMultiplier;
        int finalPierce = pierceLevel + addedPierce;
        GameObject firedBullet = Instantiate(bullet, firePosition.transform.position, Quaternion.identity);
        Debug.Log(direction * speed);
        firedBullet.GetComponent<Rigidbody2D>().velocity = direction * speed;
        BulletManager bm = firedBullet.GetComponent<BulletManager>();
        bm.damage = finalDamage;
        bm.pierceLevel = finalPierce;
        bm.psm = psm;
    }
}
