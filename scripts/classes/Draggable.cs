using Godot;
using System;

public partial class Draggable : Node
{
	[Signal]
	public delegate void justPutDownEventHandler();
	bool pickedUp;
	bool canPickUp;
	bool snapBack;
	Vector2 dragOffset;
	Vector2 originalPosition;
	Control targetControl;

	public Draggable(Control target)
	{
		this.targetControl = target;
        targetControl.AddChild(this);
    }

	public override void _Ready()
	{
		targetControl.MouseEntered += () => canPickUp = true;
		targetControl.MouseExited += () => canPickUp = false;
		originalPosition = targetControl.GlobalPosition;
	}

	void pickUp()
	{
		pickedUp = true;
        targetControl.Scale = 1.2f * Vector2.One;
		dragOffset = targetControl.GetGlobalMousePosition() - targetControl.GlobalPosition;
	}

	void putDown()
	{
		pickedUp = false;
        targetControl.Scale = Vector2.One;
		EmitSignal("justPutDown");
	}

	void clampToParent()
	{
		Vector2 viewportSize = targetControl.GetViewportRect().Size;

        targetControl.SetGlobalPosition(
			new Vector2(
				Math.Clamp(targetControl.GlobalPosition.X, 0, viewportSize.X - targetControl.Size.X),
				Math.Clamp(targetControl.GlobalPosition.Y, 0 , viewportSize.Y - targetControl.Size.Y)
			)
		);
	}

	void _snapBack()
	{
        targetControl.SetGlobalPosition(originalPosition);
	}

	public override void _Input(InputEvent @event) {
		if (pickedUp) {
			if (@event is InputEventMouseMotion) {
                targetControl.SetGlobalPosition(targetControl.GetGlobalMousePosition() - dragOffset);
			}
		}
		if (@event is InputEventMouseButton mouseEvent) {
			if (mouseEvent.ButtonIndex == MouseButton.Left) {
				if (mouseEvent.Pressed) {
					if (canPickUp && !pickedUp) {
						pickUp();
					} else if (pickedUp) {
						putDown();
					}
				}
			}
		}
	}
}
