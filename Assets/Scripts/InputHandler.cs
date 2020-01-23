using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.W))
		{
			MovePlayer(new Vector2(0, 1));
		}
		else if (Input.GetKeyDown(KeyCode.S))
		{
			MovePlayer(new Vector2(0, -1));
		}
		else if (Input.GetKeyDown(KeyCode.A))
		{
			MovePlayer(new Vector2(-1, 0));
		}
		else if (Input.GetKeyDown(KeyCode.D))
		{
			MovePlayer(new Vector2(1, 0));
		}
    }

	private void MovePlayer(Vector2 direction)
	{
		if (!DetectCollision(direction))
		{
			transform.Translate(direction.x, direction.y, 0);
		}
	}

	private bool DetectCollision(Vector2 direction)
	{
		Vector2 startPosition = new Vector2(
			transform.position.x,
			transform.position.y
			);

		Vector2 endPosition = startPosition + direction;

		RaycastHit2D hit = Physics2D.Linecast(startPosition, endPosition);

		if (hit.transform != null)
		{
			print(hit.transform.name);
			return true;
		}

		return false;
	}
}
