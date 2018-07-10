using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldProperties : MonoBehaviour{
	public static Vector3 playerPos;
	public static float gamma = MenuControl.gammaValue;
	public static int counter = 0;
	private int length;
	public Light[] lights;

	public Transform ration;
	public Transform poison;
	public Transform money;
	public Transform arrows;
	public Transform bow;

	public GameObject player;
	public GameObject[] enemyWeapon;
	public GameObject[] enemy;
	public GameObject[] enemySpawn;
	private bool[] enemiesRespawing;
	private UnityStandardAssets.Characters.ThirdPerson.AICharacterControl[] control;

	public GameObject[] spawner;
	public static GameObject[] spawners;
	public static bool[] avail;
	private int max, rand;
	private float timeChange, time;
	private float enemyRespawn, enemyLimit;
	private System.Random random;

	void Start() {
		if(gamma == 0f) {
			gamma = 0.1f;
		}
		player.transform.position = playerPos;
		random = new System.Random();
		max = 10;
		length = spawner.Length;
		timeChange = 0;
		rand = random.Next();
		time = rand % 10;
		enemyLimit = 5;
		enemyRespawn = 0;

		control = new UnityStandardAssets.Characters.ThirdPerson.AICharacterControl[enemy.Length];
		for (int x = 0; x < enemy.Length; x++) {
			control[x] = gameObject.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl>();
		}
		

		//Set gamma for this scene
		if (MenuControl.gammaValue == 0) {
			gamma = 0;
		}

		//Apply gamma to lights
		for (int x = 0; x < lights.Length; x++) {
			lights[x].intensity = gamma;
		}

		//Set available slots to true
		//TODO: Make static struct of avail and spawners
		avail = new bool[length];
		for (int x = 0; x < length; x++) {
			avail[x] = true;
		}

		enemiesRespawing = new bool[enemyWeapon.Length];
		for (int x = 0; x < enemyWeapon.Length; x++) {
			enemiesRespawing[x] = false;
		}

		//Copy spawners to a static array
		spawners = new GameObject[length];
		for (int x = 0; x < length; x++) {
			spawners[x] = spawner[x];
		}
	}

	void Update() {
		if (counter < max) {
			timeChange += Time.deltaTime;
			int index = rand % (length-1);
			if (avail[index] && time < timeChange) {
				rand = random.Next();
				time = rand % 10;
				int percent = rand % 100;
				//int item = rand % 5;
				timeChange = 0;
				if (percent < 30) {
					Instantiate(ration, spawners[index].transform.position, Quaternion.identity);
					counter++;
					avail[index] = false;
				}
				else if (percent >= 30 && percent < 50) {
					Instantiate(poison, spawners[index].transform.position, Quaternion.identity);
					counter++;
					avail[index] = false;
				}
				else if (percent >= 50 && percent < 70) {
					Instantiate(money, spawners[index].transform.position, Quaternion.identity);
					counter++;
					avail[index] = false;
				}
				else if (percent >= 70 && percent < 90) {
					Instantiate(arrows, spawners[index].transform.position, Quaternion.identity);
					counter++;
					avail[index] = false;
				}
				else if (percent >= 90 && percent < 100 ) {
					Instantiate(bow, spawners[index].transform.position, Quaternion.identity);
					counter++;
					avail[index] = false;
				}
			}
		}

		for (int x = 0; x < enemyWeapon.Length; x++) {
			enemiesRespawing[x] = enemyWeapon[x].GetComponent<MeleeCombat>().IsRespawning();
			if(enemiesRespawing[x]) {
				enemyRespawn += Time.deltaTime;
				if (enemyRespawn >= enemyLimit) {
					enemyRespawn = 0;
					enemy[x].SetActive(true);
					enemy[x].transform.position = enemySpawn[x].transform.position;
					control[x].target = enemySpawn[x].transform;
				}
			}
		}

		
		
		//TODO: add for when you reopen game or close scene
	}
}
