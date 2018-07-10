using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sector {
	private Transform[,,] mapTransform;
	private int[,,] mapType;
	private int[,,] mapRotation;

	public bool generated;  //Allows for quicker checking of generation
	public bool instanciated; //Allows for quick checking whether it is in the game

	//-------CONSTANTS/-------
	private int sectorX;
	public int  x {
		get { return sectorX; }
	}
	private int sectorY;
	public int y {
		get { return sectorY; }
	}

	private int sectorHeight;
	public int SectorHeight {
		get { return sectorHeight; }
	}

	private float tileLength;
	public float TileLength {
		get { return tileLength; }
	}

	private float tileHeight;
	public float TileHeight {
		get { return sectorHeight; }
	}

	//Constructor for sector struct
	public Sector(int x, int y, int height, float tileLength, float tileHeight) {
		sectorX = x;
		sectorY = y;
		sectorHeight = height;
		this.tileLength = tileLength;
		this.tileHeight = tileHeight;
		mapTransform = new Transform[sectorX, sectorHeight, sectorY];
		mapType= new int[sectorX, sectorHeight, sectorY];
		mapRotation = new int[sectorX, sectorHeight, sectorY];
		generated = false;
		instanciated = false;
	}

	//Set the transform to the block transform
	public void SetMapTransform(int x, int y, int z, Transform instance) {
		if (x > sectorX - 1 || x < 0) {
			return;
		}
		if (y > sectorHeight - 1 || y < 0) {
			return;
		}
		if (z > sectorY - 1 || z < 0) {
			return;
		}
		mapTransform[x, y, z] = instance;
	}

	//Get the transform located in the map array
	public Transform GetMapTransform(int x, int y, int z) {
		if (x > sectorX - 1 || x < 0) {
			return null;
		}
		else if (y > sectorHeight - 1 || y < 0) {
			return null;
		}
		else if (z > sectorY - 1 || z < 0) {
			return null;
		}
		return mapTransform[x, y, z];
	}

	//Set the transform to the block transform
	public void SetMapType(int x, int y, int z, int type) {
		if (x > sectorX - 1 || x < 0) {
			return;
		}
		if (y > sectorHeight - 1 || y < 0) {
			return;
		}
		if (z > sectorY - 1 || z < 0) {
			return;
		}
		mapType[x, y, z] = type;
	}

	//Get the transform located in the map array
	public int GetMapType(int x, int y, int z) {
		if (x > sectorX - 1 || x < 0) {
			return 0;
		}
		if (y > sectorHeight - 1 || y < 0) {
			return 0;
		}
		if (z > sectorY - 1 || z < 0) {
			return 0;
		}
		return mapType[x, y, z];
	}

	//Set the transform to the block transform
	public void SetMapRotation(int x, int y, int z, int type) {
		if (x > sectorX - 1 || x < 0) {
			return;
		}
		if (y > sectorHeight - 1 || y < 0) {
			return;
		}
		if (z > sectorY - 1 || z < 0) {
			return;
		}
		mapRotation[x, y, z] = type;
	}

	//Get the transform located in the map array
	public int GetMapRotation(int x, int y, int z) {
		if (x > sectorX - 1 || x < 0) {
			return 0;
		}
		if (y > sectorY - 1 || y < 0) {
			return 0;
		}
		if (z > sectorHeight - 1 || z < 0) {
			return 0;
		}
		return mapRotation[x, y, z];
	}

	public Vector3 FindLocationFirstInstance(int transform) {
		for (int x = 0; x < sectorX; x++) {
			for(int y= 0; y < sectorHeight; y++) {
				for(int z = 0; z < sectorY; z++) {
					if(mapType[x,y,z] == transform) {
						return new Vector3(x, y, z);
					}
				}
			}
		}
		return new Vector3(0,0,0);
	}

	public void SetSector(int type) {
		for (int x = 0; x < sectorX; x++) {
			for (int y = 0; y < sectorHeight; y++) {
				for (int z = 0; z < sectorY; z++) {
					SetMapType(x, y, z, type);
				}
			}
		}
	}

	public void SetSector(Transform type) {
		for (int x = 0; x < sectorX; x++) {
			for (int y = 0; y < sectorHeight; y++) {
				for (int z = 0; z < sectorY; z++) {
					SetMapTransform(x, y, z, type);
				}
			}
		}
	}

	//Instantiates the transform of the tile 
	//TODO: Add coroutines
	public void InstanciateSector() {
		for (int x = 0; x < sectorX; x++) {
			for (int y = 0; y < sectorHeight; y++) {
				for (int z = 0; z < sectorY; z++) {
					int transInt = GetMapType(x, y, z);
					int transBelow = GetMapType(x, y - 1, z);
					int transAbove = GetMapType(x, y + 1, z);
					if (transInt != GenHeader.EMPTY &&
						transAbove != GenHeader.HALL_HATCH_DOWN &&
						transBelow != GenHeader.HALL_LADDER_UP) {

						Vector3 position = new Vector3(
							tileLength * x,
							tileHeight * y,
							tileLength * z);
						int transRot = GetMapRotation(x, y, z);
						Transform transform = GetMapTransform(x, y, z);
						Quaternion rotation = Quaternion.identity;
						rotation.eulerAngles = new Vector3(0, 90 * transRot, 0);
						SetMapTransform(x, y, z,  GameObject.Instantiate(transform, position, rotation));
					}
					else {
						SetMapType(x, y, z, GenHeader.EMPTY);								
					}
				}
			}
		}
		instanciated = true;
	}

	//Instantiates the transform of the tile
	public void DeinstanciateSector() {
		for (int x = 0; x < sectorX; x++) {
			for (int y = 0; y < sectorHeight; y++) {
				for (int z = 0; z < sectorY; z++) {
					GameObject.Destroy(GetMapTransform(x, y, z));
				}
			}
		instanciated = false;
		}
	}

	//Cleans up the memory of the sectors 
	public void DestroySector() {
		DeinstanciateSector();
	}
}
