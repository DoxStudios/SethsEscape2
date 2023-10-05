using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public bool paused = false;
    public bool controls = false;
    public GameObject standardView;
    public GameObject pauseView;
    public GameObject controlView;

    public GameObject handgunPickup;
    public GameObject spasmPickup;

    public Vector3 handgun;
    public Vector3 spasm;

    PlayerStatsManager playerStatsManager;
    timer playTimer;

    // Start is called before the first frame update
    void Start()
    {
        playerStatsManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatsManager>();
        playTimer = GameObject.FindGameObjectWithTag("Timer").GetComponent<timer>();

        handgun = GameObject.FindGameObjectWithTag("Handgun").transform.position;
        spasm = GameObject.FindGameObjectWithTag("Spasm").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Pause"))
        {
            if(paused)
            {
                Time.timeScale = 1;
                paused = false;
                controls = false;
            }
            else
            {
                Time.timeScale = 0;
                paused = true;
            }
        }


        standardView.SetActive(!paused);
        pauseView.SetActive(paused && !controls);
        controlView.SetActive(controls);
    }

    public void Resume()
    {
        Time.timeScale = 1;
        paused = false;
    }

    public void EnterControls()
    {
        controls = true;
    }

    public void ExitControls()
    {
        controls = false;
    }

    public void Restart()
    {
        Resume();
        playerStatsManager.lastCheckpoint = GameObject.FindGameObjectWithTag("Spawn").transform;
        playerStatsManager.health = 0;
        playerStatsManager.skipAnimation = true;
        playTimer.time = 0f;
        playTimer.started = false;
        playTimer.ended = false;
        playerStatsManager.primary.GetComponent<WeaponManager>().state = 6;
        playerStatsManager.primary.GetComponent<WeaponManager>().title = "";
        playerStatsManager.primary.GetComponent<WeaponManager>().name = "";
        playerStatsManager.primary.GetComponent<WeaponManager>().currentAmmo = 0;
        playerStatsManager.secondary.GetComponent<WeaponManager>().state = 6;
        playerStatsManager.secondary.GetComponent<WeaponManager>().title = "";
        playerStatsManager.secondary.GetComponent<WeaponManager>().name = "";
        playerStatsManager.secondary.GetComponent<WeaponManager>().currentAmmo = 0;

        GameObject[] weaponDrops = GameObject.FindGameObjectsWithTag("WeaponDrop");
        foreach(GameObject weaponDrop in weaponDrops)
        {
            Destroy(weaponDrop);
        }

        Destroy(GameObject.FindGameObjectWithTag("Handgun"));
        Destroy(GameObject.FindGameObjectWithTag("Spasm"));

        Instantiate(handgunPickup, handgun, transform.rotation);
        Instantiate(spasmPickup, spasm, transform.rotation);
    }

    public void MainMenu()
    {
        Resume();
        SceneManager.LoadScene(0);
    }
}
