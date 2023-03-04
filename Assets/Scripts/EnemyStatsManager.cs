using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatsManager : MonoBehaviour
{
    public int health = 1;
    public int damage = 1;
    public float outgoingKnockbackAmount;
    public float outgoingKnockbackTime;
    public float outgoingStunTime;

    bool stunned = false;

    public void Heal(int amount)
	{
		health += amount;
	}

	public void Damage(int amount)
	{
		health -= amount;
	}

	void Update()
	{
		if(health <= 0)
		{
			Destroy(gameObject);
		}
	}
}
