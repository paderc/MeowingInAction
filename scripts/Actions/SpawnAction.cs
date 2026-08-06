using Godot;
using System;

[GlobalClass]
public partial class SpawnAction : Action
{
	[Export]
	int value = 0;
	public override void perform(CardActionHandler handler)
	{
		for (int i = 0; i < value; i++)
		{
			Ally ally = new Ally();
			handler.currentHovered.addEntity(ally);
		}
	}
}
