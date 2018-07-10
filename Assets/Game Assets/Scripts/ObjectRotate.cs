using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotate : MonoBehaviour {
	[SerializeField]
	public float rotationX = 0.0f, rotationY = 0.0f, rotationZ = 0.0f;
	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		this.transform.Rotate(rotationX * Time.deltaTime, rotationY*Time.deltaTime, rotationZ * Time.deltaTime);
	}
}
