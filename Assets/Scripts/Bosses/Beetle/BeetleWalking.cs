using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BeetleWalking : StateMachineBehaviour
{
    GameObject boss;
    Vector3 target;
    Tilemap ground;
    Rigidbody2D rb;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = GameObject.FindGameObjectWithTag("Boss");
        rb = boss.GetComponent<Rigidbody2D>();
        boss.GetComponent<BeetleStateControl>().StateStart(false);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player.transform.position.x < boss.transform.position.x)
        {
            target = new Vector3(boss.transform.position.x - Random.Range(3, 10), boss.transform.position.y, boss.transform.position.z);
        }
        else
        {
            target = new Vector3(boss.transform.position.x + Random.Range(3, 10), boss.transform.position.y, boss.transform.position.z);
        }

        ground = GameObject.Find("Tilemap").GetComponent<Tilemap>();

        int iterations = 0;

        while(ground.GetTile(ground.WorldToCell(target)) != null)
        {
            iterations++;
            if(player.transform.position.x < boss.transform.position.x)
            {
                target = new Vector3(boss.transform.position.x - Random.Range(3, 10), boss.transform.position.y, boss.transform.position.z);
            }
            else
            {
                target = new Vector3(boss.transform.position.x + Random.Range(3, 10), boss.transform.position.y, boss.transform.position.z);
            }

            if(iterations > 10)
            {
                GameObject.FindGameObjectWithTag("Boss").GetComponent<BeetleStateControl>().EndState();
                break;
            }
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb.velocity = new Vector2((target.x - boss.transform.position.x), 0).normalized * 20;

        if(Mathf.Abs(boss.transform.position.x - target.x) < 0.5)
        {
            rb.velocity = new Vector2(0, 0);
            GameObject.FindGameObjectWithTag("Boss").GetComponent<BeetleStateControl>().EndState();
        }
    }
}
