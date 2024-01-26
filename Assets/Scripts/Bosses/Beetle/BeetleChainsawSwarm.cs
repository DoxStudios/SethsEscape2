using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeetleChainsawSwarm : StateMachineBehaviour
{
       
    float nextTime;
    BeetleUtils beetleUtils;
    float stateTime = 0;
    bool thrown;

    Transform chainsawLeft;
    Transform chainsawRight;

    float triggerTime = 0.25f;
    Transform player;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        nextTime = Random.Range(0.25f, 0.75f);
        GameObject.FindGameObjectWithTag("Boss").GetComponent<BeetleStateControl>().StateStart(true);
        chainsawLeft = GameObject.Find("CardinalChainsawLeft").transform;
        chainsawRight = GameObject.Find("CardinalChainsawRight").transform;
        beetleUtils = GameObject.FindGameObjectWithTag("Boss").GetComponent<BeetleUtils>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        thrown = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        stateTime += Time.deltaTime;

        if(stateTime > triggerTime && !thrown)
        {
            thrown = true;
            if(player.position.x < GameObject.FindGameObjectWithTag("Boss").transform.position.x)
            {
                for(int i = 0; i < 5; i++)
                {
                    beetleUtils.ShootChainsaw(new Vector3(player.position.x + Random.Range(-2, 2), player.position.y + Random.Range(-2, 2), player.position.z), chainsawLeft);
                }
            }
            else
            {
                for(int i = 0; i < 5; i++)
                {
                    beetleUtils.ShootChainsaw(new Vector3(player.position.x + Random.Range(-2, 2), player.position.y + Random.Range(-2, 2), player.position.z), chainsawRight);
                }
            }
        }

        if(stateTime >= nextTime)
        {
            GameObject.FindGameObjectWithTag("Boss").GetComponent<BeetleStateControl>().EndState();
        }
    }
}
