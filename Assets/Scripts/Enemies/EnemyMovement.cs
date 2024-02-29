using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyMovement : MonoBehaviour
{
    GameObject player;

    public bool isaacNewton;
    public bool ranged = false;
    public float followRadius;
    public float speed = 3500;
    public float nextWaypointDistance = 3f;

    public float detectRadius;
    public float initialRadius;
    public bool playerDetected;
    public bool initialDetected;
    public bool inRange;
    public EnemyWeapon ew;
    public bool stopInRange;
    public bool isWorm = false;
    public bool isFly = false;
    public bool isJunebug = false;
    public GameObject explosion;
    public float explosiveRange;
    public float damage;
    public float explosivePower;
    public float knockback;
    public float knockbackTime;
    public float stunTime;
    bool juneDashed = false;


    Transform target;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    float groundedRadius = 1f;
    Seeker seeker;
    EnemyStatsManager esm;
    Rigidbody2D rb;
    float distanceToPlayer;
    SpriteRenderer spriteRenderer;

    Animator animator;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        target = player.transform;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        esm = GetComponent<EnemyStatsManager>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);

        if(isaacNewton)
        {
            rb.gravityScale = 10;
        }
        else
        {
            rb.gravityScale = 0;
        }
    }

    void UpdatePath()
    {
        if(seeker.IsDone() && playerDetected)
        seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    void FixedUpdate()
    {

        if(player.transform.position.x < transform.position.x)
        {
            spriteRenderer.flipX = !(isWorm || isFly || esm.isMoth);
        }
        else
        {
            spriteRenderer.flipX = (isWorm || isFly || esm.isMoth);
        }

        playerDetected = ((player.transform.position - transform.position).magnitude < detectRadius);
        initialDetected = ((player.transform.position - transform.position).magnitude < initialRadius);

        if(!isJunebug)
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

                distanceToPlayer = Vector2.Distance(rb.position, target.position);

                if(playerDetected && path != null && !reachedEndOfPath && esm.state == 0)
                {
                    Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
                    Vector2 force = direction * speed * Time.deltaTime;


                    if((animator.GetCurrentAnimatorStateInfo(0).IsName("Walk") || animator.GetCurrentAnimatorStateInfo(0).IsName("Aggressive Walk")) && playerDetected)
                    {
                        rb.AddForce(force);
                    }

                    float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

                    if(distance < nextWaypointDistance)
                    {
                        currentWaypoint++;
                    }
                }

                inRange = distanceToPlayer <= followRadius;

                if(inRange && ranged)
                {
                    if(stopInRange)
                    {
                        if(isWorm)
                        {
                            if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                            {
                                rb.velocity = new Vector2(0, rb.velocity.y);
                            }
                        }
                        else
                        {
                            rb.velocity = new Vector2(0, rb.velocity.y);
                        }
                    }
                    ew.Fire(isFly);
                }
            }
        }
        else if(playerDetected && !juneDashed)
        {
            juneDashed = true;
            rb.velocity = (player.transform.position - transform.position).normalized * speed;
        }
    }

    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(isJunebug)
        {
            Debug.Log("Boom");
            GameObject explosionObject = Instantiate(explosion, transform.position, Quaternion.identity);
            explosionObject.transform.localScale = new Vector3(explosiveRange*2, explosiveRange*2, 1);

            Vector3 explosionSource = transform.position;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionSource, explosiveRange);

            foreach(Collider2D hit in colliders)
            {
                if(hit.gameObject.tag == "Player")
                {
                    PlayerStatsManager psm = hit.gameObject.GetComponent<PlayerStatsManager>();
                    psm.PlayerExplosive((damage / (explosionSource - psm.gameObject.transform.position).magnitude * 20), transform, explosivePower * knockback);
                }
            }

            esm.health = 0;
        }
    }
}