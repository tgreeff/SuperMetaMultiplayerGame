using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace UnityStandardAssets.Characters.ThirdPerson {

	public class Patrol : MonoBehaviour {
	
		public GameObject[] waypoints;
		private AICharacterControl control;
		private int count, activeWaypoint;
		public Transform player;

		private float dist = 10;
		NavMeshAgent navMeshAgent;


		// Use this for initialization
		void Start () {
			//NavMeshAgent navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
			activeWaypoint = 0;
			control = gameObject.GetComponent<AICharacterControl>();
			control.target = waypoints[activeWaypoint].transform;
			count = waypoints.Length;	
			
			if(player == null) {
				player = GameObject.FindGameObjectWithTag("Player").transform;
			}
		}
	
		void Update () {			
			this.gameObject.transform.LookAt(new Vector3(control.target.transform.position.x , 0, control.target.transform.position.z));
			Ray r = new Ray(transform.position, transform.TransformDirection( player.position));
			RaycastHit rayHit;

			Physics.Raycast(r, out rayHit, dist);
			if(rayHit.collider != null && rayHit.collider.gameObject.tag.Equals("Player")) {
				control.target = player;
				if (dist == 10) {
					dist = 0.5f;
				}
				if(dist == 0.5f) {
					SetSpeed(0f);
				}
			}
			else {
				SetSpeed(0.5f);
			}
		}

		void OnTriggerEnter(Collider other){
			if (other.tag.Equals("Waypoint") && other.gameObject.Equals(waypoints[activeWaypoint]) && (control.target != player)) {
				activeWaypoint++;
				if(activeWaypoint >= count) {
					activeWaypoint = 0;
				}
				control.target = waypoints[activeWaypoint].transform;
			}
		}

		public void SetSpeed(float value) {
			if(navMeshAgent == null) {
				//NavMeshAgent navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
			}
			else {
				navMeshAgent.speed = value;
			}
		}
	}
}
