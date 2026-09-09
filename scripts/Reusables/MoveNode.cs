using Godot;
using System;

public partial class MoveNode : Node
{
	static PackedScene entityMoveNodeScene;

	float speed = 10;

	[Signal]
	public delegate void ArrivedEventHandler();
	
	Node3D node;
	Vector3 _targetPosition;
	Vector3 targetPosition
	{
		get => _targetPosition;
		set
		{
			if (_targetPosition != value)
			{
				_targetPosition = value;
				SetProcess(true);
			}
		}
	}
	
	public override void _Ready()
	{
		node = GetParent<Node3D>();
	}
	public static MoveNode create()
	{
		if (entityMoveNodeScene == null) entityMoveNodeScene = GD.Load<PackedScene>(Paths.entityMoveNodeUID);
		return entityMoveNodeScene.Instantiate<MoveNode>();
	}
	public void correctDirection(Direction direction)
	{
		float angleDeg = 0;
		switch (direction)
		{
			case Direction.UP: angleDeg = 180f; break;
			case Direction.RIGHT: angleDeg = 90f; break;
			case Direction.LEFT: angleDeg = -90f; break;
			case Direction.DOWN: angleDeg = 0f; break;
		}
		node.RotationDegrees = new Vector3(node.RotationDegrees.X, angleDeg, node.RotationDegrees.Z);
	}
	public void moveTo(Vector3 newPosition)
	{
		targetPosition = newPosition;
	}
	
	public override void _Process(double delta)
	{
		float deltaF = (float)delta;

		Vector3 direction = targetPosition - node.GlobalPosition;
		float distance = direction.Length();
		Vector3 speedVector = direction.Normalized() * speed;

		if (distance < speed * deltaF)
		{
			node.GlobalPosition = targetPosition;
			EmitSignal(SignalName.Arrived);
			SetProcess(false);
		}
		else node.GlobalPosition += speedVector * deltaF;
	}
}
