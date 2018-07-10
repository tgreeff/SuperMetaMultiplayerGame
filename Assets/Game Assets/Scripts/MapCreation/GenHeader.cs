using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenHeader {

	//-------GENERATION CONSTANTS/-------
	public const float TILE_HEIGHT = 4.21f;
	public const float TILE_SIZE = 10.5f;

	//-------HALL CONSTANTS/-------
	public const int EMPTY = 0;
	public const int START = 1;
	public const int HALL = 2;
	public const int HALL_CORNER = 3;
	public const int HALL_TRI = 4;
	public const int HALL_QUAD = 5;
	public const int HALL_DEAD_END = 6;
	public const int HALL_DOOR_END = 7;
	public const int HALL_HATCH_DOWN = 8;
	public const int HALL_LADDER_UP = 9;

	//-------LAND CONSTANTS-------
	public const int FLAT = 0;
	public const int PLAINS = 1;
	public const int TUNDRA = 2;
	public const int OASIS = 3;
	public const int STEPPE = 4;
	public const int HILL = 5;
	public const int DESERT = 6;	
	public const int MOUNTAIN = 7;
	public const int FOREST = 8;
	public const int MARSH = 9;
	public const int SWAMP = 10;
	public const int CITY= 11;
	public const int ISLAND = 12;
	public const int BEACH = 13;
	public const int CAVE = 14;
	public const int OCEAN = 15;
	public const int SPIRE = 16;

	public struct Decisions {
		public bool renderTerrain;
		public bool renderWater;
		public bool renderWall;
		public bool renderBarrier;
		public bool renderDecoration;
		public bool renderStructure;
	}
}
