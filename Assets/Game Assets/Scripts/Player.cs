using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	private Transform[] armor;
	//TODO: add armor, customiztions, and weapons
	
	//Constants for enemies
	public const int MAX_ENEMY_COUNT = 15;
	public const int ENEMY_PREFAB_COUNT = 2;

	public void Start() {
		
	}

	public void Update() {

	}

	public Player() {
		//TODO: add initial spawning to world
		armor = new Transform[5];
		for (int x = 0; x < 5; x++) {
			
		}
		
	}

	public void SpawnPlayers(int playerX, int playerY, int playerZ, int count) {	
		//TODO: ADD for when player dies	
		System.Random rand = new System.Random();
		
		Vector3Int[] tileLocations = new Vector3Int[100];
		int tileIndex = 0;
		for(int x = -5; x < 4; x++){
			for (int z = -5; z < 4; z++) {
				try {			
					tileLocations[tileIndex] = new Vector3Int(playerX + x , playerY, playerZ + z);
					tileIndex++;				
				} catch (Exception e) {
					Debug.Log(e.ToString());
				}
			}			
		}
		try {	//TODO: Add optimization or while respawning	
			int i = UnityEngine.Random.Range(0, tileIndex);
			int x = tileLocations[i].x;
			int y = tileLocations[i].y;
			int z = tileLocations[i].z;
			
		} finally {

		}

	}

	//Instantiates the transform of the tile 
	public void Instantiate(int type, int x, int y, int z) {
		Transform transform = armor[type];
		Vector3 position = new Vector3(
			(GenHeader.TILE_SIZE * x) + transform.position.y,
			GenHeader.TILE_HEIGHT * y + transform.position.y,
			(GenHeader.TILE_SIZE * z) + transform.position.z);
		int transRot = 1;

		Quaternion rotation = Quaternion.identity;
		rotation.eulerAngles = new Vector3(0, 90 * transRot, 0);
		GameObject.Instantiate(transform, position, rotation);
	}
}
