using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadManager : MonoBehaviour
{

    public WeaponManager handGun;

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
        WeaponManager wm = psm.currentPrimary.GetComponent<WeaponManager>();
        if(currentWeapon != previousWeapon)
        {
            updateWeapon(wm);
        }
        if(!wm.isActive)
        {
            loadTime -= Time.deltaTime;

            if(wm.loadOneAtATime)
            {
                if(wm.currentAmmo < wm.maxAmmo && loadTime <= 0)
                {
                    wm.currentAmmo += 1;
                    loadTime = wm.reloadTime / wm.maxAmmo;
                }
            }
            else
            {
                if(loadTime <= 0)
                {
                    wm.currentAmmo = wm.maxAmmo;
                }
            }
        }
        previousWeapon = currentWeapon;
    }

    void updateWeapon(WeaponManager wm)
    {
        if(wm.loadOneAtATime)
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
        if(psm.currentPrimary.GetComponent<WeaponManager>().isActive)
        {
            return handGun;
        }
        else
        {
            return null;
        }
    }
}
