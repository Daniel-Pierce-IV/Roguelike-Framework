using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity
{
	public EntityData data;
	public Stats stats;
	public int x;
	public int y;
	public List<Vector2Int> travelPath = new List<Vector2Int>();

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
		stats = data.stats.Clone();
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

	public void Attack(Entity target)
	{
		int totalDamage = target.stats.TakeDamage(stats.power);

		if (totalDamage > 0)
		{
			Debug.Log(
				data.name +
				" attacks " +
				target.data.name +
				" for " +
				totalDamage +
				" damage");
		}
		else
		{
			Debug.Log(
				data.name +
				" attacks " +
				target.data.name +
				" but does no damage");
		}
	}
}
