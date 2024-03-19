using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeetleMovement : MonoBehaviour
{
    public float speed;
    public GameObject chainsaw;

    Animator animator;
    GameObject player;
    Rigidbody2D rb;
    EnemyWeapon enemyWeapon;
    List<GameObject> chainsaws = new List<GameObject>();
    bool thrown = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        enemyWeapon = GetComponentInChildren<EnemyWeapon>();
    }

    // Update is called once per frame
    void Update()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Hunt"))
        {
            if(player.transform.position.x < transform.position.x)
            {
                rb.velocity = new Vector2(-speed, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(speed, rb.velocity.y);
            }
        }

        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            if(player.transform.position.x < transform.position.x)
            {
                rb.velocity = new Vector2(speed, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(-speed, rb.velocity.y);
            }
        }

        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_1"))
        {
            enemyWeapon.Fire();
        }

        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Throw") && !thrown)
        {
            thrown = true;

            GameObject chainsawInstance =  Instantiate(chainsaw, transform.position, Quaternion.identity);
            Vector2 direction = (player.transform.position - transform.position).normalized;
            chainsawInstance.GetComponent<Rigidbody2D>().velocity = direction * 100;
            EnemyChainsawDamage ecd = chainsawInstance.GetComponent<EnemyChainsawDamage>();
            EnemyChainsawHealth ech = chainsawInstance.GetComponent<EnemyChainsawHealth>();
            ChainsawMovement chainsawMovement = chainsawInstance.GetComponentInChildren<ChainsawMovement>();

            chainsawMovement.speed = 80f;

            ecd.damage = 25f;
            //ecd.damage = 0f;
            ech.health = 1f;
            ech.boss = false;

            GameObject chainsawObject = chainsawInstance;

            chainsaws.Add( (GameObject) chainsawObject);
        }

        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Draw"))
        {
            thrown = false;
        }

        if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Walk") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Hunt"))
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    public void KillChainsaws()
    {
        foreach(GameObject chainsaw in chainsaws)
        {
            Destroy(chainsaw);
        }
    }
}
