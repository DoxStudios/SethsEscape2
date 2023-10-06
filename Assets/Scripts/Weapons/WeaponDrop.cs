using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDrop : MonoBehaviour
{

    public GameObject weaponPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DropWeapon()
    {
        GameObject weapon = Instantiate(weaponPrefab, transform.position, transform.rotation);
        WeaponManager wm = weapon.GetComponent<WeaponManager>();

        wm.damage += Random.Range(-5, 16);

        if(wm.burstCount != 1)
        {
            wm.burstCount += Random.Range(-1, 2);
            wm.burstOffset += Random.Range(-1, 2);
        }

        wm.speed += Random.Range(-10, 21);
        wm.maxShotsPerSecond += Random.Range(0, 2);
        if(wm.type == WeaponManager.WeaponType.Revolver)
        {
            wm.currentAmmo += Random.Range(0, 3);
        }   
        else
        {
            wm.currentAmmo += Random.Range(-2, 10);
        }

        Title title = new Title();
        Title.titles t = title.GenerateTitle();

        wm.title = title.GetTitle(t);
        wm = title.ApplyTitle(wm, t);
    }
}


public class Title
{
    public enum titles
    {
        Loaded,
        Strengthened,
        Rapid,
        Lucky,
        None
    }

    public string GetTitle(titles title)
    {
        switch (title)
        {
            case titles.Loaded:
                return "Loaded";
            case titles.Strengthened:
                return "Strengthened";
            case titles.Rapid:
                return "Rapid";
            case titles.Lucky:
                return "Lucky";
            default:
                return "";
        }
    }

    public titles GenerateTitle()
    {
        int title = Random.Range(0, 3);
        switch (title)
        {

            case 0:
                return titles.Strengthened;
            case 1:
                return titles.Rapid;
            case 2:
                return titles.Lucky;
            default:
                return titles.None;
        }
    }

    public WeaponManager ApplyTitle(WeaponManager wm, titles title)
    {
        switch (title)
        {
            case titles.Strengthened:
                switch (wm.type)
                {
                    case WeaponManager.WeaponType.Pistol:
                        wm.damage += Random.Range(15, 26);
                        break;
                    case WeaponManager.WeaponType.Revolver:
                        wm.damage += Random.Range(15, 26);
                        break;
                    case WeaponManager.WeaponType.Shotgun:
                        wm.damage += Random.Range(20, 41);
                        break;
                    case WeaponManager.WeaponType.Rifle:
                        wm.damage += Random.Range(30, 51);
                        break;
                    case WeaponManager.WeaponType.Explosive:
                        wm.damage += Random.Range(60, 76);
                        break;
                    case WeaponManager.WeaponType.DoubleShotgun:
                        wm.damage += Random.Range(50, 101);
                        break;
                }

                break;
            case titles.Rapid:
                switch (wm.type)
                {
                    case WeaponManager.WeaponType.Pistol:
                        wm.maxShotsPerSecond += Random.Range(3, 6);
                        wm.currentAmmo += Random.Range(5, 11);
                        break;
                    case WeaponManager.WeaponType.Revolver:
                        wm.maxShotsPerSecond += Random.Range(3, 6);
                        break;
                    case WeaponManager.WeaponType.Shotgun:
                        wm.maxShotsPerSecond += Random.Range(2,4);
                       wm.currentAmmo += Random.Range(2,6);
                        break;
                    case WeaponManager.WeaponType.Rifle:
                        wm.maxShotsPerSecond += Random.Range(2,4);
                        wm.currentAmmo += Random.Range(5, 8);
                        break;
                    case WeaponManager.WeaponType.Explosive:
                        wm.maxShotsPerSecond += Random.Range(1, 3);
                        wm.currentAmmo += Random.Range(1, 3);
                        break;
                    case WeaponManager.WeaponType.DoubleShotgun:
                        wm.maxShotsPerSecond += 100;
                        wm.fireFrames = 0;
                        break;
                }

                break;
            case titles.Lucky:
                switch (wm.type)
                {
                    case WeaponManager.WeaponType.Pistol:
                        wm.damage += Random.Range(10, 16);
                        wm.maxShotsPerSecond += 2;
                        break;
                    case WeaponManager.WeaponType.Revolver:
                        wm.damage += Random.Range(10, 16);
                        wm.maxShotsPerSecond += 2;
                        break;
                    case WeaponManager.WeaponType.Shotgun:
                        wm.damage += Random.Range(10, 31);
                        wm.burstCount += 2;
                        wm.burstOffset -= 1.5f;
                        wm.maxShotsPerSecond += 3;
                        break;
                    case WeaponManager.WeaponType.Rifle:
                        wm.damage += Random.Range(20, 41);
                        wm.maxShotsPerSecond += 3;
                        break;
                    case WeaponManager.WeaponType.Explosive:
                        wm.damage += Random.Range(50, 66);
                        wm.maxShotsPerSecond += 2;
                        break;
                    case WeaponManager.WeaponType.DoubleShotgun:
                        wm.damage += Random.Range(45, 66);
                        wm.maxShotsPerSecond += 300;
                        wm.fireFrames = 0;
                        wm.burstCount += 5;
                        wm.burstOffset += 0.5f;
                        break;
                }

                break;
            default:
                break;
        }

        if(wm.type == WeaponManager.WeaponType.DoubleShotgun)
        {
            wm.currentAmmo = 2;
            
            int chance = Random.Range(1, 21);
            if(chance == 1)
            {
                wm.currentAmmo = 3;
            }
        }

        return wm;
    }
}
