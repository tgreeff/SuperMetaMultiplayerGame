using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace UnityStandardAssets.Characters.ThirdPerson {
	public class Combat : MonoBehaviour {
		private GameObject player;
		private MeleeCombat parentMelee;
		private Transform holder;
		private bool stop = false;
		private float counter = 0f;

		// Use this for initialization
		void Start () {
			player = gameObject.transform.parent.gameObject;
			parentMelee = gameObject.GetComponentInParent<MeleeCombat>();		
		}
	
		// Update is called once per frame
		void Update () {	
		
			if(stop) {
				if(counter == 0f) {
					holder = player.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl>().target;
				}
				player.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl>().target = null;
				if(counter < 1) {
					counter += Time.deltaTime;
				}
				else {
					counter = 0f;
					player.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl>().target = holder;
					gameObject.GetComponentInParent<Patrol>().SetSpeed(0.5f);
				}
			}
		}
	
		void OnTriggerEnter(Collider other) {
			if(other.gameObject.tag.Equals("Player") && !parentMelee.swinging && player.tag.Equals("Enemy")){
				parentMelee.swinging = true;
				gameObject.GetComponentInParent<Patrol>().SetSpeed(0f);
			}
		}

		void OnTriggerStay(Collider other) {
			if (other.gameObject.tag.Equals("Player") && !parentMelee.swinging && player.tag.Equals("Enemy")) {
				if (!parentMelee.trigger) {
					parentMelee.swinging = true;
				}
				gameObject.GetComponentInParent<Patrol>().SetSpeed(0f);
			}
		}
	}
}
