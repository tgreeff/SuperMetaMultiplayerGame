using System;
using UnityEngine;

/**
 * Generates the terrain that will be used in the level. This decides the general structure of the map that will be made.
 * 
 * TODO: add structure to make symmetrical maps when needed.
 */
public class TerrainGen {	
	public TerrainMeshMaker terrain;
	public SeedGenerator seedGen;
	public Seed seed;
	public int landType; //Set to make sure it can't be changed but seen from outside
	private float tileSize;
	
	private Transform[] water;
	private Material[] materials;
	private Seed waterSeed;
	private float waterHeight;

	//Land formations
	private bool useWater;
	private bool river;
	private bool mountain;
	private bool lake;
	private bool mesa;
	private bool pools;
	private bool beach;
	private bool stalagmite;

	public TerrainGen(Material[] material, Transform[] water, bool xSymmetry, bool ySymmetry) {
		// Copy materials
		this.materials = new Material[material.Length];
		for(int x = 0; x < materials.Length; x++) {
			materials[x] = material[x];
		}

		// Copy water
		this.water = new Transform[water.Length];
		for (int x = 0; x < water.Length; x++) {
			this.water[x] = water[x];
		}

		mesa = RandomBool();
		mountain = RandomBool();
		stalagmite = RandomBool();

		useWater = RandomBool();
		AddWater(useWater);

		if(useWater) {
			System.Random random = new System.Random();
			float height = (float)(random.Next(0, 2) + random.NextDouble());
			SetWaterHeight(height);
		}
	}

	public void AddWater(bool setActive) {
		water[0].gameObject.SetActive(setActive);	
	}

	public void SetWaterHeight(float height) {
		Vector3 heightAdjust = new Vector3(
			water[0].gameObject.transform.position.x,
			water[0].gameObject.transform.position.y - height,
			water[0].gameObject.transform.position.z);
		water[0].gameObject.transform.position =  heightAdjust;
	}

	public void AddDunes() {

	}

	public void AddPath(int length, Vector2Int direction, Vector2Int location) {

	}

	//Flat Terrain - 0
	public void MakeFlatTerrain() {
		tileSize = 5;
			
		seed = new Seed(100, 100);
		seedGen = new SeedGenerator(seed);
		seedGen.SetMapDouble();

		seedGen.BlurMap();
		
		RunTerrainMaker();
	}

	//Plains Terrain - 1
	public void MakePlainsTerrain() {
		System.Random random = new System.Random();
		tileSize = 5;

		seed = new Seed(100, 100);
		seedGen = new SeedGenerator(seed);
		seedGen.SetMapDouble();

		//Set Map blur random values
		if (RandomBool()) {
			seedGen.SeedProperties(RandomBool(), false, false, false, false);
			seedGen.BlurMap();
			seedGen.SeedProperties(false, false, false, false, false);
		}

		//seedGen.SeedProperties(false, false, false, false);
		int numCircles = random.Next(1, 5);
		for (int x = 0; x < numCircles; x++) {
			Vector2 radius = new Vector2(random.Next(3, seed.X), random.Next(3, seed.Y));
			Vector2 center = new Vector2(random.Next(0, seed.X), random.Next(0, seed.Y));
			seedGen.CircleFill(radius, center, random.NextDouble() + random.Next(-1, 3));
			seedGen.BlurAreaCircle(radius, center);		
		}
		
		//Set Double						
		if (RandomBool()) {
			seedGen.SeedProperties(RandomBool(), false, false, false, false);
			seedGen.SetMapDouble();
			seedGen.SeedProperties(false, false, false, false, false);
		}

		//Set Map blur
		if (RandomBool()) {
			seedGen.SeedProperties(RandomBool(), false, false, false, false);
			seedGen.BlurMap();
			seedGen.SeedProperties(false, false, false, false, false);
		}

		//Add Water
		if (RandomBool()) {
			water[0].gameObject.SetActive(true);
		}

		RunTerrainMaker();
	}

	//Tundra Terrain - 2
	//TODO - add area for shore or add hills
	//TODO - improve and add yellow grass and rocks
	public void MakeTundraTerrain() {
		tileSize = 4f;
		seed = new Seed(125, 125);
		seedGen = new SeedGenerator(seed);

		seedGen.SetMapDouble();

		seedGen.SeedProperties(true, false, false, false, false);
		seedGen.SetMapRandom(2);

		RunTerrainMaker();
	}

