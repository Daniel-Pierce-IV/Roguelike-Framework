using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueGameManager : MonoBehaviour
{
	public static GameStates gameState = GameStates.PlayerTurn;
	public static Entity player;

	[SerializeField] RogueMap rogueMap;

	void OnEnable()
	{
		rogueMap.GenerateMap();
		player = rogueMap.Player;
		EventSystem.Instance.Combat += PrintCombatMessages;
	}

	public void PrintCombatMessages(CombatEventArgs args)
	{
		string attackerName = args.Attacker.data.name;
		string defenderName = args.Defender.data.name;

		Debug.Log(
			attackerName +
			" attacked " +
			defenderName +
			" for " +
			args.Damage +
			" damage");
	}

	public void MovePlayer(Vector2Int direction)
	{
		// Disallow any game interaction when the player dies
		if (rogueMap.Player.stats.IsDead()) return;

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
					//rogueMap.AttackEntity(rogueMap.Player, discoveredEntities[0]);
					rogueMap.Player.Attack(discoveredEntities[0]);
				}
				else
				{
					// Player didnt act, so don't end their turn
					return;
				}
			}
		}

		PassCurrentTurn();
	}

	public void PassCurrentTurn()
	{
		gameState = GameStates.EnemyTurn;
		rogueMap.SimulateEntities();
	}
}

public enum GameStates
{
	PlayerTurn,
	EnemyTurn
}
