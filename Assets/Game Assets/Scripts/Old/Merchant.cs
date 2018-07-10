using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Merchant : MonoBehaviour {

	public GameObject shopPanel;
	public GameObject interact;

	// Use this for initialization
	void Start() {
		if (shopPanel == null) {
			shopPanel = GameObject.Find("Shop");
		}

		if (interact == null) {
			interact = GameObject.Find("Interaction");
		}

		interact.GetComponent<Text>().text = "";
	}

	// Update is called once per frame
	void Update() {

	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag.Equals("Player")) {
			interact.GetComponent<Text>().text = "Press E to open the shop.";
		}
	}

	void OnTriggerStay(Collider other) {
		if (Input.GetButton("E") && (other.gameObject.tag.Equals("Player")) && !shopPanel.activeSelf) {
			interact.GetComponent<Text>().text = "Press E to close the shop.";
			shopPanel.SetActive(true);
		}
		else if(Input.GetButton("E") && (other.gameObject.tag.Equals("Player")) && shopPanel.activeSelf) {
			interact.GetComponent<Text>().text = "Press E to open the shop.";
			shopPanel.SetActive(false);
		}
	}

	void OnTriggerExit(Collider other) {
		interact.GetComponent<Text>().text = "";
		shopPanel.SetActive(false);
	}
}