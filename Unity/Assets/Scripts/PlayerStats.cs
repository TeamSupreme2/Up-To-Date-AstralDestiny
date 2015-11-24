using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerStats : MonoBehaviour {
	public int startingHealth = 100;
    public int currentHealth;
	public int startingPower = 0;
	public int currentPower;
    public Slider healthSlider;
	public Slider powerSlider;
	public int lives = 7;
	public Transform player;
	public Transform destination;
	public GameObject PowerPU;
	public GameObject missile;
	public float deathTimer;

    PlayerControl playerMovement;
    WeaponFire playerShooting;
	//private bool isDead = false;
	//private bool PowerTokenSpawn = false;

    // Use this for initialization
    void Start()
    {
        playerMovement = GetComponent<PlayerControl>();
        playerShooting = GetComponentInChildren<WeaponFire>();

        currentHealth = startingHealth;
		currentPower = startingPower;

		deathTimer = Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
	{

		if (currentHealth == 0) 
		{
			PowerPU.transform.position = playerMovement.transform.position;
			Death ();
			Respawn ();
			PowerPU.SetActive(true);
		}

		if (lives <= 0) 
		{
			Death();

		}

		if (currentPower == 5) 
		{
			MissileSpawn(missile);
		}
		else
		{
			MissileDespawn(missile);
		}
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        healthSlider.value = currentHealth;

		if (currentHealth <= 0)
		{
			lives = lives - 1;
		}
    }

	public void GivePower(int amount)
	{
		currentPower += amount;
		powerSlider.value = currentPower;
	}

	public void GiveHealth(int amount)
	{
		currentHealth += amount;
		healthSlider.value = currentHealth;
	}

    void Death()
    {
		//animator.SetBool("isDead", true);
        //isDead = true;
		//PowerTokenSpawn = true;
        playerMovement.enabled = false;
        playerShooting.enabled = false;

		player.transform.position = destination.position;
    }

	void Respawn()
	{
		//animator.SetBool("isDead", false);
		playerMovement.enabled = true;
		playerShooting.enabled = true;

		currentHealth = startingHealth;
		healthSlider.value = currentHealth;
		currentPower = startingPower;
		powerSlider.value = currentPower;

		GameObject.Instantiate(Resources.Load<GameObject>("bulwarkSpawnSparks"), transform.position, transform.rotation);
	}

	void MissileSpawn(GameObject other)
	{
		if (other.gameObject.tag == "SuperMissile") 
		{
			other.gameObject.SetActive(true);
		}
	}

	void MissileDespawn(GameObject other)
	{
		if (other.gameObject.tag == "SuperMissile") 
		{
			other.gameObject.SetActive(false);
		}
	}

}