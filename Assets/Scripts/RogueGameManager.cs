using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueGameManager : MonoBehaviour
{
	public static GameStates gameState = GameStates.PlayerTurn;

	[SerializeField] RogueMapData rogueMapData;
	[SerializeField] EntityData playerData = null;
	[SerializeField] GameEvent onMapChanged;

	RogueMap rogueMap;

	void Start()
	{
		rogueMap = new RogueMap(playerData, onMapChanged);
		rogueMap.GenerateMap(rogueMapData);
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
