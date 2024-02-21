using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeetlePitchThrow : StateMachineBehaviour
{
    
    BeetleUtils beetleUtils;
    Transform originLeft;
    Transform originRight;
    float nextTime;
    float stateTime = 0;

    bool fired;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        beetleUtils = GameObject.FindGameObjectWithTag("Boss").GetComponent<BeetleUtils>();
        nextTime = Random.Range(0.25f, 0.75f);
        GameObject.FindGameObjectWithTag("Boss").GetComponent<BeetleStateControl>().StateStart(true);
        originLeft = GameObject.Find("BasicPosLeft").transform;
        originRight = GameObject.Find("BasicPosRight").transform;
        fired = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        stateTime += Time.deltaTime;

        if(stateTime >= 0.5f && !fired)
        {
            fired = true;

            if(beetleUtils.gameObject.transform.position.x > beetleUtils.psm.gameObject.transform.position.x)
            {
                beetleUtils.ShootBomb(new Vector3(beetleUtils.psm.gameObject.transform.position.x, beetleUtils.gameObject.transform.position.y, beetleUtils.psm.gameObject.transform.position.z), originLeft, false, true, 100);
            }
            else
            {
                beetleUtils.ShootBomb(new Vector3(beetleUtils.psm.gameObject.transform.position.x, beetleUtils.gameObject.transform.position.y, beetleUtils.psm.gameObject.transform.position.z), originRight, false, true, 100);
            }
        }

        if(stateTime >= nextTime)
        {
            GameObject.FindGameObjectWithTag("Boss").GetComponent<BeetleStateControl>().EndState();
        }
    }
}
