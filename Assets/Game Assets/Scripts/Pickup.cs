using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pickup : MonoBehaviour {
	public Texture image;
	public string type;
	public int quantity;
	public int money;
	public int health;

	private Weapon item;

	// Use this for initialization
	void Start () {
		item = new Weapon(this.gameObject, image, type, quantity, money, health);
	}

	//Adds the item to the inventory
	public void addItem() {
		if (!(HUDController.count == 10) && type != "Money") {
			HUDController.weapons[HUDController.count] = item;
			HUDController.count++;
		}
		else {
			HUDController.money += item.money;
			if (HUDController.money > 9999) {
				HUDController.money = 9999;
			}
		}

		HUDController.health += item.health;
		if (HUDController.health > 100) {
			HUDController.health = 100;
		}
	}

	void OnTriggerEnter(Collider other) {
			if (other.gameObject.tag == ("Player")) {
				addItem();
				Destroy(gameObject);
				WorldProperties.counter--;
				Debug.Log("Collided with: " + this.name + ":" + WorldProperties.counter);
			}
	}
	
}
