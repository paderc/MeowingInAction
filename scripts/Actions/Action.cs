using Godot;
using System;

[GlobalClass]
public abstract partial class Action : Resource
{
	public abstract void perform(CardActionHandler cardActionHandler);
	public abstract void undo(CardActionHandler cardActionHandler);
	public abstract void preview(CardActionHandler cardActionHandler);
	public abstract void undoPreview(CardActionHandler cardActionHandler);
	
}
