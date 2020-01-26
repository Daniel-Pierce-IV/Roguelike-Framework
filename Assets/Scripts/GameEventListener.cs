using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
	[SerializeField] GameEvent gameEvent;
	[SerializeField] UnityEvent responseToGameEvent;

	private void OnEnable()
	{
		gameEvent.AddListener(this);
	}

	private void OnDisable()
	{
		gameEvent.RemoveListener(this);
	}

	public void OnEventBroadcast()
	{
		responseToGameEvent.Invoke();
	}
}
