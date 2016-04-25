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

	public GameObject bulletPrefab;

	private int health;
	private float forwardSpeed;
	private float backwardSpeed;
	private float turnSpeedDegrees;
	private int missileSpeed;
	private int missileDamage;
	private float missileShotDelay;
	private int missileRange;
	private float shootTimer = 0;

	private EnemySpawning enemySpawner;

	private ControlPoints controlPoints;

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

		controlPoints = GameObject.Find("ControlPoints").GetComponent<ControlPoints>();

		AIRig rig = GetComponentInChildren<AIRig>(); 
		if (rig == null) return; 
		BTAsset tree = AssetDatabase.LoadAssetAtPath<BTAsset>("Assets/AI/BehaviorTrees/EnemyShip.asset"); 
		BasicMind mind = (BasicMind)rig.AI.Mind; 
		mind.SetBehavior(tree, new List<BTAssetBinding>()); 

	}
	
	// Update is called once per frame
	void Update ()
	{
		shootTimer += Time.deltaTime;
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

	void Shoot()
	{
		if (shootTimer > missileShotDelay)
		{
			shootTimer = 0;
			//create the bullet and then set the needed information in the new bullet
			GameObject newBullet = (GameObject)Instantiate(bulletPrefab, transform.FindChild("BulletSpawnPos").position, transform.rotation);
			newBullet.GetComponent<BulletScript>().setBullet(missileSpeed, missileDamage, missileRange, null);
		}
	}
}
