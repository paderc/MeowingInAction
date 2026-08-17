using Godot;
using Godot.Collections;
using System;

public partial class EntityHandler
{
	GridBlock block;
	public Array<EntityGUI> entities = new Array<EntityGUI>();
	public Array<EntityGUI> previewEntities = new Array<EntityGUI>();

	public EntityHandler(GridBlock block) 
	{
		this.block = block;
	}
	public void addPreviewEntity(EntityGUI entityGUI)
	{
		block.AddChild(entityGUI);
		entityGUI.changeColor(Colors.Green);
		previewEntities.Add(entityGUI);
		repositionAllEntities();
	}
	public void clearPreviewEntities()
	{
		foreach (EntityGUI entityGUI in previewEntities)
		{
			block.RemoveChild(entityGUI);
		}
		previewEntities.Clear();
		repositionAllEntities();
	}
	public void makePreviewPermanent()
	{
		foreach (EntityGUI entityGUI in previewEntities)
		{
			entityGUI.resetMaterial();
			entities.Add(entityGUI);
		}
		previewEntities.Clear();
		repositionAllEntities();
	}
	public static int getTileCount(int entityCount)
	{
		return Mathf.Max(1, Mathf.CeilToInt(Mathf.Sqrt(entityCount)));
	}
	public void repositionAllEntities()
	{
		Array<EntityGUI> entityGUIs = entities + previewEntities;
		int tileCount = getTileCount(entityGUIs.Count);
		float step = block.blockSize / tileCount;

		Vector3 topLeft = block.GlobalPosition + new Vector3(
			-block.blockSize / 2f + step / 2f,
			0,
			-block.blockSize / 2f + step / 2f
		);

		for (int i = 0; i < entityGUIs.Count; i++)
		{
			int row = i / tileCount;
			int col = i % tileCount;

			Vector3 target = topLeft + new Vector3(col * step, 0, row * step);
			target.Y = entityGUIs[i].GlobalPosition.Y;

			entityGUIs[i].moveTo(target);
		}
	}
}
