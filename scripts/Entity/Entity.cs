using Godot;
using System;

[GlobalClass]
public abstract partial class Entity : Resource
{
	[Export]
	Vector2I position;
	[Export]
	public Direction direction = Direction.UP;
    public abstract void setupEntityGUI(EntityGUI entityGUI);
}
