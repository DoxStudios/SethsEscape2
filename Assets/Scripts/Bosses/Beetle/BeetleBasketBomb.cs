using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeetleBasketBomb : StateMachineBehaviour
{
      
    GameObject player;
    GameObject boss;
    Rigidbody2D rb;
    bool lept;
    bool halt;
    float stateTime;
    BeetleUtils beetleUtils;

    BeetleStateControl beetleStateControl;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player");
        boss = GameObject.FindGameObjectWithTag("Boss");
        beetleUtils = boss.GetComponent<BeetleUtils>();
        beetleStateControl = boss.GetComponent<BeetleStateControl>();
        beetleStateControl.StateStart(true);
        rb = boss.GetComponent<Rigidbody2D>();
        lept = false;
        halt = false;
        stateTime = 0;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        stateTime += Time.deltaTime;
        if(!lept)
        {
            Vector3 target = player.transform.position;
            target.y += 75;
            rb.AddForce((target - boss.transform.position).normalized * 5000);
            lept = true;
        }

        if(Mathf.Abs(boss.transform.position.x - player.transform.position.x) < 2.5 && !halt)
        {

            beetleUtils.ShootBomb(player.transform.position, beetleUtils.gameObject.transform, false, false, 50);

            halt = true;
        }

        if(beetleStateControl.grounded && stateTime > 0.25)
        {
            beetleStateControl.EndState();
        }

    }
}
