using System;

public class CombatEventArgs : EventArgs
{
	public Entity Attacker { get; set; }
	public Entity Defender { get; set; }
	public int Damage { get; set; }

	public CombatEventArgs(Entity attacker, Entity defender, int damage)
	{
		Attacker = attacker;
		Defender = defender;
		Damage = damage;
	}
}
