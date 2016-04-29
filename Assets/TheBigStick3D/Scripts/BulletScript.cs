using UnityEngine;
using System.Collections;

/**
 * An individual bullet. Has direction, velocity, damage, etc. values. Will continue
 * until it fizzles out, or hits an opposite side ship. Bullets remember who shot them.
 **/
public class BulletScript : MonoBehaviour {

	private int travelSpeed;
	private int damage;
	private int range;
	private Vector3 startPos;
	private MuseumPlayer origin; // Null if from enemy, otherwise it's a specific player
								 // This could be cool if we could give the ships random names of enemy ships from the era and have it say
								 // "You've been killed by X" where X is a historically accurate ship from the battle
								 // Would have to change the OnCollisionEnter function

	[SerializeField]
	private GameObject explosionPrefab; //Used to create the boom when the ship is sunk


	// Use this for initialization
	void Start () {
	
	}

	//gets information from the player that fired this bullet
	public void setBullet(int travelSpeed, int damage, int range, MuseumPlayer player)
	{
		this.travelSpeed = travelSpeed;
		this.damage = damage;
		this.range = range;
		this.origin = player;
		startPos = transform.position;
	}

	// Update is called once per frame
	void Update () {
	
		//always move forward on the map
		transform.position -= travelSpeed * Time.deltaTime * transform.forward;

		//until it fizzles out of course from traveling too far
		if ((transform.position - startPos).magnitude >= range) {
			Destroy(this.gameObject);
		}

		//if a player shot this, make sure it doesn't hit any other players (VERY BAD, SEE PLAYERSHIP.UPDATE)
		if (origin != null) {
			GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");

			foreach(GameObject p in allPlayers) {
				Physics.IgnoreCollision(this.GetComponent<BoxCollider>(), p.GetComponent<CharacterController>());
			}
		}


	}

	void OnCollisionEnter(Collision other) {
		//print(other.gameObject.name);

		if (origin != null) {
			if (other.gameObject.name.Contains("Enemy")) {
				//Destroy(other.gameObject);
				if (other.gameObject.GetComponent<EnemyShip>().takeDamage(damage)) {
					origin.addScore(50); //Just assume it's never the boss. TODO TODO TODO
					Instantiate(explosionPrefab, transform.position, Quaternion.Euler(new Vector3(90, 0, 0)));
				}
				//Destroy(this.gameObject);
			}
		}
		else {
			if (other.gameObject.name.Contains("Player")) {
				//Destroy(other.gameObject);
				other.gameObject.GetComponent<PlayerShip>().takeDamage(damage);
				//Destroy(this.gameObject);
			}
		}
		//don't break on map, and can pass through other players
		//if (!other.gameObject.name.Equals("Map") || 
		if (!(other.gameObject.name.Contains("Player") && origin != null)) {
			Destroy(this.gameObject); //break on any other hit
		}


	}

	void OnTriggerEnter(Collider other) {
		print("TRIGGERED");
		print(other.gameObject.name);

		if (origin != null) {
			if (other.gameObject.name.Contains("Enemy")) {
				//Destroy(other.gameObject);
				if (other.gameObject.GetComponent<EnemyShip>().takeDamage(damage)) {
					origin.addScore(50); //Just assume it's never the boss. TODO TODO TODO
					Instantiate(explosionPrefab, transform.position, Quaternion.Euler(new Vector3(90, 0, 0)));
				}
				//Destroy(this.gameObject);
			}
		}
		else {
			if (other.gameObject.name.Contains("Player")) {
				//Destroy(other.gameObject);
				other.gameObject.GetComponent<PlayerShip>().takeDamage(damage);
				//Destroy(this.gameObject);
			}
		}
		//don't break on map, and can pass through other players
		//if (!other.gameObject.name.Equals("Map") || 
		if (!(other.gameObject.name.Contains("Player") && origin != null)) {
			Destroy(this.gameObject); //break on any other hit
		}


	}

}
