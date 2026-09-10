using Godot;
using System;

public partial class Draggable : Node
{
	[Signal]
	public delegate void justPutDownEventHandler();
	[Signal]
	public delegate void justPickedUpEventHandler();
	bool pickedUp;
	bool canPickUp;
	public bool canUse = true;
	Vector2 dragOffset;
	Vector2 originalPosition;
	Control targetControl;
	Action onMouseEntered = null;
	Action onMouseExited = null;
	public Draggable(Control target)
	{
		this.targetControl = target;
        targetControl.AddChild(this);
    }

	public override void _Ready()
	{   
		onMouseEntered = () => canPickUp = true;
		onMouseExited = () => canPickUp = false;
        targetControl.MouseEntered += onMouseEntered;
		targetControl.MouseExited += onMouseExited;
		originalPosition = targetControl.GlobalPosition;
	}

	public void pickUp()
	{
		if (!canUse) return;
		pickedUp = true;
        targetControl.Scale = 1.2f * Vector2.One;
		dragOffset = targetControl.GetGlobalMousePosition() - targetControl.GlobalPosition;
		EmitSignal(SignalName.justPickedUp);
	}
	public void silentPutDown()
	{
        pickedUp = false;
        targetControl.Scale = Vector2.One;
    }
    public void putDown()
	{
		pickedUp = false;
        targetControl.Scale = Vector2.One;
		EmitSignal(SignalName.justPutDown);
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
					}
				}
				if (mouseEvent.IsReleased())
				{
					if (pickedUp)
					{
						putDown();
					}
				}
			}
		}
	}
}
