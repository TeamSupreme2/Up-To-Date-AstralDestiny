using UnityEngine;
using UnityEngine.UI;
using InControl;
using System.Collections;

public class WeaponFire : MonoBehaviour {
    public float fireRate = 0.5f;
	public int ammo = 10;
    public float range = 100f;
    public int playerIndex;
    public LayerMask WhatToHit;
	public Text ammoText;
	public Slider ammoCD;
	public GameObject projectile;
	public GameObject superMissile;
	public Transform origin;

	//Sound
	public AudioClip missileMove;
	private AudioSource source;
	private float volLowRange = 0.5f;
	private float volHighRange = 1.0f;


	//private Ray hit;
    private float timer;
    Transform firePoint;
	private Vector2 aimPosition;
	private Vector2 firePointPosition;
	private LineRenderer lr;
	
	// Use this for initialization
	void Start () 
    {
		aimPosition = new Vector2 (1, 0);
        firePoint = transform.FindChild("FirePoint");
        if(firePoint == null)
        {
            Debug.LogError("No FirePoint Found");
        }
		SetCountText ();

		lr = GetComponent<LineRenderer> ();
		source = GetComponent<AudioSource> ();

	}
	
	// Update is called once per frame
	void Update () 
	{	
		if (InputManager.Devices.Count <= playerIndex) 
		{
			return;
		}
		InputDevice device = InputManager.Devices [playerIndex];

		DrawLine ();
		if (device.RightStickX.Value >= 0.1 || device.RightStickY.Value >= 0.1 || device.RightStickX.Value <= -0.1 || device.RightStickY.Value <= -0.1) 
		{
			aimPosition = new Vector2 (device.RightStick.Vector.x, device.RightStick.Vector.y).normalized;
		} 
		//else 
		//{
		//	aimPosition = new Vector2 (device.RightStick.Vector.x, device.RightStick.Vector.y);
		//}

        timer += Time.deltaTime;


		//Regular Rocket shoot
        if (device.RightBumper.WasPressed && timer >= fireRate && ammo != 0)
        {
            Shoot();
			transform.forward = new Vector3 (aimPosition.x, aimPosition.y, 0) - transform.position;
			Instantiate (projectile, firePointPosition + (aimPosition/2), Quaternion.LookRotation(aimPosition)); 

			float vol = Random.Range(volLowRange, volHighRange);
			source.PlayOneShot(missileMove, vol);

			ammo = ammo - 1;
			SetCountText();
			ammoCD.value = fireRate;
		}
		ammoCD.value = timer;

		if (projectile.transform.position.x > range) 
		{
			Destroy(projectile);
		}

		PlayerStats playerHealth = GetComponentInParent<PlayerStats> ();

		//Super shoot
		if (device.RightTrigger.WasPressed && playerHealth.currentPower == 5)
		{
			Shoot ();
			transform.forward = new Vector3(aimPosition.x, aimPosition.y, 0) - transform.position;
			Instantiate(superMissile, firePointPosition + aimPosition, Quaternion.LookRotation(aimPosition));
			playerHealth.currentPower = 0;
			playerHealth.powerSlider.value = 0;
		}

		if (superMissile.transform.position.x > range) 
		{
			Destroy (superMissile);
		}
	}

	public void GiveAmmo(int amount)
	{
		ammo += amount;
		SetCountText();
	}

    void Shoot()
    {
		timer = 0.0f;

		firePointPosition = new Vector2 (firePoint.position.x, firePoint.position.y);

	}
	void SetCountText()
	{
		ammoText.text = "Ammo: " + ammo.ToString ();
	}

	void DrawLine()
	{
		lr.SetPosition (0, origin.position);
		lr.SetPosition(1, new Vector3(transform.position.x + aimPosition.x * range,transform.position.y+aimPosition.y * range,transform.position.z));
	}
}