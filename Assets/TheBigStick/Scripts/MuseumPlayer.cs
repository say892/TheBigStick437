using UnityEngine;
using System.Collections;

/**
 * A museum player is each individual player. They are identified with their phone, and this 
 * "should" remember them even after a few days. It depends on the phone I think. Anyway,
 * this is mainly to keep track of what players have which upgrades.
 **/
public class MuseumPlayer {

	private HFTGamepad player;
	private int score;

	private int playerUpgrades;


	public MuseumPlayer() {
		//This should never be used, only to test without needing a controller
	}

	public MuseumPlayer(HFTGamepad p) {
		player = p;
		score = 0;
		playerUpgrades = 0;
	}

	public HFTGamepad getPlayer() {
		return player;
	}

	public int getScore() {
		return score;
	}

	public void addScore(int s) {
		score += s;
	}

	public int getPlayerUpgrades() {
		return playerUpgrades;
	}

	public void addPlayerUpgrade(shipUpgades upgradeNum) {
		playerUpgrades += (1 << (int)upgradeNum);
	}
		
}

/**
 * Enum for ship stats. 
 * Trick: To convert from upgrade number to bitfield, do 1 << (int)upgrade number. Math is cool.
 **/
public enum shipUpgades {
	
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
public struct shipUpgradesBit {
	public static int healthU =           0x00000001;
	public static int forwardSpeedU =     0x00000002;
	public static int backwardSpeedU =    0x00000004;
	public static int turnSpeedDegreesU = 0x00000008;
	public static int missileSpeedU =     0x00000010;
	public static int missileDamageU =    0x00000020;
	public static int missileShotDelayU = 0x00000040;
	public static int missileRangeU =     0x00000080;
}