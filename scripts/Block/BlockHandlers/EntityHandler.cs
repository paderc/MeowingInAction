using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class EntityHandler
{
	static float appearOffset = 30;
	GridBlock block;
	Entity.DeadEventHandler deadEventHandler;
	public Array<Entity> entities = new Array<Entity>();
	public Array<EntityGUI> previewEntities = new Array<EntityGUI>();

	public EntityHandler(GridBlock block) 
	{
		this.block = block;
		deadEventHandler = (entity) =>
		{
			GD.PushError("Unhandled death");
		};
	}
	public void addEntity(Entity entity)
	{
		block.AddChild(entity);
		entity.gui.GlobalPosition = entity.faction == Faction.Ally ? new Vector3(-appearOffset, 0, 0) : new Vector3(appearOffset, 0, 0);
		
		entities.Add(entity);
		entity.Dead += deadEventHandler;
		repositionAllEntities();
	}
	public void removeEntity(Entity entity)
	{
		entities.Remove(entity);
		entity.Dead -= deadEventHandler;
		repositionAllEntities();
	}
	public void addSpawnPreviewEntity(Color color, int count)
	{
		for (int i = 0; i < count; i++)
		{
			EntityGUI entityGUI = EntityGUI.getEntityGUI();
			entityGUI.Name = "PreviewEntity" + i.ToString();
			block.AddChild(entityGUI);
			entityGUI.changeColor(color);
			previewEntities.Add(entityGUI);
			repositionAllEntities();
		}
	}
	public void clearSpawnPreviewEntities()
	{
		if (previewEntities.Count == 0 ) return;
		HashSet<EntityGUI> confirmedEntities = new HashSet<EntityGUI>(previewEntities);
		previewEntities.Clear();
		foreach (EntityGUI entityGUI in confirmedEntities)
		{
			block.RemoveChild(entityGUI);
		}
		confirmedEntities.Clear();
		repositionAllEntities();
	}
	
	public void previewDamage(int damage)
	{
		foreach (Entity entity in entities)
		{
			if (entity.damagable)
			{
				if (entity.checkIfDeadAfter(damage))
				{
					entity.gui.changeColor(Colors.Red);
				}
				else
				{
					entity.gui.changeColor(Colors.Orange);
				}
			}
		}
	}
	public void makeDamagePermanent(int damage)
	{
		HashSet<Entity> confirmedEntities = new HashSet<Entity>(entities);
		foreach (Entity entity in confirmedEntities)
		{
			entity.takeDamage(damage);
		}
	}
	public void undoPreviewDamage()
	{
		HashSet<Entity> confirmedEntities = new HashSet<Entity>(entities);
		foreach (Entity entity in entities)
		{
			entity.gui.resetMaterial();
		}
	}
	public static int getTileCount(int entityCount)
	{
		return Mathf.Max(1, Mathf.CeilToInt(Mathf.Sqrt(entityCount)));
	}
	public void repositionAllEntities()
	{
		Array<EntityGUI> entityGUIs = new Array<EntityGUI>(previewEntities);
		foreach(Entity entity in entities)
		{
			entityGUIs.Add(entity.gui);
		}
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

			entityGUIs[i].moveNode.moveTo(target);
		}
	}
	public void connectEntityGUI(Entity entity)
	{
		entity.Dead += deadEventHandler;
	}
	public void disconnectEntityGUI(Entity entity)
	{
		entity.Dead -= deadEventHandler;
	}
}
