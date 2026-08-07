using Godot;
using Godot.Collections;
using System;

[GlobalClass]
public partial class SpawnAction : Action
{
	[Export]
	Array<SpawnEntry> entries;
	public override void perform(CardActionHandler handler)
	{
		foreach (SpawnEntry entry in entries)
			{
			for (int i = 0; i < entry.amount; i++)
			{
				handler.currentHovered.addEntity(entry.entity);
			}
		}
	}
	public override void undo(CardActionHandler cardActionHandler)
	{
		throw new NotImplementedException();
	}
	public override void preview(CardActionHandler cardActionHandler)
	{
		throw new NotImplementedException();
	}

	

	public override void undoPreview(CardActionHandler cardActionHandler)
	{
		throw new NotImplementedException();
	}
}
