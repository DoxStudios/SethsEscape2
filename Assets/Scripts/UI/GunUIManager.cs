using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class GunUIManager : MonoBehaviour
{
    public TextMeshProUGUI ammoCount; 
    public WeaponManager wm;

    int prevAmmo = 0;
    int maxAmmo;

    // Start is called before the first frame update
    void Start()
    {
        maxAmmo = wm.maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        int ammo = wm.currentAmmo;



        if(ammo != prevAmmo)
        {
            ammoCount.text = ammo.ToString() + "/" + maxAmmo.ToString();
        }

        prevAmmo = ammo;
    }
}
