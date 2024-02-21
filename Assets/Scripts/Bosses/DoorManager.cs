using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{

    public DoorOperator leftDoor;
    public DoorOperator rightDoor;
    public BossStatsManager bsm;

    public void ResetArena()
    {
        leftDoor.Reset();
        rightDoor.Reset();
    }

    public void Close()
    {
        leftDoor.Close();
        rightDoor.Close();
    }

    public void Open()
    {
        leftDoor.Open();
        rightDoor.Open();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        bsm.PlayerEnter();
    }
}
