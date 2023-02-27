using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 45f;
    public float jumpForce = 60f;
    public float doubleJumpForce = 45f;
    public bool doubleJump = true;
    public LayerMask ground;
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public GameObject groundCheck;

    float groundedRadius = 1f;
    bool isGrounded = true;
    bool doubleJumpRemaining = true;

    void Update()
    {

       isGrounded = checkGrounded();
       if(isGrounded)
       {
            doubleJumpRemaining = true;
       }

        float horizontal = Input.GetAxisRaw("Horizontal");
        Vector3 move = new Vector3(horizontal, 0, 0);
        if(horizontal < 0)
        {
            sr.flipX = true;
        }
        else if(horizontal > 0)
        {
            sr.flipX = false;
        }
        transform.position += move * speed * Time.deltaTime;

        if((isGrounded || doubleJumpRemaining) && Input.GetButtonDown("Jump"))
        {
            if(!isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                doubleJumpRemaining = false;
                rb.AddForce(transform.up * doubleJumpForce, ForceMode2D.Impulse);
            }
            else
            {
                isGrounded = false;
                rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            }
            
        }
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
}
