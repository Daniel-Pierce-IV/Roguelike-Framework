using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RogueMap
{
	RogueMapData mapData;
	RogueTile[,] rogueTiles;
	Tilemap tilemap;
	List<Room> rooms;
	List<Entity> entities;
	EntityData playerData;
	Vector2Int playerCoordinates;

	public RogueMap(Tilemap tilemap, EntityData playerData)
	{
		this.tilemap = tilemap;
		this.playerData = playerData;
	}

	public void GenerateMap(RogueMapData mapData)
	{
		this.mapData = mapData;
		rooms = new List<Room>();
		entities = new List<Entity>();

		InitializeMap();
		CreateRooms();
		BuildRooms();
		InitializePlayer();
		//CreateHorizontalTunnel(rogueTileMap, floor, 6, 2, 2);
	}

	public void RenderToTilemap()
	{
		for (int x = 0; x < rogueTiles.GetLength(0); x++)
		{
			for (int y = 0; y < rogueTiles.GetLength(1); y++)
			{
				tilemap.SetTile(
					rogueTiles[x, y].GetTilemapPosition(),
					rogueTiles[x, y].data.spriteTile);
			}
		}

		foreach (Entity entity in entities)
		{
			tilemap.SetTile(
					entity.GetTilemapPosition(),
					entity.data.spriteTile);
		}
	}

	void InitializeMap()
	{
		rogueTiles = new RogueTile[mapData.mapWidth, mapData.mapHeight];

		// The map is initially filled with walls
		for (int x = 0; x < mapData.mapWidth; x++)
		{
			for (int y = 0; y < mapData.mapHeight; y++)
			{
				rogueTiles[x, y] = new RogueTile(mapData.wallTileData, x, y);
			}
		}
	}

	void CreateRooms()
	{
		for (int i = 0; i < mapData.maximumNumberOfRooms; i++)
		{
			// Random.Range second argument is non-inclusive, so we add 1 to width and height
			int width = Random.Range(mapData.minimumRoomWidth, mapData.maximumRoomWidth + 1);
			int height = Random.Range(mapData.minimumRoomHeight, mapData.maximumRoomHeight + 1);

			// Random map position, avoiding going past map boundaries
			int x = Random.Range(0, mapData.mapWidth - width);
			int y = Random.Range(0, mapData.mapHeight - height);

			Room newRoom = new Room(x, y, width, height);
			bool isValid = true;

			foreach (Room otherRoom in rooms)
			{
				if (newRoom.Intersects(otherRoom))
				{
					isValid = false;
				}
			}

			if (isValid)
			{
				rooms.Add(newRoom);
			}
		}
	}

	void BuildRooms()
	{
		foreach (Room room in rooms)
		{
			// Plus 1 to x/y so we leave a wall
			for (int x = room.x + 1; x < room.x2; x++)
			{
				for (int y = room.y + 1; y < room.y2; y++)
				{
					//Debug.Log("x: " + x + " y: " + y);
					rogueTiles[x, y].data = mapData.floorTileData;
				}
			}
		}
	}

	void InitializePlayer()
	{
		entities.Add(
			new Entity(
				playerData,
				rooms[0].CenterPoint().x,
				rooms[0].CenterPoint().y));
	}

	public void MovePlayer(Vector2Int direction)
	{
		Vector2Int destination = entities[0].GetRogueMapPosition() + direction;
		RogueTile destinationTile = rogueTiles[destination.x, destination.y];

		if (!destinationTile.data.blocksMovement)
		{
			entities[0].x = destination.x;
			entities[0].y = destination.y;

			// Camera follows player
			Camera.main.transform.position = new Vector3(
				destination.x - 0.5f,
				destination.y + 0.75f,
				Camera.main.transform.position.z);
		}

		RenderToTilemap();
	}

	//static void CreateHorizontalTunnel(RogueTile[,] rogueTileMap, RogueTileData floor, int x1, int x2, int y)
	//{
	//	for (int x = Mathf.Min(x1, x2); x < Mathf.Max(x1, x2); x++)
	//	{
	//		rogueTileMap[x, y].data = floor;
	//	}
	//}

	//static void CreateVerticalTunnel(RogueTile[,] rogueTileMap, RogueTileData floor, int y1, int y2, int x)
	//{
	//	for (int y = Mathf.Min(y1, y2); y < Mathf.Max(y1, y2); y++)
	//	{
	//		rogueTileMap[x, y].data = floor;
	//	}
	//}
}

