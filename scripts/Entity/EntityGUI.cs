using Godot;
using System;

public partial class EntityGUI : Node3D
{
	static PackedScene entityMoveNodeScene = ResourceLoader.Load<PackedScene>(Paths.entityMoveNodeUID);

	public Entity entity;
	private float baseY;
	private float speed = 2.0f;
	private float oscillation = 0f;
	private bool goUp = true;
	public Vector3 targetPosition;
	Direction direction;

	EntityMoveNode currentMoveNode;
	MeshInstance3D meshInstance;

	public override void _Ready()
	{
		baseY = Position.Y;
		meshInstance = GetNode<MeshInstance3D>("Muchkin1_002");
	}

	public void moveTo(Vector3 newPosition)
	{
		this.targetPosition = newPosition;
		currentMoveNode = entityMoveNodeScene.Instantiate<EntityMoveNode>();
		AddChild(currentMoveNode);
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
	public void changeColor(Color color)
	{
		StandardMaterial3D material = meshInstance.MaterialOverride as StandardMaterial3D;

		if (material == null)
		{
			var originalMaterial = meshInstance.GetActiveMaterial(0);
			material = originalMaterial.Duplicate() as StandardMaterial3D;
			meshInstance.MaterialOverride = material;
		}

		material.AlbedoColor = color;
	}
	public void resetMaterial()
	{
		meshInstance.MaterialOverride = null;
	}
	public static EntityGUI getEntityGUI(Entity entity)
	{
		PackedScene scene = GD.Load<PackedScene>(Paths.entityGUIUID);
		EntityGUI entityGUI = scene.Instantiate<EntityGUI>();
		entityGUI.entity = entity;
		entity.setupEntityGUI(entityGUI);

		entityGUI.changeDirection(entity.direction);

		return entityGUI;
	}

	public void changeDirection(Direction direction)
	{
		this.direction = direction;
		correctDirection();
	}
	public void correctDirection()
	{
		float angleDeg = 0;
		switch (this.direction)
		{
			case Direction.UP: angleDeg = 180f; break;
			case Direction.RIGHT: angleDeg = 90f; break;
			case Direction.LEFT: angleDeg = -90f; break;
			case Direction.DOWN: angleDeg = 0f; break;
		}
		this.RotationDegrees = new Vector3(this.RotationDegrees.X, angleDeg, this.RotationDegrees.Z);
	}
	
}
