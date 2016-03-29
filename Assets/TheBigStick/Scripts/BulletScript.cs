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
		transform.position += travelSpeed * Time.deltaTime * transform.up;

		//until it fizzles out of course from traveling too far
		if ((transform.position - startPos).magnitude >= range) {
			Destroy(this.gameObject);
		}

	}

	void OnCollisionEnter2D(Collision2D other) {
		if (origin != null) {
			if (other.gameObject.name.Contains("enemy")) {
				//Destroy(other.gameObject);
				if (other.gameObject.GetComponent<EnemyShip>().takeDamage(damage)) {
					origin.addScore(50); //Just assume it's never the boss. TODO TODO TODO
				}
				Destroy(this.gameObject);
			}
		}
		else {
			if (other.gameObject.name.Contains("player")) {
				//Destroy(other.gameObject);
				other.gameObject.GetComponent<PlayerShip>().takeDamage(damage);
				Destroy(this.gameObject);
			}
		}


	}

}