	//Oasis Terrain - 3
	public void MakeOasisTerrain() {
		System.Random random = new System.Random();
		tileSize = 4f;
		seed = new Seed(125, 125);
		seedGen = new SeedGenerator(seed);

		seedGen.SeedProperties(false, false, false, false, false);

		//Lower the terrain
		Vector2 radius = new Vector2(seed.X / 2, seed.Y / 2);
		Vector2 center = new Vector2(seed.X / 2, seed.Y / 2);
		seedGen.SquareFill(radius, center, random.NextDouble() - random.Next(1, 3));

		//Left side - mesa
		for(int y = 0; y < 4; y++) {
			for (int i = 0; i < 2; i++) {
				radius = new Vector2(random.Next(5, 20), random.Next(5, 20)); 
				center = new Vector2(0, random.Next((4 - (y + 1)) * seed.Y / 4, (4 - y) * seed.Y / 4));
				seedGen.CircleFill(radius, center, random.Next(15, 50));
			}
		}
		//Right side - mesa
		for (int y = 0; y < 4; y++) {
			for (int i = 0; i < 2; i++) {
				radius = new Vector2(random.Next(5, 20), random.Next(5, 20));
				center = new Vector2(seed.X - 1, random.Next((4 - (y + 1)) * seed.Y / 4, (4 - y) * seed.Y / 4));
				seedGen.CircleFill(radius, center, random.Next(15, 50));
			}
		}
		//Top side - mesa
		for (int x = 0; x < 4; x++) {
			for (int i = 0; i < 2; i++) {
				float randomX = random.Next((4 - (x + 1)) * seed.X / 4, (4 - x) * seed.X / 4);
				radius = new Vector2(random.Next(5, 20), random.Next(5, 20));
				center = new Vector2(randomX, seed.Y - 1);
				seedGen.CircleFill(radius, center, random.Next(15, 50));
			}
		}
		//Bottom side - mesa
		for (int x = 0; x < 4; x++) {
			for (int i = 0; i < 2; i++) {
				float randomX = random.Next((4 - (x + 1)) * seed.X / 4, (4 - x) * seed.X / 4);
				radius = new Vector2(random.Next(5, 20), random.Next(5, 20));
				center = new Vector2(randomX, 0);
				seedGen.CircleFill(radius, center, random.Next(15, 50));			
			}
		}

		//Blur 4 corners
		radius = new Vector2(seed.X/2, seed.Y/2);
		center = new Vector2(0, 0);
		seedGen.BlurAreaCircle(radius, center);
		center = new Vector2(seed.X-1, 0);
		seedGen.BlurAreaCircle(radius, center);
		center = new Vector2(0, seed.Y - 1);
		seedGen.BlurAreaCircle(radius, center);
		center = new Vector2(seed.X - 1, seed.Y - 1);
		seedGen.BlurAreaCircle(radius, center);

		seedGen.SeedProperties(true, false, false, false, false);
		//Bottom-Left - islands
		for (int i = 0; i < 4; i++) {	
			float randomX = random.Next(seed.X / 4, seed.X / 2);
			float randomY = random.Next(seed.Y / 4, seed.Y / 2);
			radius = new Vector2(random.Next(10, 15), random.Next(10, 15));
			center = new Vector2(randomX, randomY);
			seedGen.MountainGradual(radius, center, random.Next(10, 15), true);
		}
		//Bottom-Right - islands
		for (int i = 0; i < 4; i++) {
			float randomX = random.Next(seed.X / 2, 3 * seed.X / 4);
			float randomY = random.Next(seed.Y / 4, seed.Y / 2);
			radius = new Vector2(random.Next(10, 15), random.Next(10, 15));
			center = new Vector2(randomX, randomY);
			seedGen.MountainGradual(radius, center, random.Next(10, 15), true);
		}
		//Top-Left - islands
		for (int i = 0; i < 4; i++) {
			float randomX = random.Next(seed.X / 4, seed.X / 2);
			float randomY = random.Next(seed.X / 2, 3 * seed.X / 4);
			radius = new Vector2(random.Next(10, 15), random.Next(10, 15));
			center = new Vector2(randomX, randomY);
			seedGen.MountainGradual(radius, center, random.Next(10, 15), true);
		}
		//Top-Right - islands
		for (int i = 0; i < 4; i++) {
			float randomX = random.Next(seed.X / 2, 3 * seed.X / 4);
			float randomY = random.Next(seed.X / 2, 3 * seed.X / 4);
			radius = new Vector2(random.Next(10, 15), random.Next(10, 15));
			center = new Vector2(randomX, randomY);
			seedGen.MountainGradual(radius, center, random.Next(10, 15), true);
		}

		seedGen.BlurMap();
		AddWater(true);

		RunTerrainMaker();
	}

