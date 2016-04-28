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

	private GUIStyle greenStyle;
	private GUIStyle redStyle;
	private int originalHealth;

	private enum enemyStates {
		wandering,
		capturing,
		attacking
	}

	private enemyStates currentState;

	private ControlPoints allControlPoints;

	private ControlPoint currentTarget;

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

		currentState = enemyStates.wandering;
		allControlPoints = GameObject.Find("ControlPoints").GetComponent<ControlPoints>();

		//AIRig rig = GetComponentInChildren<AIRig>(); 
		//if (rig == null) return; 
		//BTAsset tree = AssetDatabase.LoadAssetAtPath<BTAsset>("Assets/AI/BehaviorTrees/EnemyShip.asset"); 
		//BasicMind mind = (BasicMind)rig.AI.Mind; 
		//mind.SetBehavior(tree, new List<BTAssetBinding>());

	}
	
	// Update is called once per frame
	void Update ()
	{
		shootTimer += Time.deltaTime;

		if (currentState == enemyStates.wandering) doWanderingState();
		else if (currentState == enemyStates.capturing) doCapturingState();
		else if (currentState == enemyStates.attacking) doChasingState();
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
		//Vector3 vec = transform.position;
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
			newBullet.GetComponent<BulletScript>().setBullet(-missileSpeed, missileDamage, missileRange, null);
		}
	}

	void doWanderingState() {

		//I walk a lonely... sea?
		if (currentTarget == null) {

			//attempt to balance enemies shouldn't have more than 5 (still possible though)
			if (allControlPoints.getNumEnemyPoints() <= 4) {

				//if the players have an advantage, take their points
				if (allControlPoints.getNumPlayerPoints() >= 5) {
					currentTarget = allControlPoints.findNearestPlayerPoint(transform.position);
				}
				//or give them some time to come back and only go for unoccupied
				else {
					currentTarget = allControlPoints.findRandomNeutralPoint();
				}

			}
			//if we're at max points let's just patrol randomly
			else {
				currentTarget = allControlPoints.findRandomEnemyPoint();
			}

		}
		//do we have a heading?
		else if (Vector3.Angle(transform.forward, currentTarget.transform.position - transform.position) > 1.0F) {
			rotateTowardsPosition(currentTarget.transform.position);
		}
		//FULL STEAM AHEAD
		else {
			moveForward();
		}

		//ATTACK
		GameObject nearestPlayer = findNearestPlayer(transform.position);
		if (nearestPlayer != null) currentState = enemyStates.attacking;


		//we have arrived
		if (distanceToPoint(currentTarget) < 5*5) {
			//we own this point!
			if (currentTarget.getInfluence() < -40) {
				currentTarget = null; //wander somewhere else
			}
			else {
				//take this point
				currentState = enemyStates.capturing;
			}
		}

	}

	void doCapturingState() {
		if (currentTarget.getInfluence() < -60) {
			//get a new target, you did your job solider.
			currentTarget = null;
			currentState = enemyStates.wandering;
		}


		GameObject nearestPlayer = findNearestPlayer(transform.position);
		if (nearestPlayer != null) {
			//ATTACK!
			if (Vector3.Angle(transform.forward, nearestPlayer.transform.position - transform.position) > 1.0F) {
				rotateTowardsPosition(nearestPlayer.transform.position);
			}
			else {
				Shoot();
			}
		}
	}


	void doChasingState() {

		GameObject nearestPlayer = findNearestPlayer(transform.position);
		if (nearestPlayer != null) {
			//ATTACK!
			if (Vector3.Angle(transform.forward, nearestPlayer.transform.position - transform.position) > 1.0F) {
				rotateTowardsPosition(nearestPlayer.transform.position);
			}
			else if(distanceToPlayer(nearestPlayer) > 7*7) {
				moveForward();
			}
			else {
				Shoot();
			}

			//if (distanceToPlayer(nearestPlayer) > 10) {
				
			//}

		}
		else {
			currentState = enemyStates.wandering;
		}



	}



	int distanceToPoint(ControlPoint p) {
		return (int)(p.transform.position - transform.position).sqrMagnitude;
	}

	int distanceToPlayer(GameObject p) {
		return (int)(p.transform.position - transform.position).sqrMagnitude;
	}

	void rotateTowardsPosition(Vector3 pos) {
		Vector3 newDir = Vector3.RotateTowards(transform.forward, pos - transform.position,
			Mathf.Deg2Rad * turnSpeedDegrees * Time.deltaTime, 0);
		transform.rotation = Quaternion.LookRotation(newDir);
	}

	void moveForward() {
		transform.position += transform.forward * forwardSpeed * Time.deltaTime;
	}

	GameObject findNearestPlayer(Vector3 pos) {

		Collider[] hitColliders = Physics.OverlapSphere(transform.position, 10); //gets all objects within range
	
		List<GameObject> nearbyPlayers = new List<GameObject>();

		//check to see if they are ships
		foreach(Collider c in hitColliders) {
			if(c.name.Contains("Player")) {
				nearbyPlayers.Add(c.gameObject);
			}
		}

		GameObject closest = null;
		float distance = 9999999;
		foreach(GameObject p in nearbyPlayers) {
			float tempDist = (p.transform.position - pos).sqrMagnitude;
			if (tempDist < distance) {
				closest = p;
				tempDist = distance;
			}
		}

		return closest;
	}
}
