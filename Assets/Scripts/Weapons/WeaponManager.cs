using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    public string title;
    public string name;
    public float damage;
    public float contDmg;
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
    public bool gravity;
    public float currentFireTime;
    public float explosivePower;
    public float explosiveRange;

    PlayerStatsManager psm;


    public enum WeaponType
    {
        Pistol,
        Shotgun,
        Rifle,
        Explosive,
        DoubleShotgun,
        Revolver,
        Chainsaw,
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
                    if(type == WeaponType.Chainsaw)
                    {
                        state = 6;
                    }
                    else
                    {
                        state = 4;
                    }
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

        if(GetComponent<AudioSource>() !=null)
        {
            GetComponent<AudioSource>().Play();
        }

        int bulletsFired = 0;

        while(bulletsFired < burstCount)
        {
            bulletsFired += 1;
            Vector3 direction;
            if(psm.useAim)
            {
                direction = psm.aim;
                direction = new Vector3(direction.x, direction.y, 0);
                direction = Quaternion.AngleAxis(Random.Range(-burstOffset, burstOffset), Vector3.up) * direction;
                direction.Normalize();
            }
            else
            {
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = mousePos.z - (Camera.main.transform.position.z);
                Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
                direction = worldMousePos - firePosition.position;
                direction = Quaternion.AngleAxis(Random.Range(-burstOffset, burstOffset), Vector3.forward) * direction;
                direction.Normalize();
            }

            float finalDamage = damage * damageMultiplier;
            int finalPierce = pierceLevel + addedPierce;

            GameObject firedBullet = Instantiate(bullet, firePosition.position, Quaternion.identity);
            firedBullet.GetComponent<Rigidbody2D>().velocity = direction * speed;
            if(gravity)
            {
                firedBullet.GetComponent<Rigidbody2D>().gravityScale = 36;
            }
            if(type == WeaponType.Explosive)
            {
                ExplosivesManager em = firedBullet.GetComponent<ExplosivesManager>();
                em.damage = finalDamage;
                em.knockbackTimeMultiplier = knockbackTimeMultiplier;
                em.knockbackMultiplier = knockbackMultiplier;
                em.stunTimeMultiplier = stunTimeMultiplier;
                em.pierceLevel = finalPierce;
                em.psm = psm;
                em.survivalTime = bulletSurvivalTime;
                em.explosivePower = explosivePower;
                em.explosiveRange = explosiveRange;

            }
            else if(type == WeaponType.Chainsaw)
            {
                ChainsawMovement cm = firedBullet.GetComponentInChildren<ChainsawMovement>();
                ChainsawDamage cd = firedBullet.GetComponent<ChainsawDamage>();
                cm.speed = speed;
                cd.initDmg = finalDamage;
                cd.contDmg = contDmg;
                cd.psm = psm;

                state = 6;
            }
            else
            {
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
}
