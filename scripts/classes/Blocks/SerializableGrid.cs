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
	public SerializableGrid(string name, BattleGrid battleGrid)
	{
		this.name = name;

		foreach (GridBlock block in battleGrid.blocks)
		{
			SerializableBlock sBlock = new SerializableBlock();
			sizeX = battleGrid.gridSize.X;
			sizeY = battleGrid.gridSize.Y;
			sBlock.type = block.type;
			sBlock.position = block.gridPosition;
			this.grid.Add(sBlock);
		}
	}
	public SerializableGrid(){}
}
