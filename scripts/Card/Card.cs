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

    public void doActions(CardActionHandler cardActionHandler)
	{
		foreach (Action action in actionList)
		{
			action.perform(cardActionHandler);
		}
	}
	public Card()
	{
		
	}
}
