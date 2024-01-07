using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOperator : MonoBehaviour
{

    BoxCollider2D boxCollider2D;

    bool opened = false;
    bool closed = false;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reset()
    {
        opened = false;
        closed = false;
        boxCollider2D.enabled = false;
    }

    public void Open()
    {
        if(!closed)
        {
            opened = true;
            boxCollider2D.enabled = false;
        }
    }

    public void Close()
    {
        if(!opened)
        {
            closed = true;
            boxCollider2D.enabled = true;
        }
    }
}
