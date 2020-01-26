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
			rogueMap.MovePlayer(direction);
		}

		rogueMap.SimulateEntities();
	}
}

public enum GameStates
{
	PlayerTurn,
	EnemyTurn
}
