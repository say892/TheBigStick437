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

	public void addPlayerUpgrade(shipUpgrades upgradeNum) {
		int bit = 1 << (int)upgradeNum;
		//make sure that the player doesn't have the upgrade already
		if ((playerUpgrades & bit) == 0) {
			playerUpgrades += (bit);
		}
	}
}
