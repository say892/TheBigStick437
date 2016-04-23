using UnityEngine;
using System.Collections.Generic;
using RAIN.BehaviorTrees;
using RAIN.Core;
using RAIN.Minds;
using UnityEditor;

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

		AIRig rig = GetComponentInChildren<RAIN.Core.AIRig>();
		if (rig == null) return;
		BTAsset tree = AssetDatabase.LoadAssetAtPath<BTAsset>("Assets/AI/BehaviorTrees/EnemyShip.asset");
		BasicMind mind = (BasicMind)rig.AI.Mind;
		mind.SetBehavior(tree, new List<BTAssetBinding>());
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
