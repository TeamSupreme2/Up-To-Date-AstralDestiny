using UnityEngine;
using System.Collections;

public class PowerPickUp : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.Rotate (new Vector3 (0, 20, 0) * Time.deltaTime * 6);
		transform.Translate (new Vector3 (0, Mathf.Sin (Time.time * 8), 0) * 0.001f) ;
	}
}
