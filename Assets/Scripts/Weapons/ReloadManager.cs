using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadManager : MonoBehaviour
{

    public WeaponManager handGun;
    public WeaponManager spasm;

    PlayerStatsManager psm;
    public float loadTime;


    WeaponManager currentWeapon = null;
    WeaponManager previousWeapon = null;

    // Start is called before the first frame update
    void Start()
    {
        psm = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatsManager>();
    }

    // Update is called once per frame
    void Update()
    {
        currentWeapon = weaponToLoad();
        
        if(currentWeapon != previousWeapon)
        {
            updateWeapon(currentWeapon);
        }

        if(currentWeapon != null)
        {
            if(psm.currentPrimary.GetComponent<WeaponManager>().state == 0)
            {
                loadTime -= Time.deltaTime;

                if(currentWeapon.loadOneAtATime)
                {
                    
                }
                else
                {
                    if(loadTime <= 0)
                    {
                    }
                }
            }
        }
        previousWeapon = currentWeapon;
    }

    void updateWeapon(WeaponManager wm)
    {
        if(wm == null)
        {
            loadTime = 0;
        }
        else if(wm.loadOneAtATime)
        {
            
        }
        else
        {
            loadTime = wm.reloadTime;
        }
    }

    WeaponManager weaponToLoad()
    {
        int priority = 0;

        while(priority < 20)
        {
            
            priority += 1;
        }
        return null;
    }
}
