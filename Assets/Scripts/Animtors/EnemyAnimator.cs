using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GnomeAnimator : MonoBehaviour
{
    EnemyMovement enemyMovement;
    EnemyWeapon enemyWeapon;

    Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        enemyMovement = GetComponent<EnemyMovement>();
        enemyWeapon = GetComponentInChildren<EnemyWeapon>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("InitialDetected", enemyMovement.initialDetected);
        animator.SetBool("DetectRange", enemyMovement.playerDetected);
        animator.SetBool("FireRange", enemyMovement.inRange);
        animator.SetBool("FireFrames", enemyWeapon.state == 2);
    }
}
