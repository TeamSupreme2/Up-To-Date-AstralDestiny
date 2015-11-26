using UnityEngine;
using System.Collections;

public class RandomSpawn : MonoBehaviour {

	public Transform[] spawnPoints;
	//public GameObject ammoToken;
	//public GameObject healthToken;
	public GameObject[] pickUps;

	public float timer;
	public float respawnTimer;
	public float destroyTimer;

	// Use this for initialization
	void Start () 
	{
		InvokeRepeating ("SpawnTokens", timer, respawnTimer);

	}
	
	// Update is called once per frame
	void Update () 
	{
		destroyTimer += Time.deltaTime;
		timer += Time.deltaTime;
	}

	public void SpawnTokens()
	{
		//Set the index number of the array Randomly
		int spawnIndex = Random.Range (0, spawnPoints.Length);
		int objectIndex = Random.Range (0, pickUps.Length);

		Instantiate (pickUps [objectIndex], spawnPoints [spawnIndex].position, spawnPoints [spawnIndex].rotation);
	}
}
