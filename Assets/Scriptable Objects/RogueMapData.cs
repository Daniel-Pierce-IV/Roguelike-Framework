using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Rogue Map Data", menuName = "Rogue Map Data")]
public class RogueMapData : ScriptableObject
{
	public int mapWidth;
	public int mapHeight;

	public int minimumRoomWidth;
	public int maximumRoomWidth;
	public int minimumRoomHeight;
	public int maximumRoomHeight;

	public int maximumNumberOfRooms;

	public RogueTileData floorTileData;
	public RogueTileData wallTileData;

	public int maximumMonstersPerRoom;
	public EntityData[] monstersToSpawn;
}