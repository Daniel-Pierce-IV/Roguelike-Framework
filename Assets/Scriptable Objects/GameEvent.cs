using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Game Event", menuName = "Game Event")]
public class GameEvent : ScriptableObject
{
	private List<GameEventListener> listeners = new List<GameEventListener>();

	public void Broadcast()
	{
		// Traverse the list backwards in case any listeners remove
		// themselves when they receive the broadcast
		for (int i = listeners.Count - 1; i >= 0; i--)
		{
			listeners[i].OnEventBroadcast();
		}
	}

	public void AddListener(GameEventListener listener)
	{
		listeners.Add(listener);
	}

	public void RemoveListener(GameEventListener listener)
	{
		listeners.Remove(listener);
	}
}
