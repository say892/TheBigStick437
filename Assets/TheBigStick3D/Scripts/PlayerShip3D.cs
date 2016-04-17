using UnityEngine;
using System.Collections;

/**
 * Every player has their own ship, which they control by using their phone. 
 * This is the object that is spawned by HappyFunTimes, so we need to add the player to the list of players if 
 * we haven't done so already. 
 **/
public class PlayerShip3D : MonoBehaviour {

	private HFTInput mInput;
	private HFTGamepad mGamepad;
	private MuseumPlayer3D mPlayer;

	private GameMasterScript3D GameMaster;

	[SerializeField]
	private GameObject bulletPrefab;

	[SerializeField]
	private Material upgradePlayerText;

	private float shootTimer;

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
	 
		mInput = GetComponent<HFTInput>();
		mGamepad = GetComponent<HFTGamepad>();


		GameMaster = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameMasterScript3D>();
		//Add to master list of players
		mPlayer = GameMaster.addPlayer(mGamepad);

		//shade the player's ship the color of their controller
		GetComponent<MeshRenderer>().material.color = mGamepad.Color;

		checkUpgrades(); //Fill out our ship stats

		shootTimer = 0;

		transform.position = GameMaster.getSpawnPos();
	}
	
	// Update is called once per frame
	void Update () {

		doUserInput();

		//Don't forget to update the shot timer
		shootTimer += Time.deltaTime;

		//TODO don't do this every frame. Testing only.
		checkUpgrades();

		if (Input.GetKeyDown(KeyCode.L)) {
			GetComponent<Renderer>().material = upgradePlayerText;
			GetComponent<MeshRenderer>().material.color = mGamepad.Color;
		}
	}



	void doUserInput() {

		//If up is held, full steam ahead
		if (Input.GetAxis("Vertical") > 0.2F || mInput.GetAxis("Vertical") < -0.2F) {
			transform.position -= transform.forward * forwardSpeed * Time.deltaTime;
		}
		//Or we can go back using down
		else if(Input.GetAxis("Vertical") < -0.2F || mInput.GetAxis("Vertical") > 0.2F) {
			//there is no transform.left 
			transform.position += transform.forward * backwardSpeed * Time.deltaTime;
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
			if(shootTimer > missileShotDelay) {
				shootTimer = 0;
				//create the bullet and then set the needed information in the new bullet
				GameObject newBullet = (GameObject)Instantiate(bulletPrefab, transform.FindChild("BulletSpawnPos").position, transform.rotation);
				newBullet.GetComponent<BulletScript3D>().setBullet(missileSpeed, missileDamage, missileRange, mPlayer);
			}
		}


	}

	public void takeDamage(int damage) {
		health -= damage;
		if (health < 0) {
			//DIE FOREVER! Or respawn, you know, whatever it comes to.

		}
	}

	//Do this whenever you reload back into the game?
	public void checkUpgrades() {

		//get all of the players upgrades
		int myUpgrades = mPlayer.getPlayerUpgrades();

		//Health
		if ((myUpgrades & shipUpgrades3DBit.healthU) != 0) {
			health = ShipValues3D.playerHealthUpgrade;
		}
		else {
			health = ShipValues3D.playerHealth;
		}

		//Forward Speed
		if ((myUpgrades & shipUpgrades3DBit.forwardSpeedU) != 0) {
			forwardSpeed = ShipValues3D.playerForwardSpeedUpgrade;
		}
		else {
			forwardSpeed = ShipValues3D.playerForwardSpeed;
		}

		//Backward Speed
		if ((myUpgrades & shipUpgrades3DBit.backwardSpeedU) != 0) {
			backwardSpeed = ShipValues3D.playerBackwardSpeedUpgrade;
		}
		else {
			backwardSpeed = ShipValues3D.playerBackwardSpeed;
		}

		//Turn Speed
		if ((myUpgrades & shipUpgrades3DBit.turnSpeedDegreesU) != 0) {
			turnSpeedDegrees = ShipValues3D.playerTurnSpeedDegreesUpgrade;
		}
		else {
			turnSpeedDegrees = ShipValues3D.playerTurnSpeedDegrees;
		}

		//Missile Travel Speed
		if ((myUpgrades & shipUpgrades3DBit.missileSpeedU) != 0) {
			missileSpeed = ShipValues3D.playerMissileSpeedUpgrade;
		}
		else {
			missileSpeed = ShipValues3D.playerMissileSpeed;
		}

		//Missile Damage
		if ((myUpgrades & shipUpgrades3DBit.missileDamageU) != 0) {
			missileDamage = ShipValues3D.playerMissileDamageUpgrade;
		}
		else {
			missileDamage = ShipValues3D.playerMissileDamage;
		}

		//Missile Shot Delay
		if ((myUpgrades & shipUpgrades3DBit.missileShotDelayU) != 0) {
			missileShotDelay = ShipValues3D.playerMissileShotDelayUpgrade;
		}
		else {
			missileShotDelay = ShipValues3D.playerMissileShotDelay;
		}

		//Missile Range Distance
		if ((myUpgrades & shipUpgrades3DBit.missileRangeU) != 0) {
			missileRange = ShipValues3D.playerMissileRangeUpgrade;
		}
		else {
			missileRange = ShipValues3D.playerMissileRange;
		}

			

	}
		
	void OnGUI() {
		/*
		string thing = 
			health +  "\n" +
			forwardSpeed +  "\n" +
			backwardSpeed +  "\n" +
			turnSpeedDegrees +  "\n" +
			missileSpeed +  "\n" +
			missileDamage +  "\n" +
			missileShotDelay +  "\n" +
			missileRange +  "\n";
		GUI.Label(new Rect(20, 50, 200, 1000), thing);*/
	}

}
