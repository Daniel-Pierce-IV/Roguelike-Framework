using System;
using UnityEngine;

[Serializable]
public class Stats
{
	public int maxHp;
	public int power;
	public int defense;

	[NonSerialized] public int curHp;

	// Make a copy
	public Stats(Stats stats)
	{
		this.maxHp = stats.maxHp;
		this.curHp = this.maxHp;
		this.power = stats.power;
		this.defense = stats.defense;
	}

	public Stats Clone()
	{
		return new Stats(this);
	}

	public int TakeDamage(int amount)
	{
		int totalDamage = amount - defense;
		curHp -= Math.Max(0, totalDamage);

		return totalDamage;
	}

	public bool IsAlive()
	{
		return curHp > 0;
	}

	public bool IsDead()
	{
		return curHp <= 0;
	}
}
