using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponAnimator : MonoBehaviour
{
    WeaponManager wm;

    // Start is called before the first frame update
    void Start()
    {
        wm = GetComponent<WeaponManager>();
    }

    // Update is called once per frame
    void Update()
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
