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

	public RogueTileData floorTileData;
	public RogueTileData wallTileData;

	// NOTE: default value set in inspector
	public EntityData playerData;

	public int maxMonstersPerRoom;
	public EntityData[] monstersToSpawn;

	public RogueTile[,] rogueTiles;
	public List<Entity> entities;

	public Entity Player
	{
		get { return entities[0]; }
	}
	
	List<Room> rooms;
	public DijkstrasAlgorithm pathFinder = new DijkstrasAlgorithm();

	public void MoveEntityToPosition(Entity entity, Vector2Int position)
	{
		entity.Position = position;
		EventSystem.Instance.OnMapChange();
	}

	public void MoveEntityAlongPath(Entity entity)
	{
		Vector2Int newPosition = entity.travelPath[0];
		entity.travelPath.RemoveAt(0);

		if (PositionCanBeMovedTo(newPosition))
		{
			entity.Position = newPosition;
			EventSystem.Instance.OnMapChange();
		}
	}

	public void AttackEntity(Entity attacker, Entity defender)
	{
		if (attacker == Player)
		{
			Debug.Log("You attack the " + defender.data.name + "!");
		}
		else
		{
			Debug.Log("The " + attacker.data.name + " attacks you!");
		}
	}

	public List<Entity> EntitiesAtPosition(Vector2Int position)
	{
		List<Entity> discoveredEntities = new List<Entity>();

		foreach (var entity in entities)
		{
			if (entity.Position == position)
			{
				discoveredEntities.Add(entity);
			}
		}

		return discoveredEntities;
	}

	public List<Entity> AttackableEntitiesAtPosition(Vector2Int position)
	{
		List<Entity> discoveredEntities = new List<Entity>();

		foreach (var entity in entities)
		{
			if (entity.Position == position && entity.data.stats != null)
			{
				discoveredEntities.Add(entity);
			}
		}

		return discoveredEntities;
	}

	public bool PositionCanBeMovedTo(Vector2Int position)
	{
		if (rogueTiles[position.x, position.y].data.blocksMovement)
		{
			return false;
		}

		foreach (var entity in EntitiesAtPosition(position))
		{
			if (entity.data.blocksMovement)
			{
				return false;
			}
		}

		return true;
	}

	public void SimulateEntities()
	{
		foreach (var entity in entities)
		{
			if (entity.CanAct())
			{
				entity.Act();
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
		pathFinder.GenerateNodes(rogueTiles);
		EventSystem.Instance.OnMapChange();
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
		entities.Add(new Entity(playerData, rooms[0].CenterPoint(), this));
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
				if (EntitiesAtPosition(spawnPoint).Count == 0)
				{
					int monsterIndex = Random.Range(0, monstersToSpawn.Length);
					EntityData monsterData = monstersToSpawn[monsterIndex];
					Entity monster = new Entity(monsterData, spawnPoint, this);
					entities.Add(monster);
				}
			}
		}
	}

	void UpdateTile(int x, int y, RogueTileData rogueTileData)
	{
		rogueTiles[x, y].data = rogueTileData;
	}

	bool RandomBool()
	{
		return Random.Range(0, 2) == 0;
	}

	// Using Chebychev distance
	public int DistanceToPoint(Vector2Int from, Vector2Int to)
	{
		return Mathf.Max(
			Mathf.Abs(to.x - from.x),
			Mathf.Abs(to.y - from.y));
	}

	public List<Vector2Int> TemporarilyBlockedPositions()
	{
		List<Vector2Int> positions = new List<Vector2Int>();

		foreach (var entity in entities)
		{
			if (entity.data.blocksMovement)
			{
				positions.Add(entity.Position);
			}
		}

		return positions;
	}
}