	//TODO: Will need grass and shrubs, no trees. This is flat
	//Steppe Terrain - 4
	public void MakeSteppeTerrain() {
		tileSize = 4f;
		seed = new Seed(125, 125);
		seedGen = new SeedGenerator(seed);

		seedGen.SetMapDouble();
		RunTerrainMaker();
	}

	//TODO: improve location of hills
	//Hill terrain - 5
	public void MakeHillTerrain() {
		tileSize = 3;
		seed = new Seed(100, 100);
		seedGen = new SeedGenerator(seed);
		System.Random random = new System.Random();
		seedGen.SeedProperties(false, false, false, false, false);
		//Lower the terrain
		Vector2 radius = new Vector2(seed.X / 2, seed.Y / 2);
		Vector2 center = new Vector2(seed.X / 2, seed.Y / 2);
		seedGen.SquareFill(radius, center, random.NextDouble() - random.Next(1, 3));

		//Small Hills
		seedGen.SetMapDouble();
		
		//Blur small hills
		if(RandomBool()) {
			seedGen.BlurMap();
		}

		//Large Hills - locked to keep shape
		seedGen.SeedProperties(true, false, false, false, false);
		int count = random.Next(0, 15);
		for(int i = 0; i < count; i++) {
			int height = random.Next(2, 15);
			radius = new Vector2(random.Next(5, 15), random.Next(5, 15));
			center = new Vector2(random.Next(0, seed.X - 1), random.Next(0, seed.Y - 1));
			seedGen.MountainGradual(radius, center, height, true);
			seedGen.BlurAreaCircle(radius, center);
		}

		seed.SetSeedLock(false);

		//Large Craters - locked to keep shape
		seedGen.SeedProperties(false, true, false, false, false);
		count = random.Next(0, 15);
		for (int i = 0; i < count; i++) {
			int height = random.Next(2, 15);
			radius = new Vector2(random.Next(5, 15), random.Next(5, 15));
			center = new Vector2(random.Next(0, seed.X - 1), random.Next(0, seed.Y - 1));
			seedGen.MountainGradual(radius, center, height, true);
			seedGen.BlurAreaCircle(radius, center);
		}

		seed.SetSeedLock(false);

		seedGen.BlurMap();

		RunTerrainMaker();
	}

	//TODO: Can be flat or mountainous
	//Desert terrain - 6
	public void MakeDesertTerrain() {
		tileSize = 4f;
		seed = new Seed(125, 125);
		seedGen = new SeedGenerator(seed);

		System.Random random = new System.Random();

		//Lower the terrain
		Vector2 radius = new Vector2(seed.X / 2, seed.Y / 2);
		Vector2 center = new Vector2(seed.X / 2, seed.Y / 2);
		seedGen.SquareFill(radius, center, random.NextDouble() - random.Next(1, 3));

		//Lock
		seedGen.SeedProperties(false, false, false, false, true);

		bool addMountain = RandomBool();
		if(addMountain) {

			//North
			double height = random.Next(15, 30) + random.NextDouble();
			int iterations = random.Next(5, 10);
			for (int i = 0; i < iterations; i++) {

				int passesAtHeight = random.Next(5, 10);
				for (int j = 0; j < iterations; j++) {

					float randomX = random.Next(0, seed.X - 1);
					radius = new Vector2(random.Next(5, 10), random.Next(5, 10));
					center = new Vector2(randomX, seed.Y - i - 1);
					seedGen.CircleFill(radius, center, height);
				}

				if (height > 0) {
					height -= random.NextDouble() + random.Next(0, 5);
				}
			}

			//East
			height = random.Next(15, 30) + random.NextDouble();
			iterations = random.Next(5, 10);
			for (int i = 0; i < iterations; i++) {

				int passesAtHeight = random.Next(5, 10);
				for (int j = 0; j < iterations; j++) {

					float randomY = random.Next(0, seed.Y - 1);
					radius = new Vector2(random.Next(5, 10), random.Next(5, 10));
					center = new Vector2(seed.X - i - 1, randomY);
					seedGen.CircleFill(radius, center, height);
				}

				if (height > 0) {
					height -= random.NextDouble() + random.Next(0, 5);
				}
			}

			//West
			height = random.Next(15, 30) + random.NextDouble();
			iterations = random.Next(5, 10);
			for (int i = 0; i < iterations; i++) {

				int passesAtHeight = random.Next(5, 10);
				for (int j = 0; j < iterations; j++) {

					float randomY = random.Next(0, seed.Y - 1);
					radius = new Vector2(random.Next(5, 10), random.Next(5, 10));
					center = new Vector2(i, randomY);
					seedGen.CircleFill(radius, center, height);
				}

				if (height > 0) {
					height -= random.NextDouble() + random.Next(0, 5);
				}
			}
		}

		seedGen.SeedProperties(false, false, false, false, false);
		AddWater(useWater);
		SetWaterHeight((float)0);

		RunTerrainMaker();
	}

