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


	//Sound
	

	// Use this for initialization
	void Start () 
    {
        _rigidbody = GetComponent<Rigidbody2D>();
		playerHealth = GetComponent<PlayerStats> ();
		weaponFire = GetComponentInChildren<WeaponFire> ();
		animator = GetComponent<Animator> ();

	}

	void Update()
	{

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

		//Raycast to detect if he is grounded
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

		//Character movement
		if (device.LeftStickX.Value < -0.1) 
		{
			velocity.x = -moveSpeed; 
			animator.SetBool ("isWalking", true);
			transform.rotation = Quaternion.Euler (0, 0, 0);
		} 
		else if (device.LeftStickX.Value > 0.1) 
		{
			velocity.x = moveSpeed;
			animator.SetBool ("isWalking", true);
			transform.rotation = Quaternion.Euler (0, 180, 0);
		} 
		else 
		{
			animator.SetBool ("isWalking", _grounded = false);
		}

		//Allow the player to jump only if he is grounded
		if (_grounded) 
		{
			if (device.LeftBumper.WasPressed || device.Action1.WasPressed) 
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
		int addHealth = 15;
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
		int addAmmo = 5;
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

	public void HealthToken()
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
	}

	public void AmmoToken()
	{
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
}
