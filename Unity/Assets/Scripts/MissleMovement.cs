using UnityEngine;
using System.Collections;

public class MissleMovement : MonoBehaviour {
	public int missileSpeed;
	public int damagePerShot = 15;
	public GameObject rocketExplosion;

	private Rigidbody2D rb;
	//private PlayerHealth playerHealth;
	// Use this for initialization
	void Start () 
	{
		rb = GetComponent<Rigidbody2D> ();
		//playerHealth = playerHealth.GetComponentsInParent<PlayerHealth>();
		//weaponFire = GetComponent<WeaponFire> ();
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		Camera.main.transform.Translate (new Vector3 (Mathf.Sin (Time.time * 8) * Time.deltaTime, 0, 0));
		if(collision.gameObject.tag == "Player 2" || collision.gameObject.tag == "Player 1")
		{
			GameObject otherPlayer = collision.transform.gameObject;
			PlayerStats other = otherPlayer.GetComponent<PlayerStats>();
			Debug.Log("Hit! " + collision.collider.name);
			if(other.currentHealth > 0)
			{
				other.TakeDamage(damagePerShot);
			}
		}
		GameObject.Instantiate(Resources.Load<GameObject>("RocketExplosion"), transform.position, transform.rotation);

		Destroy (this.gameObject);
	}
	// Update is called once per frame
	void Update () 
	{
		rb.AddForce (transform.forward * missileSpeed, ForceMode2D.Force);
	}
}
