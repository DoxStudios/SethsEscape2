using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeetleBasicShot : StateMachineBehaviour
{
    
    public float fireTime;

    Transform originLeft;
    Transform originRight;

    float nextTime;

    float stateTime = 0;

    GameObject player;
    BeetleUtils beetleUtils;
    bool fired = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        nextTime = Random.Range(0.25f, 0.75f);
        GameObject.FindGameObjectWithTag("Boss").GetComponent<BeetleStateControl>().StateStart(true);
        player = GameObject.FindGameObjectWithTag("Player");
        beetleUtils = GameObject.FindGameObjectWithTag("Boss").GetComponent<BeetleUtils>();
        originLeft = GameObject.Find("BasicPosLeft").transform;
        originRight = GameObject.Find("BasicPosRight").transform;

        fired = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        stateTime += Time.deltaTime;

        if(stateTime >= fireTime && !fired)
        {
            fired = true;
            if(player.transform.position.x < beetleUtils.gameObject.transform.position.x)
            {
                beetleUtils.ShootBullet(player.transform.position, originLeft);
            }
            else
            {
                beetleUtils.ShootBullet(player.transform.position, originRight);
            }
        }

        if(stateTime >= nextTime)
        {
            GameObject.FindGameObjectWithTag("Boss").GetComponent<BeetleStateControl>().EndState();
        }
    }
}
