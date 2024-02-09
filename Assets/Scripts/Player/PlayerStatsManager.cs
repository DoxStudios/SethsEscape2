using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PlayerStatsManager : MonoBehaviour
{
	public float damage;
	public float defense;
	public float rangedDamage;
	public float outgoingKnockbackAmount;
	public float outgoingKnockbackTime;
	public float outgoingStunTime;


	public bool movingLeft = false;
	public bool stunned = false;
	public bool explosiveStun;
	public bool dead = false;
	public float health = 100;
	public bool inWall = false;
	public int addedPierce = 0;
	public GameObject primary;
	public GameObject secondary;
	public GameObject headCannon;
	public GameObject currentPrimary;
	public Slider slider;
	public timer playTimer;
	public GameObject pickupTarget = null;

	public bool useAim = false;
	public Vector2 aim;

	GameObject currentSecondary;
	float maxHealth = 100;
	GameObject canvas;
	float prevHealth;
	Rigidbody2D rb;
	Transform sprite;
	public Transform lastCheckpoint;
	public bool skipAnimation = false;
	int dropChance = 30;

	bool alternateDropSlot = false;


	string currentWeapon = "No";

	public void Eat(InputAction.CallbackContext context)
	{
		if(context.performed)
		{
			if(pickupTarget != null)
			{
				if(pickupTarget.GetComponent<gunpickup>() == null)
				{
					if(primary.GetComponent<WeaponManager>().state != 6 && secondary.GetComponent<WeaponManager>().state != 6)
					{
						AddWeapon(pickupTarget.GetComponent<GunBall>().weapon, true);
						pickupTarget = null;
					}
				}
				else
				{
					AddWeapon(pickupTarget.GetComponent<gunpickup>().weapon, false);
					Destroy(pickupTarget);
					pickupTarget = null;
				}
			}
		}
	}
	

	public bool dropWeapon(float multiplier)
	{
		int percent = Random.Range(1, 101);
		
		if(percent <= (dropChance * multiplier))
		{
			dropChance = 30;
			return true;
		}
		else
		{
			dropChance += 10;
			return false;
		}
	}

	public void AddWeapon(WeaponManager weapon, bool gunBall)
	{
		GameObject slot;

		if(gunBall)
		{
			slot = GetGunBallSlot(weapon);
		}
		else
		{
			slot = GetSlot();
		}

		WeaponManager weaponSlot = slot.GetComponent<WeaponManager>();
		slot.GetComponent<AudioSource>().clip = weapon.gameObject.GetComponent<AudioSource>().clip;

		weaponSlot.damage = weapon.damage;
		weaponSlot.burstCount = weapon.burstCount;
		weaponSlot.burstOffset = weapon.burstOffset;
		weaponSlot.speed = weapon.speed;
		weaponSlot.knockbackMultiplier = weapon.knockbackMultiplier;
		weaponSlot.knockbackTimeMultiplier = weapon.knockbackTimeMultiplier;
		weaponSlot.stunTimeMultiplier = weapon.stunTimeMultiplier;
		weaponSlot.bulletSurvivalTime = weapon.bulletSurvivalTime;
		weaponSlot.pierceLevel = weapon.pierceLevel;
		weaponSlot.bullet = weapon.bullet;
		weaponSlot.maxShotsPerSecond = weapon.maxShotsPerSecond;
		weaponSlot.fireFrames = weapon.fireFrames;
		weaponSlot.currentAmmo = weapon.currentAmmo;
		weaponSlot.loadOneAtATime = weapon.loadOneAtATime;
		weaponSlot.reloadTime = weapon.reloadTime;
		weaponSlot.reloadWhileActive = weapon.reloadWhileActive;
		weaponSlot.priority = weapon.priority;
		weaponSlot.secondaryPosition = weapon.secondaryPosition;
		weaponSlot.secondaryFirePosition = weapon.secondaryFirePosition;
		weaponSlot.primaryPosition = weapon.primaryPosition;
		weaponSlot.primaryFirePosition = weapon.primaryFirePosition;
		weaponSlot.IMAGE_1 = weapon.IMAGE_1;
		weaponSlot.IMAGE_2 = weapon.IMAGE_2;
		weaponSlot.IMAGE_3 = weapon.IMAGE_3;
		weaponSlot.GFXScale = weapon.GFXScale;
		weaponSlot.gunTexture = weapon.gunTexture;
		weaponSlot.UIScale = weapon.UIScale;
		weaponSlot.title = weapon.title;
		weaponSlot.name = weapon.name;
		weaponSlot.tripleImage = weapon.tripleImage;
		weaponSlot.type = weapon.type;
		weaponSlot.explosivePower = weapon.explosivePower;
		weaponSlot.explosiveRange = weapon.explosiveRange;

		weaponSlot.state = 0;
	}

	public GameObject GetSlot()
	{
		if(primary.GetComponent<WeaponManager>().currentAmmo == 0)
		{
			return primary;
		}
		
		if(secondary.GetComponent<WeaponManager>().currentAmmo == 0)
		{
			return secondary;
		}

		return currentPrimary;
	}

	public GameObject GetGunBallSlot(WeaponManager wm)
	{
		if(primary.GetComponent<WeaponManager>().currentAmmo == 0)
		{
			return primary;
		}
		
		if(secondary.GetComponent<WeaponManager>().currentAmmo == 0)
		{
			return secondary;
		}

		if(currentPrimary.GetComponent<WeaponManager>().state != 0)
		{
			return currentPrimary;
		}

		if(primary.GetComponent<WeaponManager>().name != wm.name)
		{
			return primary;
		}

		if(secondary.GetComponent<WeaponManager>().name != wm.name)
		{
			return secondary;
		}

		alternateDropSlot = !alternateDropSlot;
		if(!alternateDropSlot)
		{
			return currentPrimary;
		}
		else
		{
			if(currentPrimary == primary)
			{
				return secondary;
			}
			else
			{
				return primary;
			}
		}
	}

	public void Heal(float amount)
	{
		health += amount;
		if(health > maxHealth)
		{
			health = maxHealth;
		}
	}


	public void Damage(float amount, Transform knockbackPosition, float knockbackAmount, float knockbackTime, float stunTime)
	{
		if(!dead && !stunned)
		{
			health -= (amount) * defense;
			DealKnockback(knockbackPosition, knockbackAmount, knockbackTime, stunTime);
		}
	}

	public void PlayerExplosive(float amount, Transform knockbackPosition, float knockbackAmount)
	{
		if(!dead && !stunned)
		{
			health -= (amount) * defense;
			DealExplosiveKnockback(knockbackPosition, knockbackAmount);
		}
	}

	public void DamageWithoutKnockback(float amount)
	{
		if(!dead && !stunned)
		{
			health -= amount;
		}
	}

	void Start()
	{
		currentPrimary = primary;
		currentSecondary = headCannon;
		rb = GetComponent<Rigidbody2D>();
		canvas = GameObject.FindGameObjectsWithTag("Canvas")[0];
		lastCheckpoint = GameObject.FindGameObjectWithTag("Spawn").transform;
		transform.position = lastCheckpoint.position;
		sprite = transform.Find("sprite");

		Spawn();
	}

	void Spawn()
	{

		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		foreach(GameObject enemy in enemies)
		{
			Destroy(enemy);
		}

		GameObject[] spawners = GameObject.FindGameObjectsWithTag("Spawner");
		foreach(GameObject spawner in spawners)
		{
			spawner.GetComponent<Spawner>().Spawn();
		}
	}

	void Update()
	{

		if(Input.GetKeyDown("k"))
		{
			health = 0;
			skipAnimation = true;
		}

		SelectSecondaryWeapon();
		AimWeapon();


		UpdateHealth();
	}

	void ActivateWeapon(GameObject weapon)
	{
		WeaponManager cpwm = currentPrimary.GetComponent<WeaponManager>();
		if((cpwm.state == 1 || cpwm.state == 4 || cpwm.state == 0 || cpwm.state == 6) && weapon.GetComponent<WeaponManager>().state != 6)
		{
			if(cpwm.state != 6)
			{
				currentPrimary.GetComponent<WeaponManager>().state = 0;
			}
			currentPrimary = weapon;
			if(currentPrimary.GetComponent<WeaponManager>().currentAmmo <= 0)
			{
				currentPrimary.GetComponent<WeaponManager>().state = 4;
			}
			else
			{
				currentPrimary.GetComponent<WeaponManager>().state = 1;
			}
		}
	}

	public void ControlsChanged(PlayerInput input)
	{
		if(input.currentControlScheme == "Keyboard&Mouse")
		{
			useAim = false;
		}
		else
		{
			useAim = true;
		}
	}

	public void Aim_Vertical(InputAction.CallbackContext context)
	{
		if(context.performed)
		{
			aim.y = context.ReadValue<float>();
		}
	}

	public void Aim_Horizontal(InputAction.CallbackContext context)
	{
		if(context.performed)
		{
			aim.x = context.ReadValue<float>();
		}
	}

	public void Fire(InputAction.CallbackContext context)
	{
		if(context.performed)
		{
			currentPrimary.GetComponent<WeaponManager>().Fire(rangedDamage, addedPierce);
		}
	}

	public void Primary(InputAction.CallbackContext context)
	{
		ActivateWeapon(primary);
	}

	public void Secondary(InputAction.CallbackContext context)
	{
		ActivateWeapon(secondary);
	}

	public void Cycle(InputAction.CallbackContext context)
	{
		if(context.performed)
		{
			if(currentPrimary.GetComponent<WeaponManager>().state == 0 || currentPrimary.GetComponent<WeaponManager>().state == 6)
			{
				ActivateWeapon(primary);
			}
			else if(currentPrimary == primary)
			{
				ActivateWeapon(secondary);
			}
			else if(currentPrimary == secondary)
			{
				WeaponManager cpwm = currentPrimary.GetComponent<WeaponManager>();
				if(cpwm.state == 1 || cpwm.state == 4)
				{
					cpwm.state = 0;
				}
			}
		}
	}


	void SelectSecondaryWeapon()
	{
		if(Input.GetButtonDown("headCannon"))
		{
			Debug.Log("Head Cannon");
			currentSecondary.SetActive(false);
			//currentSecondary = headCannon;
			currentSecondary.SetActive(true);
		}
		if(Input.GetButtonDown("laserEyes"))
		{
			Debug.Log("Laser Eyes");
			currentSecondary.SetActive(false);
			//currentSecondary = laserEyes;
			currentSecondary.SetActive(true);
		}
	}

	void AimWeapon()
	{
		//TODO: Acutally make it
	}

	void UpdateHealth()
	{
		if(health != prevHealth)
		{
			if(health <= 0)
			{
				dead = true;
				stunned = true;
				rb.gravityScale = 0f;
			}

			slider.value = health;
		}
		prevHealth = health;

		if(dead)
		{
			sprite.GetComponent<SpriteRenderer>().flipX = false;
			sprite.Rotate(Vector3.forward * 60 * Time.deltaTime);
			sprite.localScale += new Vector3(-10, -10, 0) * Time.deltaTime;
			if(currentPrimary.GetComponent<WeaponManager>().state != 6)
			{
				currentPrimary.GetComponent<WeaponManager>().state = 0;
			}
			//currentSecondary.SetActive(false);

			if(sprite.localScale.x < -22 || skipAnimation)
			{
				setRight();
				transform.position = lastCheckpoint.position;
				sprite.rotation = Quaternion.Euler(0, 0, 0);
				sprite.localScale = new Vector3(1, 1, 1);
				dead = false;
				stunned = false;
				health = maxHealth;
				rb.constraints = RigidbodyConstraints2D.FreezeRotation;
				rb.gravityScale = 15f;
				inWall = false;
				skipAnimation = false;
				DoorManager[] doors = FindObjectsOfType<DoorManager>();
				foreach(DoorManager door in doors)
				{
					door.ResetArena();
				}
				//currentSecondary.SetActive(true);
				Spawn();
			}
		}
	}

	public void setLeft()
	{
		movingLeft = true;
	}

	public void setRight()
	{
		movingLeft = false;
	}

	public void DealKnockback(Transform sender, float knockbackAmount, float knockbackTime, float stunTime)
	{
		StopAllCoroutines();
		Vector2 direction = (transform.position-sender.position).normalized;
		rb.AddForce(direction*knockbackAmount*100, ForceMode2D.Impulse);
		stunned = true;
		StartCoroutine(ResetKnockback(knockbackTime));
		StartCoroutine(ResetStun(stunTime));
	}

	public void DealExplosiveKnockback(Transform sender, float knockbackAmount)
	{
		StopAllCoroutines();
		Vector2 direction = (transform.position-sender.position).normalized;
		rb.AddForce(direction*knockbackAmount*100, ForceMode2D.Impulse);
		explosiveStun = true;
		stunned = true;
		StartCoroutine(ResetStun(0.3f));
	}

	private IEnumerator ResetKnockback(float delay)
	{
		yield return new WaitForSeconds(delay);
		Vector3 minimizedVelocity = new Vector2(rb.velocity.x/5, rb.velocity.y/5);
		rb.velocity = minimizedVelocity;
	}

	private IEnumerator ResetStun(float delay)
	{
		yield return new WaitForSeconds(delay);
		if(!dead)
		{
			stunned = false;
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.tag == "Checkpoint" || col.gameObject.tag == "Spawn")
		{
			lastCheckpoint = col.gameObject.transform;
		}

		if(col.gameObject.tag == "Endpoint")
		{
			playTimer.ended = true;
		}
	}
}
