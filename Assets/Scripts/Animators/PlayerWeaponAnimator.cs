using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponAnimator : MonoBehaviour
{
    WeaponManager wm;
    PlayerStatsManager psm;

    // Start is called before the first frame update
    void Start()
    {
        wm = GetComponent<WeaponManager>();
        psm = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatsManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(wm.tripleImage)
        {
            if(wm.state == 2 && wm.currentFireTime > (wm.fireFrames * 0.5))
            {
                wm.CURRENT = wm.IMAGE_2;
            }
            else if(wm.state == 2 && wm.currentFireTime < (wm.fireFrames *0.5))
            {
                wm.CURRENT = wm.IMAGE_3;
            }
            else
            {
                wm.CURRENT = wm.IMAGE_1;
            }
        }
        else
        {
            if(wm.type == WeaponManager.WeaponType.DoubleShotgun)
            {
                if(wm.state == 2)
                {
                    if(psm.movingLeft)
                    {
                        wm.CURRENT = wm.FlippedImage2;
                    }
                    else
                    {
                        wm.CURRENT = wm.IMAGE_2;
                    }
                }
                else
                {
                    if(psm.movingLeft)
                    {
                        wm.CURRENT = wm.FlippedImage1;
                    }
                    else
                    {
                        wm.CURRENT = wm.IMAGE_1;
                    }
                
                }
            }
            else
            {
                if(wm.state == 2)
                {
                    wm.CURRENT = wm.IMAGE_2;
                }
                else
                {
                    wm.CURRENT = wm.IMAGE_1;
                }
            }
        }
    }
}
