using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Makes a custom terrain mesh where the height of each plot point can be manipulated by a seed.
 * 
 * TODO: Make multiple meshes in the event that too many tiles have been made for a mesh object.
 */
public class TerrainMeshMaker {
	public Sector terrain;
	private Seed meshSeed; 
	private Vector3 position;

	public TerrainMeshMaker(Seed seed, int x, int y, Vector3 position, int height, float tileLength) {
		meshSeed = seed;
		terrain = new Sector(x, y, height, tileLength, 1);
		this.position = position;
	}

	public void CreateTerrain(Material material, float tileSize) {
		
		for (int x = 0; x < terrain.x; x++) {
			for (int y = 0; y < terrain.y; y++) {
				//Make Mesh
				Vector3 i0 = new Vector3(0, (float) meshSeed.GetNoiseValue(x, y), 0);
				Vector3 i1 = new Vector3(0, (float) meshSeed.GetNoiseValue(x, y + 1), tileSize);
				Vector3 i2 = new Vector3(tileSize, (float) meshSeed.GetNoiseValue(x + 1, y + 1), tileSize);
				Vector3 i3 = new Vector3(tileSize, (float) meshSeed.GetNoiseValue(x + 1, y), 0);
				Mesh mesh = CreateTile(i0, i1, i2, i3);

				//Make GameObject with mesh and material
				GameObject obj = new GameObject("Tile", typeof(MeshRenderer), typeof(MeshFilter), typeof(MeshCollider));
				obj.GetComponent<MeshCollider>().sharedMesh = mesh;
				obj.GetComponent<MeshFilter>().mesh = mesh;
				obj.GetComponent<MeshFilter>().mesh.MarkDynamic();
				obj.GetComponent<Renderer>().material = material;

				Vector3 position = new Vector3(x * tileSize, 0, y * tileSize);
				obj.transform.position = position;
				terrain.SetMapTransform(x, 0, y, obj.transform);				
			}
		}
	}

	public void CreateCombinedTerrain(Material material, float tileSize) {
		int length = terrain.x * terrain.y;
		CombineInstance[] combine = new CombineInstance[length];

		for (int x = 0; x < terrain.x; x++) {
			for (int y = 0; y < terrain.y; y++) {
				//Make Mesh
				Vector3 i0 = new Vector3(0, (float)meshSeed.GetNoiseValue(x, y), 0);
				Vector3 i1 = new Vector3(0, (float)meshSeed.GetNoiseValue(x, y + 1), tileSize);
				Vector3 i2 = new Vector3(tileSize, (float)meshSeed.GetNoiseValue(x + 1, y + 1), tileSize);
				Vector3 i3 = new Vector3(tileSize, (float)meshSeed.GetNoiseValue(x + 1, y), 0);
				Mesh mesh = CreateTile(i0, i1, i2, i3);

				//Make GameObject with mesh and material
				GameObject obj = new GameObject("Tile", typeof(MeshRenderer), typeof(MeshFilter), typeof(MeshCollider));
				obj.GetComponent<MeshCollider>().sharedMesh = mesh;
				obj.GetComponent<MeshFilter>().mesh = mesh;
				obj.GetComponent<MeshFilter>().mesh.MarkDynamic();
				obj.GetComponent<Renderer>().material = material;

				Vector3 tilePos = new Vector3((tileSize * terrain.x) - (x + position.x) * tileSize, 0, (tileSize * terrain.y) - (y + position.y) * tileSize);
				obj.transform.position = tilePos;
				terrain.SetMapTransform(x, 0, y, obj.transform);

				//Add to combine array
				combine[terrain.y * x + y] = new CombineInstance();
				combine[terrain.y * x + y].mesh = mesh;
				combine[terrain.y * x + y].transform = obj.transform.worldToLocalMatrix;

				//Destroy old GameObject
				obj.SetActive(false);
				GameObject.Destroy(obj);
			}
		}
		GameObject tile = new GameObject("Terrain", typeof(MeshRenderer), typeof(MeshFilter), typeof(MeshCollider));
		tile.GetComponent<MeshFilter>().mesh = new Mesh();
		tile.GetComponent<MeshFilter>().mesh.CombineMeshes(combine, true, true, true);
		tile.GetComponent<Renderer>().material = material;
		tile.GetComponent<MeshCollider>().sharedMesh = tile.GetComponent<MeshFilter>().mesh;
		tile.transform.position = new Vector3(tileSize * terrain.x, 0, tileSize * terrain.y);
	}

	private Mesh CreateTile(Vector3 index0, Vector3 index1, Vector3 index2, Vector3 index3) {
		MeshGen mesh = new MeshGen();

		// Mesh Generation
		mesh.Vertices.Add(index0);
		mesh.UV.Add(new Vector2(0f, 0f));
		mesh.Normals.Add(Vector3.up);

		mesh.Vertices.Add(index1);
		mesh.UV.Add(new Vector2(0f, 1f));
		mesh.Normals.Add(Vector3.up);

		mesh.Vertices.Add(index2);
		mesh.UV.Add(new Vector2(1f, 1f));
		mesh.Normals.Add(Vector3.up);

		mesh.Vertices.Add(index3);
		mesh.UV.Add(new Vector2(1f, 0f));
		mesh.Normals.Add(Vector3.up);

		mesh.CreateTriangles(new Vector3Int(0, 1, 2));
		mesh.CreateTriangles(new Vector3Int(0, 2, 3));

		mesh.ApplyMeshChanges();

		return mesh.Mesh;
	}
}
