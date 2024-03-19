using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeetleAnimator : MonoBehaviour
{

    public float detectRange;
    public float moveRange;
    public float shotgunRange;

    bool checkedForshotgun = false;

    GameObject player;
    EnemyWeapon enemyWeapon;
    Animator animator;
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemyWeapon = GetComponentInChildren<EnemyWeapon>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);
        animator.SetBool("FireFrames", enemyWeapon.state == 2);
        animator.SetBool("DetectRange", distance < detectRange);
        animator.SetBool("MoveRange", distance > moveRange && distance < detectRange);

        spriteRenderer.flipX = player.transform.position.x > transform.position.x;

        if(distance < shotgunRange && !checkedForshotgun)
        {
            checkedForshotgun = true;
            animator.SetBool("Shotgun", Random.Range(0, 3) == 0);
        }
    }
}