	//Mountain Terrain - 7
	public void MakeMountainTerrain() {
		tileSize = 3;
		seed = new Seed(100, 100);
		seedGen = new SeedGenerator(seed);

		RunTerrainMaker();
	}

	//Forest terrain - 8
	public void MakeForestTerrain() {
		tileSize = 3;
		seed = new Seed(100, 100);
		seedGen = new SeedGenerator(seed);

		RunTerrainMaker();
	}

	//Marsh terrain - 9
	public void MakeMarshTerrain() {
		tileSize = 3;
		seed = new Seed(100, 100);
		seedGen = new SeedGenerator(seed);

		RunTerrainMaker();
	}

	//Swamp terrain - 10
	public void MakeSwampTerrain() {
		System.Random random = new System.Random();
		tileSize = 5;

		seed = new Seed(100, 100);
		seedGen = new SeedGenerator(seed);
		seedGen.SetMapDouble();

		//Set Map blur random values
		if (RandomBool()) {
			seedGen.SeedProperties(RandomBool(), false, false, false, false);
			seedGen.BlurMap();
			seedGen.SeedProperties(false, false, false, false, false);
		}

		//TODO: add for symmetry
		//seedGen.SeedProperties(false, false, false, false);
		int numCircles = random.Next(1, 5);
		for (int x = 0; x < numCircles; x++) {
			Vector2 radius = new Vector2(random.Next(3, seed.X), random.Next(3, seed.Y));
			Vector2 center = new Vector2(random.Next(0, seed.X), random.Next(0, seed.Y));
			seedGen.CircleFill(radius, center, random.NextDouble() + random.Next(-1, 3));
			seedGen.BlurAreaCircle(radius, center);
		}

		//Set Double						
		if (RandomBool()) {
			seedGen.SeedProperties(RandomBool(), false, false, false, false);
			seedGen.SetMapDouble();
			seedGen.SeedProperties(false, false, false, false, false);
		}

		//Set Map blur
		if (RandomBool()) {
			seedGen.SeedProperties(RandomBool(), false, false, false, false);
			seedGen.BlurMap();
			seedGen.SeedProperties(false, false, false, false, false);
		}

		//Add Water
		if (RandomBool()) {
			water[0].gameObject.SetActive(true);
		}

		RunTerrainMaker();
	}

	//City terrain - 11
	public void MakeCityTerrain() {
		tileSize = 3;
		seed = new Seed(125, 125);
		seedGen = new SeedGenerator(seed);

		RunTerrainMaker();
	}

	//Island terrain - 12
	public void MakeIslandTerrain() {
		tileSize = 3;

		seed = new Seed(125, 125);
		seedGen = new SeedGenerator(seed);

		RunTerrainMaker();
	}

	//Beach terrain - 13
	public void MakeBeachTerrain() {
		tileSize = 3;
		seed = new Seed(125, 125);
		seedGen = new SeedGenerator(seed);

		RunTerrainMaker();
	}

	//Cave terrain - 14
	public void MakeCaveTerrain() {
		tileSize = 3;
		seed = new Seed(125, 125);
		seedGen = new SeedGenerator(seed);

		RunTerrainMaker();
	}

	//Ocean terrain - 15
	public void MakeOceanTerrain() {
		tileSize = 3;
		seed = new Seed(125, 125);
		seedGen = new SeedGenerator(seed);
		RunTerrainMaker();
	}

