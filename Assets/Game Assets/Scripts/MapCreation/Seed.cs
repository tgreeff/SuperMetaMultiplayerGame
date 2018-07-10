using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed{
	public double[,] NoiseMap { get; private set; }
	public bool[,] LockMap {get; private set; }
	public bool isInstanciated;

	public int X { get; private set; }
	public int Y { get; private set; }

	public Seed(int x, int y) {
		X = x;
		Y = y;

		NoiseMap = new double[X, Y];
		LockMap = new bool[X, Y];
	}

	/**
	 * Returns whether the position is locked.
	 */
	public bool IsLocked(int x, int y) {
		if ((x >= 0 && x < X) && (y >= 0 && y < Y)) {
			return LockMap[x, y];
		}
		else {
			return false;
		}
	}

	
	/**
	 * Sets all locked indexs.
	 */
	 public void SetSeedLock(bool locked) {
		for(int x = 0; x < X; x++) {
			for (int y = 0; y < Y; y++) {
				LockMap[x, y] = locked;
			}
		}
	}

	/**
	 * Takes a value from a generated map and returns the value. 
	 */
	public double GetNoiseValue(double x, double y) {
		if ((x >= 0 && x < X) && (y >= 0 && y < Y)) {
			return NoiseMap[(int) x, (int) y];
		}
		else {
			return 0;
		}
	}

	/**
	 * Takes the average of 2 seeds and places in the object called seed.
	 */
	public void Average(Seed s) {
		for(int x = 0; x < X; x++) {
			for(int y = 0; y < Y; y++) {
				double total = NoiseMap[x, y] + s.NoiseMap[x, y];
				NoiseMap[x, y] = total / 2;
			}
		}
	}

	public void Add(Seed s) {
		for (int x = 0; x < X; x++) {
			for (int y = 0; y < Y; y++) {
				NoiseMap[x, y] += s.NoiseMap[x, y];
			}
		}
	}

	public void Multiply(Seed s) {
		for (int x = 0; x < X; x++) {
			for (int y = 0; y < Y; y++) {
				NoiseMap[x, y] *= s.NoiseMap[x, y];
			}
		}
	}

	public void CopyTo(Seed s) {
		for (int x = 0; x < X; x++) {
			for (int y = 0; y < Y; y++) {
				s.NoiseMap[x, y] = NoiseMap[x, y];
				LockMap[x, y] = s.LockMap[x, y];
			}
		}
	}

	public void CopyFrom(Seed s) {
		for (int x = 0; x < X; x++) {
			for (int y = 0; y < Y; y++) {
				NoiseMap[x, y] = s.NoiseMap[x, y];
			}
		}
	}
}
