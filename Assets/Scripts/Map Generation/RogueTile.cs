using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueTile
{
	public RogueTileData data;
	public int x;
	public int y;

	public RogueTile(RogueTileData rogueTileData, int x, int y)
	{
		this.data = rogueTileData;
		this.x = x;
		this.y = y;
	}

	public Vector3Int GetTilemapPosition()
	{
		return new Vector3Int(x, y, 0);
	}
}
