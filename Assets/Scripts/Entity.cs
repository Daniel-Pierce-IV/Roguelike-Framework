using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity
{
	public EntityData data;
	public int x;
	public int y;

	public Vector2Int Position
	{
		get { return new Vector2Int(x, y); }
		set { x = value.x; y = value.y; }
	}

	// The RogueMap that spawned the entity
	RogueMap rogueMap;

	public Entity(EntityData entityData, Vector2Int position, RogueMap rogueMap)
	{
		this.data = entityData;
		this.Position = position;
		this.rogueMap = rogueMap;
	}

	public Vector3Int TilemapPosition()
	{
		return new Vector3Int(x, y, 0);
	}

	public bool CanAct()
	{
		return data.ai != null;
	}

	public void Act()
	{
		data.ai.Act(this, rogueMap);
	}
}
