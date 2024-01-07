using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{

    public DoorOperator leftDoor;
    public DoorOperator rightDoor;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetArena()
    {
        leftDoor.Reset();
        rightDoor.Reset();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            leftDoor.Close();
            rightDoor.Close();
        }
    }
}
