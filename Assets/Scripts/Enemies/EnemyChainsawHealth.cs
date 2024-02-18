using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChainsawHealth : MonoBehaviour
{
    
    public float health = 1f;
    public bool boss = false;

    // Update is called once per frame
    void Update()
    {
        if(health <= 0f)
        {
            if(boss)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatsManager>().Heal(5);
            }
            Destroy(gameObject);
        }
    }

    public void Damage(float damage)
    {
        health -= damage;
    }
}
