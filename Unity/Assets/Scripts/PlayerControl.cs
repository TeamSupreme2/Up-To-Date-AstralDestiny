using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using InControl;

public class PlayerControl : MonoBehaviour {
    public float moveSpeed = 0.0f;
    public float jumpHeight = 0.0f;
    public int playerIndex;
	public GameObject healthPU;
	public GameObject ammoPU;
	public GameObject powerPU;

	Vector2 movement;

    private Rigidbody2D _rigidbody;
	private PlayerStats playerHealth;
	private WeaponFire weaponFire;
    private Animator animator;
    private bool _grounded;


	private float healthTimer = 0.0f;
	private float ammoTimer = 0.0f;
	private bool healthShouldRespawn = false;
	private bool ammoShouldRespawn = false;


	// Use this for initialization
	void Start () 
    {
        _rigidbody = GetComponent<Rigidbody2D>();
		playerHealth = GetComponent<PlayerStats> ();
		weaponFire = GetComponentInChildren<WeaponFire> ();
        animator = GetComponent<Animator>();

	}

	void Update()
	{
		if (healthShouldRespawn) 
		{
			healthTimer += Time.deltaTime;

			if (healthTimer > 15.0f) 
			{
				healthPU.SetActive(true);
				healthTimer = 0.0f;
			}
		}

		if (ammoShouldRespawn) 
		{
			ammoTimer += Time.deltaTime;

			if(ammoTimer > 8.0f)
			{
				ammoPU.SetActive(true);
				ammoTimer = 0.0f;
			}
		}
	}
	// Update is called once per frame
	void FixedUpdate () 
	{	
		if (InputManager.Devices.Count <= playerIndex) 
		{
			return;
		}

		InputDevice device = InputManager.Devices[playerIndex];

		
		_grounded = false;
		animator.SetBool("isGrounded", false);
		RaycastHit2D[] Hits =
			Physics2D.CircleCastAll(transform.position, 0.01f + 0.01f, Vector2.down, 0.1f);
		
		foreach(RaycastHit2D hit in Hits)
		{
			if(hit.normal.y > 0.01f && hit.rigidbody != _rigidbody)
			{
				animator.SetBool("isGrounded", true);
				_grounded = true;

			}
		}
		
		Vector2 velocity = _rigidbody.velocity;
		animator.SetBool ("isJumping", false);
		if (device.LeftStickX.Value < -0.1) 
		{
			velocity.x = -moveSpeed; 
			animator.SetBool ("isWalking", true);
			transform.rotation = Quaternion.Euler (0, 180, 0);
		} 
		else if (device.LeftStickX.Value > 0.1) 
		{
			velocity.x = moveSpeed;
			animator.SetBool ("isWalking", true);
			transform.rotation = Quaternion.Euler (0, 0, 0);
		} 
		else 
		{
			animator.SetBool ("isWalking", _grounded = false);
		}
		if (_grounded) 
		{
			if (device.LeftBumper.WasPressed) 
			{
				velocity.y = jumpHeight;
				animator.SetBool ("isJumping", true);
				GameObject.Instantiate (Resources.Load<GameObject> ("bulwarkLandingPuff"), transform.position, transform.rotation);
					_grounded = false;
			}
		}
		_rigidbody.velocity = velocity;
	}

	//Pickups
	void OnTriggerEnter2D(Collider2D other)
	{
		//Health Token
		int addHealth = 10;
		if(other.gameObject.tag == "HealthPU")
		{
			if(playerHealth.currentHealth < 100)
			{
				playerHealth.GiveHealth(addHealth);
				other.gameObject.SetActive(false);
				healthShouldRespawn = true;
			}
		}
		//Ammo Token
		int addAmmo = 2;
		if (other.gameObject.tag == "AmmoPU")
		{
			if(weaponFire.ammo >= 0)
			{
				weaponFire.GiveAmmo(addAmmo);
				other.gameObject.SetActive(false);
				ammoShouldRespawn = true;
			}
		}

		//Power Token
		int addToken = 1;
		if (other.gameObject.tag == "PowerPU")
		{
			if(playerHealth.currentPower < 5)
			{
				playerHealth.GivePower(addToken);
				other.gameObject.SetActive(false);
			}
		}
	}
}
