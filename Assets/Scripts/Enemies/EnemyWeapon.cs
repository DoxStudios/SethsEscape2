using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{

    public int damage;
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

    public int state = 1;

    float cooldown;
    float currentFireTime;

    PlayerStatsManager psm;
    EnemyStatsManager esm;

    public bool ready = true;

    public Vector3 primaryFirePosition;
    public Vector3 secondaryFirePosition;

    public SpriteRenderer spriteRenderer;

    Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInParent<Animator>();
        psm = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatsManager>();
        esm = GetComponentInParent<EnemyStatsManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(state == 3)
        {
            cooldown -= Time.deltaTime;
            if(cooldown <= 0)
            {
                state = 1;
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

        if(spriteRenderer.flipX)
        {
            firePosition.localPosition = secondaryFirePosition;
        }
        else
        {
            firePosition.localPosition = primaryFirePosition;
        }
    }

    public void Fire()
    {
        if(state != 1) return;
        if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_1")) return;
        state = 2;
        currentFireTime = fireFrames;
        cooldown = 1 / maxShotsPerSecond;

        int bulletsFired = 0;

        while(bulletsFired < burstCount)
        {
            bulletsFired += 1;

            Vector3 direction = psm.gameObject.transform.position - firePosition.position;
            direction = new Vector3(direction.x + Random.Range(-burstOffset, burstOffset), direction.y + Random.Range(-burstOffset, burstOffset), direction.z);
            direction.Normalize();

            GameObject firedBullet = Instantiate(bullet, firePosition.position, Quaternion.identity);
            firedBullet.GetComponent<Rigidbody2D>().velocity = direction * speed;
            EnemyBulletManager bm = firedBullet.GetComponent<EnemyBulletManager>();
            bm.damage = damage;
            bm.knockbackTimeMultiplier = knockbackTimeMultiplier;
            bm.knockbackMultiplier = knockbackMultiplier;
            bm.stunTimeMultiplier = stunTimeMultiplier;
            bm.pierceLevel = pierceLevel;
            bm.esm = esm;
            bm.survivalTime = bulletSurvivalTime;
        }
    }
}
