using Godot;
using Godot.Collections;
using System;

[GlobalClass]
public partial class SpawnAction : BlockAction
{
	[Export]
	Array<SpawnEntry> entries;
	public override void perform(CardActionHandler handler)
	{
		foreach (GridBlock block in handler.hoveredBlocks)
		{
			block.entityHandler.makePreviewPermanent();
		}
	}
	public override void undo(CardActionHandler handler)
	{
		throw new NotImplementedException();
	}
	public override void preview(CardActionHandler handler)
	{
		foreach (GridBlock block in handler.hoveredBlocks)
		{
			foreach (SpawnEntry entry in entries)
			{
				for (int i = 0; i < entry.amount; i++)
				{
					EntityGUI entityGUI = EntityGUI.getEntityGUI(entry.entity);
					block.entityHandler.addPreviewEntity(entityGUI);
				}
			}
		}
	}
	public override void undoPreview(CardActionHandler handler)
	{
		foreach (GridBlock block in handler.previewBlocks)
		{
			block.entityHandler.clearPreviewEntities();
		}
	}
}
