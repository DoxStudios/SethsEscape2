using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunpickup : MonoBehaviour
{

    public WeaponManager weapon;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            if(weapon.state == 6)
            {
                weapon.state = 0;
            }

            Destroy(gameObject);
        }
    }
}
