using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDetection : MonoBehaviour
{
    public CentipedeMovement centipedeMovement;

    void OnCollisionEnter2D(Collision2D collision)
    {
        centipedeMovement.Rise();
    }
}
