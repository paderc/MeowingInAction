using Godot;
using System;

public partial class EntityMoveNode : Node
{
	float speed = 5;

	EntityGUI entityGUI;
	public override void _Ready()
	{
		if (GetParent().GetType() != typeof(EntityGUI)) GD.PushError("EntityMoveNode attached to something wrong");
		entityGUI = GetParent<EntityGUI>();
	}
	public override void _Process(double delta)
	{
		float deltaF = (float)delta;

		Vector3 direction = entityGUI.targetPosition - entityGUI.GlobalPosition;
		float distance = direction.Length();
		Vector3 speedVector = direction.Normalized() * speed;

		if (distance < speed * deltaF)
		{
			entityGUI.GlobalPosition = entityGUI.targetPosition;
			entityGUI.correctDirection();
			this.Free();
		}
		else entityGUI.GlobalPosition += speedVector * deltaF;
	}
}
