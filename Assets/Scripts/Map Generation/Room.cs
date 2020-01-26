using UnityEngine;

class Room
{
	public int x;
	public int y;
	public int x2;
	public int y2;
	public int width;
	public int height;

	// Width/height is total length, including walls
	// Actual walking area is (width - 2) * (height - 2)
	public Room(int x, int y, int width, int height)
	{
		this.x = x;
		this.y = y;
		this.width = width;
		this.height = height;
		this.x2 = x + width;
		this.y2 = y + height;
	}

	public Vector2Int CenterPoint()
	{
		return new Vector2Int(
			(x + x2) / 2,
			(y + y2) / 2);
	}

	public bool Intersects(Room room)
	{
		// Determines intersections by comparing start/end coordinates for both rooms
		return (
			x <= room.x2 && x2 >= room.x &&
			y <= room.y2 && y2 >= room.y);
	}

	// Return a random point from within the room
	public Vector2Int RandomPosition()
	{
		return new Vector2Int(
			Random.Range(x + 1, x2),
			Random.Range(y + 1, y2));
	}
}
