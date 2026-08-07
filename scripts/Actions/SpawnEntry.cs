using Godot;
using System;

[GlobalClass]
public partial class SpawnEntry : Resource
{
	[Export]
	public Entity entity;
	[Export]
	public int amount;
}
