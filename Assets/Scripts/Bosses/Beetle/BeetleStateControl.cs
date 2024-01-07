using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeetleStateControl : MonoBehaviour
{

    Animator animator;
    GameObject player;

    public int phase = 1;
    public bool transition = true;
    public int next = 0;
    public int lastSelected = 0;
    public int attacks = 0;
    public float moveThreshold;

    string category = "";
    public bool firstFrameCompleted = false;
    int[] phase1 = {0, 1, 3, 4, 6, 8, 9, 10};
    int[] phase2 = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12};
    BossStatsManager bsm;

    // Start is called before the first frame update
    void Start()
    {
        bsm = GetComponent<BossStatsManager>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(bsm.health <= 500)
        {
            phase = 2;
        }
        else
        {
            phase = 1;
        }
        

        if(!firstFrameCompleted)
        {    

            transition = false;

            if(next == 0)
            {
                attacks = 0;
                if((player.transform.position - transform.position).magnitude >  moveThreshold)
                {
                    category = "Move";
                }
            }
           
            
            while(lastSelected == next)
            {
                if(phase == 1)
                {
                    next = phase1[Random.Range(0, phase1.Length)];
                }
                else
                {
                    next = phase2[Random.Range(0, phase2.Length)];
                }
            }
           

            if(category == "Move")
            {
                
            }

            if(attacks == 2 && phase == 1)
            {
                next = 0;
            }

 
            category = "";

            firstFrameCompleted = true;
            lastSelected = next;
        }

        animator.SetBool("Transition", transition);
        animator.SetInteger("Next", next);
    }

    public void StateStart(bool attack)
    {
        if(attack)
        {
            attacks++;
        }

        firstFrameCompleted = false;
    }

    public void EndState()
    {
        transition = true;
    }
}
