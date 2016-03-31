using UnityEngine;
using System.Collections;

public class AnimationEnd3D : MonoBehaviour {

	private float timer;

	// Use this for initialization
	void Start () {
		timer = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
		timer += Time.deltaTime;

		//Sorry everyone
		if (timer > 0.833F) {
			Destroy(this.gameObject);	
		}
	}
}
