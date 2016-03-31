using UnityEngine;
using System.Collections;

public class FakeEnemies3D : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		Vector3 currentRot = transform.rotation.eulerAngles;
		currentRot.z += 4 * Time.deltaTime;
		transform.rotation = Quaternion.Euler(currentRot);
	}
}
