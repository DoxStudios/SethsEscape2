using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class GunUIManager : MonoBehaviour
{
    public TextMeshProUGUI ammoCount; 
    public WeaponManager wm;
    public GameObject activeDisplay;

    int prevAmmo = 0;
    int maxAmmo;

    bool equipped = false;

    // Start is called before the first frame update
    void Start()
    {
        maxAmmo = wm.maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        int ammo = wm.currentAmmo;

        activeDisplay.SetActive(!(wm.state == 0 || wm.state == 6));

        if(ammo != prevAmmo)
        {
            ammoCount.text = ammo.ToString();
        }

        prevAmmo = ammo;
    }
}
