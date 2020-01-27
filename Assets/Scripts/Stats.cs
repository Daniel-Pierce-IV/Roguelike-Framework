using System;

[Serializable]
public class Stats
{
	public int maxHp;
	public int power;
	public int defense;

    public Stats(int maxHp, int power, int defense)
	{
		this.maxHp = maxHp;
		this.power = power;
		this.defense = defense;
	}
}
