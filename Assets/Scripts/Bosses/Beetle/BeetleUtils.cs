using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeetleUtils : MonoBehaviour
{

    public GameObject bullet;
    public GameObject chainsaw;
    public GameObject bomb;
    public GameObject spinHitbox;
    public GameObject throwHitbox;

    BossStatsManager bsm;
    BeetleStateControl bsc;

    // Start is called before the first frame update
    void Start()
    {
        bsm = GetComponent<BossStatsManager>();
        bsc = GetComponent<BeetleStateControl>();
    }

    public void ShootBullet(Vector3 target, Transform origin)
    {
        for(int i = 0; i < 5; i++)
        {
            GameObject bulletInstance = Instantiate(bullet, origin.position, Quaternion.identity);
            Vector3 finalTarget = target + new Vector3(Random.Range(-6f, 6f), Random.Range(-6f, 6f), 0);
            Vector2 direction = (finalTarget - origin.position).normalized;
            bulletInstance.GetComponent<Rigidbody2D>().velocity = direction * 100;
            BossBulletManager bbm = bulletInstance.GetComponent<BossBulletManager>();
            bbm.damage = bsm.damage;
            bbm.outgoingKnockbackAmount = bsm.outgoingKnockbackAmount;
            bbm.outgoingKnockbackTime = bsm.outgoingKnockbackTime;
            bbm.outgoingStunTime = bsm.outgoingStunTime;
        }
    }

    public void ShootChainsaw(Vector3 target, Transform origin)
    {
        GameObject chainsawInstance = Instantiate(chainsaw, origin.position, Quaternion.identity);
        Vector2 direction = (target - origin.position).normalized;
        chainsawInstance.GetComponent<Rigidbody2D>().velocity = direction * 100;
        EnemyChainsawDamage ecd = chainsawInstance.GetComponent<EnemyChainsawDamage>();
        EnemyChainsawHealth ech = chainsawInstance.GetComponent<EnemyChainsawHealth>();
        ChainsawMovement chainsawMovement = chainsawInstance.GetComponentInChildren<ChainsawMovement>();

        chainsawMovement.speed = 80f;

        ecd.damage = 25f;
        ech.health = 1f;
    }

    public void ChainsawHitbox(bool state)
    {
        spinHitbox.SetActive(state);
    }

    public void ChainsawThrowHitbox(bool state)
    {
        throwHitbox.SetActive(state);
    }

    public void ShootBomb(Transform target, Transform origin)
    {
        GameObject bombInstance = Instantiate(bomb, origin.position, Quaternion.identity);
        Vector2 direction = (target.position - origin.position).normalized;
        bombInstance.GetComponent<Rigidbody2D>().velocity = direction * 10;
    }
    
}
