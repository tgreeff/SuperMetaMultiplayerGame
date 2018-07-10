using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMove : MonoBehaviour {
	private bool reverseX, reverseY, reverseZ;
	private float xStart, yStart, zStart;
	private float x, y, z;

	public float xScale = 0.0f, yScale = 0.0f, zScale = 0.0f;
	public float xStop = 0.0f, yStop = 0.0f, zStop = 0.0f;

	// Use this for initialization
	void Start () {				
		x = transform.position.x;
		y = transform.position.y;
		z = transform.position.z;
		xStart = x;
		yStart = y;
		zStart = z;

		if(xStop < xStart) { //positive
			reverseX = false;
		}
		else { //negative
			reverseX = true;
			xScale *= -1;
		}

		if (yStop < yStart) { //positive
			reverseY = false;
		}
		else { //negative
			reverseY = true;
			yScale *= -1;
		}
		if (zStop < zStart) { //positive
			reverseZ = false;
		}
		else { //negative
			reverseZ = true;
			zScale *= -1;
		}

	}
	
	// Update is called once per frame
	void Update () {
		x += xScale * Time.deltaTime;
		y += yScale * Time.deltaTime;
		z += zScale * Time.deltaTime;

		//Check x
		if (reverseX) {			
			if(xStart >= x) { 
				xScale *= -1;
				x = xStart;
			}
			else if(xStop <= x) {
				xScale *= -1;
				x = xStop;
			}
		}
		else {
			if (xStart <= x) {
				xScale *= -1;
				x = xStart;
			}
			else if (xStop >= x) { 
				xScale *= -1;
				x = xStop;
			}
		}
		
		//Check y
		if(reverseY) {
			if (yStart >= y) {
				yScale *= -1;
				y = yStart;
			}
			else if (yStop <= y) {
				yScale *= -1;
				y = yStop;
			}
		}
		else {
			if (yStart <= y) {
				yScale *= -1;
				y = yStart;
			}
			else if (yStop >= y) {
				yScale *= -1;
				y = yStop;
			}
		}
		
		//Check z
		if(reverseZ) {
			if (zStart >= z) {
				zScale *= -1;
				z = zStart;
			}
			else if (zStop <= z) {
				zScale *= -1;
				z = zStop;
			}
		}
		else {
			if (zStart <= z) {
				zScale *= -1;
				z = zStart;
			}
			else if (zStop >= z) {
				zScale *= -1;
				z = zStop;
			}
		}
		transform.position = new Vector3(x, y, z);
	}
}
