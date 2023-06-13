using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public GameObject player;
    public Rigidbody2D rb;

    public bool straightLine;
    public bool ranged;
    public bool isaacNewton;
    public bool cloaker;

    public float followRadius;
    public float detectRadius;

    public bool playerDetected;

    void Start()
    {

    }

    void Update()
    {
        playerDetected = detectPlayer();

        if(straightLine && !ranged)
        {

        }
        else if(straightLine && ranged)
        {

        }
        else if(cloaker && !ranged)
        {

        }
        else if(cloaker && ranged)
        {

        }

        if(isaacNewton)
        {
            rb.gravityScale = 1;
        }
        else
        {
            rb.gravityScale = 0;
        }
    }

    bool detectPlayer()
    {
        if((player.transform.position - transform.position).magnitude < detectRadius)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}