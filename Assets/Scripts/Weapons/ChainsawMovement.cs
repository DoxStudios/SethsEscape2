using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChainsawMovement : MonoBehaviour
{
    public float speed;
    public direction currentDirection = direction.right;
    Rigidbody2D rb;

    Tilemap chainsawPath;
    Vector3Int targetPosition;
    bool inPath = false;

    Vector3Int currentCell;
    Vector3Int leftPosition;
    Vector3Int rightPosition;
    Vector3Int upPosition;
    Vector3Int downPosition;

    public enum direction {
        up,
        down,
        left,
        right,
    };


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        chainsawPath = GameObject.Find("ChainsawPath").GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentCell = chainsawPath.WorldToCell(transform.parent.position);
        leftPosition = currentCell + new Vector3Int(-1, 0, 0);
        rightPosition = currentCell + new Vector3Int(1, 0, 0);
        upPosition = currentCell + new Vector3Int(0, 1, 0);
        downPosition = currentCell + new Vector3Int(0, -1, 0);

        if(chainsawPath.GetTile(currentCell) != null && !inPath)
        {
            enterPath();
        }
        
        if(!inPath) return;
        Vector3 targetPositionWorld = chainsawPath.GetCellCenterWorld(targetPosition);
        if(Vector3.Distance(targetPositionWorld, transform.parent.position) < 1.5 || targetPosition == null)
        {
            CheckDirection();
            if(currentDirection == direction.left)
            {
                targetPosition = leftPosition;
            }
            else if(currentDirection == direction.right)
            {
                targetPosition = rightPosition;
            }
            else if(currentDirection == direction.up)
            {
                targetPosition = upPosition;
            }
            else if(currentDirection == direction.down)
            {
                targetPosition = downPosition;
            }
        }
        Vector2 moveDirection = (Vector2) (targetPositionWorld - transform.parent.position);
        //Debug.Log(targetPositionWorld.ToString() + ", " + transform.position.ToString());
        rb.MovePosition(rb.position + moveDirection.normalized * speed * Time.fixedDeltaTime);
        //transform.parent.position = new Vector3(transform.parent.position.x, chainsawPath.GetCellCenterWorld(currentCell).y, transform.parent.position.z);
        
    }

    void enterPath()
    {
        TileBase upTile = chainsawPath.GetTile(upPosition);
        TileBase downTile = chainsawPath.GetTile(downPosition);
        TileBase leftTile = chainsawPath.GetTile(leftPosition);
        TileBase rightTile = chainsawPath.GetTile(rightPosition);

        bool up;
        bool down;
        bool left;
        bool right;

        if(upTile == null)
        {
            up = false;
        }
        else
        {
            if(upTile.name != "TilesetExample_6")
            {
                up = false;
            }
            else
            {
                up = true;
            }
        }

        if(downTile == null)
        {
            down = false;
        }
        else
        {
            if(downTile.name != "TilesetExample_6")
            {
                down = false;
            }
            else
            {
                down = true;
            }
        }

        if(leftTile == null)
        {
            left = false;
        }
        else
        {
            if(leftTile.name != "TilesetExample_6")
            {
                left = false;
            }
            else
            {
                left = true;
            }
        }

        if(rightTile == null)
        {
            right = false;
        }
        else
        {
            if(rightTile.name != "TilesetExample_6")
            {
                right = false;
            }
            else
            {
                right = true;
            }
        }

        if(!up && !down)
        {
            if(rb.velocity.x > 0)
            {
                currentDirection = direction.right;
            }
            else
            {
                currentDirection = direction.left;
            }
        }
        

        if(!left && !right)
        {
            if(rb.velocity.y > 0)
            {
                currentDirection = direction.up;
            }
            else
            {
                currentDirection = direction.down;
            }
        }

        rb.velocity = Vector2.zero;
        rb.gravityScale = 0;
        inPath = true;
        transform.parent.position = chainsawPath.GetCellCenterWorld(currentCell);
        targetPosition = currentCell;
    }

    void CheckDirection()
    {
        TileBase upTile = chainsawPath.GetTile(upPosition);
        TileBase downTile = chainsawPath.GetTile(downPosition);
        TileBase leftTile = chainsawPath.GetTile(leftPosition);
        TileBase rightTile = chainsawPath.GetTile(rightPosition);

        bool up;
        bool down;
        bool left;
        bool right;

        if(upTile == null)
        {
            up = false;
        }
        else
        {
            if(upTile.name != "TilesetExample_6")
            {
                up = false;
            }
            else
            {
                up = true;
            }
        }

        if(downTile == null)
        {
            down = false;
        }
        else
        {
            if(downTile.name != "TilesetExample_6")
            {
                down = false;
            }
            else
            {
                down = true;
            }
        }

        if(leftTile == null)
        {
            left = false;
        }
        else
        {
            if(leftTile.name != "TilesetExample_6")
            {
                left = false;
            }
            else
            {
                left = true;
            }
        }

        if(rightTile == null)
        {
            right = false;
        }
        else
        {
            if(rightTile.name != "TilesetExample_6")
            {
                right = false;
            }
            else
            {
                right = true;
            }
        }


        if(currentDirection == direction.left)
        {
            if(!left)
            {
                if(up)
                {
                    currentDirection = direction.up;
                }
                else if(down)
                {
                    currentDirection = direction.down;
                }
                else
                {
                    currentDirection = direction.right;
                }
            }
        }
        if(currentDirection == direction.right)
        {
            if(!right)
            {
                if(up)
                {
                    currentDirection = direction.up;
                }
                else if(down)
                {
                    currentDirection = direction.down;
                }
                else
                {
                    currentDirection = direction.left;
                }
            }
        }
        if(currentDirection == direction.up)
        {
            if(!up)
            {
                if(left)
                {
                    currentDirection = direction.left;
                }
                else if(right)
                {
                    currentDirection = direction.right;
                }
                else
                {
                    currentDirection = direction.down;
                }
            }
        }
        if(currentDirection == direction.down)
        {
            if(!down)
            {
                if(left)
                {
                    currentDirection = direction.left;
                }
                else if(right)
                {
                    currentDirection = direction.right;
                }
                else
                {
                    currentDirection = direction.up;
                }
            }
        }
    }
}
