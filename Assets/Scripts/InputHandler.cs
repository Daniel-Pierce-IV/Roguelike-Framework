using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
	RogueGameManager manager;

	void Start()
	{
		manager = GameObject.FindObjectOfType<RogueGameManager>();
	}

	// Update is called once per frame
	void Update()
    {
		if (Input.GetKeyDown(KeyCode.W))
		{
			manager.MovePlayer(new Vector2Int(0, 1));
		}
		else if (Input.GetKeyDown(KeyCode.S))
		{
			manager.MovePlayer(new Vector2Int(0, -1));
		}
		else if (Input.GetKeyDown(KeyCode.A))
		{
			manager.MovePlayer(new Vector2Int(-1, 0));
		}
		else if (Input.GetKeyDown(KeyCode.D))
		{
			manager.MovePlayer(new Vector2Int(1, 0));
		}
		else if (Input.GetKeyDown(KeyCode.E))
		{
			manager.MovePlayer(new Vector2Int(-1, 1));
		}
		else if (Input.GetKeyDown(KeyCode.R))
		{
			manager.MovePlayer(new Vector2Int(1, 1));
		}
		else if (Input.GetKeyDown(KeyCode.X))
		{
			manager.MovePlayer(new Vector2Int(-1, -1));
		}
		else if (Input.GetKeyDown(KeyCode.C))
		{
			manager.MovePlayer(new Vector2Int(1, -1));
		}

	}
}
