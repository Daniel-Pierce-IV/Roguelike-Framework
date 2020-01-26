using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity
{
	public EntityData data;
	public int x;
	public int y;

	public Entity(EntityData entityData, int x, int y)
	{
		this.data = entityData;
		this.x = x;
		this.y = y;
	}

	public Vector3Int TilemapPosition()
	{
		return new Vector3Int(x, y, 0);
	}

	public Vector2Int RogueMapPosition()
	{
		return new Vector2Int(x, y);
	}
}
