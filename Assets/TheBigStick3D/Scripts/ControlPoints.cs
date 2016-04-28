using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ControlPointNames {
	Guadacanal,
	Tahiti,
	Aleutains,
	Hawaii, 
	Tokyo,
	Guam,
	WakeIsland,
	SanFrancisco,
	Midway,
};

public class ControlPoints : MonoBehaviour {

	private ControlPoint[] allPoints;

	// Use this for initialization
	void Start () {


		ControlPoint[] tempPoints = GetComponentsInChildren<ControlPoint>();
		allPoints = new ControlPoint[tempPoints.Length];

		//this is probably a stupid way to do it.
		foreach (ControlPoint p in tempPoints) {
			if (p.name.Equals("Guadacanal")) allPoints[(int)ControlPointNames.Guadacanal] = p;
			if (p.name.Equals("Tahiti")) allPoints[(int)ControlPointNames.Tahiti] = p;
			if (p.name.Equals("Aleutians")) allPoints[(int)ControlPointNames.Aleutains] = p;
			if (p.name.Equals("Hawaii")) allPoints[(int)ControlPointNames.Hawaii] = p;
			if (p.name.Equals("Tokyo")) allPoints[(int)ControlPointNames.Tokyo] = p;
			if (p.name.Equals("Guam")) allPoints[(int)ControlPointNames.Guam] = p;
			if (p.name.Equals("Wake Island")) allPoints[(int)ControlPointNames.WakeIsland] = p;
			if (p.name.Equals("San Francisco")) allPoints[(int)ControlPointNames.SanFrancisco] = p;
			if (p.name.Equals("Midway")) allPoints[(int)ControlPointNames.Midway] = p;
		}
	
		//foreach(ControlPoint p in allPoints) {
			//print (p.name);
			//print (p.getInfluence());
		//}
		//set the 2 standard control points
		allPoints[(int)ControlPointNames.SanFrancisco].setActive();
		allPoints[(int)ControlPointNames.SanFrancisco].setInfluence(100);

		allPoints[(int)ControlPointNames.Tokyo].setActive();
		allPoints[(int)ControlPointNames.Tokyo].setInfluence(-100);


		//turn other points on
		allPoints[(int)ControlPointNames.Guadacanal].setActive();
		allPoints[(int)ControlPointNames.Tahiti].setActive();
		allPoints[(int)ControlPointNames.Aleutains].setActive();
		allPoints[(int)ControlPointNames.Hawaii].setActive();
		allPoints[(int)ControlPointNames.Guam].setActive();
		allPoints[(int)ControlPointNames.WakeIsland].setActive();
		allPoints[(int)ControlPointNames.Midway].setActive();


	}
	
	// Update is called once per frame
	void Update () {


		//never lose these two points
		allPoints[(int)ControlPointNames.SanFrancisco].setInfluence(100);
		allPoints[(int)ControlPointNames.Tokyo].setInfluence(-100);

		/*Works randomly
		if (Input.GetKeyDown(KeyCode.Q)) {
			int rand = Random.Range(0, 8);

			if (!allPoints[rand].isActive()) {
				allPoints[rand].setActive();
			}
			
		}*/
	}

	public Vector3 getEnemySpawnPos() {

		List<ControlPoint> validPoints = new List<ControlPoint>();

		foreach (ControlPoint p in allPoints) {
			if (p.isActive() && p.getInfluence() < -40) {
				validPoints.Add(p);
			}
		}

		int num = Random.Range(0, validPoints.Count);
		return validPoints[num].gameObject.transform.GetChild(1).position;
	}

	public Vector3 getPlayerSpawnPos() {
		List<ControlPoint> validPoints = new List<ControlPoint>();

		foreach (ControlPoint p in allPoints) {
			if (p.isActive() && p.getInfluence() > 40) {
				validPoints.Add(p);
			}
		}

		int num = Random.Range(0, validPoints.Count);
		print((validPoints[num].gameObject).name);
		return validPoints[num].gameObject.transform.GetChild(1).position;
	}

	public ControlPoint[] getActivePoints() {
		
		List<ControlPoint> validPoints = new List<ControlPoint>();

		foreach (ControlPoint p in allPoints) {
			if (p.isActive()) {
				validPoints.Add(p);
			}
		}
		return validPoints.ToArray();
	}

	public ControlPoint findNearestEnemyPoint(Vector3 pos) {

		List<ControlPoint> validPoints = new List<ControlPoint>();

		foreach (ControlPoint p in allPoints) {
			if (p.isActive() && p.getInfluence() < -40) {
				validPoints.Add(p);
			}
		}

		ControlPoint closest = null;
		float distance = 9999999;
		foreach(ControlPoint p in validPoints) {
			float tempDist = (p.transform.position - pos).sqrMagnitude;
			if (tempDist < distance) {
				closest = p;
				tempDist = distance;
			}
		}

		return closest;
	}

	public ControlPoint findRandomEnemyPoint() {

		List<ControlPoint> validPoints = new List<ControlPoint>();

		foreach (ControlPoint p in allPoints) {
			if (p.isActive() && p.getInfluence() < -40) {
				validPoints.Add(p);
			}
		}

		int num = Random.Range(0, validPoints.Count);
		return validPoints[num];

	}

	public ControlPoint findNearestPlayerPoint(Vector3 pos) {

		List<ControlPoint> validPoints = new List<ControlPoint>();

		foreach (ControlPoint p in allPoints) {
			if (p.isActive() && p.getInfluence() > 40) {
				if (!p.name.Equals("San Francisco")) {
					validPoints.Add(p);
				}

			}
		}

		ControlPoint closest = null;
		float distance = 9999999;
		foreach(ControlPoint p in validPoints) {
			float tempDist = (p.transform.position - pos).sqrMagnitude;
			if (tempDist < distance) {
				closest = p;
				tempDist = distance;
			}
		}

		return closest;
	}

	public ControlPoint findNearestNeutralPoint(Vector3 pos) {

		List<ControlPoint> validPoints = new List<ControlPoint>();

		foreach (ControlPoint p in allPoints) {
			if (p.isActive() && p.getInfluence() > -40 && p.getInfluence() < 40) {
				validPoints.Add(p);
			}
		}

		ControlPoint closest = null;
		float distance = 9999999;
		foreach(ControlPoint p in validPoints) {
			float tempDist = (p.transform.position - pos).sqrMagnitude;
			if (tempDist < distance) {
				closest = p;
				tempDist = distance;
			}
		}

		return closest;
	}

	public ControlPoint findRandomNeutralPoint() {

		List<ControlPoint> validPoints = new List<ControlPoint>();

		foreach (ControlPoint p in allPoints) {
			if (p.isActive() && p.getInfluence() > -40 && p.getInfluence() < 40) {
				validPoints.Add(p);
			}
		}

		int num = Random.Range(0, validPoints.Count);
		return validPoints[num];

	}


	public int getNumEnemyPoints() {
		List<ControlPoint> validPoints = new List<ControlPoint>();

		foreach (ControlPoint p in allPoints) {
			if (p.isActive() && p.getInfluence() < -40) {
				validPoints.Add(p);
			}
		}

		return validPoints.Count;
	}

	public int getNumPlayerPoints() {
		List<ControlPoint> validPoints = new List<ControlPoint>();

		foreach (ControlPoint p in allPoints) {
			if (p.isActive() && p.getInfluence() > 40) {
				validPoints.Add(p);
			}
		}

		return validPoints.Count;
	}

}
