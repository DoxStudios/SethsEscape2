using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeetleSpinThrow : StateMachineBehaviour
{
       
    float nextTime;
    BeetleUtils beetleUtils;
    float stateTime = 0;
    float triggerTime = 0.25f;
    bool thrown;
    GameObject player;
    Transform chainsawLeft;
    Transform chainsawRight;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        nextTime = Random.Range(0.25f, 0.75f);
        GameObject.FindGameObjectWithTag("Boss").GetComponent<BeetleStateControl>().StateStart(true);
        beetleUtils = GameObject.FindGameObjectWithTag("Boss").GetComponent<BeetleUtils>();
        beetleUtils.ChainsawThrowHitbox(true);
        thrown = false;
        player = GameObject.FindGameObjectWithTag("Player");
        chainsawLeft = GameObject.Find("CardinalChainsawLeft").transform;
        chainsawRight = GameObject.Find("CardinalChainsawRight").transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        stateTime += Time.deltaTime;

        if(stateTime > triggerTime && !thrown)
        {
            thrown = true;
            if(player.transform.position.x < beetleUtils.gameObject.transform.position.x)
            {
                beetleUtils.ShootChainsaw(player.transform.position, chainsawLeft);
            }
            else
            {
                beetleUtils.ShootChainsaw(player.transform.position, chainsawRight);
            }
        }

        if(stateTime >= nextTime)
        {
            beetleUtils.ChainsawThrowHitbox(false);
            GameObject.FindGameObjectWithTag("Boss").GetComponent<BeetleStateControl>().EndState();
        }
    }
}
