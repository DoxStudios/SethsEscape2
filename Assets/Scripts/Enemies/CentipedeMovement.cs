using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class CentipedeMovement : MonoBehaviour
{
    public GameObject groundHitbox;
    public GameObject standingHitbox;
    public Transform knockbackPoint;
    public float detectRange;
    public float followDistance;
    public float slamDistance;
    public float speed;

    SpriteRenderer sr;
    Animator animator;
    GameObject player;
    PlayerStatsManager playerStatsManager;
    Rigidbody2D rb;
    EnemyStatsManager esm;


    bool rising = false;
    bool parry = false;
    bool slam = false;
    bool move = false;

    bool slamDone = false;


    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        esm = GetComponent<EnemyStatsManager>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerStatsManager = player.GetComponent<PlayerStatsManager>();
    }

    // Update is called once per frame
    void Update()
    {

        if(player.transform.position.x < transform.position.x)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }

        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Parry"))
        {
            parry = false;
        }

        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || animator.GetCurrentAnimatorStateInfo(0).IsName("Crawling") || animator.GetCurrentAnimatorStateInfo(0).IsName("Slam"))
        {
            groundHitbox.SetActive(true);
            standingHitbox.SetActive(false);
        }
        else
        {
            groundHitbox.SetActive(false);
            standingHitbox.SetActive(true);
        }

        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Slam"))
        {
            if(!slamDone)
            {
                playerStatsManager.Damage(95, knockbackPoint, 1f, 0.5f, 0.5f);
                slamDone = true;
            }
        }
        else
        {
            slamDone = false;
        }

        float distance = Vector2.Distance(transform.position, player.transform.position);

        if(distance < detectRange && distance > followDistance)
        {
            move = true;
        }
        else
        {
            move = false;
        }

        if(distance < followDistance)
        {
            rising = true;
        }

        if(distance < slamDistance)
        {
            slam = true;
        }
        else
        {
            slam = false;
        }

        if(move)
        {
            Walk();
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        animator.SetBool("Rise", rising);
        animator.SetBool("Parry", parry);
        animator.SetBool("Slam", slam);
        animator.SetBool("Move", move);
    }

    public void Rise()
    {
        rising = true;
    }

    public void Parry()
    {
        parry = true;
        playerStatsManager.Damage(95 * 2, knockbackPoint, 4, 5 ,1);
    }

    void Walk()
    {
       if(player.transform.position.x > transform.position.x)
       {
            rb.velocity = new Vector2(speed, rb.velocity.y);
       }
       else
       {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
       }
    }
}
