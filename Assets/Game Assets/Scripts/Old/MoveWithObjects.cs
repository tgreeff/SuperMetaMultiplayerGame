using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithObjects : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//Used https://answers.unity.com/questions/12083/how-to-get-a-character-to-move-with-a-moving-platf.html
	//to attempt to get the player to move with the platform horizontally.
	void OnCollisionEnter(Collision other) {
		if (other.gameObject.name.Equals("PlatformTrigger")) {
			this.transform.parent = other.transform;
			//this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
		}

	}

	void OnCollisionExit(Collision other) {
		if (other.gameObject.name.Equals("PlatformTrigger")) {
			this.transform.parent = null;
			//this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
		}
	}
}
