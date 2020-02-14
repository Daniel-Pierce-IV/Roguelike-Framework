﻿using System.Collections.Generic;
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
		this.x2 = x + width - 1;
		this.y2 = y + height - 1;
	}

	public Vector2Int CenterPoint()
	{
		return new Vector2Int(
			(x + x2) / 2,
			(y + y2) / 2);
	}

	// Return a random point from within the room
	public Vector2Int RandomPosition()
	{
		return new Vector2Int(
			Random.Range(x + 1, x2),
			Random.Range(y + 1, y2));
	}

	// Determines intersections by comparing start/end coordinates for both rooms
	public bool Intersects(Room otherRoom, bool canShareWalls = true)
	{
		if (canShareWalls)
		{
			return (
				x < otherRoom.x2 && x2 > otherRoom.x &&
				y < otherRoom.y2 && y2 > otherRoom.y);
		}
		
		return (
			x <= otherRoom.x2 && x2 >= otherRoom.x &&
			y <= otherRoom.y2 && y2 >= otherRoom.y);
	}

	public bool Intersects(List<Room> otherRooms, bool canShareWalls = true)
	{
		foreach (Room otherRoom in otherRooms)
		{
			if (Intersects(otherRoom, canShareWalls))
			{
				return true;
			}
		}

		return false;
	}
}
