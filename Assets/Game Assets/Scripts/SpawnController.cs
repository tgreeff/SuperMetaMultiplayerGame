using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour {
	public int index;

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "player") {
			//WorldProperties.avail[index] = false;
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject.tag == "player") {
			//WorldProperties.avail[index] = true;
		}
	}
}