	//Spire terrain - 16
	public void MakeSpireTerrain() {
		tileSize = 4f;
		seed = new Seed(125, 125);
		seedGen = new SeedGenerator(seed);
		seedGen.SetMapDouble();
		seedGen.SetMapRandom(2);

		seedGen.SeedProperties(true, false, false, false, false);
		Vector2Int direction = new Vector2Int(1, 0);
		Vector2Int location = new Vector2Int(10, 10);
		seedGen.Line(5, 10, direction, location);

		Vector2 radius = new Vector2(49, 49);
		Vector2 center = new Vector2(50, 50);
		seedGen.CircleFill(radius, center, 2.5f);

		seedGen.SeedProperties(false, false, false, false, false);
		radius = new Vector2(10, 10);
		center = new Vector2(30, 30);
		seedGen.CircleFill(radius, center, 5);

		radius = new Vector2(9, 9);
		center = new Vector2(30, 30);
		seedGen.CircleFill(radius, center, 6);

		radius = new Vector2(8, 8);
		center = new Vector2(30, 30);
		seedGen.CircleFill(radius, center, 7);

		radius = new Vector2(7, 7);
		center = new Vector2(30, 30);
		seedGen.CircleFill(radius, center, 8);

		radius = new Vector2(6, 6);
		center = new Vector2(30, 30);
		seedGen.CircleFill(radius, center, 9);

		center = new Vector2(90, 25);
		seedGen.SquareFill(radius, center, 15);
		RunTerrainMaker();
	}

	//Glacier terrain - 17
	public void MakeGlacierTerrain() {
		tileSize = 3;
		seed = new Seed(125, 125);
		seedGen = new SeedGenerator(seed);

		RunTerrainMaker();
	}

	//Pillars terrain - 18
	public void MakePillarsTerrain() {
		tileSize = 3;
		seed = new Seed(100, 100);
		seedGen = new SeedGenerator(seed);
		System.Random random = new System.Random();
		seedGen.SeedProperties(false, false, false, false, false);
		//Lower the terrain
		Vector2 radius = new Vector2(seed.X / 2, seed.Y / 2);
		Vector2 center = new Vector2(seed.X / 2, seed.Y / 2);
		seedGen.SquareFill(radius, center, random.NextDouble() - random.Next(1, 3));

		//Small Hills
		seedGen.SetMapDouble();

		//Blur small hills
		if (RandomBool()) {
			seedGen.BlurMap();
		}

		//Large Hills - locked to keep shape
		seedGen.SeedProperties(true, false, false, false, false);
		int count = random.Next(0, 15);
		for (int i = 0; i < count; i++) {
			int height = random.Next(2, 15);
			radius = new Vector2(random.Next(5, 15), random.Next(5, 15));
			center = new Vector2(random.Next(0, seed.X - 1), random.Next(0, seed.Y - 1));
			seedGen.MountainGradual(radius, center, height, true);
			seedGen.BlurAreaCircle(radius, center);
		}

		seed.SetSeedLock(false);

		//Large Craters - locked to keep shape
		seedGen.SeedProperties(false, true, false, false, false);
		count = random.Next(0, 15);
		for (int i = 0; i < count; i++) {
			int height = random.Next(2, 15);
			radius = new Vector2(random.Next(5, 15), random.Next(5, 15));
			center = new Vector2(random.Next(0, seed.X - 1), random.Next(0, seed.Y - 1));
			seedGen.MountainGradual(radius, center, height, true);
			seedGen.BlurAreaCircle(radius, center);
		}

		seed.SetSeedLock(false);

		seedGen.BlurMap();

		RunTerrainMaker();
	}

	/**
	 * Makes the terrain at the origin of the scene.
	 */
	public void RunTerrainMaker() {
		Vector3 position = new Vector3(0, 0, 0);
		terrain = new TerrainMeshMaker(seed, seed.X, seed.Y, position, 1, tileSize);
		terrain.CreateCombinedTerrain(materials[landType], tileSize);
	}

	/**
	 * Makes the terrain at the position specified of the scene.
	 */
	public void RunTerrainMaker(Vector3 position) {
		terrain = new TerrainMeshMaker(seed, seed.X, seed.Y, position, 1, tileSize);
		terrain.CreateCombinedTerrain(materials[landType], tileSize);
	}

	/**
	 * Generates a random bool value.
	 */
	private bool RandomBool() {
		System.Random rand = new System.Random();
		int randBool = rand.Next(0, 99);
		if (randBool < 50) {
			return true;
		}
		else {
			return false;
		}
	}
}
