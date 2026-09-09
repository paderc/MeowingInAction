using Godot;
using System;

[GlobalClass]
public partial class SpawnEntry : Entry
{
	[Export]
	public Faction faction;
	[Export]
	public int health;
	public override string ToString()
	{
		return $"Spawn {amount} of {faction} with {health} health";
	}
}
