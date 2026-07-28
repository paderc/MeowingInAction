using Godot;
using Godot.Collections;
using System;

[GlobalClass]
public partial class Card : Resource
{
	[Export]
	public Array<Action> actionList = new Array<Action>();
	[Export]
	public int cost;
	
	public Card()
	{
		
	}
}
