using Godot;
using System;

public partial class CardAction : Node
{
	Action action;
	int value;

	public CardAction(Action action, int value)
	{
		this.action = action;
		this.value = value;
	}
}
