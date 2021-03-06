﻿using UnityEngine;
using System.Collections;
using HappyFunTimes;

/**
 * Every player has their own ship, which they control by using their phone. 
 * This is the object that is spawned by HappyFunTimes, so we need to add the player to the list of players if 
 * we haven't done so already. 
 **/
public class PlayerShip : MonoBehaviour {


	// Message received from controller about upgrade string sent.
	private class MessageCharacter : MessageCmdData {
		public string upgradeName = "";
	}

	private class ToController : MessageCmdData
	{
		public string upgradeName = "";
		public ToController(string upgrade)
		{
			upgradeName = upgrade;
		}
	}


	private HFTInput mInput;
	private HFTGamepad mGamepad;
	private MuseumPlayer mPlayer;
	private NetPlayer mNetPlayer;

	private GameMasterScript GameMaster;

	[SerializeField]
	private GameObject bulletPrefab;

	[SerializeField]
	private Material upgradePlayerText;
	
	[SerializeField]
	private Canvas canvas;


	private float shootTimer;

	private int health;
	private float forwardSpeed;
	private float backwardSpeed;
	private float turnSpeedDegrees;
	private int missileSpeed;
	private int missileDamage;
	private float missileShotDelay;
	private int missileRange;

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
	 
		mInput = GetComponent<HFTInput>();
		mGamepad = GetComponent<HFTGamepad>();


		GameMaster = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameMasterScript>();
		enemySpawner = GameObject.FindGameObjectWithTag("GameController").GetComponent<EnemySpawning>();
		controlPoints = GameObject.Find("ControlPoints").GetComponent<ControlPoints>();
		//Add to master list of players
		mPlayer = GameMaster.addPlayer(mGamepad);
		//shade the player's ship the color of their controller
		GetComponent<MeshRenderer>().material.color = mGamepad.Color;

		checkUpgrades(); //Fill out our ship stats

		shootTimer = 0;

		//transform.position = GameMaster.getSpawnPos();
		transform.position = controlPoints.getPlayerSpawnPos();
		transform.eulerAngles = new Vector3(0, 30, 0);

