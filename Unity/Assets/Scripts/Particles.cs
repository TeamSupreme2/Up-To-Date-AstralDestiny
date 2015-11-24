using UnityEngine;
using System.Collections;

public class Particles : MonoBehaviour {

	public float timer;
	// Use this for initialization
	void Start () 
	{
		timer += Time.deltaTime;

	}
	
	// Update is called once per frame
	void Update () 
	{
			Destroy(this.gameObject, timer);
	}
}
