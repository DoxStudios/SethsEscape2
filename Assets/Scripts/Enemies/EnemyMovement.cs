using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public GameObject player;
    public Rigidbody2D rb;

    public bool isaacNewton;

    public float followRadius;
    public float detectRadius;

    public bool playerDetected;

    void Start()
    {

    }

    void Update()
    {
        playerDetected = detectPlayer();

        

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