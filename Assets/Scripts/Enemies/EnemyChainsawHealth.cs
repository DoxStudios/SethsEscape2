using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChainsawHealth : MonoBehaviour
{
    
    public float health = 1f;

    // Update is called once per frame
    void Update()
    {
        if(health <= 0f)
        {
            Destroy(gameObject);
        }
    }

    public void Damage(float damage)
    {
        health -= damage;
    }
}
