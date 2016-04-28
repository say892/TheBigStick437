using UnityEngine;
using System.Collections;

public class ControlPoint : MonoBehaviour {

	private float influence; //the status of a point. -100 for full enemy control, 100 for full player control.
	private int nearbyShips; //the number of ships nearby +1 for every player ship, -1 for every enemy ship. 0 if no ships/even ships around
	private MeshRenderer rend;
	private SpriteRenderer spriteRend;

	private bool active;

	private bool awardedPoints;

	void Awake() {
		influence = 0; //start nuetral. 
		rend = GetComponent<MeshRenderer>();
		spriteRend = GetComponentInChildren<SpriteRenderer>();
		active = false;
		rend.enabled = false;
		spriteRend.enabled = false;
		awardedPoints = false;

	}
	// Use this for initialization
	void Start () {


		//Only check every .25 seconds
		//InvokeRepeating("updateInfluence", 0, 0.25F);
		//It will take 25 seconds for one player themselves to get to 100 influence assuming no enemies nearby...
	}

	/**
	 * Gets the stronger "team" of ships by seeing who is within range of the control point
	 * Returns a positive number if there are more players, negative if more enemies, 0 if tie.
	 */
	void updateInfluence() {
		//Note: Range is hardcoded to 10 as that is what the scale of the capture zone sprite is. 
		//You can use Transform.GetChild(0).transform.localScale.x to get this value if we want different sized zones.
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, 10); //gets all objects within range

		float totalVal = 0;

		//check to see if they are ships
		foreach(Collider c in hitColliders) {
			if (c.name.Contains("Enemy")) {
				totalVal--;
			}
			else if(c.name.Contains("Player")) {
				totalVal++;
			}
		}

		nearbyShips = (int)totalVal;

		print(gameObject.name + ", " + hitColliders.Length);
		//if there are no ships, slowly lose influence
		if (hitColliders.Length == 0 && influence < 0) totalVal = .25F;
		else if (hitColliders.Length == 0 && influence > 0) totalVal = -.25F;


		//decide updated influence based off of player vs. ship counts
		influence += totalVal;

		//clamp that shiddy shiz
		influence = Mathf.Clamp(influence, -100, 100);


		//update the sphere color to show who has the most influence
		if (influence <= 0) rend.material.color = Color.Lerp(Color.white, Color.red, -influence/100);
		else rend.material.color = Color.Lerp(Color.white, Color.blue, influence/100);

		//updating ring based off of influence (FOR ZACH)
		//if (influence <= 0) spriteRend.material.color = Color.Lerp(Color.white, Color.red, -influence/100);
		//else spriteRend.material.color = Color.Lerp(Color.white, Color.blue, influence/100);

		//update the circle ring to be a solid color if one team has total control
		if (influence >= 40)
		{
			spriteRend.color = Color.blue;
			if (!awardedPoints) {
				awardedPoints = true;
				foreach(Collider c in hitColliders) {
					if(c.name.Contains("Player")) {
						c.GetComponent<PlayerShip>().addScore(500);
					}
				}
			}
		}
		else if (influence <= -40) spriteRend.color = Color.red;
		else {
			awardedPoints = false;
			spriteRend.color = Color.white;
		}


		//if ((int)influence == 40 && totalVal > 0) {
		//	
	//		print("IT'S OURS BOIS " + hitColliders.Length);
	//	}



	}

	public bool isActive() {
		return active;
	}

	public float getInfluence() {
		return influence;
	}

	public int getNearbyShips() {
		return nearbyShips;
	}

	public void setActive() {
		active = true;
		rend.enabled = true;
		spriteRend.enabled = true;
		InvokeRepeating("updateInfluence", 0, 0.25F);
	}

	public void setInactive() {
		active = false;
		rend.enabled = false;
		spriteRend.enabled = false;
		CancelInvoke();
	}

	public void setInfluence(float influence) {
		this.influence = influence;
	}

	void OnDrawGizmosSelected() {
		//Gizmos.DrawSphere(transform.position, 10); //ignore. Used for testing.
	}

	void OnGUI() {
		//GUI.Label(new Rect(20, 100, 100, 200), "Control point influence: " + influence);
	}
}


