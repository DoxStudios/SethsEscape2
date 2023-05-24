using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{

    public float damage;
    public float speed;
    public float bulletSurvivalTime;
    public int pierceLevel;
    public GameObject bullet;
    public Transform firePosition;

    PlayerStatsManager psm;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        psm = player.GetComponent<PlayerStatsManager>();
    }

    public void Fire(float damageMultiplier, int addedPierce)
    {
        Vector3 mousePos = Input.mousePosition;
		mousePos.z = mousePos.z - (Camera.main.transform.position.z);
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3 direction = worldMousePos - firePosition.position;
        direction.Normalize();

        float finalDamage = damage * damageMultiplier;
        int finalPierce = pierceLevel + addedPierce;
        
        GameObject firedBullet = Instantiate(bullet, firePosition.position, Quaternion.identity);
        firedBullet.GetComponent<Rigidbody2D>().velocity = direction * speed;
        BulletManager bm = firedBullet.GetComponent<BulletManager>();
        bm.damage = finalDamage;
        bm.pierceLevel = finalPierce;
        bm.psm = psm;
        bm.survivalTime = bulletSurvivalTime;
    }
}
