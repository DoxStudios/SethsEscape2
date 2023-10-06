using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    public string title;
    public string name;
    public float damage;
    public int burstCount;
    public float burstOffset;
    public float speed;
    public float knockbackMultiplier;
    public float knockbackTimeMultiplier;
    public float stunTimeMultiplier;
    public float bulletSurvivalTime;
    public int pierceLevel;
    public GameObject bullet;
    public Transform firePosition;
    public float maxShotsPerSecond;
    public float fireFrames;
    public int currentAmmo;
    public bool loadOneAtATime;
    public float reloadTime;

    public bool reloadWhileActive = false;
    public int priority;

    public int state = 0;
    
    public GameObject GFX;
    public Sprite CURRENT;
    public Sprite IMAGE_1;
    public Sprite IMAGE_2;
    public Sprite IMAGE_3;
    public Texture gunTexture;

    public bool tripleImage = false;

    public Vector3 secondaryPosition;
    public Vector3 secondaryFirePosition;

    public Vector3 primaryPosition;
    public Vector3 primaryFirePosition;
    public Vector3 GFXScale;
    public Vector3 UIScale;

    public WeaponType type;

    float currentReloadTime;

    float cooldown;
    public float currentFireTime;

    PlayerStatsManager psm;


    public enum WeaponType
    {
        Pistol,
        Shotgun,
        Rifle,
        Explosive,
        DoubleShotgun,
        Revolver,
    };

    void Start()
    {
        psm = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatsManager>();
        CURRENT = IMAGE_1;
    }

    void Update()
    {
        
        if(state == 7)
        {
            return;
        }

        if (psm.movingLeft)
        {
            GFX.GetComponent<SpriteRenderer>().flipX = true;
            GFX.transform.localPosition = secondaryPosition;
            firePosition.localPosition = secondaryFirePosition;
        }
        else
        {
            GFX.GetComponent<SpriteRenderer>().flipX = false;
            GFX.transform.localPosition = primaryPosition;
            firePosition.localPosition = primaryFirePosition;
        }

        if(state == 0 || state == 6)
        {
            GFX.SetActive(false);
        }

        if(state == 1 || state == 4)
        {
            GFX.SetActive(true);
        }

        GFX.GetComponent<SpriteRenderer>().sprite = CURRENT;
        GFX.transform.localScale = GFXScale;
        
        if(state == 3)
        {
            cooldown -= Time.deltaTime;
            if(cooldown <= 0)
            {
                if(currentAmmo > 0)
                {
                    state = 1;
                }
                else
                {
                    state = 4;
                }
            }
        }

        if(state == 2)
        {
            currentFireTime -= Time.deltaTime;
            if(currentFireTime <= 0)
            {
                state = 3;
            }
        }

        if(reloadWhileActive && Input.GetButtonDown("ReloadHandgun") && state != 0 && state != 6)
        {
            Debug.Log("Reload");
            state = 5;
            currentReloadTime = reloadTime;
        }
    }

    public void Fire(float damageMultiplier, int addedPierce)
    {
        if(state != 1) return;
        if(Time.timeScale == 0) return;
        state = 2;
        currentFireTime = fireFrames;
        currentAmmo -= 1;
        cooldown = 1 / maxShotsPerSecond;

        int bulletsFired = 0;

        while(bulletsFired < burstCount)
        {
            bulletsFired += 1;

            Vector3 mousePos = Input.mousePosition;
            mousePos.z = mousePos.z - (Camera.main.transform.position.z);
            Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
            Vector3 direction = worldMousePos - firePosition.position;
            direction = new Vector3(direction.x + Random.Range(-burstOffset, burstOffset), direction.y + Random.Range(-burstOffset, burstOffset), direction.z);
            direction.Normalize();

            float finalDamage = damage * damageMultiplier;
            int finalPierce = pierceLevel + addedPierce;

            GameObject firedBullet = Instantiate(bullet, firePosition.position, Quaternion.identity);
            firedBullet.GetComponent<Rigidbody2D>().velocity = direction * speed;
            BulletManager bm = firedBullet.GetComponent<BulletManager>();
            bm.damage = finalDamage;
            bm.knockbackTimeMultiplier = knockbackTimeMultiplier;
            bm.knockbackMultiplier = knockbackMultiplier;
            bm.stunTimeMultiplier = stunTimeMultiplier;
            bm.pierceLevel = finalPierce;
            bm.psm = psm;
            bm.survivalTime = bulletSurvivalTime;
        }
    }
}
