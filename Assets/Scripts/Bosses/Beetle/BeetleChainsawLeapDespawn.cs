using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeetleChainsawLeapDespawn : MonoBehaviour
{

    BeetleUtils beetleUtils;

    // Start is called before the first frame update
    void Start()
    {
        beetleUtils = GetComponentInParent<BeetleUtils>();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Ground" || col.gameObject.tag == "Bullet")
        {
            Debug.Log(col.gameObject.tag);
            beetleUtils.ShootChainsaw(col.transform.position, transform);
            beetleUtils.ChainsawLeapHitbox(false);
        }
    }

    void OnTriggerEnter2D()
    {
        //Debug.Log(col.gameObject.tag);
        beetleUtils.ShootChainsaw(transform.position, transform);
        beetleUtils.ChainsawLeapHitbox(false);
    }
}
