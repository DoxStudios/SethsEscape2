using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.Tilemaps;

public class MothMovement : MonoBehaviour
{
    public float speed = 100f;
    public Vector3 target;
    public bool targetOverride = false;
    public Transform targetOverrideTransform;
    public float detectionRange = 500f;
    public float attackRange = 75f;
    public float nextWaypointDistance = 3f;
    public float nextTargetDistance = 3f;
    public float targetRange = 5f;

    Rigidbody2D rb;
    GameObject player;
    Seeker seeker;
    Path path;
    int currentWaypoint = 0;
    bool playerDetected;
    bool inAttackRange;
    bool reachedEndOfPath = false;
    EnemyStatsManager esm;
    Tilemap ground;
    Animator animator;
    GameObject renderer;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        InvokeRepeating("UpdatePath", 0f, 0.5f);
        esm = GetComponent<EnemyStatsManager>();
        ground = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        renderer = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerDetected = Vector3.Distance(transform.position, player.transform.position) < detectionRange;
        inAttackRange = Vector3.Distance(transform.position, player.transform.position) < attackRange;


        if(targetOverride)
        {
            animator.SetBool("Prep", true);
            animator.SetBool("Fly", false);    
        }
        else
        {
            animator.SetBool("Prep", inAttackRange);
            animator.SetBool("Fly", !inAttackRange);
        }

        if(!targetOverride)
        {
            if(inAttackRange)
            {

                renderer.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(target.y - transform.position.y, target.x - transform.position.x) * Mathf.Rad2Deg + 180);

                if(Vector3.Distance(transform.position, target) < nextTargetDistance)
                {
                    if(Random.Range(0, 101) < 33)
                    {
                        target = player.transform.position;
                    }
                    else
                    {
                        target = new Vector3(player.transform.position.x + Random.Range(targetRange * -1, targetRange), player.transform.position.y + Random.Range(targetRange * -1, targetRange), 0);

                        while(ground.GetTile(ground.WorldToCell(target)) != null)
                        {
                            target = new Vector3(player.transform.position.x + Random.Range(targetRange * -1, targetRange), player.transform.position.y + Random.Range(targetRange * -1, targetRange), 0);
                        }
                    }
                }


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
                }

                if( path != null && !reachedEndOfPath)
                {
                    Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
                    Vector2 force = direction * speed * 3;

                    rb.velocity = force;
                    
                    float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

                    if(distance < nextWaypointDistance)
                    {
                        currentWaypoint++;
                    }
                }
            }
            else if(playerDetected)
            {
                renderer.transform.rotation = Quaternion.Euler(0, 0, 0);

                target = player.transform.position;

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
                }

                if( path != null && !reachedEndOfPath)
                {
                    Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
                    Vector2 force = direction * speed;

                    rb.velocity = force;
                    
                    float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

                    if(distance < nextWaypointDistance)
                    {
                        currentWaypoint++;
                    }
                }
            }
        }
        else
        {
            renderer.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(targetOverrideTransform.position.y - transform.position.y, targetOverrideTransform.position.x - transform.position.x) * Mathf.Rad2Deg + 180);

            rb.velocity = (targetOverrideTransform.position - transform.position).normalized * speed * 4;
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

    void UpdatePath()
    {
        if(seeker.IsDone() && playerDetected)
        seeker.StartPath(rb.position, target, OnPathComplete);
    }
}
