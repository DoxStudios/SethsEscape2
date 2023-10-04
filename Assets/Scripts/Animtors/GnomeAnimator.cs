using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GnomeAnimator : MonoBehaviour
{
    public Sprite DEFAULT;

    public Sprite WALK_1;
    public Sprite WALK_2;
    public Sprite WALK_3;

    public Sprite READY;
    
    public Sprite ATTACK_1;
    public Sprite ATTACK_2;

    Sprite CURRENT;

    public SpriteRenderer spriteRenderer;
    EnemyMovement enemyMovement;
    public EnemyWeapon enemyWeapon;

    GameObject player;

    State state;


    public float walkInterval = 0.1f;
    public float readyTime = 0.2f;
    public float trueReadyTime = 0.5f;
    float timer = 0f;


    enum State
    {
        walking,
        ready,
        attacking,
        idle
    }

    // Start is called before the first frame update
    void Start()
    {
        CURRENT = DEFAULT;
        player = GameObject.FindGameObjectWithTag("Player");
        enemyMovement = GetComponent<EnemyMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.sprite = CURRENT;

        CheckState();
        UpdateImage();

        if(player.transform.position.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }



    void CheckState()
    {
        if(state == State.idle)
        {
            if(enemyMovement.playerDetected && !enemyMovement.inRange)
            {
                state = State.walking;
            }
        }

        if(enemyMovement.inRange && state != State.attacking)
        {
            state = State.ready;
        }
        
        if(!enemyMovement.inRange && state == State.attacking)
        {
            state = State.ready;
        }

        if(state == State.walking && !enemyMovement.playerDetected)
        {
            state = State.idle;
        }
    }

    void UpdateImage()
    {
        if(state == State.idle)
        {
            CURRENT = DEFAULT;
        }


        if(state == State.walking)
        {
            timer += Time.deltaTime;
            if(timer < (walkInterval * 1))
            {
                CURRENT = WALK_1;
            }
            else if(timer < (walkInterval * 2))
            {
                CURRENT = WALK_2;
            }
            else if(timer < (walkInterval * 3))
            {
                CURRENT = WALK_3;
            }
            else
            {
                timer = 0f;
            }

            enemyWeapon.ready = false;
        }

        if(state == State.ready)
        {
            enemyWeapon.ready = false;

            if(timer > readyTime)
            {
                CURRENT = ATTACK_1;
            }
            else
            {
                CURRENT = READY;
            }
            timer += Time.deltaTime;

            if(timer > trueReadyTime)
            {
                if(enemyMovement.inRange)
                {
                    state = State.attacking;
                    enemyWeapon.ready = true;
                }
                else
                {
                    state = State.walking;
                }
                
                timer = 0f;
            }
        }

        if(state == State.attacking)
        {
            if(enemyWeapon.state != 2)
            {
                CURRENT = ATTACK_1;
            }

            if(enemyWeapon.state == 2)
            {
                CURRENT = ATTACK_2;
            }
        }
    }
}
