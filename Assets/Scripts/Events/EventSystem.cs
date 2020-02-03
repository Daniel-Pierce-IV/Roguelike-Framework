using System;

public class EventSystem
{
	// Override the default delegate, since the "sender" object isn't needed
	public event EventHandler<CombatEventArgs> Combat;
	public delegate void EventHandler<CombatEventArgs>(CombatEventArgs e);

	// Singleton pattern
	public static EventSystem Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new EventSystem();
			}

			return instance;
		}
	}

	private static EventSystem instance;

	private EventSystem() { }

	public void OnCombat(CombatEventArgs args)
	{
		Combat.Invoke(args);
	}
}
