using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GunUIManager : MonoBehaviour
{
    public TextMeshProUGUI ammoCount; 
    public TextMeshProUGUI gunName;
    public WeaponManager wm;
    public GameObject activeDisplay;
    public GameObject gunImage;

    public Color activeColor;
    public Color inactiveColor;

    int prevAmmo = 0;

    bool equipped = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        int ammo = wm.currentAmmo;

        activeDisplay.SetActive(!(wm.state == 0 || wm.state == 6));

        gunImage.SetActive(wm.state != 6);
        gunImage.GetComponent<RawImage>().texture = wm.gunTexture;
        gunImage.GetComponent<RawImage>().SetNativeSize();
        gunImage.transform.localScale = wm.UIScale;
        gunName.text = wm.title + " " + wm.name;
        ammoCount.gameObject.SetActive(wm.state != 6);

        gunName.color = wm.state == 0 ? inactiveColor : activeColor;
        ammoCount.color = wm.state == 0 ? inactiveColor : activeColor;

        if(ammo != prevAmmo)
        {
            ammoCount.text = ammo.ToString() + " Ammo";
        }

        prevAmmo = ammo;
    }
}
