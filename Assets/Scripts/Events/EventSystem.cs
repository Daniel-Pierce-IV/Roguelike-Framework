using System;

public class EventSystem
{
	// Override the default delegate, since the "sender" object isn't needed
	public event EventHandler<CombatEventArgs> Combat;
	public delegate void EventHandler<CombatEventArgs>(CombatEventArgs e);

	// These set up their own events/delegates
	public Action MapChange;
	public Action<Entity> Death;

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

	public void OnMapChange()
	{
		MapChange();
	}

	public void OnDeath(Entity entity)
	{
		Death(entity);
	}
}
