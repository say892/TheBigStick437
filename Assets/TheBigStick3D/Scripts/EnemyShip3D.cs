using UnityEngine;
using System.Collections;

/**
 * An enemy ship. Like the player ship, but controlled by AI.
 **/
public class EnemyShip3D : MonoBehaviour {

	private int health;
	private float forwardSpeed;
	private float backwardSpeed;
	private float turnSpeedDegrees;
	private int missileSpeed;
	private int missileDamage;
	private float missileShotDelay;
	private int missileRange;

	// Use this for initialization
	void Start () {
	
		health = ShipValues3D.enemyHealth;
		forwardSpeed = ShipValues3D.enemyForwardSpeed;
		backwardSpeed = ShipValues3D.enemyBackwardSpeed;
		turnSpeedDegrees = ShipValues3D.enemyTurnSpeedDegrees;
		missileSpeed = ShipValues3D.enemyMissileSpeed;
		missileDamage = ShipValues3D.enemyMissileDamage;
		missileShotDelay = ShipValues3D.enemyMissileShotDelay;
		missileRange = ShipValues3D.enemyMissileRange;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//get hit son. Returns true if sunk.
	public bool takeDamage(int damage) {
		health -= damage;
		if (health <= 0) {
			Destroy(this.gameObject);
			return true;
		}
		return false;
	}

	void OnGUI() {

		//No idea how I do a health bar.
	}
}
