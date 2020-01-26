using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Rogue Map", menuName = "Rogue Map")]
public class RogueMap : ScriptableObject
{
	public int mapWidth;
	public int mapHeight;

	public int minRoomWidth;
	public int maxRoomWidth;
	public int minRoomHeight;
	public int maxRoomHeight;

	public int maxRoomsPerMap;

	// NOTE: default value set in inspector
	public GameEvent onMapChange;

	public RogueTileData floorTileData;
	public RogueTileData wallTileData;

	// NOTE: default value set in inspector
	public EntityData playerData;

	public int maxMonstersPerRoom;
	public EntityData[] monstersToSpawn;

	public RogueTile[,] rogueTiles;
	public List<Entity> entities;
	
	List<Room> rooms;

	public void MovePlayer(Vector2Int direction)
	{
		Vector2Int destination = entities[0].RogueMapPosition() + direction;
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
				onMapChange.Broadcast();
			}

			RogueGameManager.gameState = GameStates.EnemyTurn;
		}
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

	public void GenerateMap()
	{
		InitializeMap();
		CreateRooms();
		BuildRooms();
		BuildTunnels();
		SpawnPlayer();
		SpawnMonsters();
		onMapChange.Broadcast();
	}

	void InitializeMap()
	{
		rogueTiles = new RogueTile[mapWidth, mapHeight];
		rooms = new List<Room>();
		entities = new List<Entity>();

		// The map is initially filled with walls
		for (int x = 0; x < mapWidth; x++)
		{
			for (int y = 0; y < mapHeight; y++)
			{
				rogueTiles[x, y] = new RogueTile(wallTileData, x, y);
			}
		}
	}

	void CreateRooms()
	{
		for (int i = 0; i < maxRoomsPerMap; i++)
		{
			// Random.Range second argument is non-inclusive, so we add 1 to width and height
			int width = Random.Range(minRoomWidth, maxRoomWidth + 1);
			int height = Random.Range(minRoomHeight, maxRoomHeight + 1);

			// Random map position, avoiding going past map boundaries
			int x = Random.Range(0, mapWidth - width);
			int y = Random.Range(0, mapHeight - height);

			Room newRoom = new Room(x, y, width, height);

			if (!newRoom.Intersects(rooms))
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
					UpdateTile(x, y, floorTileData);
				}
			}
		}
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

	void CreateHorizontalTunnel(int x1, int x2, int y)
	{
		for (int x = Mathf.Min(x1, x2); x <= Mathf.Max(x1, x2); x++)
		{
			UpdateTile(x, y, floorTileData);
		}
	}

	void CreateVerticalTunnel(int y1, int y2, int x)
	{
		for (int y = Mathf.Min(y1, y2); y <= Mathf.Max(y1, y2); y++)
		{
			UpdateTile(x, y, floorTileData);
		}
	}

	// Spawn the player in the center of the room created first
	void SpawnPlayer()
	{
		entities.Add(
			new Entity(
				playerData,
				rooms[0].CenterPoint().x,
				rooms[0].CenterPoint().y));
	}

	void SpawnMonsters()
	{
		foreach (var room in rooms)
		{
			int monsterCount = Random.Range(0, maxMonstersPerRoom + 1);

			for (int i = 0; i < monsterCount; i++)
			{
				Vector2Int spawnPoint = room.RandomPosition();

				// Ensure possible spawn point isn't already occupied
				if (GetEntityAt(spawnPoint) == null)
				{
					int monsterIndex = Random.Range(0, monstersToSpawn.Length);
					EntityData monsterData = monstersToSpawn[monsterIndex];

					Entity monster = new Entity(monsterData, spawnPoint.x, spawnPoint.y);
					entities.Add(monster);
				}
			}
		}
	}

	void UpdateTile(int x, int y, RogueTileData rogueTileData)
	{
		rogueTiles[x, y].data = rogueTileData;
	}

	Entity GetEntityAt(Vector2Int position)
	{
		foreach (var entity in entities)
		{
			if (entity.RogueMapPosition() == position)
			{
				return entity;
			}
		}

		return null;
	}

	bool RandomBool()
	{
		return Random.Range(0, 2) == 0;
	}
}
