using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : MonoBehaviour
{
    public GameObject player;
    public float fireCooldown;
    public EnemyWeapon ew;

    float cooldown;
    Transform target;
    EnemyStatsManager esm;
    EnemyMovement em;

    void Start()
    {
        esm = GetComponent<EnemyStatsManager>();
        em = GetComponent<EnemyMovement>();
        target = player.transform;
    }

    void Update()
    {
        if(esm.state == 0 && Vector2.Distance(transform.position, target.position) <= em.followRadius)
        {
            esm.state = 2;
            cooldown = fireCooldown;
            ew.Fire();
        }

        if(esm.state == 2)
        {
            cooldown -= Time.deltaTime;
            if(cooldown <= 0)
            {
                esm.state = 0;
            }
        }
    }
}
