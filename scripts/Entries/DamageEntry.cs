using Godot;
using System;

[GlobalClass]
public partial class DamageEntry : Entry
{
	public override string ToString()
	{
		return $"Deal {amount} damage";
	}
}
