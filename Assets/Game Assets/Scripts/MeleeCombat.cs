using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCombat : MonoBehaviour {
	public Transform hand;
	public GameObject weapon;
	public GameObject player;

	public bool swinging = false;
	public bool trigger = false;
	private bool respawn = false;
	private int count = 0, counter = 0;
	public int health = 100;

	// Use this for initialization
	void Start() {
		
	}

	// Update is called once per frame
	void Update() {
		bool parent = weapon.transform.parent == hand.transform;

		if ((Input.GetMouseButtonDown(0)) && !swinging && parent) {
			swinging = true;
		}
		else if (swinging) {
			weapon.transform.Rotate(new Vector3( 20f, -30f, 5f));
			count += 1;			
		}
		else if (trigger) {
			counter += 1;
		}

		if (count >= 10) {
			swinging = false;

			count = 0;
			trigger = true;
			
			weapon.transform.position = hand.transform.position;
			weapon.transform.rotation = hand.transform.rotation;
		}
		else if (counter >= 200) {
			counter = 0;
			trigger = false;
			swinging = false;
		}

		else if (health <= 0) {
			player.SetActive(false);
			respawn = true;
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag.Equals("Player") && swinging && !trigger) {
			HUDController.health -= 20;
			swinging = false;
			trigger = true;
		}
		if (other.gameObject.tag.Equals("Enemy") && swinging) {
			swinging = false;
			trigger = true;
			other.gameObject.GetComponentInChildren<MeleeCombat>().health -= 50;
		}
	}

	public bool IsRespawning() {
		return respawn;
	}

	public bool IsSwinging() {
		return swinging;
	}
}
