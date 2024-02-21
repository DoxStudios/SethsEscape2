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
    public GameObject chainsawLeapHitbox;
    public DoorManager doorManager;

    BossStatsManager bsm;
    BeetleStateControl bsc;

    List<GameObject> chainsaws = new List<GameObject>();

    public PlayerStatsManager psm;
    Rigidbody2D rb;
    public bool inFight = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bsm = GetComponent<BossStatsManager>();
        bsc = GetComponent<BeetleStateControl>();
        psm = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatsManager>();
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        if(!inFight)
        {
            bsm.health = 1000;
            rb.velocity = Vector2.zero;
            bsm.sr.color = bsm.defaultColor;
        }


        if(psm.health <= 0)
        {
            bsm.health = 1000;
            DeleteChainsaws();
            doorManager.ResetArena();
            inFight = false;
        }

        if(bsm.health <= 0)
        {
            DeleteChainsaws();
            doorManager.Open();
            inFight = false;
        }
    }

    public void PlayerEnter()
    {
        if(!inFight)
        {
            doorManager.Close();


            inFight = true;
        }
    }

    public void ShootBullet(Vector3 target, Transform origin)
    {
        for(int i = 0; i < 5; i++)
        {
            GameObject bulletInstance = Instantiate(bullet, origin.position, Quaternion.identity);
            Vector3 finalTarget = target + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0);
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
        GameObject chainsawInstance =  Instantiate(chainsaw, origin.position, Quaternion.identity);
        Vector2 direction = (target - origin.position).normalized;
        chainsawInstance.GetComponent<Rigidbody2D>().velocity = direction * 100;
        EnemyChainsawDamage ecd = chainsawInstance.GetComponent<EnemyChainsawDamage>();
        EnemyChainsawHealth ech = chainsawInstance.GetComponent<EnemyChainsawHealth>();
        ChainsawMovement chainsawMovement = chainsawInstance.GetComponentInChildren<ChainsawMovement>();

        chainsawMovement.speed = 80f;

        ecd.damage = 25f;
        //ecd.damage = 0f;
        ech.health = 1f;
        ech.boss = true;

        GameObject chainsawObject = chainsawInstance;

        chainsaws.Add( (GameObject) chainsawObject);
    }

    public void ShootBomb(Vector3 target, Transform origin, bool bowling, bool pitch, float speed)
    {
        GameObject bombInstance = Instantiate(bomb, origin.position, Quaternion.identity);
        Vector2 direction = (target - origin.position).normalized;
        bombInstance.GetComponent<Rigidbody2D>().velocity = direction * speed;

        BossExplosivesManager bem = bombInstance.GetComponent<BossExplosivesManager>();
        bem.bowling = bowling;

        if(pitch)
        {
            Rigidbody2D brb = bombInstance.GetComponent<Rigidbody2D>();
            brb.gravityScale = 0;
        }

        if(bowling)
        {
            Rigidbody2D brb = bombInstance.GetComponent<Rigidbody2D>();
            brb.gravityScale = 10;
        }
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

    public void ChainsawLeapHitbox(bool state)
    {
        chainsawLeapHitbox.SetActive(state);
    }

    public bool GetChainsawLeap()
    {
        return chainsawLeapHitbox.activeSelf;
    }

    public void DeleteChainsaws()
    {
        foreach(GameObject chainsaw in chainsaws)
        {
            Destroy(chainsaw);
        }
    }
    
}
