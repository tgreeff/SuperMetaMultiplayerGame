using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class HUDController : MonoBehaviour {

	[SerializeField]
	public Button[] slot;
	public GameObject inventoryPanel, dialogPanel, diedPanel;
	public GameObject shopPanel;
	public Button saveButton, exitButton, purchaseButton, diedExitButton, diedLoadButton;
	public Text moneyBalanceText, healthBalanceText, healthTotalText, interactionText, dialogText;
	public Image compass;
	public Texture boxingBox;
	public GameObject playerObject;

	//Static Variables
	public static int count = 0;
	public static int money = 0;
	public static int health = 100;
	public static Button[] slotStatic;
	public static Weapon[] weapons = new Weapon[10];

	private Player player;
	private float interactionTime, interactionStopTime;
	private float dialogTime, dialogStopTime;
	private bool dialogOn;
	private int index;

	// Use this for initialization
	public void Start () {		
		slotStatic = new Button[slot.Length];
		for(int x = 0; x < slot.Length; x++) {
			slotStatic[x] = slot[x];
			slot[x].onClick.AddListener(sellClick);
		}

		saveButton.onClick.AddListener(saveClick);
		exitButton.onClick.AddListener(exitClick);
		purchaseButton.onClick.AddListener(purchaseClick);
		diedExitButton.onClick.AddListener(exitClick);
		diedLoadButton.onClick.AddListener(loadClick);

		moneyBalanceText.text = "$0";

		interactionTime = 0;
		interactionStopTime = 5;

		dialogPanel.SetActive(false);
		dialogText.text = "";
		dialogOn = false;
		dialogTime = 0;
		dialogStopTime = 10;

		inventoryPanel.SetActive(false);
		shopPanel.SetActive(false);
		diedPanel.SetActive(false);

		index = 0;
	}

	// Update is called once per frame
	public void Update () {
		moneyBalanceText.text = "" + money.ToString();
		healthBalanceText.text = health.ToString();
		if (Input.GetKeyDown(KeyCode.I)) {
			inventoryPanel.SetActive(!inventoryPanel.activeSelf);
		}
		else if(inventoryPanel.activeSelf && weapons[index] != null) {
			Weapon i = weapons[index];		

			slot[index].gameObject.GetComponentInChildren<Text>().text = i.name;
			slot[index].gameObject.GetComponent<RawImage>().texture = i.image;			
			slot[index] = slotStatic[index];
			index++;
			if (index > slot.Length) {
				index = 0;
			}	
		}

		if(health <= 0) {			
			diedPanel.SetActive(true);
		}

		//Modified example from: https://answers.unity.com/questions/476128/how-to-change-quaternion-by-180-degrees.html
		Vector3 rotation = player.transform.rotation.eulerAngles;
		rotation = new Vector3(0, 180, rotation.y);
		compass.transform.rotation = Quaternion.Euler(rotation);

		if (interactionText.text != "") {
			interactionTime += Time.deltaTime;
			if (interactionTime > interactionStopTime) {
				interactionTime = 0;
				interactionText.text = "";
			}
		}

		if (dialogOn) {
			dialogTime += Time.deltaTime;
			if (dialogTime > dialogStopTime) {
				dialogTime = 0;
				dialogOn = false;
				dialogText.text = "";
				dialogPanel.SetActive(false);
			}
		}
	}

	public void saveClick() {
		try {
			interactionText.text = "Saving...";
			StreamWriter writer = new StreamWriter("Saves/Save.sav");
			writer.WriteLine("gamma=" + WorldProperties.gamma.ToString());
			writer.WriteLine("money=" + money.ToString());
			writer.WriteLine("health=" + health.ToString());
			writer.WriteLine("position=" + player.transform.position.ToString());
			for (int x = 0; x < weapons.Length; x++) {
				writer.WriteLine("index=" + x.ToString());
				writer.WriteLine(weapons[x].toFileString());
			}
			writer.Close();
		}
		catch (Exception e) {
			interactionText.text = "Saving Failed.";
			Debug.Log(e.Message);
		}
		interactionText.text = "Saving Done.";

	}

	public void exitClick() {
		SceneManager.LoadScene(0);	
	}

	//Buys boxing box
	public void purchaseClick() {
		if(money >= 500) {			
			if (!(HUDController.count == 10)) {
				Weapon w = new Weapon(null, boxingBox, "Boxing Box", 1, 0, 0);
				HUDController.weapons[HUDController.count] = w;
				HUDController.count++;

				money += -500;
				dialogText.text += "\n [Merchant] You bought the Boxing Box for 500 gold. \n";
				dialogOn = true;
				dialogPanel.SetActive(true);
			}	
			else {
				dialogText.text += "\n [Merchant] You don't have enough space to buy this. \n";
				dialogOn = true;
				dialogPanel.SetActive(true);
			}
		}
		else {
			dialogText.text += "\n [Merchant] You don't have enough gold to buy this. \n";
			dialogOn = true;
			dialogPanel.SetActive(true);
		}
	}

	//Sells items in inventory
	public void sellClick() {
		if(shopPanel.activeSelf) {
			for (int x = 0; x < slot.Length; x++) {
				String button = EventSystem.current.currentSelectedGameObject.name;
				if (button == slot[x].name && weapons[x] != null) {
					slotStatic[x].name = "";
					slotStatic[x].image = null;
					slot[index].gameObject.GetComponentInChildren<Text>().text = "";
					slot[x].gameObject.GetComponent<RawImage>().texture = null;

					switch(weapons[x].name) {
						case "Ration":
							money += 500;
							break;
						case "Poison":
							money += 250;
							break;
						case "Bow":
							money += 300;
							break;
						case "Money":
							money -= 500;
							break;
						case "Boxing Box":
							money += 450;
							break;
					}
					weapons[x] = null;
				}
			}
		}	
	}

	public void loadClick() {
		health = 100;
		diedPanel.SetActive(false);
		player.transform.position = new Vector3(0, 0, 0);
		dialogOn = true;
		dialogText.text = "[Mini Me] Welcome back! You died";
		dialogPanel.SetActive(true);
	}
}
