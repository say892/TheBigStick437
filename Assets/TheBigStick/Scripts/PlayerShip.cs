using UnityEngine;
using System.Collections;

/**
 * Every player has their own ship, which they control by using their phone. 
 * This is the object that is spawned by HappyFunTimes, so we need to add the player to the list of players if 
 * we haven't done so already. 
 **/
public class PlayerShip : MonoBehaviour {

	private HFTInput mInput;
	private HFTGamepad mGamepad;
	private MuseumPlayer mPlayer;

	private GameMasterScript GameMaster;

	[SerializeField]
	private GameObject bulletPrefab;

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


		GameMaster = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameMasterScript>();
		//Add to master list of players
		mPlayer = GameMaster.addPlayer(mGamepad);

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
	}



	void doUserInput() {

		//If up is held, full steam ahead
		if (Input.GetAxis("Vertical") > 0.2F || mInput.GetAxis("Vertical") < -0.2F) {
			transform.position += transform.up * forwardSpeed * Time.deltaTime;
		}
		//Or we can go back using down
		else if(Input.GetAxis("Vertical") < -0.2F || mInput.GetAxis("Vertical") > 0.2F) {
			//there is no transform.left 
			transform.position -= transform.up * backwardSpeed * Time.deltaTime;
		}

		//rotate left or right depending on the left/right button we press.
		if (Input.GetAxis("Horizontal") > 0.2F || mInput.GetAxis("Horizontal") > 0.2F) {
			transform.Rotate(new Vector3(0, 0, -turnSpeedDegrees * Time.deltaTime));
		}
		else if(Input.GetAxis("Horizontal") < -0.2F || mInput.GetAxis("Horizontal") < -0.2F) {
			transform.Rotate(new Vector3(0, 0, turnSpeedDegrees * Time.deltaTime));
		}

		//FIRE AT WILL!
		if (Input.GetKey(KeyCode.Space) || mInput.GetButton("Fire1")) {
			//If we're loaded of course
			if(shootTimer > missileShotDelay) {
				shootTimer = 0;
				//create the bullet and then set the needed information in the new bullet
				GameObject newBullet = (GameObject)Instantiate(bulletPrefab, transform.FindChild("BulletSpawnPos").position, transform.rotation);
				newBullet.GetComponent<BulletScript>().setBullet(missileSpeed, missileDamage, missileRange, mPlayer);
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
		if ((myUpgrades & shipUpgradesBit.healthU) != 0) {
			health = ShipValues.playerHealthUpgrade;
		}
		else {
			health = ShipValues.playerHealth;
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
		
	void OnGUI() {
		string thing = 
			health +  "\n" +
			forwardSpeed +  "\n" +
			backwardSpeed +  "\n" +
			turnSpeedDegrees +  "\n" +
			missileSpeed +  "\n" +
			missileDamage +  "\n" +
			missileShotDelay +  "\n" +
			missileRange +  "\n";
		GUI.Label(new Rect(20, 50, 200, 1000), thing);
	}

}
