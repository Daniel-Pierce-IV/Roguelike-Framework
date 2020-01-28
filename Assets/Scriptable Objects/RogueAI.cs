using UnityEngine;

[CreateAssetMenu(fileName = "New AI", menuName = "Artificial Intelligence")]
public class RogueAI : ScriptableObject
{
	public void Act(Entity entity, RogueMap rogueMap)
	{
		// Attack player on sight
		const float sightRange = 5f;
		const float attackRange = 2f;

		Vector2Int direction = rogueMap.Player.Position - entity.Position;

		if (direction.magnitude < attackRange)
		{
			rogueMap.AttackEntity(entity, rogueMap.Player);
		}
		else if (direction.magnitude < sightRange)
		{
			Vector2Int newPosition = entity.Position + NormalizedVector2Int(direction);
			
			if (rogueMap.PositionCanBeMovedTo(newPosition))
			{
				rogueMap.MoveEntityToPosition(entity, newPosition);
			}
		}
		//Debug.Log("The " + entity.data.name + " is itching for a fight.");
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
