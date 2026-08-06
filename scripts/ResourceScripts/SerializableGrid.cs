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
	public Array<Array<SerializableBlock>> grid = new Array<Array<SerializableBlock>>();
	public SerializableGrid(string name, BattleGrid battleGrid)
	{
		this.name = name;

		foreach (Array<GridBlock> gridRow in battleGrid.blocks)
		{
			Array<SerializableBlock> row = new Array<SerializableBlock>();
			grid.Add(row);
			foreach (GridBlock block in gridRow)
			{
				SerializableBlock sBlock = new SerializableBlock();
				sizeX = battleGrid.gridSize.X;
				sizeY = battleGrid.gridSize.Y;
				sBlock.type = block.type;
				sBlock.position = block.gridPosition;
				row.Add(sBlock);
			}
		}
	}
	public SerializableGrid(){}
}
