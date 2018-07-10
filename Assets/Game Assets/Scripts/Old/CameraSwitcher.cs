using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour {
	public GameObject camera1, camera2;
	private bool secondaryOn = false;

	// Use this for initialization
	void Start () {
		if(camera1 == null) {
			camera1 = GameObject.FindGameObjectWithTag("MainCamera");
		}
		if(camera2 == null) {
			camera2 = GameObject.FindGameObjectWithTag("SecondaryCamera");
		}
		camera2.SetActive(false);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(Input.GetKey(KeyCode.C)) {
			if(secondaryOn) {
				secondaryOn = false;
				camera1.SetActive(false);
				camera2.SetActive(true);
			}
			else {
				secondaryOn = true;
				camera1.SetActive(true);
				camera2.SetActive(false);
			}
		}
	}
}
