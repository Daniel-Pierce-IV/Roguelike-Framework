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
	public List<Vector2Int> PositionsInView { get; private set; }

	public Entity target;
	public Vector2Int targetsLastKnownPosition= Vector2Int.zero;

	public Vector2Int Position
	{
		get { return new Vector2Int(x, y); }
		set { 
			x = value.x;
			y = value.y;
			UpdateFieldOfView();
		}
	}

	// The RogueMap that spawned the entity
	RogueMap rogueMap;

	public Entity(EntityData entityData, Vector2Int position, RogueMap rogueMap)
	{
		this.data = entityData;
		this.rogueMap = rogueMap;
		this.Position = position;
		stats = data.stats.Clone();
	}

	public Vector3Int TilemapPosition()
	{
		return new Vector3Int(x, y, 0);
	}

	public bool CanAct()
	{
		return data.ai != null && stats.IsAlive();
	}

	public void Act()
	{
		data.ai.Act(this, rogueMap);
	}

	public void Attack(Entity target)
	{
		int totalDamage = target.stats.TakeDamage(stats.power);
		
		EventSystem.Instance.OnCombat(
			new CombatEventArgs(this, target, totalDamage));

		if (target.stats.IsDead())
		{
			EventSystem.Instance.OnDeath(target);
		}
	}

	public void UpdateFieldOfView()
	{
		PositionsInView = rogueMap.PositionsVisibleFromPosition(Position);
	}

	// Set/unset target based on sight
	// Update last known position of target if available for tracking
	public void AssessFieldOfView()
	{
		if (EntityIsInFieldOfView(rogueMap.Player))
		{
			target = rogueMap.Player;
			targetsLastKnownPosition = target.Position;
		}
		else
		{
			target = null;
		}
	}

	private bool EntityIsInFieldOfView(Entity entity)
	{
		return PositionsInView.Contains(entity.Position);
	}
}
