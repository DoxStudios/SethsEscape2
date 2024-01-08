using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeetleCardinalShot : StateMachineBehaviour
{
    
    public float shotTime;

    Transform shotgunLeft;
    Transform shotgunRight;
    Transform chainsawLeft;
    Transform chainsawRight;

    float nextTime;

    bool fired = false;

    GameObject player;

    float stateTime = 0;
    BeetleUtils beetleUtils;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player");
        beetleUtils = GameObject.FindGameObjectWithTag("Boss").GetComponent<BeetleUtils>();
        nextTime = Random.Range(0.25f, 0.75f);
        GameObject.FindGameObjectWithTag("Boss").GetComponent<BeetleStateControl>().StateStart(true);
        shotgunLeft = GameObject.Find("CardinalShotgunLeft").transform;
        shotgunRight = GameObject.Find("CardinalShotgunRight").transform;
        chainsawLeft = GameObject.Find("CardinalChainsawLeft").transform;
        chainsawRight = GameObject.Find("CardinalChainsawRight").transform;
        fired = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        stateTime += Time.deltaTime;

        if(stateTime >= shotTime && !fired)
        {
            fired = true;
            if(player.transform.position.x < GameObject.FindGameObjectWithTag("Boss").transform.position.x)
            {
                beetleUtils.ShootBullet(player.transform.position, shotgunLeft);
                beetleUtils.ShootBullet(beetleUtils.gameObject.transform.position + new Vector3(1, 0, 0), shotgunRight);
                beetleUtils.ShootChainsaw(player.transform.position, chainsawLeft);
                beetleUtils.ShootChainsaw(beetleUtils.gameObject.transform.position + new Vector3(1, 0, 0), chainsawRight);
            }
            else
            {
                beetleUtils.ShootBullet(player.transform.position, shotgunRight);
                beetleUtils.ShootBullet(beetleUtils.gameObject.transform.position + new Vector3(-1, 0, 0), shotgunLeft);
                beetleUtils.ShootChainsaw(player.transform.position, chainsawRight);
                beetleUtils.ShootChainsaw(beetleUtils.gameObject.transform.position + new Vector3(-1, 0, 0), chainsawLeft);
            }
        }

        if(stateTime >= nextTime)
        {
            GameObject.FindGameObjectWithTag("Boss").GetComponent<BeetleStateControl>().EndState();
        }
    }
}
