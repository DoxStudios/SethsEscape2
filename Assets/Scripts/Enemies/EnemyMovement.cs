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
    public bool playerDetected;
    public bool inRange;
    public EnemyWeapon ew;

    Transform target;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    float groundedRadius = 1f;
    Seeker seeker;
    EnemyStatsManager esm;
    Rigidbody2D rb;
    float distanceToPlayer;

    void Start()
    {
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
        playerDetected = ((player.transform.position - transform.position).magnitude < detectRadius);
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


                if(distanceToPlayer > followRadius)
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
                rb.velocity = new Vector2(0, 0);
                ew.Fire();
            }
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
}