using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MothMovement : MonoBehaviour
{
    public float speed = 100f;
    public Vector3 target;
    public float detectionRange = 500f;
    public float attackRange = 75f;
    public float nextWaypointDistance = 3f;
    public bool targetOverride = false;
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

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        InvokeRepeating("UpdatePath", 0f, 0.5f);
        esm = GetComponent<EnemyStatsManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerDetected = Vector3.Distance(transform.position, player.transform.position) < detectionRange;
        inAttackRange = Vector3.Distance(transform.position, player.transform.position) < attackRange;

        if(inAttackRange)
        {
            if(!targetOverride)
            {
                if(Vector3.Distance(transform.position, target) < nextTargetDistance)
                {
                    if(Random.Range(0, 101) < 33)
                    {
                        target = player.transform.position;
                    }
                    else
                    {
                        Debug.Log("Targeting Not Player");
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
                Vector2 force = direction * speed;

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

            if(!targetOverride)
            {
                target = player.transform.position;
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
