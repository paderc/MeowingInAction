using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class SerializableGrid : Resource
{
	[Export]
	public int sizeX;
	[Export]
	public int sizeY;
	[Export]
	public string name;
	[Export]
	public Array<SerializableBlock> grid = new Array<SerializableBlock>();
	public SerializableGrid(string name, Battle battle)
	{
		this.name = name;

		foreach (GridBlock block in battle.blocks)
		{
			SerializableBlock sBlock = new SerializableBlock();
			sizeX = battle.gridSize.X;
			sizeY = battle.gridSize.Y;
			sBlock.type = block.type;
			sBlock.position = block.gridPosition;
			this.grid.Add(sBlock);
		}
	}
	public SerializableGrid(){}
}
