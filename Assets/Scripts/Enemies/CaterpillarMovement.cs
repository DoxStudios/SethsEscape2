using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaterpillarMovement : MonoBehaviour
{

    public float detectRange;
    public float followDistance;
    public float riseDistance;
    public float speed;

    GameObject player;
    Animator animator;

    bool move = false;
    bool fire = false;
    SpriteRenderer sr;
    EnemyWeapon weapon;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        weapon = GetComponentInChildren<EnemyWeapon>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        if(player.transform.position.x < transform.position.x)
        {
            weapon.firePosition.localPosition = weapon.secondaryFirePosition;
            animator.SetBool("Flip", true);
            if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Move") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                sr.flipX = false;
            }
            else
            {
                sr.flipX = true;
            }
        }
        else
        {
            weapon.firePosition.localPosition = weapon.primaryFirePosition;
            animator.SetBool("Flip", false);
            sr.flipX = false;
        }

        if((player.transform.position - transform.position).magnitude < detectRange && (player.transform.position - transform.position).magnitude > followDistance)
        {
            move = true;
        }
        else
        {
            move = false;
        }

        if((player.transform.position - transform.position).magnitude < followDistance)
        {
            fire = true;
        }
        else
        {
            fire = false;
        }

        if((player.transform.position - transform.position).magnitude < riseDistance)
        {
            animator.SetBool("Rise", true);
        }

        if(move && !fire)
        {
            if(transform.position.x < player.transform.position.x)
            {
                rb.velocity = new Vector2(speed, 0);
            }
            else
            {
                rb.velocity = new Vector2(-speed, 0);
            }
        }

        animator.SetBool("Move", move);
        animator.SetBool("Fire", fire);


        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Fire") || animator.GetCurrentAnimatorStateInfo(0).IsName("Fire Flipped"))
        {
            weapon.Fire(false, true);
        }
    }
}
