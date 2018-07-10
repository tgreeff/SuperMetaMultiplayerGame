using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This class provides a way to set up seeds. Boolean values are available to set 
 * per the seed generator that has been set. 
 */ 
public class SeedGenerator {
	public Seed seed;
	bool add;
	bool subtract;
	bool multiply;
	bool divide;
	bool lockAtSet;

	public SeedGenerator(int x, int y) {
		seed = new Seed(x, y);
	}

	public SeedGenerator(Seed s) {
		seed = s;
	}

	public SeedGenerator(int x, int y, bool add, bool subtract, bool multiply, bool divide, bool lockAtSet) {
		seed = new Seed(x, y);
		this.add = add;
		this.subtract = subtract;
		this.multiply = multiply;
		this.divide = divide;
		this.lockAtSet = lockAtSet;
	}

	public SeedGenerator(Seed s, bool add, bool subtract, bool multiply, bool divide, bool lockAtSet) {
		seed = s;
		this.add = add;
		this.subtract = subtract;
		this.multiply = multiply;
		this.divide = divide;
		this.lockAtSet = lockAtSet;
	}

	/**
	 * Sets the properties for the seed to add, subtract, multiply, or divide in that order.
	 * If there are multple true values, the furthest left bool value will be favored.
	 * Lock is independent of the other bool values.
	 */ 
	public void SeedProperties(bool add, bool subtract, bool multiply, bool divide, bool lockAtSet) {
		this.add = add;
		this.subtract = subtract;
		this.multiply = multiply;
		this.divide = divide;
		this.lockAtSet = lockAtSet;
	}

	/**
	 * Sets the map value based on the properties of the seed gen as 
	 * long as it is in the map range.
	 */
	public void SetHeight(int x, int y, double value) {
		if(seed.IsLocked(x, y)) {
			return;
		}

		seed.LockMap[x, y] = lockAtSet;

		if (!seed.isInstanciated) {
			seed.NoiseMap[x, y] = value;
		}
		else if (add) {
			seed.NoiseMap[x, y] += value;
		}
		else if (subtract) {
			seed.NoiseMap[x, y] -= value;
		}
		else if (multiply) {
			seed.NoiseMap[x, y] *= value;
		}
		else if (divide) {
			seed.NoiseMap[x, y] /= value;
		}
		else {
			seed.NoiseMap[x, y] = value;
		}
	}

	/**
	 * Sets the map to a set of random double values. Will set if all properties are false.
	 */
	public Seed SetMapDouble() {		
		System.Random random = new System.Random();
		for(int x = 0; x < seed.X; x++) {
			for (int y = 0; y < seed.Y; y++) {
				SetHeight(x, y, random.NextDouble());
			}
		}
		seed.isInstanciated = true;
		return seed;
	}

	public Seed ScaleMap(double scale, double xScale, double yScale) {
		for (int x = 0; x < seed.X; x++) {
			for (int y = 0; y < seed.Y; y++) {				
				SetHeight(x, y, scale * seed.GetNoiseValue(xScale * x, yScale * y));
			}
		}
		return seed;
	}

	/**
	 * Sets the map to a set of random values.
	 */
	public Seed SetMapRandom(int max) {
		System.Random random = new System.Random();
		for (int x = 0; x < seed.X; x++) {
			for (int y = 0; y < seed.Y; y++) {
				SetHeight(x, y, (Mathf.Abs(random.Next() % max)));
			}
		}
		seed.isInstanciated = true;
		return seed;
	}

	/**
	 * Set the map using a buffer of values that will be averaged.
	 */
	public Seed SetMapAverageFilter(int numFilter) {
		System.Random random = new System.Random();
		for (int x = 0; x < seed.X; x++) {
			for (int y = 0; y < seed.Y; y++) {

				float total = 0f;
				for(int f = 0; f > numFilter; f++) {
					total += Mathf.Abs(random.Next());
				}
				SetHeight(x, y, total / numFilter);
			}
		}
		seed.isInstanciated = true;
		return seed;
	}

