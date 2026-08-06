using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class EntityGUI : Node3D
{
	static string entityGUIScenePath = "res://scenes/EntityGUI.tscn";

	public Entity entity;
	private float baseY;
	private float speed = 2.0f;
	private float oscillation = 0f;
	private bool goUp = true;

	static Vector3 rotationVector = new Vector3(0, 1, 0).Normalized();
	public override void _Ready()
	{
		baseY = Position.Y;
	}

	public override void _Process(double delta)
	{
		float step = speed * (float)delta;

		if (goUp)
		{
			oscillation += step;
			if (oscillation >= 1.0f)
			{
				oscillation = 1.0f;
				goUp = false;
			}
		}
		else
		{
			oscillation -= step;
			if (oscillation <= 0.0f)
			{
				oscillation = 0.0f;
				goUp = true;
			}
		}
		Position = new Vector3(Position.X, baseY + oscillation, Position.Z);
	}

	public static EntityGUI getEntityGUI(Entity entity)
	{
		PackedScene scene = GD.Load<PackedScene>(entityGUIScenePath);
		EntityGUI entityGUI = scene.Instantiate<EntityGUI>();
		entityGUI.entity = entity;
		entity.setupEntityGUI(entityGUI);

		entityGUI.changeDirection(entity.direction);

		return entityGUI;
	}

	void changeDirection(Direction direction)
	{
		float angleDeg = 0;
		switch (direction)
		{
			case Direction.UP: angleDeg = 90f; break;
			case Direction.RIGHT: angleDeg = 0f; break;
			case Direction.LEFT: angleDeg = 180f; break;
			case Direction.DOWN: angleDeg = 270f; break;
		}
		this.RotationDegrees = new Vector3(this.RotationDegrees.X, angleDeg, this.RotationDegrees.Z);
	}
	
}
