using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBall : MonoBehaviour
{
    GameObject player;
    PlayerStatsManager psm;
    public GameObject weaponBase;
    public WeaponManager weapon;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        psm = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatsManager>();
        weapon = weaponBase.GetComponent<WeaponManager>();
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
        }
        else if(psm.pickupTarget == gameObject)
        {
            psm.pickupTarget = null;
        }
    }
}
