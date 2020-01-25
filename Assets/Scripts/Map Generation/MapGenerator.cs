public static class MapGenerator
{
	public static RogueTile[,] GenerateMap(
		int width,
		int height,
		RogueTileData floor,
		RogueTileData wall)
	{
		RogueTile[,] rogueTileMap = InitializeMap(width, height, wall);
		CreateRooms(rogueTileMap, floor);

		return rogueTileMap;
	}

	static RogueTile[,] InitializeMap(int width, int height, RogueTileData wall)
	{
		RogueTile[,] rogueTileMap = new RogueTile[width, height];

		// The map is initially filled with walls
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				rogueTileMap[x, y] = new RogueTile(wall, x, y);
			}
		}

		return rogueTileMap;
	}

	static void CreateRooms(RogueTile[,] rogueTileMap, RogueTileData floor)
	{
		Room room1 = new Room(1, 1, 3, 3);
		Room room2 = new Room(5, 5, 3, 4);

		CreateRoom(rogueTileMap, floor, room1);
		CreateRoom(rogueTileMap, floor, room2);
	}

	static void CreateRoom(RogueTile[,] rogueTileMap, RogueTileData floor, Room room)
	{
		for (int x = room.x + 1; x < room.x2; x++)
		{
			for (int y = room.y + 1; y < room.y2; y++)
			{
				rogueTileMap[x, y].data = floor;
			}
		}
	}
}
