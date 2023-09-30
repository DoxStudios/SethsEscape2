using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunpickup : MonoBehaviour
{
    GameObject player;
    PlayerStatsManager psm;
    WeaponManager weapon;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        psm = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatsManager>();
        weapon = GetComponent<WeaponManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if((player.transform.position - transform.position).magnitude <= 20)
        {
            if(psm.pickupTarget == null)
            {
                psm.pickupTarget = gameObject;
            }


            if (Input.GetKeyDown(KeyCode.E) && psm.pickupTarget == gameObject)
            {
                psm.pickupTarget = null;
                psm.AddWeapon(weapon);
                Destroy(gameObject);
            }
        }
        else if(psm.pickupTarget == gameObject)
        {
            psm.pickupTarget = null;
        }
    }
}
