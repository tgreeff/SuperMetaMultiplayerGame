using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Weapon{
	public GameObject item { get; private set; }
	public Texture image { get; private set; }
	public string name {get; private set;}
	public int quantity {get; set;}
	public int money { get; set; }
	public int health { get; set; }

	public Weapon(GameObject it, Texture i, string n, int quant, int m, int h) {
		item = it;
		image = i;
		name = n;
		quantity = quant;
		money = m;
		health = h;
	}

	public string toFileString() {
		return "item=" + item.name + "\n" +
			"name=" + name + "\n" +
			"quantity=" + quantity + "\n" +
			"money=" + money + "\n" +
			"health=" + health;
	}
}
