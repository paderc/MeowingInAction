using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[GlobalClass]
public partial class SpawnAction : BlockAction
{
	[Export]
	Array<SpawnEntry> entries;
	public override async Task confirmChangesLikeOn(GridBlock block)
	{
		foreach (SpawnEntry entry in entries)
		{
			for (int i = 0; i < entry.amount; i++)
			{
				block.entityHandler.addEntity(new Entity(entry.faction, entry.health));
			}
			GD.Print(entry.ToString() + " onto " + block.ToString());
			await Task.CompletedTask;
		}
	}

	public override void previewLikeOn(GridBlock block)
	{
		foreach (SpawnEntry entry in entries)
		{
			Color color = entry.faction == Faction.Ally ? Colors.Green : Colors.Red;
			block.entityHandler.addSpawnPreviewEntity(color, entry.amount);
		}
	}

	public override void undoPreviewLikeOn(GridBlock block)
	{
		block.entityHandler.clearSpawnPreviewEntities();
	}
}
