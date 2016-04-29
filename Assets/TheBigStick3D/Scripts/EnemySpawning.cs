using UnityEngine;
using System.Collections;

public class EnemySpawning : MonoBehaviour {
	private const int ENEMIESPERPLAYER = 2;
	private const int DELAYFORENEMYRESPAWN = 9; //seconds

	[SerializeField]
	private GameObject enemyShipPrefab;

	private Transform USSIowaLocation;

	[SerializeField]
	private GameObject controlPointsObj;

	private ControlPoints controlPointsScr;


	// Use this for initialization
	void Start () {
		StartCoroutine("newEnemy");
		StartCoroutine("newEnemy");

		USSIowaLocation = GameObject.Find("USS Iowa PlayerBase").transform;

		controlPointsScr = controlPointsObj.GetComponent<ControlPoints>();

	}
	
	// Update is called once per frame
	void Update () {


	}

	public void spawnEnemy() {
		StartCoroutine("newEnemy");
	}

	//spawns ONE enemy at a position
	IEnumerator newEnemy() {
		//yield return new WaitForSeconds(3);
		yield return new WaitForSeconds(DELAYFORENEMYRESPAWN);

		//PICK THE RIGHT POSITION TO SPAWN
		Vector3 spawnPos = controlPointsScr.getEnemySpawnPos();

		Instantiate(enemyShipPrefab, spawnPos, Quaternion.identity);
	}


	//this function will get the 2 farthest enemies from the USS Iowa and delete them.
	public void deleteFarthestEnemies() {

		GameObject[] numEnemies = GameObject.FindGameObjectsWithTag("Enemy");

		float farthestDistance = 0;
		GameObject farthest = null;
		float nextDistance = 0;
		GameObject next = null;

		foreach(GameObject obj in numEnemies) {

			float distance = (obj.transform.position - USSIowaLocation.position).sqrMagnitude;

			if (distance > farthestDistance) {
				next = farthest;
				nextDistance = farthestDistance;

				farthest = obj;
				farthestDistance = distance;
			}
			else if (distance > nextDistance) {
				nextDistance = distance;
				next = obj;
			}
		}

		GameObject.Destroy(farthest);
		GameObject.Destroy(next);


	}


}
