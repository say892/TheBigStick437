using UnityEngine;
using System.Collections;

/**
 * THIS FILE IS NOT A CLASS
 * 
 * This file holds all the structs and values for ship movement, etc.
 * 
 **/
public struct ShipValues {

	//values are in units/sec where units are Unity Units, degrees, or missile-shots. Use time.DeltaTime whenever possible
	public static int		playerHealth = 2000;
	public static float		playerForwardSpeed = 2.0F;
	public static float		playerBackwardSpeed = 1.5F;
	public static float		playerTurnSpeedDegrees = 100;
	public static int		playerMissileSpeed = 4;
	public static int		playerMissileDamage = 500;
	public static float		playerMissileShotDelay = 2.0F;
	public static int		playerMissileRange = 20;

	public static int		playerHealthUpgrade = 5000;
	public static float		playerForwardSpeedUpgrade = 3.5F;
	public static float		playerBackwardSpeedUpgrade = 2F;
	public static float		playerTurnSpeedDegreesUpgrade = 130;
	public static int		playerMissileSpeedUpgrade = 7;
	public static int		playerMissileDamageUpgrade = 1500;
	public static float		playerMissileShotDelayUpgrade = 1.5F;
	public static int		playerMissileRangeUpgrade = 30;

	public static int		enemyHealth = 1000;
	public static float		enemyForwardSpeed = 1.5F;
	public static float		enemyBackwardSpeed = 1F;
	public static float		enemyTurnSpeedDegrees = 90;
	public static int		enemyMissileSpeed = 3;
	public static int		enemyMissileDamage = 300;
	public static float		enemyMissileShotDelay = 5F;
	public static int		enemyMissileRange = 15;

	public static int		enemyHealthBoss = 5000;
	public static float		enemyForwardSpeedBoss = 3.5F;
	public static float		enemyBackwardSpeedBoss = 2F;
	public static float		enemyTurnSpeedDegreesBoss = 120;
	public static int		enemyMissileSpeedBoss = 5;
	public static int		enemyMissileDamageBoss = 1000;
	public static float		enemyMissileShotDelayBoss = 3F;
	public static int		enemyMissileRangeBoss = 25;


}

/**
 * Enum for ship stats. 
 * Trick: To convert from upgrade number to bitfield, do 1 << (int)upgrade number. Math is cool.
 **/
public enum shipUpgrades
{
	health = 0,
	forwardSpeed = 1,
	backwardSpeed = 2,
	turnSpeedDegree = 3,
	missileSpeed = 4,
	missileDamage = 5,
	missileShotDelay = 6,
	missileRange = 7,
}

/**
 * Bitfields for easier stat checking. It's a total waste of space, but it works.
 **/
public struct shipUpgradesBit
{
	public static int healthU = 0x00000001;
	public static int forwardSpeedU = 0x00000002;
	public static int backwardSpeedU = 0x00000004;
	public static int turnSpeedDegreesU = 0x00000008;
	public static int missileSpeedU = 0x00000010;
	public static int missileDamageU = 0x00000020;
	public static int missileShotDelayU = 0x00000040;
	public static int missileRangeU = 0x00000080;
}

public struct shipUpgradeCodes {
	public static string healthU = "Health";
	public static string forwardSpeedU = "Forward";
	public static string backwardSpeedU = "Backward";
	public static string turnSpeedDegreesU = "Turn";
	public static string missileSpeedU = "Speed";
	public static string missileDamageU = "Damage";
	public static string missileShotDelayU = "Shot";
	public static string missileRangeU = "Range";
}