		enemySpawner.spawnEnemy(); //spawn 2 enemies at the start of every player
		enemySpawner.spawnEnemy();
	}

	//called when the player is spawned
	void InitializeNetPlayer(SpawnInfo spawnInfo) {
		// Save the netplayer object
		mNetPlayer = spawnInfo.netPlayer;
		// Register handler to call if the player disconnects from the game.
		mNetPlayer.OnDisconnect += removePlayer;
		//mNetPlayer.OnBusy += removePlayer;

		//gets the players upgrade string
		mNetPlayer.RegisterCmdHandler<MessageCharacter>("upgrade", onUpgrade);

		//the player wants to go back to the Dpad/button setup
		mNetPlayer.RegisterCmdHandler<MessageCharacter>("retToGame", backToGame);

		//the player wants to try to enter an upgrade.
		mNetPlayer.RegisterCmdHandler<MessageCharacter>("goToUpgrade", gotoUpgrades);
	}

	
	// Update is called once per frame
	void Update () {

		transform.position = new Vector3(transform.position.x, 0, transform.position.z);

		doUserInput();

		//print(health);
		//Don't forget to update the shot timer
		shootTimer += Time.deltaTime;

		//TODO don't do this every frame. Testing only.
		//checkUpgrades();

		//if (Input.GetKeyDown(KeyCode.L)) {
		//	GetComponent<Renderer>().material = upgradePlayerText;
		//	GetComponent<MeshRenderer>().material.color = mGamepad.Color;
		//}


		//ignore all other players, we can drive through them 
		//TERRIBLY inefficient... but this is due tomorrow. We're litterally presenting in less than 24 hours.
		GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");

		foreach(GameObject p in allPlayers) {
			Physics.IgnoreCollision(GetComponent<CharacterController>(), p.GetComponent<CharacterController>());
		}

		//I wanna plow some enemies
		GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");

		foreach(GameObject e in allEnemies) {
			Physics.IgnoreCollision(GetComponent<CharacterController>(), e.GetComponent<BoxCollider>());
		}



			

	}

	void OnGUI()
	{
		if (GetComponent<MeshRenderer>().enabled) {
		//Vector3 vec = transform.position;
			Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
			GUI.Label(new Rect(screenPos.x - 10, Screen.height - (screenPos.y + 15), 20, 4), "", redStyle);
			GUI.Label(new Rect(screenPos.x - 10, Screen.height - (screenPos.y + 15), 20 * health / originalHealth, 4), "", greenStyle);
		}
	}

	void doUserInput() {

		//If up is held, full steam ahead
		if (Input.GetAxis("Vertical") > 0.2F || mInput.GetAxis("Vertical") < -0.2F) {
			//transform.position -= transform.forward * forwardSpeed * Time.deltaTime;
			GetComponent<CharacterController>().Move(-transform.forward * forwardSpeed * Time.deltaTime);
			//GetComponent<Rigidbody>().velocity = transform.forward * forwardSpeed * Time.deltaTime;
		}
		//Or we can go back using down
		else if(Input.GetAxis("Vertical") < -0.2F || mInput.GetAxis("Vertical") > 0.2F) {
			//there is no transform.left 
			//transform.position += transform.forward * backwardSpeed * Time.deltaTime;
			GetComponent<CharacterController>().Move(transform.forward * backwardSpeed * Time.deltaTime);
			//GetComponent<Rigidbody>().velocity = -transform.forward * backwardSpeed * Time.deltaTime;
		}

		//rotate left or right depending on the left/right button we press.
		if (Input.GetAxis("Horizontal") > 0.2F || mInput.GetAxis("Horizontal") > 0.2F) {
			transform.Rotate(new Vector3(0, turnSpeedDegrees * Time.deltaTime, 0));
		}
		else if(Input.GetAxis("Horizontal") < -0.2F || mInput.GetAxis("Horizontal") < -0.2F) {
			transform.Rotate(new Vector3(0, -turnSpeedDegrees * Time.deltaTime, 0));
		}



		//FIRE AT WILL!
		if (Input.GetKey(KeyCode.Space) || mInput.GetButton("Fire1")) {
			//If we're loaded of course
			if(shootTimer > missileShotDelay && health > 0) {
				shootTimer = 0;
				//create the bullet and then set the needed information in the new bullet
				GameObject newBullet = (GameObject)Instantiate(bulletPrefab, transform.FindChild("BulletSpawnPos").position, transform.rotation);
				newBullet.GetComponent<BulletScript>().setBullet(missileSpeed, missileDamage, missileRange, mPlayer);
			}
		}


	}

	public void takeDamage(int damage) {
		//print(damage);
		health -= damage;
		//print(health);
		if (health < 0) {
			//DIE FOREVER! Or respawn, you know, whatever it comes to.
			checkUpgrades(); //use this to reset the health
			mPlayer.addScore(-150);
			GetComponent<MeshRenderer>().enabled = false;
			//GetComponent<BoxCollider>().enabled = false;
			transform.position = controlPoints.getPlayerSpawnPos();
			StartCoroutine("respawnPlayer");
		}
	}

	public void addScore(int s) {
		mPlayer.addScore(s);
	}

	//Do this whenever you reload back into the game?
	public void checkUpgrades() {

		//get all of the players upgrades
		int myUpgrades = mPlayer.getPlayerUpgrades();

		//Health
		if ((myUpgrades & shipUpgradesBit.healthU) != 0) {
			health = ShipValues.playerHealthUpgrade;
			originalHealth = health;
		}
		else {
			health = ShipValues.playerHealth;
			originalHealth = health;
		}

		//Forward Speed
		if ((myUpgrades & shipUpgradesBit.forwardSpeedU) != 0) {
			forwardSpeed = ShipValues.playerForwardSpeedUpgrade;
		}
		else {
			forwardSpeed = ShipValues.playerForwardSpeed;
		}

		//Backward Speed
		if ((myUpgrades & shipUpgradesBit.backwardSpeedU) != 0) {
			backwardSpeed = ShipValues.playerBackwardSpeedUpgrade;
		}
		else {
			backwardSpeed = ShipValues.playerBackwardSpeed;
		}

		//Turn Speed
		if ((myUpgrades & shipUpgradesBit.turnSpeedDegreesU) != 0) {
			turnSpeedDegrees = ShipValues.playerTurnSpeedDegreesUpgrade;
		}
		else {
			turnSpeedDegrees = ShipValues.playerTurnSpeedDegrees;
		}

		//Missile Travel Speed
		if ((myUpgrades & shipUpgradesBit.missileSpeedU) != 0) {
			missileSpeed = ShipValues.playerMissileSpeedUpgrade;
		}
		else {
			missileSpeed = ShipValues.playerMissileSpeed;
		}

		//Missile Damage
		if ((myUpgrades & shipUpgradesBit.missileDamageU) != 0) {
			missileDamage = ShipValues.playerMissileDamageUpgrade;
		}
		else {
			missileDamage = ShipValues.playerMissileDamage;
		}

		//Missile Shot Delay
		if ((myUpgrades & shipUpgradesBit.missileShotDelayU) != 0) {
			missileShotDelay = ShipValues.playerMissileShotDelayUpgrade;
		}
		else {
			missileShotDelay = ShipValues.playerMissileShotDelay;
		}

		//Missile Range Distance
		if ((myUpgrades & shipUpgradesBit.missileRangeU) != 0) {
			missileRange = ShipValues.playerMissileRangeUpgrade;
		}
		else {
			missileRange = ShipValues.playerMissileRange;
		}
	}

	//kick the player from the game if they leave their phone
	private void removePlayer(object sender, System.EventArgs e) {
		enemySpawner.deleteFarthestEnemies();
		Destroy(gameObject);
	}

	private IEnumerator respawnPlayer() {
		
		yield return new WaitForSeconds(2);
		GetComponent<MeshRenderer>().enabled = true;
		//GetComponent<BoxCollider>().enabled = true;
		checkUpgrades(); //REALLY make sure they have their health back (they didn't once)
		transform.position = controlPoints.getPlayerSpawnPos();


	}

	private void onUpgrade(MessageCharacter data) {
		string upgrade = data.upgradeName.ToLower();

		//Teehee
		if (upgrade.Equals("impeachzach")) {
			mPlayer.addPlayerUpgrade(shipUpgrades.health);
			mPlayer.addPlayerUpgrade(shipUpgrades.forwardSpeed);
			mPlayer.addPlayerUpgrade(shipUpgrades.backwardSpeed);
			mPlayer.addPlayerUpgrade(shipUpgrades.turnSpeedDegree);
			mPlayer.addPlayerUpgrade(shipUpgrades.missileSpeed);
			mPlayer.addPlayerUpgrade(shipUpgrades.missileDamage);
			mPlayer.addPlayerUpgrade(shipUpgrades.missileRange);
			mPlayer.addPlayerUpgrade(shipUpgrades.missileShotDelay);
		}
		if (upgrade.Equals(shipUpgradeCodes.healthU)) {
			mPlayer.addPlayerUpgrade(shipUpgrades.health);
		}
		if (upgrade.Equals(shipUpgradeCodes.forwardSpeedU)) {
			mPlayer.addPlayerUpgrade(shipUpgrades.forwardSpeed);
		}
		if (upgrade.Equals(shipUpgradeCodes.backwardSpeedU)) {
			mPlayer.addPlayerUpgrade(shipUpgrades.backwardSpeed);
		}
		if (upgrade.Equals(shipUpgradeCodes.turnSpeedDegreesU)) {
			mPlayer.addPlayerUpgrade(shipUpgrades.turnSpeedDegree);
		}
		if (upgrade.Equals(shipUpgradeCodes.missileSpeedU)) {
			mPlayer.addPlayerUpgrade(shipUpgrades.missileSpeed);
		}
		if (upgrade.Equals(shipUpgradeCodes.missileDamageU)) {
			mPlayer.addPlayerUpgrade(shipUpgrades.missileDamage);
		}
		if (upgrade.Equals(shipUpgradeCodes.missileShotDelayU)) {
			mPlayer.addPlayerUpgrade(shipUpgrades.missileShotDelay);
		}
		if (upgrade.Equals(shipUpgradeCodes.missileRangeU)) {
			mPlayer.addPlayerUpgrade(shipUpgrades.missileRange);
		}

		checkUpgrades();

	}

	private void backToGame(MessageCharacter data) {
		mGamepad.controllerOptions.controllerType = HFTGamepad.ControllerType.c_1dpad_1button;
	}

	private void gotoUpgrades(MessageCharacter data) {
		mGamepad.controllerOptions.controllerType = HFTGamepad.ControllerType.c_upgrade;
		ToController m = new ToController(mPlayer.getPlayerUpgrades().ToString());
		mNetPlayer.SendCmd("showUpgrades", m);
	}
}
