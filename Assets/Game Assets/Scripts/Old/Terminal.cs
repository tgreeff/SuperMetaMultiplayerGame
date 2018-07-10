using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Terminal : MonoBehaviour {

	public GameObject door;
	public Text interact;

	// Use this for initialization
	void Start () {		
		if (door == null) {
			door = GameObject.Find("Door");
		}
		
		if(interact == null) {
			//interact;
		}

		interact.text = "";
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag.Equals("Player")) {
			if(door.GetComponent<DoorNormal>().isOpen()) {
				interact.text = "Press E to Open the door.";
			}
			else {
				interact.text = "Press E to Close the door.";
			}
			
		}
	}

	//Used https://docs.unity3d.com/ScriptReference/Collider.OnTriggerStay.html
	//to understand the structure
	void OnTriggerStay(Collider other) {
		if (Input.GetButton("E") && (other.gameObject.tag.Equals("Player"))) {
			//Used https://forum.unity.com/threads/calling-function-from-other-scripts-c.57072/
			//to figure our how to access functions from other classes.
			door.GetComponent<DoorNormal>().trigger();
		}
		else if(other.gameObject.tag.Equals("Player")) {
			if (door.GetComponent<DoorNormal>().isOpen()) {
				interact.text = "Press E to Close the door.";
			}
			else {
				interact.text = "Press E to Open the door.";
			}
		}
		
	}

	void OnTriggerExit(Collider other) {
			interact.text = "";	
	}
}
