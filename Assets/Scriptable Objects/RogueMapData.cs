using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Rogue Map Data", menuName = "Rogue Map Data")]
public class RogueMapData : ScriptableObject
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

	public int maxMonstersPerRoom;
	public EntityData[] monstersToSpawn;
}