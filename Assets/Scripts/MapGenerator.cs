using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
	[SerializeField] int mapWidth = 9;
	[SerializeField] int mapHeight = 6;

	[SerializeField] Tilemap floorMap;
	[SerializeField] Tilemap wallMap;
	[SerializeField] Tile floorTile;
	[SerializeField] Tile wallTile;

	// Start is called before the first frame update
	void Start()
    {
		BuildFloor(mapWidth, mapHeight);
		BuildWall(mapWidth, mapHeight);
    }

    void BuildFloor(int width, int height)
	{
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				floorMap.SetTile(
					new Vector3Int(x, y, 0),
					floorTile
					);
			}
		}
	}

	void BuildWall(int width, int height)
	{
		for (int x = 0; x < width; x++)
		{
			wallMap.SetTile( new Vector3Int(x, 0, 0), wallTile);
			wallMap.SetTile( new Vector3Int(x, height - 1, 0), wallTile);
		}
		for (int y = 0; y < height; y++)
		{
			wallMap.SetTile(new Vector3Int(0, y, 0), wallTile);
			wallMap.SetTile(new Vector3Int(width - 1, y, 0), wallTile);
		}
		//for (int x = 0; x < width; x++)
		//{
		//	for (int y = 0; y < height; y++)
		//	{
		//		// Only create the map boundary walls
		//		if (x == 0 || x == width - 1)
		//		{
		//			if (y == 0 || y == height - 1)
		//			{
		//				wallMap.SetTile(
		//					new Vector3Int(x, y, 0),
		//					wallTile
		//					);
		//			}
		//		}
		//	}
		//}
	}
}
