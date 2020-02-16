using UnityEngine;

[CreateAssetMenu(fileName = "New AI", menuName = "Artificial Intelligence")]
public class RogueAI : ScriptableObject
{
	// Attack player on sight
	//const int sightRange = 5; TODO only use when not using FOV
	const int attackRange = 1;

	public void Act(Entity entity, RogueMap rogueMap)
	{
		entity.AssessFieldOfView();

		// TODO use a better value than Vector2Int.zero, preferrably null
		if (entity.targetsLastKnownPosition != Vector2Int.zero)
		{
			// We've lost the target
			if (entity.Position == entity.targetsLastKnownPosition)
			{
				entity.targetsLastKnownPosition = Vector2Int.zero;
			}

			int distance = rogueMap.DistanceToPoint(
				entity.Position,
				entity.targetsLastKnownPosition);

			if (distance <= attackRange && entity.target != null && entity.target.stats.IsAlive())
			{
				entity.Attack(rogueMap.Player);
			}
			// Try to be in a tile adjecent to the target
			else if (distance > 0)
			{
				entity.travelPath = rogueMap.pathFinder.FindPath(
					entity.Position,
					entity.targetsLastKnownPosition,
					rogueMap.TemporarilyBlockedPositions());

				// If there is no available path, try again,
				// but we'll pretend the temporary blockers are gone
				// to get as close as possible to the target
				if (entity.travelPath == null)
				{
					entity.travelPath = rogueMap.pathFinder.FindPath(
						entity.Position,
						entity.targetsLastKnownPosition);
				}

				// Remove the first element, since its our current position
				entity.travelPath.RemoveAt(0);

				rogueMap.MoveEntityAlongPath(entity);

				// Re-assess targets after moving
				entity.AssessFieldOfView();
			}
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
