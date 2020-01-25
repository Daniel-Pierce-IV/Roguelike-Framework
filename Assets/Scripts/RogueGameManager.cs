using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RogueGameManager : MonoBehaviour
{
	[SerializeField] RogueMapData rogueMapData;
	[SerializeField] EntityData playerData = null;
	[SerializeField] Tilemap tilemap = null;

	RogueMap rogueMap;

	void Start()
	{
		rogueMap = new RogueMap(tilemap, playerData);
		rogueMap.GenerateMap(rogueMapData);
		rogueMap.RenderToTilemap();
	}

	public void MovePlayer(Vector2Int direction)
	{
		rogueMap.MovePlayer(direction);
	}
}
