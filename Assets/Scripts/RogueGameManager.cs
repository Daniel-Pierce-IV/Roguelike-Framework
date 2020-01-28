using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueGameManager : MonoBehaviour
{
	public static GameStates gameState = GameStates.PlayerTurn;

	[SerializeField] RogueMap rogueMap;

	void Start()
	{
		rogueMap.GenerateMap();
	}

	public void MovePlayer(Vector2Int direction)
	{
		if (gameState == GameStates.PlayerTurn)
		{
			Vector2Int newPosition = rogueMap.Player.Position + direction;

			if (rogueMap.PositionCanBeMovedTo(newPosition))
			{
				rogueMap.MoveEntityToPosition(rogueMap.Player, newPosition);
			}
			else
			{
				List<Entity> discoveredEntities = rogueMap.AttackableEntitiesAtPosition(newPosition);

				if (discoveredEntities.Count > 0)
				{
					rogueMap.AttackEntity(rogueMap.Player, discoveredEntities[0]);
				}
				else
				{
					// Player didnt act, so don't end their turn
					return;
				}
			}
		}

		gameState = GameStates.EnemyTurn;
		rogueMap.SimulateEntities();
	}
}

public enum GameStates
{
	PlayerTurn,
	EnemyTurn
}
