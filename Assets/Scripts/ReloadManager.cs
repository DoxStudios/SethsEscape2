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
            if(!currentWeapon.isActive)
            {
                loadTime -= Time.deltaTime;

                if(currentWeapon.loadOneAtATime)
                {
                    if(currentWeapon.currentAmmo < currentWeapon.maxAmmo && loadTime <= 0)
                    {
                        currentWeapon.currentAmmo += 1;
                        loadTime = currentWeapon.reloadTime / currentWeapon.maxAmmo;
                    }
                }
                else
                {
                    if(loadTime <= 0)
                    {
                        currentWeapon.currentAmmo = currentWeapon.maxAmmo;
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
            loadTime = wm.reloadTime / wm.maxAmmo;
        }
        else
        {
            loadTime = wm.reloadTime;
        }
    }

    WeaponManager weaponToLoad()
    {
        int priority = 0;

        while(true)
        {
            if(spasm.priority == priority && spasm.currentAmmo < spasm.maxAmmo)
            {
                return spasm;
            }
            if(handGun.priority == priority && handGun.currentAmmo < handGun.maxAmmo)
            {
                return handGun;
            }
            priority += 1;
        }
    }
}
