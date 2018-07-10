using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorNormal : MonoBehaviour {

	private float currentAngle = 0.0f;
	private float rotateAngle = 1.0f;
	private float scale = 50.0f;
	private bool moveDoor = false;	

	// Use this for initialization
	void Start () {
		//obj = GameObject.Find("Door");
	}
	
	// Update is called once per frame
	void Update () {
		if(moveDoor) {			
			this.transform.Rotate(new Vector3(0, scale*rotateAngle*Time.deltaTime, 0));
			currentAngle += scale * rotateAngle * Time.deltaTime;
			if(currentAngle >= 90.0f || currentAngle <= 0.0f){
				moveDoor = false;
				rotateAngle *= -1;
			}
		}								
	}

	public void trigger() {
		moveDoor = true;
	}

	public bool isOpen() {
		if(currentAngle >= 90.0f) {
			return true;
		}
		else if (currentAngle < 90.0f) {
			return false;
		}
		else {
			return false;
		}
	}
}
