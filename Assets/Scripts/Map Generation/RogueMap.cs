using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RogueMap
{
	RogueMapData mapData;
	RogueTile[,] rogueTiles;

	Tilemap tilemap;
	List<Room> rooms;

	EntityData playerData;
	List<Entity> entities;

	float cameraXOffset = -0.5f;
	float cameraYOffset = 0.75f;

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
		BuildTunnels();
		InitializePlayer();
		SpawnMonsters();
	}

	public void MovePlayer(Vector2Int direction)
	{
		Vector2Int destination = entities[0].GetRogueMapPosition() + direction;
		RogueTile destinationTile = rogueTiles[destination.x, destination.y];

		if (!destinationTile.data.blocksMovement)
		{
			Entity entity = GetEntityAt(destination);

			if (entity != null && entity.data.blocksMovement)
			{
				Debug.Log("You attack the " + entity.data.name + "!");
			}
			else
			{
				entities[0].x = destination.x;
				entities[0].y = destination.y;
				UpdateCameraPosition();
			}

			RogueGameManager.gameState = GameStates.EnemyTurn;
		}

		RenderToTilemap();
	}

	public void SimulateEntities()
	{
		foreach (var entity in entities)
		{
			if (entity.data != playerData)
			{
				Debug.Log("The " + entity.data.name + " ponders its own existence.");
			}
		}

		RogueGameManager.gameState = GameStates.PlayerTurn;
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
					UpdateTile(x, y, mapData.floorTileData);
				}
			}
		}
	}

	void UpdateTile(int x, int y, RogueTileData rogueTileData)
	{
		rogueTiles[x, y].data = rogueTileData;
	}

	// Spawn the player in the center of the room created first
	void InitializePlayer()
	{
		entities.Add(
			new Entity(
				playerData,
				rooms[0].CenterPoint().x,
				rooms[0].CenterPoint().y));

		UpdateCameraPosition();
	}

	// Set camera's position to match player position
	void UpdateCameraPosition()
	{
		Camera.main.transform.position = new Vector3(
				entities[0].x + cameraXOffset,
				entities[0].y + cameraYOffset,
				Camera.main.transform.position.z);
	}

	void BuildTunnels()
	{
		Vector2Int center1;
		Vector2Int center2;

		for (int i = 1; i < rooms.Count; i++)
		{

			center1 = rooms[i - 1].CenterPoint();
			center2 = rooms[i].CenterPoint();

			// Flip a coin to decide if we'll dig horizontally first
			if (RandomBool())
			{
				CreateHorizontalTunnel(center1.x, center2.x, center1.y);
				CreateVerticalTunnel(center1.y, center2.y, center2.x);
			}
			else
			{
				CreateVerticalTunnel(center1.y, center2.y, center1.x);
				CreateHorizontalTunnel(center1.x, center2.x, center2.y);
			}
		}
	}

	bool RandomBool()
	{
		return Random.Range(0, 2) == 0;
	}

	void CreateHorizontalTunnel(int x1, int x2, int y)
	{
		for (int x = Mathf.Min(x1, x2); x <= Mathf.Max(x1, x2); x++)
		{
			UpdateTile(x, y, mapData.floorTileData);
		}
	}

	void CreateVerticalTunnel(int y1, int y2, int x)
	{
		for (int y = Mathf.Min(y1, y2); y <= Mathf.Max(y1, y2); y++)
		{
			UpdateTile(x, y, mapData.floorTileData);
		}
	}

	void SpawnMonsters()
	{
		foreach (var room in rooms)
		{
			int monsterCount = Random.Range(0, mapData.maximumMonstersPerRoom + 1);

			for (int i = 0; i < monsterCount; i++)
			{
				bool shouldSpawn = true;
				Vector2Int spawnPoint = room.RandomPosition();

				// Ensure possible spawn point isn't already occupied
				foreach (var entity in entities)
				{
					if (entity.GetRogueMapPosition() == spawnPoint)
					{
						shouldSpawn = false;
						break;
					}
				}

				if (shouldSpawn)
				{
					int monsterIndex = Random.Range(0, mapData.monstersToSpawn.Length);
					EntityData monsterData = mapData.monstersToSpawn[monsterIndex];

					Entity monster = new Entity(monsterData, spawnPoint.x, spawnPoint.y);
					entities.Add(monster);
				}
			}
		}
	}

	Entity GetEntityAt(Vector2Int position)
	{
		foreach (var entity in entities)
		{
			if (entity.GetRogueMapPosition() == position)
			{
				return entity;
			}
		}

		return null;
	}
}
