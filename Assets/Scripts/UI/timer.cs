using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class timer : MonoBehaviour
{

    public TextMeshProUGUI timerText;
    public GameObject player;
    public float time = 0;

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
        timerText.text = time.ToString("F2");

        if(started && !ended)
        {
            time += Time.deltaTime;
            timerText.color = running;
        }

        if(ended)
        {
            timerText.color = stopped;
        }
    }
}
