using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeetleStateControl : MonoBehaviour
{

    Animator animator;
    GameObject player;

    public GameObject groundCheck;
    public LayerMask ground;
    float groundedRadius = 5f;

    public int phase = 1;
    public bool transition = true;
    public int next = 0;
    public int lastSelected = 0;
    public int attacks = 0;
    public float moveThreshold;

    string category = "";
    public bool firstFrameCompleted = false;
    int[] phase1 = {0, 1, 3, 4, 6, 8, 9, 12};
    //int[] phase1 = {0, 11};
    int[] phase2 = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11};
    BossStatsManager bsm;
    BeetleUtils beetleUtils;
    

    public bool stunned = false;
    public bool stunBlocked = false;

    public bool grounded = true;

    bool veryFirstFrame = false;

    // Start is called before the first frame update
    void Start()
    {
        beetleUtils = GetComponent<BeetleUtils>();
        bsm = GetComponent<BossStatsManager>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

        grounded = checkGrounded();

        if(bsm.health <= 500)
        {
            phase = 2;
        }
        else
        {
            phase = 1;
        }

        if(bsm.health <= 0)
        {
            veryFirstFrame = false;
        }   

        if(!firstFrameCompleted)
        {    

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

            if(!veryFirstFrame)
            {
                next = 1;
            }

            category = "";

            firstFrameCompleted = true;
            veryFirstFrame = true;
            lastSelected = next;
        }

        if(!stunned)
        {
            if(grounded && beetleUtils.inFight)
            {
                animator.SetBool("Transition", transition);
            }
            else
            {
                animator.SetBool("Transition", false);
            }
            animator.SetInteger("Next", next);
        }
    }

    public void Stun()
    {
        if(!stunBlocked)
        {
            animator.SetInteger("Next", 13);
            animator.SetBool("Transition", true);
            stunned = true;
            stunBlocked = true;
        }
    }

    public void StateStart(bool attack)
    {
        if(attack)
        {
            attacks++;
        }
        transition = false;
        firstFrameCompleted = false;
    }

    public void EndState()
    {
        transition = true;
    }

    bool checkGrounded()
	{
		Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.transform.position, groundedRadius, ground);
		for (int i = 0; i < colliders.Length; i++) {
			if (colliders[i].gameObject != gameObject) {
				return true;
			}
		}
		return false;
	}

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Ground")
        {
            stunBlocked = false;
        }
    }
}
