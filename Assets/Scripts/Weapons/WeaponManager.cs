using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public float damage;
    public int burstCount;
    public float burstOffset;
    public float speed;
    public float knockbackMultiplier;
    public float knockbackTimeMultiplier;
    public float stunTimeMultiplier;
    public float bulletSurvivalTime;
    public int pierceLevel;
    public GameObject bullet;
    public Transform firePosition;
    public float maxShotsPerSecond;
    public float fireFrames;
    public int maxAmmo;
    public int currentAmmo;
    public bool loadOneAtATime;
    public float reloadTime;

    public bool reloadWhileActive = false;
    public int priority;

    public int state = 0;
    
    public GameObject GFX;

    public Vector3 secondaryPosition;
    public Vector3 secondaryFirePosition;

    Vector3 primaryPosition;
    Vector3 primaryFirePosition;
    float currentReloadTime;

    float cooldown;
    float currentFireTime;

    PlayerStatsManager psm;

    void Start()
    {
        currentAmmo = maxAmmo;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        psm = player.GetComponent<PlayerStatsManager>();
        cooldown = 1 / maxShotsPerSecond;

        primaryPosition = GFX.transform.localPosition;
        primaryFirePosition = firePosition.localPosition;
    }

    void Update()
    {

        if (psm.movingLeft)
        {
            GFX.GetComponent<SpriteRenderer>().flipX = true;
            GFX.transform.localPosition = secondaryPosition;
            firePosition.localPosition = secondaryFirePosition;
        }
        else
        {
            GFX.GetComponent<SpriteRenderer>().flipX = false;
            GFX.transform.localPosition = primaryPosition;
            firePosition.localPosition = primaryFirePosition;
        }

        if(state == 0 || state == 6)
        {
            GFX.SetActive(false);
        }

        if(state == 1 || state == 4)
        {
            GFX.SetActive(true);
        }
        
        if(state == 3)
        {
            cooldown -= Time.deltaTime;
            if(cooldown <= 0)
            {
                if(currentAmmo > 0)
                {
                    state = 1;
                }
                else
                {
                    state = 4;
                }
            }
        }

        if(state == 2)
        {
            currentFireTime -= Time.deltaTime;
            if(currentFireTime <= 0)
            {
                state = 3;
            }
        }

        if(reloadWhileActive && Input.GetButtonDown("ReloadHandgun") && state != 0 && state != 6)
        {
            Debug.Log("Reload");
            state = 5;
            currentReloadTime = reloadTime;
        }

        if(state == 5)
        {
            currentReloadTime -= Time.deltaTime;

            if(currentReloadTime <= 0)
            {
                state = 1;
                currentAmmo = maxAmmo;
            }
        }
    }

    public void Fire(float damageMultiplier, int addedPierce)
    {
        if(state != 1) return;
        state = 2;
        currentFireTime = fireFrames;
        currentAmmo -= 1;
        cooldown = 1 / maxShotsPerSecond;

        int bulletsFired = 0;

        while(bulletsFired < burstCount)
        {
            bulletsFired += 1;

            Vector3 mousePos = Input.mousePosition;
            mousePos.z = mousePos.z - (Camera.main.transform.position.z);
            Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
            Vector3 direction = worldMousePos - firePosition.position;
            direction = new Vector3(direction.x + Random.Range(-burstOffset, burstOffset), direction.y + Random.Range(-burstOffset, burstOffset), direction.z);
            direction.Normalize();

            float finalDamage = damage * damageMultiplier;
            int finalPierce = pierceLevel + addedPierce;

            GameObject firedBullet = Instantiate(bullet, firePosition.position, Quaternion.identity);
            firedBullet.GetComponent<Rigidbody2D>().velocity = direction * speed;
            BulletManager bm = firedBullet.GetComponent<BulletManager>();
            bm.damage = finalDamage;
            bm.knockbackTimeMultiplier = knockbackTimeMultiplier;
            bm.knockbackMultiplier = knockbackMultiplier;
            bm.stunTimeMultiplier = stunTimeMultiplier;
            bm.pierceLevel = finalPierce;
            bm.psm = psm;
            bm.survivalTime = bulletSurvivalTime;
        }
    }
}
