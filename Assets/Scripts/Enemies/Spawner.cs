using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject prefab;
    public bool isSpider = false;
    public float rotation = 0f;

    void Awake()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    public void Spawn()
    {
        if(!isSpider)
        {
            Instantiate(prefab, transform.position, transform.rotation);
        }
        else
        {
            Instantiate(prefab, transform.position, Quaternion.Euler(0, 0, rotation));
        }
    }
}
