using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RogueGameManager : MonoBehaviour
{
	[SerializeField] EntityData playerData = null;
	[SerializeField] Tilemap tilemap = null;

	[SerializeField] RogueTileData floorTile = null;
	[SerializeField] RogueTileData wallTile = null;

	RogueTile[,] rogueTileMap = null;
	List<Entity> entities = new List<Entity>();

	// Start is called before the first frame update
	void Start()
	{
		rogueTileMap = MapGenerator.GenerateMap(15, 10, floorTile, wallTile);
		entities.Add(new Entity(playerData, 3, 3));
		RenderMap();
	}

	private void RenderMap()
	{
		for (int x = 0; x < rogueTileMap.GetLength(0); x++)
		{
			for (int y = 0; y < rogueTileMap.GetLength(1); y++)
			{
				tilemap.SetTile(
					rogueTileMap[x, y].GetTilemapPosition(),
					rogueTileMap[x, y].data.spriteTile);
			}
		}

		foreach (Entity entity in entities)
		{
			tilemap.SetTile(
					entity.GetTilemapPosition(),
					entity.data.spriteTile);
		}
	}

	public void MovePlayer(Vector2Int direction)
	{
		Vector2Int position = new Vector2Int(entities[0].x, entities[0].y);
		Vector2Int destination = position + direction;
		RogueTile destinationTile = rogueTileMap[destination.x, destination.y];

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

		RenderMap();
	}
}
