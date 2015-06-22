using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// Actually makes the maze
public class MazeGenerator : MonoBehaviour {

	private enum Direction : int {
		NONE = 0,
		NORTH = 1,
		WEST  = 2,
		SOUTH = 4,
		EAST = 8,
		ALL = 1 | 2 | 4 | 8
	};

	// 2D integer vector
	private class MazePoint {
		public int x, y;

		public MazePoint(int x, int y) {
			this.x = x;
			this.y = y;
		}
	}

	// for the DFS
	private class StackFrame
	{
		public MazePoint to;
		public MazePoint from;
		public Direction d;

		public StackFrame(MazePoint f, MazePoint t, Direction d) {
			this.to = t;
			this.from = f;
			this.d = d;
		}
	}
	
	private int width = 10, height = 10; // how many squares on the grid
	private float roomWidth, roomHeight; // how many unity units is the space we are dividing up
	private float cellWidth, cellHeight;
	private int[,] maze; // the grid
	private IList<GameObject> walls;

	// public fields
	public GameObject[] eastWalls; // prefabs for the east wall
	public GameObject[] southWalls; // prefabs for the south wall of each grid cell that needs one (except edges)
	public Transform topLeftCorner; // The top left corner of the top left cell in the grid
	public Transform bottomRightCorner; // the bottom left corner of the bottom left cell in the grid
	public bool randomFacing = true; // if the walls should face randomly


	// Use this for initialization
	void Awake () {
		roomWidth = bottomRightCorner.position.x - topLeftCorner.position.x;
		roomHeight = bottomRightCorner.position.z - topLeftCorner.position.z;
		cellWidth = roomWidth / (float)width;
		cellHeight = roomHeight / (float)height;
		walls = new List<GameObject> ();

		GenerateMaze ();
	}

	public void GenerateMaze() {
		maze = new int[width,height]; // clear out any existing data
		foreach (GameObject o in walls) {
			Destroy(o);
		}
		walls.Clear ();


		Stack<StackFrame> fringe = new Stack<StackFrame> ();
		fringe.Push (new StackFrame (null, new MazePoint (0, 0), Direction.NONE)); // start at some corner

		while (fringe.Count > 0) {
			StackFrame frame = fringe.Pop ();
			MazePoint current = frame.to;

			int b = maze[current.x, current.y];
			if (b == 0) { // only interested if unvisited
				IList<StackFrame> children = Shuffle(ChildrenOf(current, frame.d));
				if (frame.d != Direction.NONE) { // make the hole
					maze[frame.from.x, frame.from.y] |= (int)frame.d;
					maze[current.x, current.y] |= (int)Flip (frame.d);
				}
				// push them on the stack
				foreach (StackFrame child in children) {
					fringe.Push(child);
				}
			}
		}

		// at this point we have the data in the int array, now we need to turn it into actuall gameobjects
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				if (((maze[x,y] & (int)Direction.SOUTH) == 0) && y < (height-1)) {
					walls.Add (GetSouthWall(x,y));
				}
				if (((maze[x,y] & (int)Direction.EAST) == 0) && x < (height-1)) {
					walls.Add (GetEastWall(x,y));
				}
			}
		}
	}

	// chooses a random game object from the south wall list and returns a copy of it
	// currently with uniform probability
	private GameObject GetSouthWall(int x, int y) {
		GameObject wall = Instantiate<GameObject> (southWalls [Random.Range (0, southWalls.Length)]);
		wall.transform.parent = transform;

		wall.transform.position = new Vector3 (topLeftCorner.position.x + cellWidth * x + cellWidth / 2.0f,
		                                       0,
		                                       topLeftCorner.position.z + cellHeight * y + cellHeight);
		if (randomFacing)
			wall.transform.Rotate (new Vector3 (0, Random.Range (0, 2) * 180, 0));

		return wall;
	}

	// and for the east wall
	private GameObject GetEastWall(int x, int y) {
		GameObject wall = Instantiate<GameObject> (eastWalls [Random.Range (0, eastWalls.Length)]);
		wall.transform.parent = transform;
		
		wall.transform.position = new Vector3 (topLeftCorner.position.x + cellWidth * x + cellWidth,
		                                       0,
		                                       topLeftCorner.position.z + cellHeight * y + cellHeight/2);
		if (randomFacing)
			wall.transform.Rotate (new Vector3 (0, Random.Range (0, 2) * 180, 0));
		
		return wall;
	}

	private IList<StackFrame> Shuffle(IList<StackFrame> list) {
		for (int i = 0; i < list.Count; i++) {
			StackFrame temp = list[i];
			int randomIndex = Random.Range(i, list.Count);
			list[i] = list[randomIndex];
			list[randomIndex] = temp;
		}
		return list; // doesn't have to return, but is convenient
	}

	

	private List<StackFrame> ChildrenOf(MazePoint mp, Direction from) {
		List<StackFrame> c = new List<StackFrame> ();
		for (int i = 0; i < 4; i++) { // for each direction
			MazePoint child = NextPoint(mp, (Direction)(1 << i));
			if ((Direction)(1 << i) != from && 
			    child.x >= 0 && child.x < 10 && 
			    child.y >= 0 && child.y < 10 && 
			    maze[child.x, child.y] == (int)Direction.NONE)
				c.Add(new StackFrame(mp, child, (Direction)(1<<i)));
		}
		return c;
	}
	
	// reverses a direction
	private Direction Flip(Direction d) {
		switch (d) {
		case Direction.WEST:
			return Direction.EAST;
		case Direction.NORTH:
			return Direction.SOUTH;
		case Direction.SOUTH:
			return Direction.NORTH;
		case Direction.EAST:
			return Direction.WEST;
		case Direction.NONE:
			return Direction.NONE;
		default:
			return Direction.NONE;
		}
	}
	
	// chooses a random direction from the available directions
	private Direction RandomDirection(MazePoint from) {
		int r = Random.Range (0, 4);
		byte d = (byte)(1 << r);
		
		MazePoint next = NextPoint (from, (Direction)d);
		
		int count = 0;
		// make sure it is valid
		while ((next.x < 0 || next.x >= 10 || next.y < 0 || next.y >= 10 ||
		        maze[next.x, next.y] != (byte)Direction.NONE) && count < 4) {
			d = (byte)CycleDirection ((Direction)d);
			next = NextPoint (from, (Direction)d);
			count++;
		}
		
		if (count == 4)
			return Direction.NONE;
		
		return (Direction)d;
	}
	
	// cycles the direction around the compass points clockwise one step
	private Direction CycleDirection( Direction d) {
		switch (d) {
		case Direction.WEST:
			return Direction.NORTH;
		case Direction.NORTH:
			return Direction.EAST;
		case Direction.SOUTH:
			return Direction.WEST;
		case Direction.EAST:
			return Direction.SOUTH;
		case Direction.NONE:
			return Direction.NONE;
		default:
			return Direction.NONE;
		}
	}
	
	private MazePoint NextPoint (MazePoint from, Direction d) {
		switch (d) {
		case Direction.EAST:
			return new MazePoint(from.x + 1, from.y);
			
		case Direction.NORTH:
			return new MazePoint(from.x, from.y-1);
			
		case Direction.SOUTH:
			return new MazePoint(from.x, from.y +1);
			
		case Direction.WEST:
			return new MazePoint(from.x-1, from.y);
			
		default:
			Debug.Log("Invalid direction: " + d);
			return null;
		}
	}
}
