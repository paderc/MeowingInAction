using Godot;
using System;
using System.Threading.Tasks;

[GlobalClass]
public abstract partial class CardAction : Resource
{
	public abstract void preview(CardActionHandler cardActionHandler);
	public abstract void undoPreview(CardActionHandler cardActionHandler);
	public abstract Task confirmChange(CardActionHandler cardActionHandler);
}
