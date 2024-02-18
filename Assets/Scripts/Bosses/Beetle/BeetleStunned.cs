using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeetleStunned : StateMachineBehaviour
{
    float nextTime = 0.5f;

    float stateTime = 0;

    BeetleStateControl beetleState;
    GameObject boss;
    BossStatsManager bsm;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = GameObject.FindGameObjectWithTag("Boss");
        beetleState = boss.GetComponent<BeetleStateControl>();
        bsm = boss.GetComponent<BossStatsManager>();
        beetleState.StateStart(false);
        stateTime = 0;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        stateTime += Time.deltaTime;
        bsm.defense = 0;

        if(stateTime >= nextTime)
        {
            bsm.defense = 0.5f;
            beetleState.stunned = false;
            beetleState.EndState();
        }
    }
}
