using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour {
	public GameObject player;
	public GameObject[] otherPlayers;

	public Transform[] halls;
	public Transform[] rooms;
	public Material[] materials;
	public Transform[] water;
	public Transform[] walls;
	public Transform[] barriers;
	public Transform[] decorations;
	public Transform[] skyboxs; //TODO

	private int currentBlockX;
	private int currentBlockY;
	private int currentBlockZ;

	public Generation generation;
	public Player enemySpawner;
	private float timer = 0;
	private float timeLimit = 30;
	private bool changeY;

	void Start () {
		timer = 30;
		changeY = false;
		generation = new Generation(halls, rooms, materials, water, walls, barriers, decorations, 0, 0);

		float tSize = GenHeader.TILE_SIZE;
		float xPos = (tSize * generation.centerSectorX);
		float yPos = (GenHeader.TILE_HEIGHT * generation.centerSectorY + 2.5f);
		float zPos = (tSize * generation.centerSectorZ);
		player.transform.position = new Vector3( xPos, yPos+1, zPos);

	}
	
	// Update is called once per frame
	void Update () {
		if (timer % 5 == 0 && timer != timeLimit) {
			UpdatePlayerPosition();
		}
		else if (timer >= timeLimit) {
			timer = 0;
		}			
		timer += Time.deltaTime;		
	}

	private void UpdatePlayerPosition() {
		float x = player.transform.position.x;
		float y = player.transform.position.y;
		float z = player.transform.position.z;
		int lastY = currentBlockY;

		//Set current location
		currentBlockX = (int) Mathf.Floor(x / GenHeader.TILE_SIZE);
		currentBlockY = (int) Mathf.Floor(y / GenHeader.TILE_HEIGHT);
		currentBlockZ = (int) Mathf.Floor(z / GenHeader.TILE_SIZE);

		if(lastY != currentBlockY) {
			changeY = true;
		}
	}
}
