using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnCamera : MonoBehaviour {
	[SerializeField]
	private GameObject target;
	public float speedModifier = 10.0f, speed;
	public Vector3 location, goalLocation;
	public float followingDistance, currentDistance;
	

	// Use this for initialization
	void Start () {
		Debug.Log ("Start Camera");
		location = this.transform.position;
		goalLocation = target.transform.position - (new Vector3(0, followingDistance, 0));
	}
	
	// Update is called once per frame
	void Update () {
		location = this.transform.position;
		goalLocation = target.transform.position - (new Vector3(0, followingDistance, 0));
		

		if (!goalLocation.Equals(location)){
			currentDistance = Vector3.Distance(location, goalLocation);
			speed = (followingDistance);
			this.transform.position = Vector3.Lerp(location, goalLocation, 1);
		}
	}
}
