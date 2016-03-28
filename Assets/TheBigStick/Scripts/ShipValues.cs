using UnityEngine;
using System.Collections;

/**
 * I didn't know where to put all of these. But it's a place to hold all the stats for all ships
 **/
public struct ShipValues {

	//values are in units/sec where units are Unity Units, degrees, or shots. Use time.DeltaTime whenever possible
	public static int playerHealth = 2000;
	public static float playerForwardSpeed = 2.0F;
	public static float playerBackwardSpeed = 1.5F;
	public static float playerTurnSpeedDegrees = 150;
	public static int playerMissileSpeed = 4;
	public static int playerMissileDamage = 500;
	public static float playerMissileShotDelay = 2.0F;
	public static int playerMissileRange = 20;

	public static int playerHealthUpgrade = 5000;
	public static float playerForwardSpeedUpgrade = 3.5F;
	public static float playerBackwardSpeedUpgrade = 2F;
	public static float playerTurnSpeedDegreesUpgrade = 180;
	public static int playerMissileSpeedUpgrade = 7;
	public static int playerMissileDamageUpgrade = 1500;
	public static float playerMissileShotDelayUpgrade = 1.5F;
	public static int playerMissileRangeUpgrade = 30;

	public static int enemyHealth = 1000;
	public static float enemyForwardSpeed = 1.5F;
	public static float enemyBackwardSpeed = 1F;
	public static float enemyTurnSpeedDegrees = 90;
	public static int enemyMissileSpeed = 3;
	public static int enemyMissileDamage = 300;
	public static float enemyMissileShotDelay = 5F;
	public static int enemyMissileRange = 15;

	public static int enemyHealthBoss = 5000;
	public static float enemyForwardSpeedBoss = 3.5F;
	public static float enemyBackwardSpeedBoss = 2F;
	public static float enemyTurnSpeedDegreesBoss = 120;
	public static int enemyMissileSpeedBoss = 5;
	public static int enemyMissileDamageBoss = 1000;
	public static float enemyMissileShotDelayBoss = 3F;
	public static int enemyMissileRangeBoss = 25;


}
