using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunPickupText : MonoBehaviour
{

    public WeaponManager gun;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, -100f);
    }

    // Update is called once per frame
    void Update()
    {
        if(gun.state == 0)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 15f);
        }
    }
}
