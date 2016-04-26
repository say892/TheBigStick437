using UnityEngine;   
using System.Collections.Generic;
using RAIN.BehaviorTrees;
using RAIN.Core;
using RAIN.Minds;
using UnityEditor;
using UnityEngine.UI;

/**
 * An enemy ship. Like the player ship, but controlled by AI.
 **/
public class EnemyShip : MonoBehaviour {

	public GameObject bulletPrefab;
	public Canvas canvas;

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

	private GUIStyle greenStyle;
	private GUIStyle redStyle;
	private int originalHealth;

	void Awake()
	{
		// Health Bar stuff
		Texture2D green = new Texture2D(1, 1);
		green.SetPixel(1, 1, Color.green);
		green.wrapMode = TextureWrapMode.Repeat;
		green.Apply();
		greenStyle = new GUIStyle();
		greenStyle.normal.background = green;

		Texture2D red = new Texture2D(1, 1);
		red.SetPixel(1, 1, Color.red);
		red.wrapMode = TextureWrapMode.Repeat;
		red.Apply();
		redStyle = new GUIStyle();
		redStyle.normal.background = red;
	}

	// Use this for initialization
	void Start () {
	
		health = ShipValues.enemyHealth;
		originalHealth = health;
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
		Vector3 vec = transform.position;
		Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
		GUI.Label(new Rect(screenPos.x - 10, Screen.height - (screenPos.y + 15), 20, 4), "", redStyle);
		GUI.Label(new Rect(screenPos.x - 10, Screen.height - (screenPos.y + 15), 20 * health / originalHealth, 4), "", greenStyle);
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
