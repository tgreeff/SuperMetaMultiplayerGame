using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour {
	public GameObject dialogPanel;
	public Text dialogText;
	public GameObject interact;
	public GameObject item;
	public Transform location;

	private bool talked = false;
	private bool talking = false;
	private int dialogIndex = 0;
	private float dialogCounter = 0;

	// Use this for initialization
	void Start() {
		if (dialogPanel == null) {
			dialogPanel = GameObject.Find("Dialog");
		}

		if(dialogText == null) {
			dialogText = dialogPanel.GetComponentInChildren<Text>();
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
			interact.GetComponent<Text>().text = "Press E to talk.";
		}
	}

	void OnTriggerStay(Collider other) {
		if (Input.GetButton("E") && (other.gameObject.tag.Equals("Player")) && !dialogPanel.activeSelf) {
			interact.GetComponent<Text>().text = "";
			dialogPanel.SetActive(true);
			if(!talked) {
				dialogText.text = "[Mini Me] HEY, I don't know you. Who are you?";
				talking = true;
			}
			else {
				dialogText.text = "[Mini Me] Hey, silent guy.";
				talking = true;
			}					
		}

		if(talking) {
			dialogCounter += Time.deltaTime;
		}	

		if ((talking) && (dialogCounter >= 3)) {
			
			switch(dialogIndex) {
				case 0:
					if (talked) {
						dialogText.text = "[Mini Me] Like I said. There's some guys out there.";
					}
					else {
						dialogText.text = "[Mini Me] Well anyway, there are some guys out there by the way.";
					}
					break;
				case 1:
					dialogText.text = "[Mini Me] I would go out there, but I can't fight them.";
					break;
				case 2:
					dialogText.text = "[Mini Me] If you can take care of them, I would be grateful.";
					break;
				case 3:
					if(talked) {
						dialogText.text = "[Mini Me] Use that bat to beat them up.";
					}
					else {
						dialogText.text = "[Mini Me] Here's a bat to beat them up with.";
						item.transform.position = location.transform.position;
						item.transform.rotation = location.transform.rotation;
						item.transform.parent = location.transform;
					}
					break;
				case 4:
					dialogText.text = "[Mini Me] Good luck out there!";
					talking = false;
					talked = true;
					break;
			}
			dialogCounter = 0;
			dialogIndex++;
			if(dialogIndex > 4) {
				dialogIndex = 0;
			}

		}
		
	}

	void OnTriggerExit(Collider other) {
		interact.GetComponent<Text>().text = "";
		dialogPanel.SetActive(false);
	}
}