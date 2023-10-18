using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainsawMovement : MonoBehaviour
{
    public float speed;
    public direction currentDirection = direction.right;
    public location currentLocation = location.floor;
    Rigidbody2D rb;
    public float raycastDistance;
    public LayerMask layerMask;
    bool justMoved = false;

    direction lastDirection;
    location lastLocation;
    List<Vector3> lastPosition = new List<Vector3>();

    public enum direction {
        up,
        down,
        left,
        right,
    };


// 
// 
// REDO to use collisions
// 
// 
// 
//     

    public enum location {
        ceiling,
        floor,
        leftWall,
        rightWall,
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        lastPosition.Add(transform.position);
        if(lastPosition.Count > 3)
        {
            lastPosition.RemoveAt(0);
        }

        if(lastPosition[0] == transform.position)
        {
            currentLocation = lastLocation;
            currentDirection = lastDirection;
        }


        lastLocation = currentLocation;
        lastDirection = currentDirection;

        if(rb.gravityScale == 0)
        {
            if(currentDirection == direction.up)
            {
                rb.velocity = new Vector2(0, speed);
            }
            else if(currentDirection == direction.down)
            {
                rb.velocity = new Vector2(0, -speed);
            }
            else if(currentDirection == direction.left)
            {
                rb.velocity = new Vector2(-speed, 0);
            }
            else if(currentDirection == direction.right)
            {
                rb.velocity = new Vector2(speed, 0);
            }
        }

        if(up().collider != null || down().collider != null || left().collider != null || right().collider != null)
        {
            justMoved = false;
        }

        if(justMoved) return;
        if(currentLocation == location.ceiling)
        {
            if(currentDirection == direction.left)
            {
                if(up().collider != null && left().collider != null)
                {
                    currentDirection = direction.down;
                    currentLocation = location.leftWall;
                    justMoved = true;
                }
                if(up().collider == null)
                {
                    currentDirection = direction.up;
                    currentLocation = location.rightWall;
                    justMoved = true;
                }
            }

            if(currentDirection == direction.right)
            {
                if(up().collider != null && right().collider != null)
                {
                    currentDirection = direction.down;
                    currentLocation = location.rightWall;
                    justMoved = true;
                }
                if(up().collider == null)
                {
                    currentDirection = direction.up;
                    currentLocation = location.leftWall;
                    justMoved = true;
                }
            }
        }

        if(currentLocation == location.floor)
        {
            if(currentDirection == direction.right)
            {
                if(down().collider != null && right().collider != null)
                {
                    currentDirection = direction.up;
                    currentLocation = location.rightWall;
                    justMoved = true;
                }
                if(down().collider == null)
                {
                    currentDirection = direction.down;
                    currentLocation = location.leftWall;
                    justMoved = true;
                }
            }

            if(currentDirection == direction.left)
            {
                if(down().collider != null && left().collider != null)
                {
                    currentDirection = direction.up;
                    currentLocation = location.leftWall;
                }
                if(down().collider == null)
                {
                    currentDirection = direction.down;
                    currentLocation = location.rightWall;
                    justMoved = true;
                }
            }
        }

        if(currentLocation == location.leftWall)
        {
            if(currentDirection == direction.up)
            {
                if(left().collider != null && up().collider != null)
                {
                    currentDirection = direction.right;
                    currentLocation = location.ceiling;
                    justMoved = true;
                }
                if(left().collider == null)
                {
                    currentDirection = direction.left;
                    currentLocation = location.floor;
                    justMoved = true;
                }
            }

            if(currentDirection == direction.down)
            {
                if(left().collider != null && down().collider != null)
                {
                    currentDirection = direction.right;
                    currentLocation = location.floor;
                    justMoved = true;
                }
                if(left().collider == null)
                {
                    currentDirection = direction.left;
                    currentLocation = location.ceiling;
                    justMoved = true;
                }
            }
        }

        if(currentLocation == location.rightWall)
        {
            if(currentDirection == direction.up)
            {
                if(right().collider != null && up().collider != null)
                {
                    currentDirection = direction.left;
                    currentLocation = location.ceiling;
                    justMoved = true;
                }
                if(right().collider == null)
                {
                    currentDirection = direction.right;
                    currentLocation = location.floor;
                    justMoved = true;
                }
            }

            if(currentDirection == direction.down)
            {
                if(right().collider != null && down().collider != null)
                {
                    currentDirection = direction.left;
                    currentLocation = location.floor;
                    justMoved = true;
                }
                if(right().collider == null)
                {
                    currentDirection = direction.right;
                    currentLocation = location.ceiling;
                    justMoved = true;
                }
            }
        }
    }
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag != "Ground") return;
        if(rb.gravityScale != 0)
        {
            InitialImpact();
        } 

        rb.gravityScale = 0;
    
        if(currentLocation == location.floor || currentLocation == location.ceiling)
        {
            if(rb.velocity.x > 0)
            {
                currentDirection = direction.right;
            }
            else if(rb.velocity.x < 0)
            {
                currentDirection = direction.left;
            }
        }
        else
        {
            if(rb.velocity.y > 0)
            {
                currentDirection = direction.up;
            }
            else if(rb.velocity.y < 0)
            {
                currentDirection = direction.down;
            }
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag != "Ground") return;
        if(rb.gravityScale != 0)
        {
            InitialImpact();
        } 

        rb.gravityScale = 0;
    
        if(currentLocation == location.floor || currentLocation == location.ceiling)
        {
            if(rb.velocity.x > 0)
            {
                currentDirection = direction.right;
            }
            else if(rb.velocity.x < 0)
            {
                currentDirection = direction.left;
            }
        }
        else
        {
            if(rb.velocity.y > 0)
            {
                currentDirection = direction.up;
            }
            else if(rb.velocity.y < 0)
            {
                currentDirection = direction.down;
            }
        }
    }

    void InitialImpact()
    {
        if(down().collider != null)
        {
            currentLocation = location.floor;
        }
        else if(up().collider != null)
        {
            currentLocation = location.ceiling;
        }
        else if(left().collider != null)
        {
            currentLocation = location.leftWall;
        }
        else if(right().collider != null)
        {
            currentLocation = location.rightWall;
        }
    }

    RaycastHit2D up()
    {
        return Physics2D.Raycast(transform.position, Vector2.up, raycastDistance * transform.parent.localScale.x, layerMask);
    }

    RaycastHit2D down()
    {
        return Physics2D.Raycast(transform.position, -Vector2.up, raycastDistance * transform.parent.localScale.x, layerMask);
    }

    RaycastHit2D left()
    {
        return Physics2D.Raycast(transform.position, -Vector2.right, raycastDistance * transform.parent.localScale.x, layerMask);
    }

    RaycastHit2D right()
    {
        return Physics2D.Raycast(transform.position, Vector2.right, raycastDistance * transform.parent.localScale.x, layerMask);
    }
}
