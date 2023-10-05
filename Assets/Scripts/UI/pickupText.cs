using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class pickupText : MonoBehaviour
{

    PlayerStatsManager psm;
    TextMeshProUGUI text; 

    // Start is called before the first frame update
    void Start()
    {
        psm = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatsManager>();
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(psm == null)
        {
            Debug.LogWarning("PlayerStatsManager not found");
        }
        if(psm.pickupTarget == null)
        {
            text.text = "";
        }
        else
        {
            WeaponManager wm = psm.pickupTarget.GetComponent<WeaponManager>();
            string pickupText = wm.title + " " + wm.name + "\nE to Eat";

            transform.position = psm.pickupTarget.transform.position + new Vector3(0, 6, 0);
            text.text = pickupText;
        }
    }
}