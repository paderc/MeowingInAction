using Godot;
using Godot.Collections;
using System;
public partial class StateHandler
{
	Node parent;
	Node current;
	public StateHandler(Node parent)
	{
		this.parent = parent;
	}
	public void switchCurrent(Node next, ILabelAnimHandler labelAnimHandler)
	{
		change(next);
		labelAnimHandler.playAnimations();
	}
	public void switchCurrent(Node next)
	{
		change(next);
	}
	void change(Node next)
	{
		removeIfNotNull(current);
		addIfNotNull(next);
		current = next;
	}
	void removeIfNotNull(Node state)
	{
		if (state == null) return;
		parent.RemoveChild(state);
		state.QueueFree();
	}
	void addIfNotNull(Node state)
	{
		if (state == null ) { GD.PushError("Cannot add null"); return; }
		parent.AddChild(state);
	}
}
