using System;
using System.Collections;
using UnityEngine;

/**
 * Manages all the seperate classes to build a level for players.
 */
public class Generation {
	//public static System.Random random = new System.Random();
	public Transform[] halls;
	public Transform[] rooms;
	public Material[] materials;
	public Transform[] water;
	public Transform[] walls;
	public Transform[] barriers;
	public Transform[] decorations;

	public int centerSectorX; 
	public int centerSectorY;
	public int centerSectorZ;

	// Use this for initialization
	public Generation(Transform[] halls, Transform[] rooms, Material[] materials, 
		Transform[] water, Transform[] walls, Transform[] barriers, 
		Transform[] decorations, int sectorX, int sectorY) {

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
		//Copy terrain over
		this.materials = new Material[materials.Length];
		for (int x = 0; x < materials.Length; x++) {
			this.materials[x] = materials[x];
		}
		//Copy water over
		this.water = new Transform[water.Length];
		for (int x = 0; x < water.Length; x++) {
			this.water[x] = water[x];
		}
		/*//Copy walls over
		this.walls = new Transform[walls.Length];
		for (int x = 0; x < walls.Length; x++) {
			this.walls[x] = walls[x];
		}
		//Copy barriers over
		this.barriers = new Transform[barriers.Length];
		for (int x = 0; x < barriers.Length; x++) {
			this.barriers[x] = barriers[x];
		}
		//Copy decorations over
		this.decorations = new Transform[decorations.Length];
		for (int x = 0; x < decorations.Length; x++) {
			this.decorations[x] = decorations[x];
		}*/

		//Initialize map creation settings
		GenHeader.Decisions decisions = new GenHeader.Decisions {
			renderStructure = RandomBool(),
			renderWater = RandomBool(),
			renderWall = RandomBool()
		};

		//Terrain can have the chance of gen if rooms and halls are made
		if (decisions.renderStructure) {
			decisions.renderTerrain = RandomBool();
		}
		//When no halls or roomms are being made, terrainn is needed.
		else {
			decisions.renderTerrain = true;
		}		
		
		//Generate level
		GenerateLevel(decisions);
	}

	private bool RandomBool() {
		System.Random random = new System.Random();
		int randBool = random.Next(0, 99);
		if (randBool < 50) {
			return true;
		}
		else {
			return false;
		}
	}

	//TODO: Add item spawn generation, add player, and saving the map for later sharing. 
	//Generates the sectors based off of the generation requirements and instantiates them
	/**
	* Picks the land type based off random value in the terrain seed.
	*/
	private void GenerateLevel(GenHeader.Decisions decisions) {
		

		//Set spawn position
		//TODO: Build sectors at terrain location
		centerSectorX = 0;
		centerSectorY = 20;
		centerSectorZ = 0;

		if(decisions.renderStructure) {
			ChooseStructure();
		}
		

		if(decisions.renderTerrain) {
			int land = ChooseTerrain();
			Debug.Log(land);
		}
	}

	private int ChooseStructure() {
		StructureGen structGen = new StructureGen(halls, rooms);
		return 0;
	}

	private int ChooseTerrain() {
		System.Random random = new System.Random();

		//TODO: Add game mode decisions for terrain
		TerrainGen terrainGen = new TerrainGen(materials, water, false, false);
		int randNum = 24; //random.Next(0, 99);

		//Flat - (VR)
		if (randNum <= 6) {
			terrainGen.landType = GenHeader.FLAT;
			terrainGen.MakeFlatTerrain();
		}
		//Plains - (Grasslands)
		else if (randNum > 6 && randNum <= 12) {
			terrainGen.landType = GenHeader.PLAINS;
			terrainGen.MakePlainsTerrain();
		}
		//Tundra - (Arctic)
		else if (randNum > 12 && randNum <= 18) {
			terrainGen.landType = GenHeader.TUNDRA;
			terrainGen.MakeTundraTerrain();
		}
		//Oasis - (Desert)
		else if (randNum > 18 && randNum <= 24) {
			terrainGen.landType = GenHeader.OASIS;
			terrainGen.MakeOasisTerrain();
		}
		//Steppe - (Forest/Desert)
		else if (randNum > 24 && randNum <= 30) {
			terrainGen.landType = GenHeader.STEPPE;
			terrainGen.MakeSteppeTerrain();
		}
		//Hill - (Countryside)
		else if (randNum > 30 && randNum <= 36) {
			terrainGen.landType = GenHeader.HILL;
			terrainGen.MakeHillTerrain();
		}
		//Desert - (Dune)
		else if (randNum > 36 && randNum <= 42) {
			terrainGen.landType = GenHeader.DESERT;
			terrainGen.MakeDesertTerrain();
		}
		//Mountain
		else if (randNum > 42 && randNum <= 48) {
			terrainGen.landType = GenHeader.MOUNTAIN;
			terrainGen.MakeMountainTerrain();
		}
		//Forest
		else if (randNum > 48 && randNum <= 54) {
			terrainGen.landType = GenHeader.FOREST;
			terrainGen.MakeForestTerrain();
		}
		//Marsh - (Bog)
		else if (randNum > 54 && randNum <= 60) {
			terrainGen.landType = GenHeader.MARSH;
			terrainGen.MakeMarshTerrain();
		}
		//Swamp
		else if (randNum > 60 && randNum <= 66) {
			terrainGen.landType = GenHeader.SWAMP;
			terrainGen.MakeSwampTerrain();
		}
		//City
		else if (randNum > 66 && randNum <= 72) {
			terrainGen.landType = GenHeader.CITY;
			terrainGen.MakeCityTerrain();
		}
		//Island
		else if (randNum > 72 && randNum <= 78) {
			terrainGen.landType = GenHeader.ISLAND;
			terrainGen.MakeIslandTerrain();
		}
		//Beach
		else if (randNum > 78 && randNum <= 84) {
			terrainGen.landType = GenHeader.BEACH;
			terrainGen.MakeBeachTerrain();
		}
		//Cave
		else if (randNum > 84 && randNum <= 90) {
			terrainGen.landType = GenHeader.CAVE;
			terrainGen.MakeCaveTerrain();
		}
		//Ocean
		else if (randNum > 90 && randNum <= 96) {
			terrainGen.landType = GenHeader.OCEAN;
			terrainGen.MakeOceanTerrain();
		}
		//Spire
		else {
			terrainGen.landType = GenHeader.SPIRE;
			terrainGen.MakeSpireTerrain();
		}

		terrainGen.seed.SetSeedLock(true);

		return terrainGen.landType;
	}
}