	/**
	 * Set the map using a buffer of values that will be checked for the min.
	 */
	public Seed SetMapMinimumFilter(int numFilter, int min, int max) {
		System.Random random = new System.Random();
		for (int x = 0; x < seed.X; x++) {
			for (int y = 0; y < seed.Y; y++) {

				int minimum = 999999999;				
				for (int f = 0; f > numFilter; f++) {					 
					int value = Mathf.Abs(random.Next(min, max));
					if(value < minimum) {
						minimum = value;
					}
				}
				SetHeight(x, y, minimum);
			}
		}
		seed.isInstanciated = true;
		return seed;
	}

	/**
	 * Set the map with a randomized equation.
	 */
	public Seed SetMapEquation(int amplitude, int frequency1, int frequency2) {
		System.Random random = new System.Random();

		for (int x = 0; x < seed.X; x++) {
			for (int y = 0; y < seed.Y; y++) {
				SetHeight(x, y, amplitude * Mathf.Sin(frequency1 * y) * Mathf.Cos(frequency2 * y));
			}
		}
		seed.isInstanciated = true;
		return seed;
	}

	public double Average3X3(Seed copy, int x, int y) {
		int count = 0;
		double total = 0;

		//Center
		total += copy.GetNoiseValue(x, y);
		count++;

		//Sides
		if (x != 0) {
			total += copy.GetNoiseValue(x - 1, y);
			count++;
		}
		if (x != seed.X - 1) {
			total += copy.GetNoiseValue(x + 1, y);
			count++;
		}
		if (y != 0) {
			total += copy.GetNoiseValue(x, y - 1);
			count++;
		}
		if (y != seed.Y - 1) {
			total += copy.GetNoiseValue(x, y + 1);
			count++;
		}

		//Corners
		if (x != 0 && y != 0) {
			total += copy.GetNoiseValue(x - 1, y - 1);
			count++;
		}
		if (x != seed.X - 1 && y != 0) {
			total += copy.GetNoiseValue(x + 1, y - 1);
			count++;
		}
		if (x != 0 && y != seed.Y - 1) {
			total += copy.GetNoiseValue(x - 1, y + 1);
			count++;
		}
		if (x != seed.X - 1 && y != seed.Y - 1) {
			total += copy.GetNoiseValue(x + 1, y + 1);
			count++;
		}

		//Average
		return total / count;
	}

	/**
	 * Sets a blur to the map to make the map more distributed.
	 * This takes an average of the points within the map.
	 */
	public Seed BlurMap() {
		Seed copy = new Seed(seed.X, seed.Y);
		seed.CopyTo(copy);

		for (int x = 0; x < seed.X; x++) {
			for (int y = 0; y < seed.Y; y++) {
				SetHeight(x, y, Average3X3(copy, x, y));
			}
		}
		seed.isInstanciated = true;
		return seed;
	}

	public void BlurAreaCircle(Vector2 radius, Vector2 center) {
		Seed copy = new Seed(seed.X, seed.Y);
		seed.CopyTo(copy);

		bool xGreater = true;
		int r = (int)radius.x;
		if (radius.x < radius.y) {
			xGreater = false;
			r = (int)radius.y;
		}

		int reduceLeft;
		if (xGreater) {
			reduceLeft = (int)(radius.x - radius.y);
		}
		else {
			reduceLeft = (int)(radius.y - radius.x);
		}

		//Fill Center
		SetHeight((int) center.x, (int) center.y, Average3X3(copy, (int) center.x, (int) center.y));

		for (int layers = r; layers > 0; layers--) {
			for (int i = 0; i < 360; i++) {
				int x = Mathf.RoundToInt(radius.x * Mathf.Sin(i) + center.x);
				int y = Mathf.RoundToInt(radius.y * Mathf.Cos(i) + center.y);
				if (x < seed.X && x >= 0 && y < seed.Y && y > 0) {
					SetHeight(x, y, Average3X3(copy, x, y));
				}
			}
			if (reduceLeft != 0) {
				if (xGreater) {
					radius = new Vector2(radius.x - 1, radius.y);
				}
				else {
					radius = new Vector2(radius.x, radius.y - 1);
				}
				reduceLeft--;
			}
			else {
				if (xGreater) {
					radius = new Vector2(radius.x - 1, radius.y - 1);
				}
				else {
					radius = new Vector2(radius.x - 1, radius.y - 1);
				}
			}
		}

	}

