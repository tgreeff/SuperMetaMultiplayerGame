using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Aids in creating meshes for terrain and other objects.
 * Used http://jayelinda.com/modelling-by-numbers-part-1a/ to help.
 */
public class MeshGen {
	private Mesh mesh;
	public Mesh Mesh {
		get { return mesh; }
	}

	private List<Vector3> vertices;
	public List<Vector3> Vertices {
		get { return vertices; }
	}

	private List<Vector2> uv;
	public List<Vector2> UV {
		get { return uv; }
	}

	private List<Vector3> normals;
	public List<Vector3> Normals{
		get { return normals; }
	}

	private List<int> indicies;

	public MeshGen() {
		mesh = new Mesh();
		mesh.Clear();

		vertices = new List<Vector3>();
		uv = new List<Vector2>();
		normals = new List<Vector3>();
		indicies = new List<int>();
	}

	public void CreateTriangles(Vector3Int i) {
		indicies.Add(i.x);
		indicies.Add(i.y);
		indicies.Add(i.z);
	}

	public void ApplyMeshChanges() {
		mesh.vertices = vertices.ToArray();
		mesh.triangles = indicies.ToArray();
		mesh.uv = uv.ToArray();
		mesh.normals = normals.ToArray();

		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
		mesh.RecalculateTangents();
	}
}
