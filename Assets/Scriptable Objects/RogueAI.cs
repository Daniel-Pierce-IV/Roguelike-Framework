using UnityEngine;

[CreateAssetMenu(fileName = "New AI", menuName = "Artificial Intelligence")]
public class RogueAI : ScriptableObject
{
	public void Act(Entity entity, RogueMap rogueMap)
	{
		// Attack player on sight
		const int sightRange = 5;
		const int attackRange = 1;

		int distance = rogueMap.DistanceToPoint(
			entity.Position,
			rogueMap.Player.Position);

		if (distance <= attackRange && rogueMap.Player.CurHp > 0)
		{
			rogueMap.AttackEntity(entity, rogueMap.Player);
		}
		else if (distance <= sightRange)
		{
			entity.travelPath = rogueMap.pathFinder.FindPath(
				entity.Position,
				rogueMap.Player.Position,
				rogueMap.TemporarilyBlockedPositions());

			// If there is no available path, try again,
			// but we'll pretend the temporary blockers are gone
			// to get as close as possible to the target
			if (entity.travelPath == null)
			{
				entity.travelPath = rogueMap.pathFinder.FindPath(
					entity.Position,
					rogueMap.Player.Position);
			}

			// Remove the first element, since its our current position
			entity.travelPath.RemoveAt(0);

			rogueMap.MoveEntityAlongPath(entity);
		}
	}

	// "Normalize" the direction to integer values (best approximation)
	// NOTE: "normalized" interger direction always adds up to 0, 1, or 2
	Vector2Int NormalizedVector2Int(Vector2Int direction)
	{
		Vector2 floatDirection = new Vector2(direction.x, direction.y);
		float distance = floatDirection.magnitude;

		return Vector2Int.RoundToInt(floatDirection / distance);
	}
}
