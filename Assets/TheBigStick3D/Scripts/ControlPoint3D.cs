using UnityEngine;
using System.Collections;

public class ControlPoint3D : MonoBehaviour {

	int influence; //the status of a point. -100 for full enemy control, 100 for full player control.

	// Use this for initialization
	void Start () {
		influence = 0; //start nuetral. 

		//Only check every .25 seconds
		InvokeRepeating("updateInfluence", 0, 0.25F);
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

		int totalVal = 0;

		//check to see if they are ships
		foreach(Collider c in hitColliders) {
			if (c.name.Contains("Enemy")) {
				totalVal--;
			}
			else if(c.name.Contains("Player")) {
				totalVal++;
			}
		}

		//decide updated influence based off of player vs. ship counts
		influence += totalVal;
		//clamp that shiddy shiz
		influence = Mathf.Clamp(influence, -100, 100);
	}

	void OnDrawGizmosSelected() {
		Gizmos.DrawSphere(transform.position, 10); //ignore. Used for testing.
	}

	void OnGUI() {
		GUI.Label(new Rect(20, 100, 100, 200), "Control point influence: " + influence);
	}
}
