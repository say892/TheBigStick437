using UnityEngine;
using System.Collections;

/**
 * An enemy ship. Like the player ship, but controlled by AI.
 **/
public class EnemyShip : MonoBehaviour {

	private int health;
	private float forwardSpeed;
	private float backwardSpeed;
	private float turnSpeedDegrees;
	private int missileSpeed;
	private int missileDamage;
	private float missileShotDelay;
	private int missileRange;

	private EnemySpawning enemySpawner;

	// Use this for initialization
	void Start () {
	
		health = ShipValues.enemyHealth;
		forwardSpeed = ShipValues.enemyForwardSpeed;
		backwardSpeed = ShipValues.enemyBackwardSpeed;
		turnSpeedDegrees = ShipValues.enemyTurnSpeedDegrees;
		missileSpeed = ShipValues.enemyMissileSpeed;
		missileDamage = ShipValues.enemyMissileDamage;
		missileShotDelay = ShipValues.enemyMissileShotDelay;
		missileRange = ShipValues.enemyMissileRange;

		enemySpawner = GameObject.FindGameObjectWithTag("GameController").GetComponent<EnemySpawning>();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//get hit son. Returns true if sunk.
	public bool takeDamage(int damage) {
		health -= damage;
		if (health <= 0) {
			Destroy(this.gameObject);
			enemySpawner.spawnEnemy(); //spawn a new enemy
			return true;
		}
		return false;
	}

	void OnGUI() {

		//No idea how I do a health bar.
	}
}