	/**
	 * Sets the proposed line to the specified height.
	 */
	public void Line(int length, double height, Vector2Int direction, Vector2Int location) {
		int x = location.x;
		int y = location.y;
		bool addX = true;
		bool addY = true;
		int xMovements = direction.x;
		int yMovements = direction.y;

		int p = 0;
		while(p < length) {
			if(x < seed.X && x > 0 && y < seed.Y && y > 0) {				
				SetHeight(x, y, height);

				//X movement
				if (xMovements != 0) {
					if(direction.x > 0) {
						x++;
					}
					else if(direction.x < 0) {
						x--;
					}	
					xMovements--;
				} else {					
					addX = false;
				}

				//Y movement
				if(yMovements != 0) {
					if (direction.y > 0) {
						y++;
					}
					else if (direction.y < 0) {
						y--;
					}
					yMovements--;
				} else {					
					addY = false;
				}

				//Reset 
				if ((addY || direction.y == 0) && (addX || direction.x == 0)) {
					xMovements = direction.x;
					yMovements = direction.y;
					addX = true;
					addY = true;
					p++;
				}				
			} else {
				return;
			}
		}		
	}

	/**
	 * Sets the proposed circle to the specified height in a line around the center.
	 */
	public void CircleOutline(Vector2 radius, Vector2 center, double height) {
		for (double i = 0; i < 360; i = i + 0.1) {
			int x = Mathf.RoundToInt(radius.x * Mathf.Sin((float) i) + center.x);
			int y = Mathf.RoundToInt(radius.y * Mathf.Cos((float) i) + center.y);
			if (x < seed.X && x >= 0 && y < seed.Y && y > 0) {
				SetHeight(x, y, height);
			}			
		}
	}

	/**
	 * Sets the proposed circle to the specified height and fills the around the center.
	 */
	public void CircleFill(Vector2 radius, Vector2 center, double height) {
		bool xGreater = true;
		int r = (int) radius.x;
		if(radius.x < radius.y) {
			xGreater = false;
			r = (int) radius.y;
		}

		int reduceLeft;
		if(xGreater) {
			reduceLeft = (int) (radius.x - radius.y);
		}
		else {
			reduceLeft = (int) (radius.y - radius.x);
		}

		//Fill Center
		SetHeight((int)center.x, (int)center.y, height);

		for (int layers = r; layers > 0; layers--) {			
			for (double i = 0; i < 360; i = i + 0.1) {
				int x = Mathf.RoundToInt(radius.x * Mathf.Sin((float) i) + center.x);
				int y = Mathf.RoundToInt(radius.y * Mathf.Cos((float) i) + center.y);
				if (x < seed.X && x >= 0 && y < seed.Y && y > 0) {
					SetHeight(x, y, height);
				}
			}
			if (reduceLeft != 0) {
				if (xGreater) {
					radius = new Vector2(radius.x - 1, radius.y);
				}
				else {
					radius = new Vector2(radius.x, radius.y - 1);
				}
				reduceLeft--;
			}
			else {
				if (xGreater) {
					radius = new Vector2(radius.x - 1, radius.y - 1);
				}
				else {
					radius = new Vector2(radius.x - 1, radius.y - 1);
				}
			}		
		}		
	}

	public void SquareOutline(Vector2 radius, Vector2 center, double height) {
		Vector2Int start1 = new Vector2Int(Mathf.RoundToInt(center.x - radius.x), Mathf.RoundToInt(center.y - radius.y));
		Vector2Int start2 = new Vector2Int(Mathf.RoundToInt(center.x - radius.x), Mathf.RoundToInt(center.y + radius.y));
		Vector2Int start3 = new Vector2Int(Mathf.RoundToInt(center.x + radius.x), Mathf.RoundToInt(center.y + radius.y));
		Vector2Int start4 = new Vector2Int(Mathf.RoundToInt(center.x + radius.x), Mathf.RoundToInt(center.y - radius.y));

		for (int x = 0; x < 2 * radius.x; x++) {
			SetHeight(start1.x + x, start1.y, height);		
			SetHeight(start3.x - x, start3.y, height);			
		}
		for (int y = 0; y < 2 * radius.y; y++) {
			SetHeight(start4.x, start4.y + y, height);
			SetHeight(start2.x, start2.y - y, height);
		}
	}

