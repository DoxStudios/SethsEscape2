using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{

    public GameObject weapon;

    void Awake()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    public void Spawn()
    {
        Instantiate(weapon, transform.position, Quaternion.identity);
    }
}
