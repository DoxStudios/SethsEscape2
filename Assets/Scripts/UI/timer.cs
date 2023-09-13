using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class timer : MonoBehaviour
{

    public TextMeshProUGUI timerText;
    public GameObject player;
    float time = 0;

    public bool started;
    public bool ended;

    public Color running;
    public Color stopped;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(started && !ended)
        {
            time += Time.deltaTime;
            timerText.text = time.ToString("F2");
            timerText.color = running;
        }

        if(ended)
        {
            timerText.color = stopped;
        }
    }
}
