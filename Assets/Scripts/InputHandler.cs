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
		transform.Translate(direction.x, direction.y, 0);
	}
}
