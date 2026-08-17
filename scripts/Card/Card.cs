using Godot;
using Godot.Collections;
using System;

[GlobalClass]
public partial class Card : Resource
{
	[Export]
	public string name;
	[Export]
	public string description;
	[Export]
	public Array<Action> actionList;
	[Export]
	public int cost;
    [Export]
    public Area area;

    public void doActions(CardActionHandler handler)
	{
		foreach (Action action in actionList)
		{
			action.perform(handler);
		}
	}
	public void preview(CardActionHandler handler)
	{
		foreach(Action action in actionList)
		{
			action.preview(handler);
		}
	}
	public Card()
	{
		
	}
}