	public void SquareFill(Vector2 radius, Vector2 center, double height) {	
		for (int x = 0; x < 2 * radius.x; x++) {
			Vector2Int start = new Vector2Int(Mathf.RoundToInt(center.x - radius.x + x), Mathf.RoundToInt(center.y - radius.y));
			for (int y = 0; y < 2 * radius.y; y++) {
				SetHeight(start.x, start.y + y, height);				
			}
		}		
	}

	/**
	 * Makes a mountain from the base radius up to the height height specified.
	 */
	public void MountainLinear(Vector2 radius, Vector2 center, double height, bool fill) {
		double h = 1;
		while ((radius.x != 1 && radius.y != 1) && h != height) {
			CircleOutline(radius, center, h);

			h++;
			radius.x--;
			radius.y--;
			if (radius.x < 1) {
				radius.x = 1;
			}
			if (radius.y < 1) {
				radius.y = 1;
			}
		}
		if(fill) {
			CircleFill(radius, center, height);
		}
	}

	/**
	 * Makes a mountain from the base radius up to the height height specified.
	 */
	public void MountainGradual(Vector2 radius, Vector2 center, double height, bool fill) {
		double h = 1;
		while ((radius.x != 1 && radius.y != 1)) {
			h += height/h;

			CircleOutline(radius, center, h - height);

			radius.x--;
			radius.y--;
			if (radius.x < 1) {
				radius.x = 1;
			}
			if (radius.y < 1) {
				radius.y = 1;
			}
		}
		
		CircleFill(radius, center, h - height);	
	}

	/**
	 * Makes a mountain from the starting from the specifed height to the base of the current map.
	 */
	public void MountainLinearHeight(Vector2 center, double height) {
		double h = seed.GetNoiseValue(center.x, center.y);
		Vector2 rad = new Vector2(1, 1);

		CircleFill(rad, center, height);

		while (h != height) {
			height--;
			rad.x++;
			rad.y++;
			CircleOutline(rad, center, height);
		}
	}

	public void MountainSteep(Vector2 radius, Vector2 center, double height, bool fill) {
		double h = height/2;
		while ((radius.x != 1 || radius.y != 1) ) {
			CircleOutline(radius, center, h - (height/2));

			h += h / height;

			radius.x--;
			radius.y--;

			if (radius.x < 1) {
				radius.x = 1;
			}
			if (radius.y < 1) {
				radius.y = 1;
			}			
		}
		
		if (fill) {
			CircleFill(radius, center, h);
		}

	}

	public void Volcano(Vector2 radius, Vector2 center, double height, bool fill) {
		double h = 1;
		while ((radius.x != 1 || radius.y != 1)) {
			CircleOutline(radius, center, h);

			h = h * 1.25;
			if (h > 500) {
				h = 500;
			}

			radius.x--;
			radius.y--;

			if (radius.x < 1) {
				radius.x = 1;
			}
			if (radius.y < 1) {
				radius.y = 1;
			}
		}
		if (fill) {
			CircleFill(radius, center, h);
		}
	}

	public void Bullseye(Vector2 radius, Vector2 center, double height, bool fill) {
		double h = 1;
		while ((radius.x != 1 || radius.y != 1)) {
			CircleOutline(radius, center, h);

			h = h * 2;
			if (h > 100) {
				h = 100;
			}

			radius.x -= radius.x / 2;
			radius.y -= radius.y / 2;
			if (radius.x < 1) {
				radius.x = 1;
			}
			if (radius.y < 1) {
				radius.y = 1;
			}
		}
		if (fill) {
			CircleFill(radius, center, h);
		}
	}

}
