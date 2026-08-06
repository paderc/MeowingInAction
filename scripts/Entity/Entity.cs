using Godot;
using System;

public abstract partial class Entity : Node
{
	Vector2I position;
	public Direction direction = Direction.UP;
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}
    public abstract void setupEntityGUI(EntityGUI entityGUI);
}
