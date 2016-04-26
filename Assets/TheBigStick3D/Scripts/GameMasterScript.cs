using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/**
 * This class keeps track of all players that have ever been connected.
 * Basically, we can put important game stuff here like spawning boss ships, etc.
 * Note: This list will be lost if the game is restarted.
 **/
public class GameMasterScript : MonoBehaviour {

	private List<MuseumPlayer> allPlayers;
	private static GameMasterScript instance = null;

	[SerializeField]
	private GameObject theBigStickSpawnPos;

	[SerializeField]
	private GameObject explodePrefab;

	[SerializeField]
	private Text scoreboard;



	void Awake() {

		//I don't know if we'll be changing scenes at all, but yay singletons
		if (instance == null) {
			instance = this;
		}
		else if (instance != this) {
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}


	// Use this for initialization
	void Start () {
		allPlayers = new List<MuseumPlayer>();

		//allPlayers.Add(new MuseumPlayer3D());
		//This is how we'll have to add upgrades
		//allPlayers[0].addPlayerUpgrade(shipUpgrades3D.health);
		//And this is how we check if the player has the upgrade. Returns 1 if they do.
		//allPlayers[0].getPlayerUpgrades() & shipUpgrades3DBit.healthU;
	
	}
	
	// Update is called once per frame
	void Update () {

		//Cheat!
		/*
		if (Input.GetKeyDown(KeyCode.L)) {
			allPlayers[0].addPlayerUpgrade(shipUpgrades3D.health);
			allPlayers[0].addPlayerUpgrade(shipUpgrades3D.forwardSpeed);
			allPlayers[0].addPlayerUpgrade(shipUpgrades3D.backwardSpeed);
			allPlayers[0].addPlayerUpgrade(shipUpgrades3D.turnSpeedDegree);
			allPlayers[0].addPlayerUpgrade(shipUpgrades3D.missileSpeed);
			allPlayers[0].addPlayerUpgrade(shipUpgrades3D.missileDamage);
			allPlayers[0].addPlayerUpgrade(shipUpgrades3D.missileRange);
			allPlayers[0].addPlayerUpgrade(shipUpgrades3D.missileShotDelay);
		}

		if (Input.GetKeyDown(KeyCode.P)) {
			Instantiate(explodePrefab, Vector3.zero, Quaternion.identity);
		}*/
	}

	public bool containsPlayer(HFTGamepad p) {
		foreach (MuseumPlayer mp in allPlayers) {
			if (mp.getPlayer().NetPlayer.GetSessionId() == p.NetPlayer.GetSessionId()) {
				return true;
			}
		}
		return false;
	}

	public MuseumPlayer findPlayer(HFTGamepad p) {
		foreach (MuseumPlayer mp in allPlayers) {
			if (mp.getPlayer().NetPlayer.GetSessionId() == p.NetPlayer.GetSessionId()) {
				return mp;
			}
		}
		return null;
	}


	public MuseumPlayer addPlayer(HFTGamepad p) {

		if (!containsPlayer(p)) {
			MuseumPlayer tempPlayer = new MuseumPlayer(p);
			allPlayers.Add(tempPlayer);
			return tempPlayer;
		}
		else {
			return findPlayer(p);
		}
	}
		

	public void removePlayer(HFTGamepad p) {
		foreach (MuseumPlayer pp in allPlayers) {
			if (pp.getPlayer().NetPlayer.GetSessionId() == p.NetPlayer.GetSessionId()) {
				allPlayers.Remove(pp);
				return;
			}
		}
	}

	public void removeAllPlayers() {
		allPlayers.Clear();
	}

	public Vector3 getSpawnPos() {
		return theBigStickSpawnPos.transform.position;
	}

	void OnGUI() {

		//GUI.Label(new Rect(20, 20, 500, 100), "There are " + allPlayers.Count + " players registered");

		//allPlayers.Sort(delegate(MuseumPlayer m1, MuseumPlayer m2)
  //      {
		//	return m2.getScore().CompareTo(m1.getScore());
		//});

		//scoreboard.text = "";
		//foreach(MuseumPlayer player in allPlayers)
		//{
		//	scoreboard.text += player.getPlayer().Name + " " + player.getScore() + "\n";
		//}
	}

}
