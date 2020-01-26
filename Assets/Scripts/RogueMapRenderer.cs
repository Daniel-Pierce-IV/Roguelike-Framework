using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RogueMapRenderer : MonoBehaviour
{
	[SerializeField] RogueMap rogueMap;
	[SerializeField] Tilemap tilemap;
	// Todo make a reference variable SO to the currently selected roguemap 

	float cameraXOffset = -0.5f;
	float cameraYOffset = 0.75f;

	public void RenderMap()
	{
		for (int x = 0; x < rogueMap.rogueTiles.GetLength(0); x++)
		{
			for (int y = 0; y < rogueMap.rogueTiles.GetLength(1); y++)
			{
				tilemap.SetTile(
					rogueMap.rogueTiles[x, y].GetTilemapPosition(),
					rogueMap.rogueTiles[x, y].data.spriteTile);
			}
		}

		foreach (Entity entity in rogueMap.entities)
		{
			tilemap.SetTile(
					entity.TilemapPosition(),
					entity.data.spriteTile);
		}

		UpdateCameraPosition();
	}

	// Set camera's position to match player position
	void UpdateCameraPosition()
	{
		Camera.main.transform.position = new Vector3(
				rogueMap.entities[0].x + cameraXOffset,
				rogueMap.entities[0].y + cameraYOffset,
				Camera.main.transform.position.z);
	}
}
