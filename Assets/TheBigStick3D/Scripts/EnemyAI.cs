using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour {


	private enum enemyStates {
		wandering,
		capturing,
		attacking
	}

	private enemyStates currentState;

	private ControlPoints allControlPoints;

	private ControlPoint currentTarget;
	//private ControlPoint lastTarget;

	// Use this for initialization
	void Start () {
		currentState = enemyStates.wandering;
		allControlPoints = GameObject.Find("ControlPoints").GetComponent<ControlPoints>();
	}
	
	// Update is called once per frame
	void Update () {


		if (currentState == enemyStates.wandering) doWanderingState();
		else if (currentState == enemyStates.capturing) doCapturingState();

	}



	void doWanderingState() {

		//I walk a lonely... sea?
		if (currentTarget == null) {

			//attempt to balance enemies shouldn't have more than 5 (still possible though)
			if (allControlPoints.getNumEnemyPoints() <= 4) {

				//if the players have an advantage, take their points
				if (allControlPoints.getNumPlayerPoints() >= 5) {
					currentTarget = allControlPoints.findNearestPlayerPoint(transform.position);
				}
				//or give them some time to come back and only go for unoccupied
				else {
					currentTarget = allControlPoints.findRandomNeutralPoint();
				}

			}
			//if we're at max points let's just patrol randomly
			else {
				currentTarget = allControlPoints.findRandomEnemyPoint();
			}

		}
		//do we have a heading?
		else if (Vector3.Angle(transform.forward, currentTarget.transform.position - transform.position) > 1.0F) {
			rotateTowardsPosition(currentTarget.transform.position);
		}
		//FULL STEAM AHEAD
		else {
			moveForward();
		}


		//we have arrived
		if (distanceToPoint(currentTarget) < 5*5) {
			//we own this point!
			if (currentTarget.getInfluence() < -40) {
				currentTarget = null; //wander somewhere else
			}
			else {
				//take this point
				currentState = enemyStates.capturing;
			}
		}

	}

	void doCapturingState() {
		if (currentTarget.getInfluence() < -60) {
			//get a new target, you did your job solider.
			//lastTarget = currentTarget;
			currentTarget = null;
			currentState = enemyStates.wandering;
		}


		GameObject nearestPlayer = findNearestPlayer(transform.position);
		if (nearestPlayer != null) {
			//ATTACK!
			if (Vector3.Angle(transform.forward, nearestPlayer.transform.position - transform.position) > 1.0F) {
				rotateTowardsPosition(nearestPlayer.transform.position);
			}
			else {
				
			}

		}




	}

	int distanceToPoint(ControlPoint p) {
		return (int)(p.transform.position - transform.position).sqrMagnitude;
	}

	void rotateTowardsPosition(Vector3 pos) {
		Vector3 newDir = Vector3.RotateTowards(transform.forward, pos - transform.position,
			Mathf.Deg2Rad * ShipValues.enemyTurnSpeedDegrees * Time.deltaTime, 0);
		transform.rotation = Quaternion.LookRotation(newDir);
	}

	void moveForward() {
		transform.position += transform.forward * ShipValues.enemyForwardSpeed * Time.deltaTime;
	}

	GameObject findNearestPlayer(Vector3 pos) {

		Collider[] hitColliders = Physics.OverlapSphere(transform.position, 10); //gets all objects within range

		List<GameObject> nearbyPlayers = new List<GameObject>();

		//check to see if they are ships
		foreach(Collider c in hitColliders) {
			if(c.name.Contains("Player")) {
				nearbyPlayers.Add(c.gameObject);
			}
		}

		GameObject closest = null;
		float distance = 9999999;
		foreach(GameObject p in nearbyPlayers) {
			float tempDist = (p.transform.position - pos).sqrMagnitude;
			if (tempDist < distance) {
				closest = p;
				tempDist = distance;
			}
		}

		return closest;
	}

}
