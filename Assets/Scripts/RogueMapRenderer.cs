using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RogueMapRenderer : MonoBehaviour
{
	[SerializeField] RogueMap rogueMap;
	[SerializeField] Tilemap backgroundTilemap;
	[SerializeField] Tilemap fogOfWarTilemap;
	//[SerializeField] Tile emptyTile;
	// Todo make a reference variable SO to the currently selected roguemap 

	float cameraXOffset = 0.5f;
	float cameraYOffset = 0.5f;

	private void OnEnable()
	{
		EventSystem.Instance.MapChange += RenderMap;
	}

	public void RenderMap()
	{
		for (int x = 0; x < rogueMap.rogueTiles.GetLength(0); x++)
		{
			for (int y = 0; y < rogueMap.rogueTiles.GetLength(1); y++)
			{
				backgroundTilemap.SetTile(
					rogueMap.rogueTiles[x, y].GetTilemapPosition(),
					rogueMap.rogueTiles[x, y].data.spriteTile);
			}
		}

		foreach (Entity entity in rogueMap.entities)
		{
			backgroundTilemap.SetTile(
					entity.TilemapPosition(),
					entity.data.spriteTile);
		}

		UpdateFogOfWar();
		UpdateCameraPosition();
	}

	// Set camera's position to match player position
	void UpdateCameraPosition()
	{
		Camera.main.transform.position = new Vector3(
				rogueMap.Player.x + cameraXOffset,
				rogueMap.Player.y + cameraYOffset,
				Camera.main.transform.position.z);
	}

	void UpdateFogOfWar()
	{
		for (int x = 0; x < rogueMap.mapWidth; x++)
		{
			for (int y = 0; y < rogueMap.mapHeight; y++)
			{
				fogOfWarTilemap.SetTile(
					new Vector3Int(x, y, 0),
					rogueMap.opaqueFogTile);
			}
		}

		// Remove any fog of war from the tile the player can see
		foreach (var position in rogueMap.Player.PositionsInView)
		{
			fogOfWarTilemap.SetTile(
					(Vector3Int) position,
					null);
		}
	}
}
