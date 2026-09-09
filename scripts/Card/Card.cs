using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[GlobalClass]
public partial class Card : Resource
{
	[Export]
	public string name;
	[Export]
	public string description;
	[Export]
	public Array<CardAction> actionList;
	[Export]
	public int cost;
	[Export]
	public Area area;

	public async Task commitChangesAsync(CardActionHandler handler)
	{
		foreach (CardAction action in actionList)
		{
			await action.confirmChange(handler);
		}
	}
	public async Task commitChangesAsync(CardActionHandler handler, Array<GridBlock> blocks)
	{
		foreach (CardAction action in actionList)
		{
			await action.confirmChange(handler);
		}
	}
	public void preview(CardActionHandler handler)
	{
		foreach(CardAction action in actionList)
		{
			action.preview(handler);
		}
	}
	public void unpreview(CardActionHandler handler)
	{
		foreach (CardAction action in actionList)
		{
			action.undoPreview(handler);
		}
	}
	public Card()
	{
		
	}
}
