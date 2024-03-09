using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class SpiderMovement : MonoBehaviour
{

    public bool stealth = true;
    public float speed;
    public float exitStealthRange;
    public float followRange;
    public float attackRange;
    public float leapRange;
    public float timeBetweenAttacks;
    public LayerMask ground;
    public GameObject groundCheck;

    Path path;
    float groundedRadius = 5f;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    float nextWaypointDistance = 3f;
    Seeker seeker;
    GameObject player;
    PlayerStatsManager psm;
    Rigidbody2D rb;
    EnemyStatsManager esm;
    Animator animator;
    SpriteRenderer spriteRenderer;
    int next;
    float timeSinceAttack;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        psm = player.GetComponent<PlayerStatsManager>();
        esm = GetComponent<EnemyStatsManager>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        seeker = GetComponent<Seeker>();
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }


    void UpdatePath()
    {
        if(seeker.IsDone() && (player.transform.position - transform.position).magnitude < followRange)
        seeker.StartPath(rb.position, player.transform.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }


    // Update is called once per frame
    void Update()
    {
        float distance = (player.transform.position - transform.position).magnitude;
        
        if(!stealth)
        {
            rb.gravityScale = 9;
            transform.rotation = Quaternion.Euler(0, 0, 0);

            next = 0;
            if(distance < followRange)
            {
                next = 1;

                if(distance > leapRange)
                {
                    Leap();
                }
                else
                {
                    Walk();
                }
            }
            if(distance < attackRange && player.transform.position.y - transform.position.y > -1)
            {
                esm.contactImmunity = true;
                next = 2;
                if(timeSinceAttack <= 0)
                {
                    psm.Damage(esm.damage, transform, esm.outgoingKnockbackAmount, esm.outgoingKnockbackTime, esm.outgoingStunTime);
                    timeSinceAttack = timeBetweenAttacks;
                }
            }
            else
            {
                esm.contactImmunity = false;
            }

            if(timeSinceAttack > 0)
            {
                timeSinceAttack -= Time.deltaTime;
            }

            animator.SetInteger("Next", next);

            if(player.transform.position.x < transform.position.x)
            {
                spriteRenderer.flipX = true;
            }
            else
            {
                spriteRenderer.flipX = false;
            }
        }
        else
        {
            rb.gravityScale = 0;
            if(distance < exitStealthRange)
            {
                stealth = false;
                Leap(true);
            }
        }
    }

    void Leap(bool bypass=false)
    {
        if(Grounded() || bypass)
        {
            if(player.transform.position.x > transform.position.x)
            {
                if(bypass && !Grounded())
                {
                    rb.AddForce(new Vector2(1, -2) * 50);
                }
                else
                {
                    rb.AddForce(new Vector2(1, 2) * 50);
                }
            }
            else
            {
                if(bypass && !Grounded())
                {
                    rb.AddForce(new Vector2(-1, -2) * 50);
                }
                else
                {
                    rb.AddForce(new Vector2(-1, 2) * 50);
                }
            }
        }
        else
        {
            Walk();
        }
    }

    void Walk()
    {
        if(path != null)
        {
            if(currentWaypoint >= path.vectorPath.Count)
            {
                reachedEndOfPath = true;
            }
            else
            {
                reachedEndOfPath = false;
            }

            float distanceToPlayer = Vector2.Distance(rb.position, player.transform.position);

            if(distanceToPlayer < followRange && path != null && !reachedEndOfPath && esm.state == 0)
            {
                Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
                Vector2 force = direction * speed * Time.deltaTime;
                force.y = 0;

                rb.AddForce(force);

                float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

                if(distance < nextWaypointDistance)
                {
                    currentWaypoint++;
                }
            }
        }
    }

    bool Grounded()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.transform.position, groundedRadius, ground);
        if(colliders.Length > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
