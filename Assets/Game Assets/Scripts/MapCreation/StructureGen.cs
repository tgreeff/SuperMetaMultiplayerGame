using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureGen {

	public Sector sector;
	private int structSeed;
	public int seed {
		get { return structSeed; }
	}

	public Transform[] halls;
	public Transform[] rooms;

	//Structure Types values
	private bool symmetrical;
	private bool loop;
	private bool xAxisSym;
	private bool yAxisSym;

	public StructureGen( Transform[] halls, Transform[] rooms) {
		sector = new Sector(16, 16, 3, 10.2f, 4.2f);
		sector.SetSector(GenHeader.EMPTY);

		//TODO - set up seed

		//Copy halls over
		this.halls = new Transform[halls.Length];
		for (int x = 0; x < halls.Length; x++) {
			this.halls[x] = halls[x];
		}

		//Copy rooms over
		this.rooms = new Transform[rooms.Length];
		for (int x = 0; x < rooms.Length; x++) {
			this.rooms[x] = rooms[x];
		}

	}


	//Add special cases as the tiles are added
	//TODO: Add ladders and other rooms with connections
	//TODO: Add vertical connections
	public void AddHalls(int xPos, int yPos, int zPos) {
		Vector4[] last = new Vector4[120];
		int i = 0;
		int rotation = 0;

		for (int x = -1; x < 2; x++) { //start from spawn
			for (int z = -1; z < 2; z++) {
				if (x == 0 ^ z == 0) {
					Vector2 from = new Vector2(xPos + x, zPos + z);
					sector.SetMapType((int)from.x, yPos, (int)from.y, GenHeader.HALL);
					Vector4[] continueLast = GeneratePath(sector, from, yPos, rotation);
					rotation -= 2;
					if (rotation < 0) {
						rotation = 4 - rotation;
					}

					for (int t = 0; t < continueLast.Length; t++) {
						if (!continueLast[t].Equals(null)) {
							last[i] = continueLast[t];
							i++;
						}
					}
				}
			}
		}
		sector.SetMapRotation(xPos + 1, yPos, zPos, 0);
		sector.SetMapRotation(xPos - 1, yPos, zPos, 2);
		sector.SetMapRotation(xPos, yPos, zPos + 1, 3);
		sector.SetMapRotation(xPos, yPos, zPos - 1, 1);
		
		//Continue with last parts
		for (int x = 0; x < last.Length; x++) {
			if (!last[x].Equals(null)) {
				Vector2 from = new Vector2(last[x].x, last[x].z);
				//GeneratePath(s, from, (int) last[x].y, (int) last[x].w);
			}
		}
		//check edge to connect other sectors
	}

	//TODO: Adds rooms connected to hallway
	public void AddRooms() {

	}

	//Creates a hallway path between 2 points in a sector
	private Vector4[] GeneratePath(Sector s, Vector2 from, int y, int dir) {
		int catchLoop = 0;
		int numberHalls = 30;

		Vector4[] continueLater = new Vector4[30];
		int w = 0;

		int x = (int)from.x;
		int z = (int)from.y;
		int lastX = (int)from.x;
		int lastZ = (int)from.y;
		int prevTransform = s.GetMapType((int)from.x, y, (int)from.y);
		int direction = dir; // 0 = East, 1 = South, 2 = West, 3 = North

		for (int n = 0; n < numberHalls && catchLoop < 1000; n++) {
			System.Random rand = new System.Random();
			bool choosePath = true;
			while (choosePath) {
				if (prevTransform == GenHeader.HALL) {
					if (direction == 0) {
						x++;
					}
					else if (direction == 1) {
						z--;
					}
					else if (direction == 2) {
						x--;
					}
					else {
						z++;
					}
					choosePath = false;
				}
				else if (prevTransform == GenHeader.HALL_CORNER) {
					direction = (direction + 1) % 4;
					if (direction == 0) {
						x++;
					}
					else if (direction == 1) {
						z--;
					}
					else if (direction == 2) {
						x--;
					}
					else {
						z++;
					}
					choosePath = false;
				}
				else if (prevTransform == GenHeader.HALL_TRI) {
					int r = rand.Next(0, 1);
					if (r == 0) { // Right
						direction = (direction + 1) % 4;
					}
					else { //Left
						direction--;
						if (direction < 0) {
							direction = 3;
						}
					}
					if (direction == 0) {
						x++;
						continueLater[w] = new Vector4(x - 2, y, z, direction);
						w++;
						if (w >= continueLater.Length) w = continueLater.Length - 1;
					}
					else if (direction == 1) {
						z--;
						continueLater[w] = new Vector4(x, y, z + 2, direction);
						w++;
						if (w >= continueLater.Length) w = continueLater.Length - 1;
					}
					else if (direction == 2) {
						x--;
						continueLater[w] = new Vector4(x + 2, y, z, direction);
						w++;
						if (w >= continueLater.Length) w = continueLater.Length - 1;
					}
					else {
						z++;
						continueLater[w] = new Vector4(x, y, z - 2, direction);
						w++;
						if (w >= continueLater.Length) w = continueLater.Length - 1;
					}
					choosePath = false;
				}
				else if (prevTransform == GenHeader.HALL_QUAD) {
					int r = rand.Next(0, 2);
					if (r == 0) { //Right
						direction = (direction + 1) % 4;
					}
					else if (r == 1) { //Straight

					}
					else { //Left
						direction--;
						if (direction < 0) {
							direction = 3;
						}
					}
					if (direction == 0) {
						x++;
						continueLater[w] = new Vector4(x - 2, y, z, direction);
						w++;
						if (w >= continueLater.Length) w = continueLater.Length - 1;
					}
					else if (direction == 1) {
						z--;
						continueLater[w] = new Vector4(x, y, z + 2, direction);
						w++;
						if (w >= continueLater.Length) w = continueLater.Length - 1;
					}
					else if (direction == 2) {
						x--;
						continueLater[w] = new Vector4(x + 2, y, z, direction);
						w++;
						if (w >= continueLater.Length) w = continueLater.Length - 1;
					}
					else {
						z++;
						continueLater[w] = new Vector4(x, y, z - 2, direction);
						w++;
						if (w >= continueLater.Length) w = continueLater.Length - 1;
					}
					choosePath = false;
				}
				else if (prevTransform == GenHeader.HALL_HATCH_DOWN) {
					if (y > 0) {
						for (int i = 0; i < 2; i++) {
							direction = (direction + 1) % 4;
						}
						//y--;
						if (direction == 0) {
							//x++;
							continueLater[w] = new Vector4(x - 1, y - 1, z, direction);
							w++;
							if (w >= continueLater.Length) w = continueLater.Length - 1;
						}
						else if (direction == 1) {
							//z--;
							continueLater[w] = new Vector4(x, y - 1, z + 1, direction);
							w++;
							if (w >= continueLater.Length) w = continueLater.Length - 1;
						}
						else if (direction == 2) {
							//x--;
							continueLater[w] = new Vector4(x + 1, y - 1, z, direction);
							w++;
							if (w >= continueLater.Length) w = continueLater.Length - 1;
						}
						else {
							//z++;
							continueLater[w] = new Vector4(x, y - 1, z - 1, direction);
							w++;
							if (w >= continueLater.Length) w = continueLater.Length - 1;
						}
					}
					else {
						prevTransform = GenHeader.HALL;
					}
					choosePath = false;
				}
				else if (prevTransform == GenHeader.HALL_LADDER_UP) {
					if (y < sector.x - 1) {
						for (int i = 0; i < 2; i++) {
							direction = (direction + 1) % 4;
						}
						//y++;
						if (direction == 0) {
							x++;
							continueLater[w] = new Vector4(x - 1, y + 1, z, direction);
							w++;
							if (w >= continueLater.Length) w = continueLater.Length - 1;
						}
						else if (direction == 1) {
							z--;
							continueLater[w] = new Vector4(x, y + 1, z + 1, direction);
							w++;
							if (w >= continueLater.Length) w = continueLater.Length - 1;
						}
						else if (direction == 2) {
							x--;
							continueLater[w] = new Vector4(x + 1, y + 1, z, direction);
							w++;
							if (w >= continueLater.Length) w = continueLater.Length - 1;
						}
						else {
							z++;
							continueLater[w] = new Vector4(x, y + 1, z - 1, direction);
							w++;
							if (w >= continueLater.Length) w = continueLater.Length - 1;
						}
					}
					else {
						prevTransform = GenHeader.HALL;
					}
					choosePath = false;
				}
			}

			int currentSpot = s.GetMapType(x, y, z);
			if (currentSpot == GenHeader.EMPTY || currentSpot == GenHeader.HALL_DEAD_END) {  //transform not taken up
				bool chooseTile = true;
				while (chooseTile) {

					int type = rand.Next(0, 99);
					if (type < 20) {
						s.SetMapType(x, y, z, GenHeader.HALL);
						prevTransform = GenHeader.HALL;
						chooseTile = false;
					}
					else if (type >= 20 && type < 50) {
						s.SetMapType(x, y, z, GenHeader.HALL_CORNER);
						prevTransform = GenHeader.HALL_CORNER;
						chooseTile = false;
					}
					else if (type >= 50 && type < 75) {
						s.SetMapType(x, y, z, GenHeader.HALL_TRI);
						prevTransform = GenHeader.HALL_TRI;
						chooseTile = false;
					}
					else if (type >= 85 && type < 90) {
						s.SetMapType(x, y, z, GenHeader.HALL_QUAD);
						prevTransform = GenHeader.HALL_QUAD;
						chooseTile = false;
					}
					else if (type >= 85 && type < 95 && prevTransform != GenHeader.HALL_HATCH_DOWN) {
						try {
							if (s.GetMapType(x, y - 1, z) == 0) {
								s.SetMapType(x, y, z, GenHeader.HALL_HATCH_DOWN);
								prevTransform = GenHeader.HALL_HATCH_DOWN;
								chooseTile = false;
							}
						}
						catch (Exception e) {
							Debug.Log(e.ToString());
						}
					}
					else if (type >= 95 && type < 100 && prevTransform != GenHeader.HALL_LADDER_UP) {
						try {
							if (s.GetMapType(x, y + 1, z) == 0) {
								s.SetMapType(x, y, z, GenHeader.HALL_LADDER_UP);
								prevTransform = GenHeader.HALL_LADDER_UP;
								chooseTile = false;
							}
						}
						catch (Exception e) {
							Debug.Log(e.ToString());
						}
					}
				}
				s.SetMapRotation(x, y, z, direction);
			}
			else { //When spot is already taken up control when back tracking				
				numberHalls++;
				lastX = x;
				lastZ = z;
				catchLoop++;
				prevTransform = GenHeader.EMPTY;
			}
		}
		return continueLater;
	}

	//Check if next to spawn
	private bool CheckForSpawn(Sector s, int x, int y, int z) {
		bool xP = false;
		bool xN = false;
		bool zP = false;
		bool zN = false;

		if (x < sector.x - 1) {
			xP = s.GetMapType(x + 1, y, z) == GenHeader.START;
		}
		if (x > 0) {
			xN = s.GetMapType(x - 1, y, z) == GenHeader.START;
		}
		if (z < sector.x - 1) {
			zP = s.GetMapType(x, y, z + 1) == GenHeader.START;
		}
		if (z > 0) {
			zN = s.GetMapType(x, y, z - 1) == GenHeader.START;
		}

		return xP || xN || zP || zN;
	}

	//Returns the Vec2 of location coordinates
	public Vector2 GetSectorFromPosition(float x, float y, float z) {
		int sectorSize = (int) GenHeader.TILE_SIZE * sector.x;
		x = x - (x % sectorSize);
		y = z - (z % sectorSize);
		return new Vector2(x, y);
	}
}
